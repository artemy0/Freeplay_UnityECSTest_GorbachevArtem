using Unity.Entities;
using UnityEngine;

namespace ProjectScripts.Camera
{
    public struct InitializeCameraTargetTag : IComponentData
    {
    }

    public struct CameraTarget : IComponentData
    {
        public UnityObjectRef<Transform> Value;
    }

    public class CameraTargetAuthoring : MonoBehaviour
    {
        private class Baker : Baker<CameraTargetAuthoring>
        {
            public override void Bake(CameraTargetAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<InitializeCameraTargetTag>(entity);
            }
        }
    }
}