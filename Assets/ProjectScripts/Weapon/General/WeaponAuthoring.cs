using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;

namespace ProjectScripts.Weapon.General
{
    public struct WeaponAttackData : IComponentData
    {
        public CollisionFilter AttackFilter;
        
        public float AdditionalAttacksCooldown;
        public float AttackCooldown;

        public float AttackRange;
        public float AttackCount;
    }

    public struct WeaponAttackPrefabData : IComponentData
    {
        public Entity AttackEntity;
        public float AttackScale;
    }

    public struct WeaponAttackState : IComponentData
    {
        public float NextAttackTime;
        public int AttackCount;
    }

    public struct AttackEvent : IBufferElementData
    {
        public float3 Origin;
        public float3 Target;
    }

    public class WeaponAuthoring : MonoBehaviour
    {
        public GameObject AttackPrefab;
        public float AttackScale = 1f;

        public LayerMask AttackLayer;
        
        public float AdditionalAttacksCooldown = 0.1f;
        public float AttackCooldown = 1f;

        public float AttackRange = 5f;
        public int AttackCount = 3;

        private class Baker : Baker<WeaponAuthoring>
        {
            public override void Bake(WeaponAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);

                var belongsToLayerMask = 1u << authoring.gameObject.layer;
                var collidesWithLayerMask = (uint)authoring.AttackLayer.value;

                var collisionFilter = new CollisionFilter
                {
                    BelongsTo = belongsToLayerMask,
                    CollidesWith = collidesWithLayerMask
                };

                AddComponent(entity, new WeaponAttackData
                {
                    AttackFilter = collisionFilter,
                    
                    AdditionalAttacksCooldown = authoring.AdditionalAttacksCooldown,
                    AttackCooldown = authoring.AttackCooldown,
                    
                    AttackRange = authoring.AttackRange,
                    AttackCount = authoring.AttackCount
                });
                AddComponent(entity, new WeaponAttackPrefabData
                {
                    AttackEntity = GetEntity(authoring.AttackPrefab, TransformUsageFlags.Dynamic),
                    AttackScale = authoring.AttackScale
                });
                AddComponent(entity, new WeaponAttackState
                {
                    NextAttackTime = 0f
                });
                AddBuffer<AttackEvent>(entity);
            }
        }
    }
}