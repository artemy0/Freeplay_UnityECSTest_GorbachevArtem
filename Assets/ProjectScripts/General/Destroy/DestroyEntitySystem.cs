using ProjectScripts.General.Spawn;
using Unity.Entities;

namespace ProjectScripts.General.Destroy
{
    [UpdateInGroup(typeof(CustomDestructionSystemGroup))]
    public partial struct DestroyEntitySystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
        }

        public void OnUpdate(ref SystemState state)
        {
            var endECBSystem = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            var endECB = endECBSystem.CreateCommandBuffer(state.WorldUnmanaged);

            foreach (var (_, entity) in SystemAPI.Query<DestroyEntityFlag>().WithEntityAccess())
            {
                if (SystemAPI.HasComponent<SpawnerLink>(entity))
                {
                    var spawnerEntity = SystemAPI.GetComponent<SpawnerLink>(entity).SpawnerEntity;
                    var spawnerCurrentEntityCount = SystemAPI.GetComponentRW<SpawnerCurrentEntityCount>(spawnerEntity);

                    spawnerCurrentEntityCount.ValueRW.Value--;
                }
                
                endECB.DestroyEntity(entity);
            }
        }
    }
}