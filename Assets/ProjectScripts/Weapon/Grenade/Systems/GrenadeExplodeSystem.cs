using ProjectScripts.Character.General.Damage;
using ProjectScripts.General.DamageReaction;
using ProjectScripts.General.Destroy;
using ProjectScripts.Weapon.Granade;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace ProjectScripts.Weapon.Grenade.Systems
{
    [UpdateInGroup(typeof(CustomAttackSystemGroup))]
    public partial struct GrenadeExplodeSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PhysicsWorldSingleton>();
            state.RequireForUpdate<BeginInitializationEntityCommandBufferSystem.Singleton>();
        }
        
        public void OnUpdate(ref SystemState state)
        {
            var ecbSystem = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSystem.CreateCommandBuffer(state.WorldUnmanaged);

            var physicsWorldSingleton = SystemAPI.GetSingleton<PhysicsWorldSingleton>();

            foreach (var (moveState, moveData, grenadeData, entity) in
                     SystemAPI.Query<RefRO<ArcMoveState>, RefRO<ArcMoveData>, GrenadeData>().WithAll<GrenadeTag>().WithEntityAccess())
            {
                if (moveState.ValueRO.Elapsed >= moveData.ValueRO.Duration)
                {
                    SystemAPI.SetComponentEnabled<DestroyEntityFlag>(entity, true);

                    var explosion = ecb.Instantiate(grenadeData.Explosion);
                    ecb.SetComponent(explosion, LocalTransform.FromPositionRotationScale(
                        new float3(moveState.ValueRO.End.x, 0f, moveState.ValueRO.End.z), quaternion.identity,
                        grenadeData.AttackRange));

                    // TODO Explode
                    var overlapHits = new NativeList<DistanceHit>(state.WorldUpdateAllocator);
                    if (!physicsWorldSingleton.OverlapSphere(
                            moveState.ValueRO.End, grenadeData.AttackRange, ref overlapHits, grenadeData.Filter))
                    {
                        continue;
                    }

                    foreach (var hitInfo in overlapHits)
                    {
                        var damageBuffer = SystemAPI.GetBuffer<DamageThisFrame>(hitInfo.Entity);
                        damageBuffer.Add(new DamageThisFrame
                        {
                            Value = grenadeData.AttackDamage
                        });
                        var damageReactionBuffer = SystemAPI.GetBuffer<DamageReaction>(hitInfo.Entity);
                        damageReactionBuffer.Add(new DamageReaction
                        {
                            FromPosition = moveState.ValueRO.End
                        });
                    }
                }
            }
        }
    }
}