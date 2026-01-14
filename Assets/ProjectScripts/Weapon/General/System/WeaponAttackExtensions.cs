using Unity.Collections;
using Unity.Mathematics;
using Unity.Physics;

namespace ProjectScripts.Weapon.General.System
{
    public static class WeaponAttackExtensions
    {
        public static float3 ClosestHitPosition(float3 position, ref NativeList<DistanceHit> distanceHits)
        {
            var closestHitPosition = float3.zero;
            var maxDistanceSq = float.MaxValue;

            foreach (var distanceHit in distanceHits)
            {
                var hitPosition = distanceHit.Position;
                var distanceSq = math.distancesq(position.xz, hitPosition.xz);

                if (distanceSq < maxDistanceSq)
                {
                    closestHitPosition = hitPosition;
                    maxDistanceSq = distanceSq;
                }
            }

            return closestHitPosition;
        }

        public static float3 SpawnPosition(float3 fromPosition)
        {
            return fromPosition + math.up();
        }

        public static quaternion SpawnRotation(float3 fromPosition, float3 toPosition)
        {
            var direction = toPosition.xz - fromPosition.xz;
            var angle = math.atan2(direction.x, direction.y);
            return quaternion.Euler(0f, angle, 0f);
        }
    }
}