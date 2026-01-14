using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ProjectScripts.General.Movement.Arc
{
    public partial struct ArcMoveSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var dt = SystemAPI.Time.DeltaTime;
            
            foreach (var (transform, stateMove, dataMove) in
                     SystemAPI.Query<RefRW<LocalTransform>, RefRW<ArcMoveState>, RefRO<ArcMoveData>>())
            {
                stateMove.ValueRW.Elapsed += dt;

                float progress = math.saturate(stateMove.ValueRO.Elapsed / dataMove.ValueRO.Duration);

                float3 pos = math.lerp(
                    stateMove.ValueRO.Start,
                    stateMove.ValueRO.End,
                    progress);

                // TODO Create more complex trajectory:
                float height = (1f - math.pow(2f * progress - 1f, 2f)) * dataMove.ValueRO.Height;
                pos.y += height;

                transform.ValueRW.Position = pos;
            }
        }
    }
}