using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets;

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
        AstarNode Startnode = grid.NodeFromWorldPosition(_startPos);
        AstarNode Endnode = grid.NodeFromWorldPosition(_endPos);

        List<AstarNode> OpenLst = new List<AstarNode>();
        HashSet<AstarNode> ClosedLst = new HashSet<AstarNode>();

        OpenLst.Add(Startnode);
        while(OpenLst.Count>0)
        {
            AstarNode CurNode = OpenLst[0];
            for(int i = 1;i<OpenLst.Count;++i)
            {
                if(OpenLst[i].Fval<=CurNode.Fval&&
                    OpenLst[i].Hval<CurNode.Hval)
                {
                    CurNode = OpenLst[i];
                }
            }
            OpenLst.Remove(CurNode);
            ClosedLst.Add(CurNode);
            if(CurNode == Endnode)
            {
                GetFinalPath(Startnode, Endnode);
                break;
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
                        OpenLst.Add(Neighbor);
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
