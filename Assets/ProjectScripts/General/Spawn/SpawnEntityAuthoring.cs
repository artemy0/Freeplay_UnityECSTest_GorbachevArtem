using Unity.Entities;
using UnityEngine;

namespace ProjectScripts.General.Spawn
{
    public struct SpawnerLink : IComponentData
    {
        public Entity SpawnerEntity;
    }
    
    public class SpawnEntityAuthoring : MonoBehaviour
    {
        private class Baker : Baker<SpawnEntityAuthoring>
        {
            public override void Bake(SpawnEntityAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<SpawnerLink>(entity);
            }
        }
    }
}