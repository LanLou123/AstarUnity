# AstarUnity

## An Astar pathfinding realization in Unity 

- **Four directions** (faster)
![](https://github.com/LanLou123/AstarUnity/raw/master/pathfinding.gif)

- **Eight directions**
![](https://github.com/LanLou123/AstarUnity/raw/master/pathfinding1.gif)


## Features:
- Prime method random maze generation
- A* pathfinding
- Heap optimization
  - instead of storing the Astarnode instances inside a list as openlist, I created a Heap class specifically for Astar nodes, therefore reduce the time complexity of both adding and deleting elements based on the value of F cost for each node, which has been reduced from ```O(n)``` to ```O(log2n)```.
  - for actual perfomance, when doing pathfinding for a 120X120 grid the total time for each finding execution changed from 15ms to 1ms, so this is a really huge improvement. 
