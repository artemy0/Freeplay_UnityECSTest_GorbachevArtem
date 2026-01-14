using Unity.Burst;
using Unity.Entities;

namespace ProjectScripts.General.Destroy
{
    [UpdateInGroup(typeof(CustomDestructionSystemGroup))]
    public partial struct DestroyAfterTimeSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var deltaTime = SystemAPI.Time.DeltaTime;

            foreach (var (destroyAfterTime, entity) in SystemAPI.Query<RefRW<DestroyEntityAfterTime>>()
                         .WithEntityAccess())
            {
                destroyAfterTime.ValueRW.Value -= deltaTime;
                if (destroyAfterTime.ValueRO.Value > 0f)
                {
                    continue;
                }

                SystemAPI.SetComponentEnabled<DestroyEntityFlag>(entity, true);
            }
        }
    }
}