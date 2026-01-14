using ProjectScripts.General.DamageReaction.Knockback;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;

namespace ProjectScripts.Character.General.Move
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateBefore(typeof(PhysicsSystemGroup))]
    public partial struct CharacterMoveSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (moveVelocity, moveDirection, moveSpeed) in
                     SystemAPI.Query<RefRW<PhysicsVelocity>, RefRO<CharacterMoveDirection>, RefRO<CharacterMoveSpeed>>().WithNone<KnockbackState>())
            {
                var currentMovement = moveDirection.ValueRO.Value * moveSpeed.ValueRO.Value;
                moveVelocity.ValueRW.Linear = new float3(currentMovement.x, 0f, currentMovement.y);
            }
        }
    }
}