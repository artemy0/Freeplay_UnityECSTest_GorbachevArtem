using Unity.Entities;
using UnityEngine;

namespace ProjectScripts.General.Spawn
{
    public struct SpawnerCurrentEntityCount : IComponentData
    {
        public int Value;
    }

    public class SpawnerAuthoring : MonoBehaviour
    {
        private class Baker : Baker<SpawnerAuthoring>
        {
            public override void Bake(SpawnerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new SpawnerCurrentEntityCount
                {
                    Value = 0
                });
            }
        }
    }
}