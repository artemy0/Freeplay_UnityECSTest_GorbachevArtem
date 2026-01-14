using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace ProjectScripts.General.DamageReaction.Knockback
{
    public struct KnockbackState : IComponentData, IEnableableComponent
    {
        public float LeftTime;
        public float3 Direction;
    }

    public struct KnockbackData : IComponentData
    {
        public float Duration;
        public float Strangth;
    }

    public class KnockbackAuthoring : MonoBehaviour
    {
        public float Duration;
        public float Strangth;

        private class Baker : Baker<KnockbackAuthoring>
        {
            public override void Bake(KnockbackAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new KnockbackData
                {
                    Duration = authoring.Duration,
                    Strangth = authoring.Strangth
                });
                AddComponent<KnockbackState>(entity);
                SetComponentEnabled<KnockbackState>(entity, false);
            }
        }
    }
}