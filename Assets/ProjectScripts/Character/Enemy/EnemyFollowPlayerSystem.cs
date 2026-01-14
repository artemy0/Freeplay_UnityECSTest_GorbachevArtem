using ProjectScripts.Character.General;
using ProjectScripts.Character.Player;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ProjectScripts.Character.Enemy
{
    [UpdateInGroup(typeof(CustomTranslationSystemGroup), OrderLast = true)]
    public partial struct EnemyFollowPlayerSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PlayerTag>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var playerEntity = SystemAPI.GetSingletonEntity<PlayerTag>();
            var playerPosition = SystemAPI.GetComponent<LocalTransform>(playerEntity).Position;

            var followPlayerJob = new EnemyFollowPlayerJob
            {
                PlayerPosition = playerPosition
            };
            followPlayerJob.ScheduleParallel();
        }
    }

    [BurstCompile]
    [WithAll(typeof(EnemyTag))]
    public partial struct EnemyFollowPlayerJob : IJobEntity
    {
        public float3 PlayerPosition;

        [BurstCompile]
        private void Execute(ref CharacterMoveDirection direction, in LocalTransform transform)
        {
            var curMoveDirection = PlayerPosition.xz - transform.Position.xz;
            curMoveDirection = math.normalize(curMoveDirection);
            direction.Value = curMoveDirection;
        }
    }
}