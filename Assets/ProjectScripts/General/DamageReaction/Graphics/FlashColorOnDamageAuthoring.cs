using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using UnityEngine;

namespace ProjectScripts.General.DamageReaction.Graphics
{
    public struct FlashColorOnDamageData : IComponentData, IEnableableComponent
    {
        public float4 Color;
        public float FlashTime;
    }

    public struct FlashColorOnDamageTimer : IComponentData
    {
        public float Value;
    }

    public class FlashColorOnDamageAuthoring : MonoBehaviour
    {
        public Color Color;
        public float FlashTime;

        private class Baker : Baker<FlashColorOnDamageAuthoring>
        {
            public override void Bake(FlashColorOnDamageAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Renderable);
                AddComponent(entity, new FlashColorOnDamageData
                {
                    Color = (Vector4)authoring.Color,
                    FlashTime = authoring.FlashTime
                });
                SetComponentEnabled<FlashColorOnDamageData>(entity, false);
                AddComponent<FlashColorOnDamageTimer>(entity);
                AddComponent(entity, new URPMaterialPropertyBaseColor { Value = new float4(1) });
            }
        }
    }
}