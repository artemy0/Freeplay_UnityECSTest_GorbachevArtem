using ProjectScripts.General.Destroy;
using Unity.Entities;
using Unity.Mathematics;

namespace ProjectScripts.Character.General.Damage
{
    [UpdateInGroup(typeof(CustomInteractionSystemGroup))]
    public partial struct ApplyDamageThisFrameSystem : ISystem
    {
        public void OnUpdate(ref SystemState system)
        {
            foreach (var (damageBuffer, currentHitPoints, maxHitPoints, entity) in
                     SystemAPI.Query<DynamicBuffer<DamageThisFrame>, RefRW<CurrentHitPoints>, RefRO<MaxHitPoints>>().WithEntityAccess())
            {
                if (damageBuffer.IsEmpty)
                {
                    continue;
                }

                var totalDamage = 0;
                foreach (var damageThisFrame in damageBuffer)
                {
                    var damage = damageThisFrame.Value;
                    totalDamage += damage;
                }

                damageBuffer.Clear();

                currentHitPoints.ValueRW.Value -= totalDamage;
                currentHitPoints.ValueRW.Value = math.clamp(currentHitPoints.ValueRW.Value, 0, maxHitPoints.ValueRO.Value);

                if (currentHitPoints.ValueRO.Value <= 0)
                {
                    SystemAPI.SetComponentEnabled<DestroyEntityFlag>(entity, true);
                }
            }
        }
    }
}