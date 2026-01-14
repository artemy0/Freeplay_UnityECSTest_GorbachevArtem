using ProjectScripts.Character.General;
using Unity.Entities;
using UnityEngine;

namespace ProjectScripts.Character.Player
{
    public struct PlayerTag : IComponentData
    {
    }

    [RequireComponent(typeof(CharacterAuthoring))]
    public class PlayerAuthoring : MonoBehaviour
    {
        private class Baker : Baker<PlayerAuthoring>
        {
            public override void Bake(PlayerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<PlayerTag>(entity);
            }
        }
    }
}