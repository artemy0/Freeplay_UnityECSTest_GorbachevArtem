using Unity.Entities;
using UnityEngine;

namespace ProjectScripts.Camera.Systems
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial struct InitializeCameraTargetSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<InitializeCameraTargetTag>();
        }

        public void OnUpdate(ref SystemState state)
        {
            if (CameraTargetSingleton.Instance == null)
            {
                return;
            }

            var targetTransform = CameraTargetSingleton.Instance.transform;
            var ecb = new EntityCommandBuffer(state.WorldUpdateAllocator);

            foreach (var (_, entity) in SystemAPI.Query<InitializeCameraTargetTag>().WithNone<CameraTarget>().WithEntityAccess())
            {
                ecb.AddComponent(entity, new CameraTarget
                {
                    Value = new UnityObjectRef<Transform>
                    {
                        Value = targetTransform
                    }
                });

                ecb.RemoveComponent<InitializeCameraTargetTag>(entity);
            }

            ecb.Playback(state.EntityManager);
        }
    }
}