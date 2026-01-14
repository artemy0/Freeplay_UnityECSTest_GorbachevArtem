using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace ProjectScripts.Character.General
{
    public struct CharacterMoveDirection : IComponentData
    {
        public float2 Value;
    }

    public struct CharacterMoveSpeed : IComponentData
    {
        public float Value;
    }

    public class CharacterAuthoring : MonoBehaviour
    {
        public float MoveSpeed;

        private class Baker : Baker<CharacterAuthoring>
        {
            public override void Bake(CharacterAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new CharacterMoveDirection
                {
                    Value = float2.zero
                });
                AddComponent(entity, new CharacterMoveSpeed
                {
                    Value = authoring.MoveSpeed
                });
            }
        }
    }
}