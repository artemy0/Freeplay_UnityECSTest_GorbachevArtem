using ProjectScripts.Character.Enemy.Spawn;
using ProjectScripts.Character.General;
using ProjectScripts.General.Spawn;
using Unity.Entities;
using UnityEngine;

namespace ProjectScripts.Character.Enemy
{
    public struct EnemyTag : IComponentData
    {
    }

    public struct EnemyAttackData : IComponentData
    {
        public int Damage;
        public float Rate;
    }

    public struct EnemyNextAttackTime : IComponentData
    {
        public float Value;
    }

    [RequireComponent(typeof(CharacterAuthoring))]
    [RequireComponent(typeof(SpawnEntityAuthoring))]
    public class EnemyAuthoring : MonoBehaviour
    {
        public int AttackDamage;
        public float AttackRate;

        private class Baker : Baker<EnemyAuthoring>
        {
            public override void Bake(EnemyAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<EnemyTag>(entity);
                AddComponent(entity, new EnemyAttackData
                {
                    Damage = authoring.AttackDamage,
                    Rate = authoring.AttackRate,
                });
                AddComponent(entity, new EnemyNextAttackTime
                {
                    Value = 0f
                });
            }
        }
    }
}