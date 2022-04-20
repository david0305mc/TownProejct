using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundManager : MonoBehaviour
{
    public static GroundManager instance;

    public const int nodeWidth = 44;
    public const int nodeHeight = 44;

    public int[,] instanceNodes;

    private void Awake()
    {
        instance = this;
    }


    public void UpdateAllNodes()
    {
        this.instanceNodes = new int[nodeWidth, nodeHeight];

        for (int x = 0; x < nodeWidth; x++)
        {
            for (int z = 0; z < nodeHeight; z++)
            {
                this.instanceNodes[x, z] = -1;
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
