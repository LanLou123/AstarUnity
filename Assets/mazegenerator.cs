using System.Collections;
using System.Collections.Generic;
using Assets;
using UnityEngine;

public class mazegenerator : MonoBehaviour {

    public GameObject cubes;
    public Material nmm;
    public Vector2 bottomleftpos;
	// Use this for initialization
	void Start () {
        bottomleftpos = new Vector2(-10, -15);
        float mazeblockwidth = cubes.GetComponent<Transform>().localScale.x/2;
        common.m1.GenerateMaze(common.mazesize[0], common.mazesize[1], new Vector2(20, 10));
        for (int i = 0; i < common.m1.height;++i)
        {
            for(int j= 0;j< common.m1.width;++j)
            {
                if(common.m1.map[i,j]==0)
                {
                    Instantiate(cubes, new Vector3(2*i+mazeblockwidth+bottomleftpos.x, 0, 2*j+mazeblockwidth+bottomleftpos.y), Quaternion.identity);
                }
            }
        }
        //GameObject ng = Instantiate(cubes, new Vector3(2 * common.m1.width + 5, 0, 2 * common.m1.height + 5), Quaternion.identity) as GameObject;
        //ng.GetComponent<Renderer>().GetComponent<Material>().SetColor("_Color", Color.red);
        for(int i=0;i< common.m1.height;++i)
        {

        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
