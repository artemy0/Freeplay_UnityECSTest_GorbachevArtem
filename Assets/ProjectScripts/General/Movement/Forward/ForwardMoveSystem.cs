using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace ProjectScripts.General.Movement.Forward
{
    public partial struct ForwardMoveSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var deltaTime = SystemAPI.Time.DeltaTime;
            foreach (var (transform, data) in SystemAPI.Query<RefRW<LocalTransform>, ForwardMoveData>())
            {
                transform.ValueRW.Position += transform.ValueRO.Forward() * data.MoveSpeed * deltaTime;
            }
        }
    }
}