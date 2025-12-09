using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AStarPathfinding : MonoBehaviour
{
    // Finds a path from Start to Target avoiding "Obstacle" layer
    public List<Vector3> FindPath(Vector3 startPos, Vector3 targetPos)
    {
        // Round positions to nearest integer to act like a grid
        Vector3 startNode = new Vector3(Mathf.Round(startPos.x), 0, Mathf.Round(startPos.z));
        Vector3 targetNode = new Vector3(Mathf.Round(targetPos.x), 0, Mathf.Round(targetPos.z));

        List<Vector3> openList = new List<Vector3> { startNode };
        HashSet<Vector3> closedList = new HashSet<Vector3>();
        Dictionary<Vector3, Vector3> cameFrom = new Dictionary<Vector3, Vector3>();

        int loops = 0;
        while (openList.Count > 0 && loops < 1000) // Safety break
        {
            loops++;
            // Get node closest to target (simple heuristic)
            Vector3 current = openList.OrderBy(n => Vector3.Distance(n, targetNode)).First();

            if (Vector3.Distance(current, targetNode) < 1.1f)
            {
                return RetracePath(cameFrom, startNode, current);
            }

            openList.Remove(current);
            closedList.Add(current);

            // Check neighbors (Up, Down, Left, Right)
            Vector3[] directions = { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };
            
            foreach (Vector3 dir in directions)
            {
                Vector3 neighbor = current + dir;
                
                if (closedList.Contains(neighbor)) continue;

                // Check collision with "Obstacle" layer
                if (Physics.CheckSphere(neighbor, 0.4f, LayerMask.GetMask("Obstacle"))) continue;

                if (!openList.Contains(neighbor))
                {
                    openList.Add(neighbor);
                    cameFrom[neighbor] = current;
                }
            }
        }
        return null; // No path found
    }

    List<Vector3> RetracePath(Dictionary<Vector3, Vector3> cameFrom, Vector3 start, Vector3 end)
    {
        List<Vector3> path = new List<Vector3>();
        Vector3 curr = end;
        while (curr != start)
        {
            path.Add(curr);
            if (!cameFrom.ContainsKey(curr)) break;
            curr = cameFrom[curr];
        }
        path.Reverse();
        return path;
    }
}