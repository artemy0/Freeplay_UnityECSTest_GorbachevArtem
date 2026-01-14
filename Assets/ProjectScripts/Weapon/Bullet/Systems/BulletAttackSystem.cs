using ProjectScripts.Character.Enemy;
using ProjectScripts.Character.General.Damage;
using ProjectScripts.General.DamageReaction;
using ProjectScripts.General.Destroy;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;

namespace ProjectScripts.Weapon.Bullet
{
    [UpdateInGroup(typeof(CustomAttackSystemGroup))]
    public partial struct BulletAttackSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<SimulationSingleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var attackJob = new BulletAttackJob
            {
                BulletLookup = SystemAPI.GetComponentLookup<BulletData>(true),
                EnemyLookup = SystemAPI.GetComponentLookup<EnemyTag>(true),
                TransformLookup = SystemAPI.GetComponentLookup<LocalTransform>(true),
                DamageBufferLookup = SystemAPI.GetBufferLookup<DamageThisFrame>(),
                DamageReactionLookup = SystemAPI.GetBufferLookup<DamageReaction>(),
                DestroyLookup = SystemAPI.GetComponentLookup<DestroyEntityFlag>(),
            };

            var simulationSystem = SystemAPI.GetSingleton<SimulationSingleton>();
            state.Dependency = attackJob.Schedule(simulationSystem, state.Dependency);
        }
    }

    [BurstCompile]
    public struct BulletAttackJob : ITriggerEventsJob
    {
        [ReadOnly] public ComponentLookup<BulletData> BulletLookup;
        [ReadOnly] public ComponentLookup<EnemyTag> EnemyLookup;
        [ReadOnly] public ComponentLookup<LocalTransform> TransformLookup;

        public BufferLookup<DamageThisFrame> DamageBufferLookup;
        public BufferLookup<DamageReaction> DamageReactionLookup;
        public ComponentLookup<DestroyEntityFlag> DestroyLookup;

        public void Execute(TriggerEvent triggerEvent)
        {
            Entity bulletEntity;
            Entity enemyEntity;

            if (BulletLookup.HasComponent(triggerEvent.EntityA) && EnemyLookup.HasComponent(triggerEvent.EntityB))
            {
                bulletEntity = triggerEvent.EntityA;
                enemyEntity = triggerEvent.EntityB;
            }
            else if (BulletLookup.HasComponent(triggerEvent.EntityB) && EnemyLookup.HasComponent(triggerEvent.EntityA))
            {
                bulletEntity = triggerEvent.EntityB;
                enemyEntity = triggerEvent.EntityA;
            }
            else
            {
                return;
            }

            var attackDamage = BulletLookup[bulletEntity].AttackDamage;
            var enemyDamageBuffer = DamageBufferLookup[enemyEntity];
            enemyDamageBuffer.Add(new DamageThisFrame
            {
                Value = attackDamage
            });

            var attackPosition = TransformLookup[bulletEntity].Position;
            var enemyDamageReactionBuffer = DamageReactionLookup[enemyEntity];
            enemyDamageReactionBuffer.Add(new DamageReaction
            {
                FromPosition = attackPosition,
            });

            DestroyLookup.SetComponentEnabled(bulletEntity, true);
        }
    }
}