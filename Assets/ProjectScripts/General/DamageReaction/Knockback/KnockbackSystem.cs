using Unity.Burst;
using Unity.Entities;
using Unity.Physics;

namespace ProjectScripts.General.DamageReaction.Knockback
{
    [UpdateInGroup(typeof(CustomTranslationSystemGroup))]
    public partial struct KnockbackSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var deltaTime = SystemAPI.Time.DeltaTime;
            
            foreach (var (physicsVelocity, knockbackState, knockbackTakenMultiplier, shouldKnockback) in
                     SystemAPI.Query<RefRW<PhysicsVelocity>, RefRW<KnockbackState>, KnockbackData, EnabledRefRW<KnockbackState>>())
            {
                knockbackState.ValueRW.LeftTime -= deltaTime;
                
                if (knockbackState.ValueRO.LeftTime <= 0f)
                {
                    shouldKnockback.ValueRW = false;
                    continue;
                }

                physicsVelocity.ValueRW.Linear = knockbackState.ValueRO.Direction * knockbackTakenMultiplier.Strangth;
            }
        }
    }
}