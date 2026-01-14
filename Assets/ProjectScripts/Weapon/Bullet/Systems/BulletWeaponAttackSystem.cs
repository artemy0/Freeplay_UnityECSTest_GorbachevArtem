using ProjectScripts.Character.Player;
using ProjectScripts.Weapon.General;
using ProjectScripts.Weapon.General.System;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;

namespace ProjectScripts.Weapon.Bullet
{
    [UpdateInGroup(typeof(CustomAttackSystemGroup))]
    [UpdateAfter(typeof(WeaponAttackSystem))]
    public partial struct BulletWeaponAttackSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PlayerTag>();
            state.RequireForUpdate<PhysicsWorldSingleton>();
            state.RequireForUpdate<BeginInitializationEntityCommandBufferSystem.Singleton>();
        }

        public void OnUpdate(ref SystemState state)
        {
            var ecbSystem = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSystem.CreateCommandBuffer(state.WorldUnmanaged);

            foreach (var (attackEvents, attackPrefab) in
                     SystemAPI.Query<DynamicBuffer<AttackEvent>, RefRO<WeaponAttackPrefabData>>().WithAll<BulletWeaponTag>())
            {
                if (attackEvents.IsEmpty)
                {
                    continue;
                }

                foreach (var attackEvent in attackEvents)
                {
                    var attackEntity = ecb.Instantiate(attackPrefab.ValueRO.AttackEntity);

                    var attackSpawnPosition = WeaponAttackExtensions.SpawnPosition(attackEvent.Origin);
                    var attackSpawnRotation = WeaponAttackExtensions.SpawnRotation(attackEvent.Origin, attackEvent.Target);
                    var attackSpawnScale = attackPrefab.ValueRO.AttackScale;

                    ecb.SetComponent(attackEntity, LocalTransform.FromPositionRotationScale(attackSpawnPosition, attackSpawnRotation, attackSpawnScale));
                }

                attackEvents.Clear();
            }
        }
    }
}