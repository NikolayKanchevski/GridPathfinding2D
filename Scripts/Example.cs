using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ultimate.Algorithms.AStar;
using UnityEngine;

public class Example : MonoBehaviour {

    public int startNodeIndex = 0; // The index of the start node in the grid.nodes list
    public int endNodeIndex = 1; // The index of the end node in the grid.nodes list
    public float nodeUnwalkablePercentange = 20f; // Chance of a node being unwalkable

    [Space]

    public Vector2Int size; // Size of the grid - X means width and Y means height

    Vector2Int originPosition;
    List<Vector3> path = new List<Vector3>();
    AStarGrid grid;

    public void Generate()
    {
        path = new List<Vector3>(); // We first define an empty list where we will store the path as a list of Vector3 points

        originPosition = new Vector2Int(-size.x / 2, -size.y / 2); // Then we calculate the origin position (bottom-left corner) so that the grid is always centered (you can change that)

        List<Node> nodes = new List<Node>(); // Since the grid works with nodes we need an empty list for them as well
        for (int y = originPosition.y; y < originPosition.y + size.y; y++)
        {
            for (int x = originPosition.x; x < originPosition.x + size.x; x++)
            {
                // For each point in the grid we create a new node with the X and Y coordinates
                nodes.Add(new Node(1f, x, y)); // 1f means that it is walkable and 0f - that is is not walkable
            }
        }

        // For each of the already defined nodes...
        for (int i; i < nodes; i++) 
        {
            // If the current node is the starting nor the end node we return!
            if (i == startNodeIndex || i == endNodeIndex) continue;

            //...else based on the unwalkable chance we set its walkable state
            bool walkable = Random.Range(0f, 100f) <= nodeUnwalkablePercentange;
            if (unwalkable) node.SetPenalty(0f); // SetPenalty(1f) == walkable and SetPenalty(0f) means unwalkable
        }


        grid = new AStarGrid(new Vector2Int(-size.x / 2, -size.y / 2), size, nodes);
    }

    public void FindPath()
    {
        // Simply instantiate a new AStarPathfinding class...
        AStarPathfinding pathfinding = new AStarPathfinding();

        //... and call the Pathding() method by specifying your grid start and end node
        path = pathfinding.Pathfind(grid, startNodeIndex, endNodeIndex);
    }

    // This method is just to visualize the nodes
    private void OnDrawGizmosSelected()
    {
        // If the grid is null or has no nodes we return as there won't be anything to visualize
        if (grid == null || grid.nodes.Count == 0) return;

        // Otherwise we loop through all the nodes...
        for (int i = 0; i < grid.nodes.Count; i++)
        {
            //...if it is the start node - paint it in red
            if (i == startNodeIndex)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawCube(new Vector3(grid.nodes[i].X + 0.5f, grid.nodes[i].Y + 0.5f, 0f), new Vector3(1f, 1f, 0f));
                continue;
            }
            //... if it is the end node - paint it in blue
            else if (i == endNodeIndex)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawCube(new Vector3(grid.nodes[i].X + 0.5f, grid.nodes[i].Y + 0.5f, 0f), new Vector3(1f, 1f, 0f));
                continue;
            }

            //... if it is an ordinary node that is walkable - paint it in green
            if (grid.nodes[i].Walkable)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawCube(new Vector3(grid.nodes[i].X + 0.5f, grid.nodes[i].Y + 0.5f, 0f), new Vector3(0.9f, 0.9f, 0f));
                continue;
            }
            //...if it is an ordinary node paint and is not walkable - paint it in black
            else
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(new Vector3(grid.nodes[i].X + 0.5f, grid.nodes[i].Y + 0.5f, 0f), new Vector3(0.9f, 0.9f, 0f));
                continue;
            }
        }


        if (path != null && path.Count > 0)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(new Vector3(grid.nodes[startNodeIndex].X, grid.nodes[startNodeIndex].Y, 0), path[0]);

            for (int i = 0; i < path.Count - 1; i++)
            {
                var p1 = new Vector3(path[i].x + 0.5f, path[i].y + 0.5f, 0);
                var p2 = new Vector3(path[i + 1].x + 0.5f, path[i + 1].y + 0.5f, 0);

                Gizmos.DrawLine(p1, p2);
            }
        }

    }
}