using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;
        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }
    //generating level demo
    public int col = 10;
    public int row = 10;
    public GameObject floor;
    public GameObject[] enemies;
    public GameObject pookie;
    //object holder
    private Transform boardHolder;
    private List<Vector3> gridPositions = new List<Vector3>();

    void InitialiseList()
    {
        gridPositions.Clear();
        for(int x = 1; x < col; x++)
        {
            for (int y = 1; y < row; y++)
            {
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    void LevelSetup()
    {
        boardHolder = new GameObject("Level").transform;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
