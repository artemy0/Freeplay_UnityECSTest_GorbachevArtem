using Unity.Entities;
using Unity.Transforms;

namespace ProjectScripts.Camera.Systems
{
    [UpdateAfter(typeof(TransformSystemGroup))]
    public partial struct MoveCameraSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (cameraTransform, cameraTarget) in SystemAPI.Query<LocalToWorld, CameraTarget>())
            {
                cameraTarget.Value.Value.position = cameraTransform.Position;
            }
        }
    }
}