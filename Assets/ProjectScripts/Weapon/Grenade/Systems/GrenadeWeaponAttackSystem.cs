using ProjectScripts.Character.Player;
using ProjectScripts.General.Movement.Arc;
using ProjectScripts.Weapon.General;
using ProjectScripts.Weapon.General.System;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace ProjectScripts.Weapon.Grenade.Systems
{
    [UpdateInGroup(typeof(CustomAttackSystemGroup))]
    [UpdateAfter(typeof(WeaponAttackSystem))]
    public partial struct GrenadeWeaponAttackSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PlayerTag>();
            state.RequireForUpdate<PhysicsWorldSingleton>();
            state.RequireForUpdate<BeginInitializationEntityCommandBufferSystem.Singleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecbSystem = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSystem.CreateCommandBuffer(state.WorldUnmanaged);

            foreach (var (attackEvents, attackPrefab) in
                     SystemAPI.Query<DynamicBuffer<AttackEvent>, RefRO<WeaponAttackPrefabData>>().WithAll<GrenadeWeaponTag>())
            {
                if (attackEvents.IsEmpty)
                {
                    continue;
                }

                foreach (var attackEvent in attackEvents)
                {
                    var grenadeEntity = ecb.Instantiate(attackPrefab.ValueRO.AttackEntity);

                    var grenadeSpawnPosition = WeaponAttackExtensions.SpawnPosition(attackEvent.Origin);
                    var grenadeSpawnRotation = quaternion.identity;
                    var grenadeSpawnScale = attackPrefab.ValueRO.AttackScale;

                    ecb.SetComponent(grenadeEntity, LocalTransform.FromPositionRotationScale(grenadeSpawnPosition, grenadeSpawnRotation, grenadeSpawnScale));
                    ecb.SetComponent(grenadeEntity, new ArcMoveState
                    {
                        Elapsed = 0f,
                        Start = attackEvent.Origin,
                        End = attackEvent.Target
                    });
                }

                attackEvents.Clear();
            }
        }
    }
}