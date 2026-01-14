using ProjectScripts.General.Destroy;
using ProjectScripts.Weapon.Granade;
using Unity.Entities;
using Unity.Physics;
using UnityEngine;

namespace ProjectScripts.Weapon.Grenade
{
    public struct GrenadeTag : IComponentData
    {
    }

    public struct GrenadeData : IComponentData
    {
        public CollisionFilter Filter;
        public Entity Explosion;

        public float AttackRange;
        public int AttackDamage;
    }
    
    [RequireComponent(typeof(DestructibleEntityAuthoring))]
    [RequireComponent(typeof(ArcMoveAuthoring))]
    public class GrenadeEntityAuthoring : MonoBehaviour
    {
        public LayerMask AttackLayer;
        public GameObject ExplostionPrefab;
        public float AttackRange;
        public int AttackDamage;

        private class Baker : Baker<GrenadeEntityAuthoring>
        {
            public override void Bake(GrenadeEntityAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                
                var belongsToLayerMask = 1u << authoring.gameObject.layer;
                var collidesWithLayerMask = (uint)authoring.AttackLayer.value;

                var collisionFilter = new CollisionFilter
                {
                    BelongsTo = belongsToLayerMask,
                    CollidesWith = collidesWithLayerMask
                };

                AddComponent<GrenadeTag>(entity);
                AddComponent(entity, new GrenadeData
                {
                    Filter = collisionFilter,
                    Explosion = GetEntity(authoring.ExplostionPrefab, TransformUsageFlags.Dynamic),

                    AttackRange = authoring.AttackRange,
                    AttackDamage = authoring.AttackDamage,
                });
            }
        }
    }
}