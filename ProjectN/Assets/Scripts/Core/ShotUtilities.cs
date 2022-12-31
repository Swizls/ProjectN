using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ShotUtilites 
{
    public static class ShotUtilities
    {
        public static bool ObstacleCheckForShot(Vector3 startPosFloat, Vector3 targetPosFloat, Tilemap tilemap)
        {
            var trajectory = GetShotTrajectory(startPosFloat, targetPosFloat, tilemap);
            IObject[] objects = GetObjectsOnTrajectory(trajectory);

            foreach (Vector3Int point in trajectory)
            {
                RuleBaseTile tile = tilemap.GetTile<RuleBaseTile>(point);
                if (!tile.isPassable)
                    return false;
            }
            foreach(IObject obj in objects)
            {
                if (!obj.CanShootThrough)
                    return false;
            }
            return true;
        }

        public static Vector3Int[] GetShotTrajectory(Vector3 startPos, Vector3 targetPos, Tilemap tilemap)
        {
            List<Vector3Int> pointsList = new();
            Vector3Int targetPosInt = tilemap.WorldToCell(targetPos);
            Vector3Int currentPointInt = tilemap.WorldToCell(startPos);

            pointsList.Add(currentPointInt);

            Vector3 direction = (targetPos - startPos).normalized;
            Vector3 nextpoint = currentPointInt;

            while (currentPointInt != targetPosInt)
            {
                if (currentPointInt.x != targetPosInt.x)
                    nextpoint.x += direction.x;
                if (currentPointInt.y != targetPosInt.y)
                    nextpoint.y += direction.y;

                currentPointInt = tilemap.WorldToCell(nextpoint);
                pointsList.Add(currentPointInt);
            }
            return pointsList.ToArray();
        }
        
        public static IObject[] GetObjectsOnTrajectory(Vector3Int[] trajectory)
        {
            List<IObject> _allObjects = new();
            for (int i = 2; i < trajectory.Length; i++)
            {
                Collider2D collider = Physics2D.OverlapPoint(new Vector2(trajectory[i].x, trajectory[i].y));
                if (collider != null && collider.TryGetComponent(out IObject obj))
                    _allObjects.Add(obj);
            }
            return _allObjects.ToArray();
        }

        public static Cover[] GetCoversOnTrajectory(Vector3Int[] trajectory)
        {
            List<Cover> _allCovers = new();
            for(int i = 2; i < trajectory.Length; i++)
            {
                Collider2D collider = Physics2D.OverlapPoint(new Vector2(trajectory[i].x, trajectory[i].y));
                if (collider != null && collider.TryGetComponent(out Cover cover))
                    _allCovers.Add(cover);
            }
            return _allCovers.ToArray();
        }
    }
}