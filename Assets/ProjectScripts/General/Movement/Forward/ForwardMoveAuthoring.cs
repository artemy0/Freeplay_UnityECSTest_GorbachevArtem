using Unity.Entities;
using UnityEngine;

namespace ProjectScripts.General.Movement.Forward
{
    public struct ForwardMoveData : IComponentData
    {
        public float MoveSpeed;
    }

    public class ForwardMoveAuthoring : MonoBehaviour
    {
        public float MoveSpeed;

        private class Baker : Baker<ForwardMoveAuthoring>
        {
            public override void Bake(ForwardMoveAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new ForwardMoveData
                {
                    MoveSpeed = authoring.MoveSpeed
                });
            }
        }
    }
}