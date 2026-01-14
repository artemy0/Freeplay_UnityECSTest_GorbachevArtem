using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace ProjectScripts.General.Movement.Arc
{
    public struct ArcMoveData : IComponentData
    {
        public float Duration;
        public float Height;
    }

    public struct ArcMoveState : IComponentData
    {
        public float Elapsed;
        public float3 Start;
        public float3 End;
    }

    public class ArcMoveAuthoring : MonoBehaviour
    {
        public float Duration;
        public float Height;

        private class Baker : Baker<ArcMoveAuthoring>
        {
            public override void Bake(ArcMoveAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<ArcMoveState>(entity);
                AddComponent(entity, new ArcMoveData
                {
                    Duration = authoring.Duration,
                    Height = authoring.Height
                });
            }
        }
    }
}