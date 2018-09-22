using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets;
using System.Diagnostics;

public class AstarPF : MonoBehaviour {

    Grid grid;
    public Transform StartPos, EndPos;
	// Use this for initialization


	void Start () {
		
	}
	    void Awake()
    {
        grid = GetComponent<Grid>();
    }
    void AstarFindPath(Vector3 _startPos,Vector3 _endPos)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();
        AstarNode Startnode = grid.NodeFromWorldPosition(_startPos);
        AstarNode Endnode = grid.NodeFromWorldPosition(_endPos);

        //List<AstarNode> OpenLst = new List<AstarNode>();
        Assets.Heap < AstarNode> OpenLst = new Assets.Heap<AstarNode>(grid.MaxSize);
        HashSet<AstarNode> ClosedLst = new HashSet<AstarNode>();

        OpenLst.add(Startnode);
        while(OpenLst.Count>0)
        {
            //AstarNode CurNode = OpenLst[0];
            //for(int i = 1;i<OpenLst.Count;++i)
            //{
            //    if(OpenLst[i].Fval<=CurNode.Fval&&
            //        OpenLst[i].Hval<CurNode.Hval)
            //    {
            //        CurNode = OpenLst[i];
            //    }
            //}
            //OpenLst.Remove(CurNode);
            AstarNode CurNode = OpenLst.RemoveFirst();
            ClosedLst.Add(CurNode);
            if(CurNode == Endnode)
            {
                sw.Stop();
                print("path found in " + sw.ElapsedMilliseconds + "ms");
                GetFinalPath(Startnode, Endnode);
                return;
            }
            foreach(AstarNode Neighbor in grid.GetNeighboringNodes(CurNode))
            {
                if (!Neighbor.wall || ClosedLst.Contains(Neighbor))
                    continue;
                int movecost = CurNode.Gval + GetNeighborDis(CurNode, Neighbor);/*GetManhattenDis(CurNode, Neighbor)*/;
                if(movecost<Neighbor.Gval||!OpenLst.Contains(Neighbor))
                {
                    Neighbor.Gval = movecost;
                    Neighbor.Hval = GetManhattenDis(Neighbor, Endnode);
                    Neighbor.parent = CurNode;
                    if (!OpenLst.Contains(Neighbor))
                        OpenLst.add(Neighbor);
                    else
                        OpenLst.UpdateItem(Neighbor);
                }
            }
        }
    }

    int GetManhattenDis(AstarNode _nodeA,AstarNode _nodeB)
    {
        int x = (int)Mathf.Abs(_nodeA.NodeGridPos.x - _nodeB.NodeGridPos.x);
        int y = (int)Mathf.Abs(_nodeA.NodeGridPos.y - _nodeB.NodeGridPos.y);
        return 10*(x + y);
    }

    int GetNeighborDis(AstarNode _nodeA,AstarNode _nodeB)
    {
        float x = Vector2.Distance(_nodeA.NodeGridPos, _nodeB.NodeGridPos);
        x *= 10;
        return (int)x; 
    }

    void GetFinalPath(AstarNode _startnode, AstarNode _endnode)
    {
        List<AstarNode> finalpath = new List<AstarNode>();
        AstarNode curnode = _endnode;
        while(curnode!=_startnode)
        {
            finalpath.Add(curnode);
            curnode = curnode.parent;
        }
        finalpath.Reverse();
        grid.Fpath = finalpath;
    }
	// Update is called once per frame
	void Update () {
        AstarFindPath(StartPos.position, EndPos.position);
	}
}
