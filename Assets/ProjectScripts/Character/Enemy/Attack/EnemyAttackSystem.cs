using ProjectScripts.Character.General.Damage;
using ProjectScripts.Character.Player;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;

namespace ProjectScripts.Character.Enemy.Attack
{
    [UpdateInGroup(typeof(CustomPhysicsSystemGroup))]
    public partial struct EnemyAttackSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<SimulationSingleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var enemyAttackJob = new EnemyAttackJob
            {
                PlayerLookup = SystemAPI.GetComponentLookup<PlayerTag>(true),
                EnemyLookup = SystemAPI.GetComponentLookup<EnemyTag>(true),
                EnemyAttackDataLookup = SystemAPI.GetComponentLookup<EnemyAttackData>(true),
                
                EnemyNextAttackTimeLookup = SystemAPI.GetComponentLookup<EnemyNextAttackTime>(),
                DamageThisFrameLookup = SystemAPI.GetBufferLookup<DamageThisFrame>(),
                ElapsedTime = SystemAPI.Time.ElapsedTime
            };

            var simulationSingleton = SystemAPI.GetSingleton<SimulationSingleton>();
            state.Dependency = enemyAttackJob.Schedule(simulationSingleton, state.Dependency);
        }
    }

    [BurstCompile]
    public struct EnemyAttackJob : ICollisionEventsJob
    {
        [ReadOnly] public ComponentLookup<PlayerTag> PlayerLookup;
        [ReadOnly] public ComponentLookup<EnemyTag> EnemyLookup;
        [ReadOnly] public ComponentLookup<EnemyAttackData> EnemyAttackDataLookup;

        public ComponentLookup<EnemyNextAttackTime> EnemyNextAttackTimeLookup;
        public BufferLookup<DamageThisFrame> DamageThisFrameLookup;

        public double ElapsedTime;

        [BurstCompile]
        public void Execute(CollisionEvent collisionEvent)
        {
            Entity playerEntity;
            Entity enemyEntity;

            if (PlayerLookup.HasComponent(collisionEvent.EntityA) &&
                EnemyLookup.HasComponent(collisionEvent.EntityB))
            {
                playerEntity = collisionEvent.EntityA;
                enemyEntity = collisionEvent.EntityB;
            }
            else if (PlayerLookup.HasComponent(collisionEvent.EntityB) &&
                     EnemyLookup.HasComponent(collisionEvent.EntityA))
            {
                playerEntity = collisionEvent.EntityB;
                enemyEntity = collisionEvent.EntityA;
            }
            else
            {
                return;
            }

            var enemyNextAttackTime = EnemyNextAttackTimeLookup[enemyEntity];
            if (enemyNextAttackTime.Value > ElapsedTime)
            {
                return;
            }

            var enemyAttackData = EnemyAttackDataLookup[enemyEntity];
            
            enemyNextAttackTime.Value = (float)ElapsedTime + enemyAttackData.Rate;
            EnemyNextAttackTimeLookup[enemyEntity] = enemyNextAttackTime;

            var damageBuffer = DamageThisFrameLookup[playerEntity];
            damageBuffer.Add(new DamageThisFrame
            {
                Value = enemyAttackData.Damage
            });
        }
    }
}