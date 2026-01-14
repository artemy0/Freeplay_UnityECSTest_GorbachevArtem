using ProjectScripts.General.Destroy;
using ProjectScripts.General.Movement.Forward;
using Unity.Entities;
using UnityEngine;

namespace ProjectScripts.Weapon.Bullet
{
    public struct BulletTag : IComponentData
    {
    }

    public struct BulletData : IComponentData
    {
        public int AttackDamage;
    }

    [RequireComponent(typeof(DestructibleEntityAuthoring))]
    [RequireComponent(typeof(ForwardMoveAuthoring))]
    public class BulletEntityAuthoring : MonoBehaviour
    {
        public int AttackDamage;

        private class Baker : Baker<BulletEntityAuthoring>
        {
            public override void Bake(BulletEntityAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<BulletTag>(entity);
                AddComponent(entity, new BulletData
                {
                    AttackDamage = authoring.AttackDamage,
                });
            }
        }
    }
}