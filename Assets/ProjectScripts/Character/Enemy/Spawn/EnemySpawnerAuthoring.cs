using ProjectScripts.General.Spawn;
using Unity.Entities;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace ProjectScripts.Character.Enemy.Spawn
{
    public struct EnemySpawnData : IComponentData
    {
        public Entity EnemyPrefab;
        public int EnemyLimit;

        public float SpawnOffset;
        public float SpawnRange;

        public float SpawnCooldown;
    }

    public struct EnemySpawnState : IComponentData
    {
        public float NextSpawnTime;
        public Random Random;
    }

    [RequireComponent(typeof(SpawnerAuthoring))]
    public class EnemySpawnerAuthoring : MonoBehaviour
    {
        public GameObject EnemyPrefab;
        public int EnemyLimit;

        public float SpawnOffset;
        public float SpawnRange;

        public float SpawnCooldown;
        public uint RandomSeed;

        private class Baker : Baker<EnemySpawnerAuthoring>
        {
            public override void Bake(EnemySpawnerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new EnemySpawnData
                {
                    EnemyPrefab = GetEntity(authoring.EnemyPrefab, TransformUsageFlags.Dynamic),
                    EnemyLimit = authoring.EnemyLimit,
                    SpawnOffset = authoring.SpawnOffset,
                    SpawnRange = authoring.SpawnRange,
                    SpawnCooldown = authoring.SpawnCooldown,
                });
                AddComponent(entity, new EnemySpawnState
                {
                    NextSpawnTime = 0f,
                    Random = Random.CreateFromIndex(authoring.RandomSeed),
                });
            }
        }
    }
}