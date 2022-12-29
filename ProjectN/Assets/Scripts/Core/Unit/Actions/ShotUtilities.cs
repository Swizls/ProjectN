using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ShotUtilites 
{
    public static class ShotUtilities
    {
        public static bool ObstacleCheckForShot(Vector3 startPosFloat, Vector3 targetPosFloat, Tilemap tilemap)
        {
            foreach (Vector3Int point in GetShotTrajectory(startPosFloat, targetPosFloat, tilemap))
            {
                RuleBaseTile tile = tilemap.GetTile<RuleBaseTile>(point);
                if (!tile.isPassable)
                    return false;
            }
            return true;
        }

        public static List<Vector3Int> GetShotTrajectory(Vector3 startPos, Vector3 targetPos, Tilemap tilemap)
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
            return pointsList;
        }
        public static Cover[] GetObstaclesOnTrajectory(List<Vector3Int> trajectory)
        {
            List<Cover> _allObstacles = new();
            for(int i = 2; i < trajectory.Count; i++)
            {
                Collider2D collider = Physics2D.OverlapPoint(new Vector2(trajectory[i].x, trajectory[i].y));
                if (collider != null && collider.TryGetComponent(out Cover obstacle))
                {
                    _allObstacles.Add(obstacle);
                }
            }
            return _allObstacles.ToArray();

        }
    }
}