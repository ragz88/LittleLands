using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomeBlock : MonoBehaviour
{
    public Biome biome;

    [SerializeField]
    protected float onSelectScaleIncrease = 1.2f;

    [SerializeField]
    protected float onSelectScaleSpeed = 2f;

    [SerializeField]
    protected float onReleaseMoveSpeed = 2f;

    [SerializeField]
    protected float onLegalMoveSpeed = 4f;

    [SerializeField]
    protected float previewMoveSpeed = 2f;

    /// <summary>
    /// How far the player must move a biomeBlock before it's accepted as a move
    /// </summary>
    [SerializeField]
    protected float moveDistanceThreshold = 0.15f;

    /// <summary>
    /// The maximum distance a biome block will move when being wiggled by the player before commiting to a move.
    /// </summary>
    [SerializeField]
    protected float movePreviewDistance = 0.3f;

    // Defines the position of this biome in the 3x3 grid
    [SerializeField]
    public int myGridPosX = -1;
    [SerializeField]
    public int myGridPosY = -1;

    // The grid positions of the biome block the player instructs us to fuse with. -1 when move illegal.
    int moveGridPosX = -1;
    int moveGridPosY = -1;


    Vector3 initialScale;
    bool blockSelected = false;

    /// <summary>
    /// How far the player needs to drag their finger before it's considered movement
    /// </summary>
    float movementSensitivity = -0.1f;

    Vector3 initialMousePos;

    /// <summary>
    /// Set to true when the player has moved the block an adequate amount in a legal direction - triggers the final move and fuse moment
    /// </summary>
    bool midLegalMove = false;
    
    // Start is called before the first frame update
    void Start()
    {
        initialScale = transform.localScale;

        // Implies the initial linking of this biomeBlock and the grid is incomplete
        if (myGridPosX == -1 || myGridPosY == -1)
        {
            for (int i = 0; i < GridController.gridInstance.gridPositions.GetLength(0); i++)
            {
                for (int j = 0; j < GridController.gridInstance.gridPositions.GetLength(1); j++)
                {
                    if (GridController.gridInstance.gridPositions[i, j].biomeBlock == this)
                    {
                        myGridPosX = i;
                        myGridPosY = j;
                    }
                }
            }

            if (myGridPosX == -1 || myGridPosY == -1)
            {
                Debug.LogError("BiomeBlock " + gameObject.name + " not found in grid");
            }
        }
        /*else
        {
            GridController.gridInstance.gridPositions[myGridPosX, myGridPosY].biomeBlock = this;
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        if (blockSelected)
        {
            if (transform.localScale.x < (initialScale * onSelectScaleIncrease).x)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, initialScale * onSelectScaleIncrease, onSelectScaleSpeed * Time.deltaTime);
            }

            Vector3 currentMousePos = Input.mousePosition;

            moveGridPosX = -1;
            moveGridPosY = -1;

            // Check if the mouse has moved far enough to warrant the movement of the block
            if ((initialMousePos - currentMousePos).sqrMagnitude  >  movementSensitivity)
            {
                // We're going to use the angle at which the user's finger has moved - an angle
                // of 0 representing a right swipe on the screen.
                float movementAngle = Vector3.Angle(currentMousePos - initialMousePos, Vector3.right);

                // As this function returns between between 0 and 180, we'll examine the change in Y value to know which
                // quadrant we're in as well.
                if (currentMousePos.y < initialMousePos.y)
                {
                    movementAngle = 360 - movementAngle;
                }

                /*print(movementAngle);
                print("MyX - " + myGridPosX);
                print("MyY - " + myGridPosY);*/

                midLegalMove = true;

                // Here we discern which direction is the most appropriate to move the block in based on this angle
                if (movementAngle >= 0 && movementAngle < 90)
                {
                    // Positive z Movement

                    transform.position = Vector3.Lerp(transform.position, 
                        GridController.gridInstance.gridPositions[myGridPosX, myGridPosY].trans.position + (Vector3.forward * movePreviewDistance), 
                        previewMoveSpeed * Time.deltaTime);

                    // Check if move is even possible. If at top of grid, we cannot move higher.
                    if (myGridPosY == 2)
                    {
                        midLegalMove = false;
                    }
                    else
                    {
                        // Check if there is a block present to fuse with
                        if (GridController.gridInstance.gridPositions[myGridPosX, myGridPosY + 1].biomeBlock == null)
                        {
                            midLegalMove = false;
                        }
                        else
                        {
                            moveGridPosX = myGridPosX;
                            moveGridPosY = myGridPosY + 1;
                        }
                    }
                }
                else if (movementAngle >= 90 && movementAngle < 180)
                {
                    // Negative x Movement
                    
                    transform.position = Vector3.Lerp(transform.position, 
                        GridController.gridInstance.gridPositions[myGridPosX, myGridPosY].trans.position + (-Vector3.right * movePreviewDistance), 
                        previewMoveSpeed * Time.deltaTime);

                    // Check if move is even possible. If at top of grid, we cannot move higher.
                    if (myGridPosX == 0)
                    {
                        midLegalMove = false;
                    }
                    else
                    {
                        // Check if there is a block present to fuse with
                        if (GridController.gridInstance.gridPositions[myGridPosX - 1, myGridPosY].biomeBlock == null)
                        {
                            midLegalMove = false;
                        }
                        else
                        {
                            moveGridPosX = myGridPosX - 1;
                            moveGridPosY = myGridPosY;
                        }
                    }
                }
                else if (movementAngle >= 180 && movementAngle < 270)
                {
                    // Negative z Movement

                    transform.position = Vector3.Lerp(transform.position, 
                        GridController.gridInstance.gridPositions[myGridPosX, myGridPosY].trans.position + (-Vector3.forward * movePreviewDistance), 
                        previewMoveSpeed * Time.deltaTime);

                    // Check if move is even possible. If at top of grid, we cannot move higher.
                    if (myGridPosY == 0)
                    {
                        midLegalMove = false;
                    }
                    else
                    {
                        // Check if there is a block present to fuse with
                        if (GridController.gridInstance.gridPositions[myGridPosX, myGridPosY - 1].biomeBlock == null)
                        {
                            midLegalMove = false;
                        }
                        else
                        {
                            moveGridPosX = myGridPosX;
                            moveGridPosY = myGridPosY - 1;
                        }
                    }
                }
                else if (movementAngle >= 270 && movementAngle < 360)
                {
                    // Positive x Movement

                    transform.position = Vector3.Lerp(transform.position, 
                        GridController.gridInstance.gridPositions[myGridPosX, myGridPosY].trans.position + (Vector3.right * movePreviewDistance), 
                        previewMoveSpeed * Time.deltaTime);

                    // Check if move is even possible. If at top of grid, we cannot move higher.
                    if (myGridPosX == 2)
                    {
                        midLegalMove = false;
                    }
                    else
                    {
                        // Check if there is a block present to fuse with
                        if (GridController.gridInstance.gridPositions[myGridPosX + 1, myGridPosY].biomeBlock == null)
                        {
                            midLegalMove = false;
                        }
                        else
                        {
                            moveGridPosX = myGridPosX + 1;
                            moveGridPosY = myGridPosY;
                        }
                    }
                }
            }
            else
            {
                midLegalMove = false;
            }
        }
        else
        {
            if (transform.localScale.x > (initialScale).x)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, initialScale, onSelectScaleSpeed * Time.deltaTime);
            }

            if (midLegalMove)
            {
                if (Vector3.Distance(transform.position, GridController.gridInstance.gridPositions[moveGridPosX, moveGridPosY].trans.position) > 2f)
                {
                    transform.position = Vector3.Lerp(transform.position, GridController.gridInstance.gridPositions[moveGridPosX, moveGridPosY].trans.position,
                        onLegalMoveSpeed * Time.deltaTime);
                }
                else
                {
                    // Initiate Fusion
                    // First, we locate the correct new biome to spawn
                    Biome newBiome = null;
                    GameObject newBiomeFusionEffect = null;

                    for (int i = 0; i < biome.fusions.Length; i++)
                    {
                        if (biome.fusions[i].fusionPartner == GridController.gridInstance.gridPositions[moveGridPosX, moveGridPosY].biomeBlock.biome)
                        {
                            newBiome = biome.fusions[i].result;
                            newBiomeFusionEffect = biome.fusions[i].effects;
                            break;
                        }
                    }

                    if (newBiome == null)
                    {
                        Debug.LogError("Biome Fusion not found in " + biome + " fusion list");
                    }
                    else
                    {
                        // Instantiate effects for the fusion, if they exist
                        if (newBiomeFusionEffect != null)
                        {
                            Instantiate(newBiomeFusionEffect,
                                GridController.gridInstance.gridPositions[moveGridPosX, moveGridPosY].trans.position,
                                Quaternion.identity);
                        }

                        // Create the new biome and store it in the grid
                        GameObject newBiomeObj = Instantiate(newBiome.biomeBody, 
                            GridController.gridInstance.gridPositions[moveGridPosX, moveGridPosY].trans.position,
                            Quaternion.identity) as GameObject;

                        // Delete the original biome in the new position
                        Destroy(GridController.gridInstance.gridPositions[moveGridPosX, moveGridPosY].biomeBlock.gameObject);

                        BiomeBlock newBiomeBlock = newBiomeObj.GetComponent<BiomeBlock>();
                        GridController.gridInstance.gridPositions[moveGridPosX, moveGridPosY].biomeBlock = newBiomeBlock;
                        newBiomeBlock.myGridPosX = moveGridPosX;
                        newBiomeBlock.myGridPosX = moveGridPosY;

                        // Finally, we delete this biome block
                        Destroy(gameObject);
                    }
                    
                }
            }
            else
            {
                if (transform.position != GridController.gridInstance.gridPositions[myGridPosX, myGridPosY].trans.position)
                {
                    transform.position = Vector3.Lerp(transform.position, GridController.gridInstance.gridPositions[myGridPosX, myGridPosY].trans.position,
                        onReleaseMoveSpeed * Time.deltaTime);
                }
            }
        }
    }

    private void OnMouseDown()
    {
        if (!midLegalMove)
        {
            blockSelected = true;
        }

        initialMousePos = Input.mousePosition;
    }

    private void OnMouseUp()
    {
        blockSelected = false;
    }
}
