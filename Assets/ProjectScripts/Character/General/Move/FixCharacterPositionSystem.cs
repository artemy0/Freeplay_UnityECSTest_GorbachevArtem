using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace ProjectScripts.Character.General.Move
{
    [UpdateInGroup(typeof(CustomPhysicsSystemGroup), OrderLast = true)]
    public partial struct FixCharacterPositionSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var transform in
                     SystemAPI.Query<RefRW<LocalTransform>>().WithAll<CharacterMoveDirection, CharacterMoveSpeed>())
            {
                transform.ValueRW.Position.y = 0f;
            }
        }
    }
}