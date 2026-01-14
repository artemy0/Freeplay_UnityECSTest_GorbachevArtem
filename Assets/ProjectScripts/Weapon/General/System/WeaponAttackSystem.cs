using ProjectScripts.Character.Player;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;

namespace ProjectScripts.Weapon.General.System
{
    [UpdateInGroup(typeof(CustomAttackSystemGroup))]
    public partial struct WeaponAttackSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PlayerTag>();
            state.RequireForUpdate<PhysicsWorldSingleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var elapsedTime = (float)SystemAPI.Time.ElapsedTime;

            var playerEntity = SystemAPI.GetSingletonEntity<PlayerTag>();
            var playerPosition = SystemAPI.GetComponent<LocalTransform>(playerEntity).Position;

            var physicsWorldSingleton = SystemAPI.GetSingleton<PhysicsWorldSingleton>();

            foreach (var (attackState, attackData, attackBuffer) in
                     SystemAPI.Query<RefRW<WeaponAttackState>, RefRO<WeaponAttackData>, DynamicBuffer<AttackEvent>>())
            {
                if (attackState.ValueRO.NextAttackTime > elapsedTime)
                {
                    continue;
                }
                
                var overlapHits = new NativeList<DistanceHit>(state.WorldUpdateAllocator);
                if (!physicsWorldSingleton.OverlapSphere(playerPosition, attackData.ValueRO.AttackRange, ref overlapHits, attackData.ValueRO.AttackFilter))
                {
                    continue;
                }
                
                var closestHitPosition = WeaponAttackExtensions.ClosestHitPosition(playerPosition, ref overlapHits);

                attackBuffer.Add(new AttackEvent
                {
                    Origin = playerPosition,
                    Target = closestHitPosition
                });

                if (attackState.ValueRO.AttackCount + 1 >= attackData.ValueRO.AttackCount)
                {
                    attackState.ValueRW.NextAttackTime = elapsedTime + attackData.ValueRO.AttackCooldown;
                    attackState.ValueRW.AttackCount = 0;
                }
                else
                {
                    attackState.ValueRW.NextAttackTime = elapsedTime + attackData.ValueRO.AdditionalAttacksCooldown;
                    attackState.ValueRW.AttackCount++;
                }
            }
        }
    }
}