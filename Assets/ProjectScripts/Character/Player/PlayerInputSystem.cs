using ProjectScripts.Character.General;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace ProjectScripts.Character.Player
{
    [UpdateInGroup(typeof(InitializationSystemGroup), OrderLast = true)]
    public partial class PlayerInputSystem : SystemBase
    {
        private ProjectInput input;

        protected override void OnCreate()
        {
            RequireForUpdate<PlayerTag>();

            input = new ProjectInput();
        }

        protected override void OnStartRunning()
        {
            input.Enable();
        }

        protected override void OnStopRunning()
        {
            input.Disable();
        }

        protected override void OnUpdate()
        {
            var curMovement = (float2)input.Player.Move.ReadValue<Vector2>();

            foreach (var moveDirection in
                     SystemAPI.Query<RefRW<CharacterMoveDirection>>().WithAll<PlayerTag>())
            {
                moveDirection.ValueRW.Value = curMovement;
            }
        }
    }
}