using Unity.Entities;
using UnityEngine;

namespace ProjectScripts.General.Destroy
{
    public struct DestroyEntityAfterTime : IComponentData
    {
        public float Value;
    }

    [RequireComponent(typeof(DestructibleEntityAuthoring))]
    public class DestroyAfterTimeAuthoring : MonoBehaviour
    {
        public float DestroyAfterTime;

        private class Baker : Baker<DestroyAfterTimeAuthoring>
        {
            public override void Bake(DestroyAfterTimeAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new DestroyEntityAfterTime { Value = authoring.DestroyAfterTime });
            }
        }
    }
}