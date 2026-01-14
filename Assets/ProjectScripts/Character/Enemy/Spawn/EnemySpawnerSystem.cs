using ProjectScripts.Character.Player;
using ProjectScripts.General.Spawn;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ProjectScripts.Character.Enemy.Spawn
{
    [UpdateInGroup(typeof(CustomInitializationSystemGroup))]
    public partial struct EnemySpawnerSystem : ISystem
    {
        // private EntityQuery enemyQuery;
        
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PlayerTag>();
            state.RequireForUpdate<BeginInitializationEntityCommandBufferSystem.Singleton>();
            // enemyQuery = SystemAPI.QueryBuilder().WithAll<EnemyTag>().Build();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var deltaTime = SystemAPI.Time.DeltaTime;
            // var enemyCount = enemyQuery.CalculateEntityCount(); // Slower, but simple solution!

            var ecbSystem = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSystem.CreateCommandBuffer(state.WorldUnmanaged);

            var playerEntity = SystemAPI.GetSingletonEntity<PlayerTag>();
            var playerPosition = SystemAPI.GetComponent<LocalTransform>(playerEntity).Position;

            foreach (var (spawnState, spawnData, spawnCount, spawnEntity) in
                     SystemAPI.Query<RefRW<EnemySpawnState>, RefRO<EnemySpawnData>, RefRW<SpawnerCurrentEntityCount>>().WithEntityAccess())
            {
                spawnState.ValueRW.NextSpawnTime -= deltaTime;
                
                if (spawnState.ValueRO.NextSpawnTime > 0 ||
                    spawnCount.ValueRO.Value >= spawnData.ValueRO.EnemyLimit)
                {
                    continue;
                }

                spawnState.ValueRW.NextSpawnTime = spawnData.ValueRO.SpawnCooldown;
                spawnCount.ValueRW.Value++;

                var enemy = ecb.Instantiate(spawnData.ValueRO.EnemyPrefab);
                var spawnAngle = spawnState.ValueRW.Random.NextFloat(0f, math.TAU);
                var spawnPoint = new float3
                {
                    x = math.sin(spawnAngle),
                    y = 0f,
                    z = math.cos(spawnAngle),
                };
                spawnPoint *= spawnData.ValueRO.SpawnOffset + spawnState.ValueRW.Random.NextFloat(0f, spawnData.ValueRO.SpawnRange);
                spawnPoint += playerPosition;

                ecb.SetComponent(enemy, LocalTransform.FromPosition(spawnPoint));
                ecb.SetComponent(enemy, new SpawnerLink
                {
                    SpawnerEntity = spawnEntity
                });
            }
        }
    }
}