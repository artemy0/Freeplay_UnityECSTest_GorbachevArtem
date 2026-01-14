using ProjectScripts.General.DamageReaction.Knockback;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ProjectScripts.Character.General.Move
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateAfter(typeof(TransformSystemGroup))]
    public partial struct CharacterRotateSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (moveTransform, moveDirection) in
                     SystemAPI.Query<RefRW<LocalTransform>, RefRO<CharacterMoveDirection>>().WithNone<KnockbackState>())
            {
                var currentDirection = moveDirection.ValueRO.Value;
                if (math.lengthsq(currentDirection) < float.Epsilon)
                {
                    continue;
                }

                currentDirection = math.normalize(currentDirection);
                moveTransform.ValueRW.Rotation = quaternion.LookRotation(new float3(currentDirection.x, 0f, currentDirection.y), math.up());
            }
        }
    }
}