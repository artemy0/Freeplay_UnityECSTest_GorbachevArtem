using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace ProjectScripts.General.DamageReaction
{
    public struct DamageReaction : IBufferElementData
    {
        public float3 FromPosition;
    }

    public class DamageReactionAuthoring : MonoBehaviour
    {
        private class Baker : Baker<DamageReactionAuthoring>
        {
            public override void Bake(DamageReactionAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddBuffer<DamageReaction>(entity);
            }
        }
    }
}