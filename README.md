<h1 align="center">Grid Pathfinding 2D</h1>
<h6 align="center">By: <a href="https://nikichatv.com/Website/N-Studios.html">N-Studios</a></h6>

<br>
<p align="">A <stron>free-to-use</strong> open source project. Built using the <string>A* pathfinding algorithm</string>, it is incredibly easy to set up a grid-based pathfinding in a Unity 2D workspace. You can have a look at or build on top of the scripts inside /Scripts folder. A setup tutorial is listed bellow and the whole Examplecs.cs is described with comments on almost each line. Feel free to use in <strong>any kind of projects</strong>. Credit is not required but is very appreciated.</p>

<br>
<h3>Setup:</h3>
<p>At the very top of your code make sure to reference the AStar namespace by doing:</p>
```
using Ultimate.Algorithms.AStar;
```
<br>
<p>Add some variables to make the tweaking of the behaviour of the pathfinding easy. Here's an example:</p>
```
public int startNodeIndex = 0;     // The index of the start node in the grid.nodes list
public int endNodeIndex = 1;     // The index of the end node in the grid.nodes list
public float nodeUnwalkablePercentange = 20f;     // Chance of a node being unwalkable
[Space]
public Vector2Int size;     // Size of the grid - X means width and Y means height
```
<br>
<p>Those are the values that can be changed from the user. However, we still need a few more, three to be precise. These are going to be the ones that are not accessible from out of the script and will store tha pathfinder info:</p>
```
Vector2Int originPosition;     // This represents the bottom-left corner node of our grid
List<Vector3> path = new List<Vector3>();     // This empty Vector3 list is goint to store the path generated after the pathfinding is done
AStarGrid grid;     // A local grid that is then being passed to the pathfinder
```
