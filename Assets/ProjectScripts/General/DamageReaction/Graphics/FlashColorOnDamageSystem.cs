using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;

namespace ProjectScripts.General.DamageReaction.Graphics
{
    [UpdateInGroup(typeof(CustomEffectsSystemGroup))]
    public partial struct FlashColorOnDamageSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var deltaTime = SystemAPI.Time.DeltaTime;
            
            foreach (var (timer, baseColorProperty, flashColorOnDamage, shouldFlash) in
                     SystemAPI.Query<RefRW<FlashColorOnDamageTimer>, RefRW<URPMaterialPropertyBaseColor>, FlashColorOnDamageData, EnabledRefRW<FlashColorOnDamageData>>())
            {
                timer.ValueRW.Value -= deltaTime;
                if (timer.ValueRO.Value > 0f)
                {
                    baseColorProperty.ValueRW.Value = flashColorOnDamage.Color;
                }
                else
                {
                    baseColorProperty.ValueRW.Value = new float4(1);
                    shouldFlash.ValueRW = false;
                }
            }
        }
    }
}