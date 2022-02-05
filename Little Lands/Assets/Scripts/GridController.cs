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


    [SerializeField]
    protected float blockPositionPadding;

    [SerializeField]
    protected GameObject gridPositionTransformPrefab;


    [SerializeField]
    protected LayerMask populatingRayLayerMask;


    public static GridController gridInstance;

    /*private void Awake()
    {
        if (gridInstance == null)
        {
            gridInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }*/

    // Start is called before the first frame update
    void Awake()
    {
        if (gridInstance == null)
        {
            gridInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }


        for (int i = 0; i < 3; i ++)
        {
            for (int j = 0; j < 3; j++)
            {
                GameObject tempTrans = Instantiate(gridPositionTransformPrefab,
                    transform.position + new Vector3(i * blockPositionPadding, 0, j * blockPositionPadding), 
                    Quaternion.identity) as GameObject;

                gridPositions[i, j].trans = tempTrans.transform;

                RaycastHit rayHit;

                if (Physics.Raycast(tempTrans.transform.position + new Vector3(0, 2, 0), Vector3.down, out rayHit, 3, populatingRayLayerMask))
                {
                    gridPositions[i, j].biomeBlock = rayHit.collider.gameObject.GetComponent<BiomeBlock>();
                    gridPositions[i, j].biomeBlock.myGridPosX = i;
                    gridPositions[i, j].biomeBlock.myGridPosX = j;
                }

                // print("Pos " + i + ", " + j + " contains:   " + gridPositions[i, j].biomeBlock);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
