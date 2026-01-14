using Unity.Entities;
using UnityEngine;

namespace ProjectScripts.Character.General.Damage
{
    public struct MaxHitPoints : IComponentData
    {
        public int Value;
    }

    public struct CurrentHitPoints : IComponentData
    {
        public int Value;
    }

    public struct DamageThisFrame : IBufferElementData
    {
        public int Value;
    }

    public class DamageableAuthoring : MonoBehaviour
    {
        public int HitPoints;

        private class Baker : Baker<DamageableAuthoring>
        {
            public override void Bake(DamageableAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new CurrentHitPoints
                {
                    Value = authoring.HitPoints
                });
                AddComponent(entity, new MaxHitPoints
                {
                    Value = authoring.HitPoints
                });
                AddBuffer<DamageThisFrame>(entity);
            }
        }
    }
}