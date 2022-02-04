using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public struct GridPosition
    {
        public Transform trans;
        public BiomeBlock biomeBlock;
    }

    public GridPosition[,] gridPositions = new GridPosition[3,3];


    public static GridController gridInstance;

    private void Awake()
    {
        if (gridInstance == null)
        {
            gridInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
