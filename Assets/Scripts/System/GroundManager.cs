using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundManager : MonoBehaviour
{
    public static GroundManager instance;

    public enum Action
    {
        ADD,
        REMOVE
    }

    public class Path
    {
        public Vector3[] nodes;

        public float GetDistanceAlongPath()
        {
            float distance = 0;
            if (nodes == null || nodes.Length <= 1)
            {
                return 0;
            }
            else
            {
                for (int index = 0; index < nodes.Length - 1; index++)
                {
                    distance += Vector3.Distance(nodes[index], nodes[index + 1]);
                }
                return distance;
            }
        }
    }

    public const int nodeWidth = 44;
    public const int nodeHeight = 44;

    private int[,] instanceNodes;
    private bool[,] pathNodesWithoutWall;
    private bool[,] pathNodesWithWall;

    private void Awake()
    {
        instance = this;
    }

    public Path GetPath(Vector3 startPoint, Vector3 endPoint, bool considerWalls)
    {
        Path path = new Path();

        if (endPoint.x < 0 || endPoint.x >= nodeWidth || endPoint.z < 0 || endPoint.z >= nodeHeight)
        {
            Debug.LogError("The target point is out of the grid!");
            return path;
        }
        Vector2 startPointInMap = new Vector2(startPoint.x, startPoint.z);
        Vector2 endPointInMap = new Vector2(endPoint.x, endPoint.z);
        SearchParameters searchParameter = null;
        if (considerWalls)
        {
            searchParameter = new SearchParameters(startPointInMap, endPointInMap, pathNodesWithWall);
        }
        else
        {
            searchParameter = new SearchParameters(startPointInMap, endPointInMap, pathNodesWithoutWall);
        }
        PathFinder pathFinder = new PathFinder(searchParameter);
        List<Vector2> points = pathFinder.FindPath();

        int index = -1;
        List<Vector3> nodes = new List<Vector3>();
        foreach (Vector2 point in points)
        {
            index++;
            Vector3 pointInGround = new Vector3(point.x, 0, point.y);
            nodes.Add(pointInGround);
        }
        path.nodes = nodes.ToArray();
        return path;
    }

    public void UpdateAllNodes()
    {
        this.instanceNodes = new int[nodeWidth, nodeHeight];
        this.pathNodesWithoutWall = new bool[nodeWidth, nodeHeight];
        this.pathNodesWithWall = new bool[nodeWidth, nodeHeight];

        for (int x = 0; x < nodeWidth; x++)
        {
            for (int z = 0; z < nodeHeight; z++)
            {
                this.instanceNodes[x, z] = -1;
                this.pathNodesWithoutWall[x, z] = true;
                this.pathNodesWithWall[x, z] = true;
            }
        }

        foreach (KeyValuePair<int, BaseItemScript> entry in Game.SceneManager.instance.GetItemInstances())
        {
            BaseItemScript item = entry.Value;
            if (!item.itemData.configuration.isCharacter)
            {
                this.UpdateBaseItemNodes(item, Action.ADD);
            }
        }
    }
    public void UpdateBaseItemNodes(BaseItemScript item, Action action)
    {

        Vector3 pos = item.GetPosition();

        int x = (int)(pos.x);
        int z = (int)(pos.z);
        int sizeX = (int)item.GetSize().x;
        int sizeZ = (int)item.GetSize().z;

        for (int indexX = x; indexX < x + sizeX; indexX++)
        {
            for (int indexZ = z; indexZ < z + sizeZ; indexZ++)
            {
                bool isCellWalkable = false;
                if ((sizeX > 2 && indexX == x) || (sizeX > 2 && indexX == x + sizeX - 1) || (sizeZ > 2 && indexZ == z) || (sizeZ > 2 && indexZ == z + sizeZ - 1))
                {
                    //use this for make outer edge walkable for items have size morethan 2x2
                    isCellWalkable = true;
                }

                if (item.itemData.name == "ArmyCamp")
                {
                    //make every cell walkable in army camp
                    isCellWalkable = true;
                }

                if (action == Action.ADD)
                {
                    //adding scene item to nodes, so walkable is false
                    this.instanceNodes[indexX, indexZ] = item.instanceId;

                    if (item.itemData.name == "Wall")
                    {
                        this.pathNodesWithoutWall[indexX, indexZ] = true;
                        this.pathNodesWithWall[indexX, indexZ] = false;
                    }
                    else
                    {
                        this.pathNodesWithoutWall[indexX, indexZ] = isCellWalkable;
                    }

                }
                else if (action == Action.REMOVE)
                {
                    if (this.instanceNodes[indexX, indexZ] == item.instanceId)
                    {
                        this.instanceNodes[indexX, indexZ] = -1;
                        this.pathNodesWithoutWall[indexX, indexZ] = true;
                        this.pathNodesWithWall[indexX, indexZ] = true;
                    }
                }

            }
        }
    }
    public Vector3 GetRandomFreePosition()
    {
        int x = Random.Range(5, nodeWidth - 5);
        int z = Random.Range(5, nodeHeight - 5);

        if (this.instanceNodes[x, z] != -1)
        {
            return GetRandomFreePosition();
        }
        return new Vector3(x, 0, z);
    }

    public Vector3 GetRandomFreePositionForItem(int sizeX, int sizeZ)
    {
        Vector3 randomPosition = new Vector3(Random.Range(0, nodeWidth), 0, Random.Range(0, nodeHeight));

        //if (!IsPositionPlacable(randomPosition, sizeX, sizeZ, -1))
        //{
        //    return GetRandomFreePositionForItem(sizeX, sizeZ);
        //}

        return randomPosition;
    }
}
