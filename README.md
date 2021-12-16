<h1 align="center">Grid Pathfinding 2D</h1>
<h6 align="center">By: <a href="https://nikichatv.com/Website/N-Studios.html">N-Studios</a></h6>

<br>
<p align="">A <strong>free-to-use</strong> open source project. Built using the <strong>A* pathfinding algorithm</strong>, on C#, it makes it incredibly easy to set up grid-based pathfinding in a Unity 2D workspace. You can have a look at or build on top of the scripts inside the /Scripts folder. A setup tutorial is listed below and the whole Examples.cs is described with comments on almost every line. Feel free to use in <strong>any kind of projects</strong>. Credit is not required but is appreciated.</p>

<br>

##  ⚙️  Setup

<p>At the very top of your code make sure to reference the AStar namespace by doing:</p>

```c#
using Ultimate.Algorithms.AStar;
```

<br>
<p>Add some variables to make the tweaking of the behavior of the pathfinding easy. Here's an example:</p>

```c#
public int startNodeIndex = 0; // The index of the start node in the grid.nodes list
public int endNodeIndex = 1; // The index of the end node in the grid.nodes list
public float nodeUnwalkablePercentange = 20f;     // Chance of a node being unwalkable
public Vector2Int size; // Size of the grid - X means width and Y means height
```

<br>
<p>Those are the values that can be changed from the user. However, we still need a few more, three to be precise. They are going to be the ones that are not accessible from out of the script and will store the pathfinder info:</p>

```c#
Vector2Int originPosition; // This represents the bottom-left corner node of our grid
AStarGrid grid; // A local grid that is then being passed to the pathfinder
List<Vector3> path = new List<Vector3>(); // This empty Vector3 list is goint to store the path generated after the pathfinding is done
```

<br>
<p>Now, time to generate the nodes:</p>

```c#
public void Generate()
{
    path = new List<Vector3>(); // We first define an empty list where we will store the path as a list of Vector3 points

    originPosition = new Vector2Int(-size.x / 2, -size.y / 2); // Then we calculate the origin position so that the grid is always centered (you can change that)

    List<Node> nodes = new List<Node>(); // Since the grid works with nodes we need an empty list for them as well
    for (int y = originPosition.y; y < originPosition.y + size.y; y++)
    {
        for (int x = originPosition.x; x < originPosition.x + size.x; x++)
        {
            // For each point in the grid we create a new node with the X and Y coordinates
            nodes.Add(new Node(1f, x, y)); // 1f means that it is walkable and 0f - that is is not walkable
        }
    }
}
```

<br>
<p>For now, we only have walkable points. What about the "obstacles"? Well, this code randomly generates unwalkable nodes (using the nodeUnwalkablePercentage):</p>

```c#
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
}
```

<br>
<p>Finally, create the grid:</p>

```c#
grid = new AStarGrid(new Vector2Int(-size.x / 2, -size.y / 2), size, nodes);
```
<br>
<p>Now that we have the full grid, it is time to find the shortest path, if there is one, right? Here's how you do it:</p>

```c#
public void FindPath()
{
    // Simply instantiate a new AStarPathfinding class...
    AStarPathfinding pathfinding = new AStarPathfinding();

    //... and call the Pathding() method by specifying your grid start and end node
    path = pathfinding.Pathfind(grid, startNodeIndex, endNodeIndex);
}
```

<br>
<p>Congratulations, you've now got a pathfinding code! <br> Just one last thing - we all love data, don't we? Why not visualize all of this? No worries, we got it covered:</p>

```c#
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
```

<br>
<p>So that's it, just keep in mind that the visualization part only works in "Editor" mode and NOT "Play" mode. You can still create a code that does that in "Play" mode - the process should be very similar.</p>

##  🤝  Support
Contributions, issues, and feature requests are welcome!

Give a  ⭐️  if you like this project!
<br>
You can also check us out on the <a href="https://assetstore.unity.com/publishers/51773">Unity Asset Store</a>.
