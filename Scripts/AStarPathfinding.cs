using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Ultimate.Algorithms.AStar
{
    public class AStarPathfinding
    {
        public List<Vector3> Pathfind(AStarGrid grid, int startNodeIndex, int endNodeIndex)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            List<Node> allNodes = grid.nodes;
            int startX = allNodes[startNodeIndex].X;
            int startY = allNodes[startNodeIndex].Y;

            int endX = allNodes[endNodeIndex].X;
            int endY = allNodes[endNodeIndex].Y;

            Node startNode = allNodes.First(o => o.X == startX && o.Y == startY);
            Node endNode = allNodes.First(o => o.X == endX && o.Y == endY);

            List<Node> openSet = new List<Node>();
            HashSet<Node> closedSet = new HashSet<Node>();

            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Node currentNode = openSet[0];

                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                    {
                        currentNode = openSet[i];
                    }
                }

                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                Debug.Log($"{currentNode.X}; {currentNode.Y} || {endNode.X}; {endNode.Y}");
                if (currentNode.X == endNode.X && currentNode.Y == endNode.Y)
                {
                    Debug.Log($"Done pathfinding in: {stopwatch.ElapsedMilliseconds}ms!");
                    stopwatch.Stop();

                    return RetracePath(startNode, endNode);
                }

                foreach (Node neighbour in GetNeighbours(allNodes.ToList(), currentNode, grid))
                {
                    if (!neighbour.Walkable || closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour) * (int)(10.0f * neighbour.penalty);
                    if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, endNode);
                        neighbour.parent = currentNode;

                        if (!openSet.Contains(neighbour))
                        {
                            openSet.Add(neighbour);
                        }
                    }
                }
            }

            stopwatch.Stop();
            Debug.Log("No path found!");
            return null;
        }

        private static List<Vector3> RetracePath(Node startNode, Node endNode)
        {
            List<Vector3> path = new List<Vector3>();
            Node currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(new Vector3(currentNode.X, currentNode.Y, 0));
                currentNode = currentNode.parent;
            }
            path.Add(new Vector3(startNode.X, startNode.Y, 0));
            
            path.Reverse();
            return path;
        }

        private static int GetDistance(Node nodeA, Node nodeB)
        {
            int dstX = Mathf.Abs(nodeA.X - nodeB.X);
            int dstY = Mathf.Abs(nodeA.Y - nodeB.Y);

            if (dstX > dstY)
                return 14 * dstY + 10 * (dstX - dstY);
            return 14 * dstX + 10 * (dstY - dstX);
        }

        private List<Node> GetNeighbours(List<Node> allNodes, Node node, AStarGrid grid)
        {
            List<Node> neighbours = new List<Node>();
            int[] indexes = new int[]
            {
                (int)Mathf.Abs(grid.originPosition.x - node.X + 1) + (int)Mathf.Abs(grid.originPosition.y - node.Y) * grid.width,
                (int)Mathf.Abs(grid.originPosition.x - node.X - 1) + (int)Mathf.Abs(grid.originPosition.y - node.Y) * grid.width,
                (int)Mathf.Abs(grid.originPosition.x - node.X) + (int)Mathf.Abs(grid.originPosition.y - node.Y + 1) * grid.width,
                (int)Mathf.Abs(grid.originPosition.x - node.X) + (int)Mathf.Abs(grid.originPosition.y - node.Y - 1) * grid.width
            };

            foreach (var index in indexes)
            {
                if (index < 0 || index >= allNodes.Count) continue;

                Node currentNode = allNodes[index];
                if (currentNode.Walkable)
                {
                    neighbours.Add(currentNode);
                }
            }

            return neighbours;
        }
    }

    public class AStarGrid
    {
        public Vector2 originPosition;
        public Vector2 size;

        public int width, height;
        public List<Node> nodes;

        public AStarGrid(Vector2Int pos, Vector2Int size, List<Node> nodes)
        {
            this.originPosition = pos;
            this.size = size;

            this.width = size.x;
            this.height = size.y;

            this.nodes = nodes;
        }

        public Node GetObject(int x, int y)
        {
            return nodes[(int)Mathf.Abs(originPosition.x - x) + (int)Mathf.Abs(originPosition.y - y) * width];
        }
    }
}