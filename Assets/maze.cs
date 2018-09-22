using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    public abstract class Maze
    {
        internal int[,] _map;
        internal Vector2 _originPos;
        internal int _hight;
        internal int _width;

        public int[,] map
        {
            get
            {
                return _map;
            }
        }

        public Vector2 originPos
        {
            get
            {
                return _originPos;
            }
        }

        public int height
        {
            get
            {
                return _hight;
            }
        }

        public int width
        {
            get
            {
                return _width;
            }
        }
        public abstract void GenerateMaze(int width, int height, Vector2 originPos);
    }

    public class MazeBlock
    {
        public int dir,x,y;
        //1 - up
        //2 - down
        //3 - left
        //4 - right
        public MazeBlock(int x, int y, int direction)
        {
            this.x = x;
            this.y = y;
            dir = direction;
        }
    }
  public class PRIMEmaze : Maze
    {
        public override void GenerateMaze(int width, int height, Vector2 originPos)
        {
            _map = new int[width, height];
            _width = width;
            _hight = height;
            _originPos = originPos;
            for(int i=0;i<height;++i)
            {
                for(int j=0;j<width;++j)
                {
                    _map[i, j] = 0;
                }
            }
            var targetX = (int)originPos.x;
            var targetY = (int)originPos.y;
            map[targetX, targetY] = 1;
            var WallList = new List<MazeBlock>();
            if(targetX>1)
            {
                var block = new MazeBlock(targetX - 1, targetY, 1);
                WallList.Add(block);
            }
            if(targetY<width-2)
            {
                var block = new MazeBlock(targetX, targetY + 1, 4);
                WallList.Add(block);
            }
            if(targetX<height-2)
            {
                var block = new MazeBlock(targetX + 1, targetY, 2);
                WallList.Add(block);
            }
            if(targetY>1)
            {
                var block = new MazeBlock(targetX, targetY - 1, 3);
                WallList.Add(block);
            }
            while(WallList.Count>0)
            {
                long tick = System.DateTime.Now.Ticks;
                System.Random ran = new System.Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));
                int blockidx = ran.Next(0, WallList.Count);
                switch(WallList[blockidx].dir)
                {
                    case 1:
                        targetX = WallList[blockidx].x - 1;
                        targetY = WallList[blockidx].y;
                        break;
                    case 2:
                        targetX = WallList[blockidx].x + 1;
                        targetY = WallList[blockidx].y;
                        break;
                    case 3:
                        targetX = WallList[blockidx].x;
                        targetY = WallList[blockidx].y - 1;
                        break;
                    case 4:
                        targetX = WallList[blockidx].x;
                        targetY = WallList[blockidx].y + 1;
                        break;

                }
                if(_map[targetX,targetY]==0)
                {
                    _map[WallList[blockidx].x, WallList[blockidx].y] = 1;
                    _map[targetX, targetY] = 1;
                    if(WallList[blockidx].dir!=2&&
                        targetX>1&&_map[targetX-1,targetY]==0&&
                        _map[targetX-2,targetY]==0)
                    {
                        var block = new MazeBlock(targetX - 1, targetY, 1);
                        WallList.Add(block);
                    }
                    if (WallList[blockidx].dir != 3 &&
                       targetY < width - 2 && _map[targetX, targetY + 1] == 0 &&
                       _map[targetX, targetY + 2] == 0)
                    {
                        var block = new MazeBlock(targetX, targetY + 1, 4);
                        WallList.Add(block);
                    }
                    if (WallList[blockidx].dir != 1 &&
                       targetX < height - 2 && _map[targetX + 1, targetY] == 0 &&
                       _map[targetX + 2, targetY] == 0)
                    {
                        var block = new MazeBlock(targetX + 1, targetY, 2);
                        WallList.Add(block);
                    }
                    if (WallList[blockidx].dir != 4 &&
                       targetY > 1 && _map[targetX, targetY - 1] == 0 &&
                       _map[targetX, targetY - 2] == 0)
                    {
                        var block = new MazeBlock(targetX, targetY - 1, 3);
                        WallList.Add(block);
                    }
                }
                WallList.RemoveAt(blockidx);
            }
        }
    }
}
