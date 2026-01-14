using ProjectScripts.Weapon.General;
using Unity.Entities;
using UnityEngine;

namespace ProjectScripts.Weapon.Grenade
{
    public struct GrenadeWeaponTag : IComponentData
    {
    }

    [RequireComponent(typeof(WeaponAuthoring))]
    public class GrenadeWeaponAuthoring : MonoBehaviour
    {
        private class Baker : Baker<GrenadeWeaponAuthoring>
        {
            public override void Bake(GrenadeWeaponAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<GrenadeWeaponTag>(entity);
            }
        }
    }
}