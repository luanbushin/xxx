


using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    public Node[,] grid = new Node[150, 150];
    public List<Node> path = new List<Node>();

    public Grid() {
        for (int i = 0; i < 150; i++)
        {
            for (int j = 0; j < 150; j++)
            {
                grid[i, j] = new Node(0, new Vector3(i,1.5f,j), i, j);
            }
        }
    }

    public Node GetFromPos(Vector3 pos)
    {
        /**float percentX = (pos.x + gridSize.x * 0.5f) / gridSize.x;
        float percentY = (pos.z + gridSize.y * 0.5f) / gridSize.y;

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((150 - 1) * percentX);
        int y = Mathf.RoundToInt((150 - 1) * percentY);
    */
        int x = (int)Mathf.Round(pos.x);
        int z = (int)Mathf.Round(pos.z);

        return grid[x, z];
    }

    public List<Node> GetNeibourhood(Node node)
    {
        List<Node> neibourhood = new List<Node>();
        //相邻上下左右格子
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0)
                {
                    continue;
                }
                if (Mathf.Abs(j) == 1 && Mathf.Abs(i) == 1) { 
                    continue;
                }
                int tempX = node._gridX + i;
                int tempY = node._gridY + j;

                if (tempX < 150 && tempX > 0 && tempY > 0 && tempY < 150)
                {
                    neibourhood.Add(grid[tempX, tempY]);
                }
            }
        }
        return neibourhood;
    }

    private void CreateGrid()
    {
    }
}
