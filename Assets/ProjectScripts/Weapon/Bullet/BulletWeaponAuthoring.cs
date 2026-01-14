using ProjectScripts.Weapon.General;
using Unity.Entities;
using UnityEngine;

namespace ProjectScripts.Weapon.Bullet
{
    public struct BulletWeaponTag : IComponentData
    {
    }

    [RequireComponent(typeof(WeaponAuthoring))]
    public class BulletWeaponAuthoring : MonoBehaviour
    {
        private class Baker : Baker<BulletWeaponAuthoring>
        {
            public override void Bake(BulletWeaponAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<BulletWeaponTag>(entity);
            }
        }
    }
}