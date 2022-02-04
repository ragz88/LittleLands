using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    [System.Serializable]
    public struct GridPosition
    {
        public Transform trans;
        public BiomeBlock biomeBlock;
    }

    [SerializeField]
    public GridPosition[,] gridPositions = new GridPosition[3,3];


    [SerializeField]
    float blockPositionPadding;

    [SerializeField]
    GameObject gridPositionTransformPrefab;

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
        for (int i = 0; i < 3; i ++)
        {
            for (int j = 0; j < 3; j++)
            {
                GameObject tempTrans = Instantiate(gridPositionTransformPrefab,
                    transform.position + new Vector3(i * blockPositionPadding, 0, j * blockPositionPadding), 
                    Quaternion.identity) as GameObject;

                gridPositions[i, j].trans = tempTrans.transform;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
