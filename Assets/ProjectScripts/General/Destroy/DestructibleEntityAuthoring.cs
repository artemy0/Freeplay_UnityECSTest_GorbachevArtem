using Unity.Entities;
using UnityEngine;

namespace ProjectScripts.General.Destroy
{
    public struct DestroyEntityFlag : IComponentData, IEnableableComponent
    {
    }

    public class DestructibleEntityAuthoring : MonoBehaviour
    {
        private class Baker : Baker<DestructibleEntityAuthoring>
        {
            public override void Bake(DestructibleEntityAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<DestroyEntityFlag>(entity);
                SetComponentEnabled<DestroyEntityFlag>(entity, false);
            }
        }
    }
}