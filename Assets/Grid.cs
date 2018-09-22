using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets;


[Serializable]
class Grid : MonoBehaviour
{
    public Transform StartPos;
    public Vector2 gridSizeWorld;
    public LayerMask wallmask;
    public float nodeRad,nodedis;

    public AstarNode[,] grid;
    public List<AstarNode> Fpath;

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    public float padding;

    public float waittime;
    public float timer;
    bool created = false;

    void Create()
    {
        grid = new AstarNode[gridSizeX, gridSizeY];
        Vector3 leftbottom = transform.position - Vector3.right * gridSizeWorld.x / 2 - Vector3.forward * gridSizeWorld.y / 2;
        for(int x = 0;x<gridSizeX;++x)
        {
            for(int y = 0;y<gridSizeY;++y)
            {
                Vector3 worldPT = leftbottom + Vector3.right * (x * nodeDiameter + nodeRad) + Vector3.forward * (y * nodeDiameter + nodeRad);
                bool wall = true;
                float halfextent = nodeRad-padding;
                if (Physics.CheckBox(worldPT,new Vector3(halfextent, halfextent, halfextent),Quaternion.identity,wallmask)/*CheckSphere(worldPT, nodeRad, wallmask)*/)
                    wall = false;
                grid[x, y] = new AstarNode(wall, x, y, worldPT);
            }
        }
    }

    public AstarNode NodeFromWorldPosition(Vector3 _WorldPos)
    {
        float X = ((_WorldPos.x + gridSizeWorld.x / 2) / gridSizeWorld.x);
        float Y = ((_WorldPos.z + gridSizeWorld.y / 2) / gridSizeWorld.y);
        X = Mathf.Clamp01(X);
        Y = Mathf.Clamp01(Y);
        int x = Mathf.RoundToInt((gridSizeX - 1) * X);
        int y = Mathf.RoundToInt((gridSizeY - 1) * Y);
        return grid[x, y];
    }

    public List<AstarNode> GetNeighboringNodes(AstarNode _AstarNode)
    {
        List<AstarNode> Neighbor = new List<AstarNode>();

        int xCheck, yCheck;

        //right
        xCheck = (int)_AstarNode.NodeGridPos.x + 1;
        yCheck = (int)_AstarNode.NodeGridPos.y;
        if (xCheck >= 0 && xCheck < gridSizeX
            && yCheck >= 0 && yCheck < gridSizeY)
            Neighbor.Add(grid[xCheck, yCheck]);
        //topright
        xCheck = (int)_AstarNode.NodeGridPos.x + 1;
        yCheck = (int)_AstarNode.NodeGridPos.y - 1;
        if (xCheck >= 0 && xCheck < gridSizeX
            && yCheck >= 0 && yCheck < gridSizeY)
            Neighbor.Add(grid[xCheck, yCheck]);
        //bottomright
        xCheck = (int)_AstarNode.NodeGridPos.x + 1;
        yCheck = (int)_AstarNode.NodeGridPos.y + 1;
        if (xCheck >= 0 && xCheck < gridSizeX
            && yCheck >= 0 && yCheck < gridSizeY)
            Neighbor.Add(grid[xCheck, yCheck]);
        //left
        xCheck = (int)_AstarNode.NodeGridPos.x - 1;
        yCheck = (int)_AstarNode.NodeGridPos.y;
        if (xCheck >= 0 && xCheck < gridSizeX
            && yCheck >= 0 && yCheck < gridSizeY)
            Neighbor.Add(grid[xCheck, yCheck]);
        //topleft
        xCheck = (int)_AstarNode.NodeGridPos.x - 1;
        yCheck = (int)_AstarNode.NodeGridPos.y - 1;
        if (xCheck >= 0 && xCheck < gridSizeX
            && yCheck >= 0 && yCheck < gridSizeY)
            Neighbor.Add(grid[xCheck, yCheck]);
        //bottomleft
        xCheck = (int)_AstarNode.NodeGridPos.x - 1;
        yCheck = (int)_AstarNode.NodeGridPos.y + 1;
        if (xCheck >= 0 && xCheck < gridSizeX
            && yCheck >= 0 && yCheck < gridSizeY)
            Neighbor.Add(grid[xCheck, yCheck]);
        //top
        xCheck = (int)_AstarNode.NodeGridPos.x;
        yCheck = (int)_AstarNode.NodeGridPos.y - 1;
        if (xCheck >= 0 && xCheck < gridSizeX
            && yCheck >= 0 && yCheck < gridSizeY)
            Neighbor.Add(grid[xCheck, yCheck]);
        //bottom
        xCheck = (int)_AstarNode.NodeGridPos.x;
        yCheck = (int)_AstarNode.NodeGridPos.y + 1;
        if (xCheck >= 0 && xCheck < gridSizeX
            && yCheck >= 0 && yCheck < gridSizeY)
            Neighbor.Add(grid[xCheck, yCheck]);
        return Neighbor;
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridSizeWorld.x, 1, gridSizeWorld.y));
        if(grid!=null)
        {
            foreach(AstarNode node in grid)
            {
                if (node.wall) Gizmos.color = Color.white;
                else Gizmos.color = Color.blue;
                if(Fpath!=null)
                {
                    if (Fpath.Contains(node))
                        Gizmos.color = Color.red;
                }
                Gizmos.DrawCube(node.position, Vector3.one * (nodeDiameter - nodedis));
            }
        }
    }
    void Start()
    {
        padding = 0.001f;
        waittime = 1.0f;
        nodeDiameter = nodeRad * 2;
        gridSizeX = Mathf.RoundToInt(gridSizeWorld.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridSizeWorld.y / nodeDiameter);
            
    }
        
    void Update()
    {
        timer += Time.deltaTime;
        if(timer>waittime&&!created)
        {
            Create();
            created = true;
        }
    }
}
