using ProjectScripts.General.DamageReaction.Graphics;
using ProjectScripts.General.DamageReaction.Knockback;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ProjectScripts.General.DamageReaction
{
    [UpdateInGroup(typeof(CustomInteractionSystemGroup))]
    public partial struct DamageReactionSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (damageReactionBuffer, transform, entity) in
                     SystemAPI.Query<DynamicBuffer<General.DamageReaction.DamageReaction>, RefRO<LocalTransform>>().WithEntityAccess())
            {
                if (damageReactionBuffer.IsEmpty)
                {
                    continue;
                }

                var currentPosition = transform.ValueRO.Position;

                var totalDirectionForce = float3.zero;
                foreach (var damageThisFrame in damageReactionBuffer)
                {
                    var directionForce = currentPosition - damageThisFrame.FromPosition;
                    totalDirectionForce += directionForce;
                }

                totalDirectionForce = math.normalizesafe(totalDirectionForce);

                damageReactionBuffer.Clear();

                if (SystemAPI.HasComponent<FlashColorOnDamageData>(entity))
                {
                    SystemAPI.SetComponentEnabled<FlashColorOnDamageData>(entity, true);
                    var flashColorOnDamageTimer = SystemAPI.GetComponentRW<FlashColorOnDamageTimer>(entity);
                    var flashTime = SystemAPI.GetComponent<FlashColorOnDamageData>(entity).FlashTime;
                    flashColorOnDamageTimer.ValueRW.Value = flashTime;
                }

                if (SystemAPI.HasComponent<KnockbackState>(entity))
                {
                    SystemAPI.SetComponentEnabled<KnockbackState>(entity, true);
                    var knockbackState = SystemAPI.GetComponentRW<KnockbackState>(entity);
                    var knockbackData = SystemAPI.GetComponentRO<KnockbackData>(entity);
                    knockbackState.ValueRW.LeftTime = knockbackData.ValueRO.Duration;
                    knockbackState.ValueRW.Direction = totalDirectionForce;
                }
            }
        }
    }
}