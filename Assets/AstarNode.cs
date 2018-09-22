using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    class AstarNode : IHeapItem<AstarNode>
    {
        public AstarNode(bool iswall, int x, int y, Vector3 position)
        {
            NodeGridPos = new Vector2(x, y);
            this.position = position;
            parent = null;
            Gval = 0;
            Hval = 0;
            wall = iswall;
        }
        public Vector3 position;
        public bool wall;
        public Vector2 NodeGridPos;
        public AstarNode parent;
        public int Gval, Hval;
        int heapIdx;
        public int Fval { get { return Gval + Hval; } }
        public int HeapIdx
        {
            get
            {
                return heapIdx;
            }
            set
            {
                heapIdx = value;
            }
        }
        public int CompareTo(AstarNode _tocomp)
        {
            int compare = Fval.CompareTo(_tocomp.Fval);
            if(compare==0)
            {
                compare = Hval.CompareTo(_tocomp.Hval);
            }
            return -compare;
        }
    }
}
