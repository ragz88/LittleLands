using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolutionDetector : MonoBehaviour
{
    /// <summary>
    /// The biomes the player needs to create to finish the level.
    /// </summary>
    public Biome[] levelSolutions;


    [SerializeField]
    GameObject winScreen;

    [SerializeField]
    GameObject loseScreen;

    bool[] solutionsFound;

    private void Start()
    {
        solutionsFound = new bool[levelSolutions.Length];

        for (int i = 0; i < solutionsFound.Length; i++)
        {
            solutionsFound[i] = false;
        }
    }


    public void CheckSolutions()
    {
        // We reset our bool array incase a previously found solution is not gone
        for (int i = 0; i < solutionsFound.Length; i++)
        {
            solutionsFound[i] = false;
        }

        for (int k = 0; k < levelSolutions.Length; k++)
        {
            // loop through our 2D grid, checking for the correct solutions
            for (int i = 0; i < GridController.gridInstance.gridPositions.GetLength(0); i++)
            {
                for (int j = 0; j < GridController.gridInstance.gridPositions.GetLength(1); j++)
                {
                    if (GridController.gridInstance.gridPositions[i,j].biomeBlock.biome == levelSolutions[k])
                    {
                        solutionsFound[k] = true;
                        break;
                    }
                }

                if (solutionsFound[k])
                {
                    break;
                }
            }
        }

        // After looping through the entire grid, we check if all the necessary solutions are present simultaneously.
        bool allSolutionsPresent = true;

        for (int i = 0; i < solutionsFound.Length; i++)
        {
            if (solutionsFound[i] == false)
            {
                allSolutionsPresent = false;
            }
        }

        if (allSolutionsPresent)
        {
            OnLevelSolved();
        }

    }

    public void CheckForStalemate()
    {
        // Loop through entire grid and check if there is still a potential match. 
        bool matchPresent = false;

        for (int i = 0; i < GridController.gridInstance.gridPositions.GetLength(0); i++)
        {
            for (int j = 0; j < GridController.gridInstance.gridPositions.GetLength(1); j++)
            {
                if (GridController.gridInstance.gridPositions[i,j].biomeBlock != null)
                {
                    // Check for match in positive z
                    if (j != GridController.gridInstance.gridPositions.GetLength(1))
                    {
                        if (GridController.gridInstance.gridPositions[i, j + 1].biomeBlock != null)
                        {
                            matchPresent = true;
                            break;
                        }
                    }

                    // Check for match in negative z
                    if (j != 0)
                    {
                        if (GridController.gridInstance.gridPositions[i, j - 1].biomeBlock != null)
                        {
                            matchPresent = true;
                            break;
                        }
                    }

                    // Check for match in positive x
                    if (i != GridController.gridInstance.gridPositions.GetLength(0))
                    {
                        if (GridController.gridInstance.gridPositions[i + 1, j].biomeBlock != null)
                        {
                            matchPresent = true;
                            break;
                        }
                    }

                    // Check for match in negative x
                    if (i != 0)
                    {
                        if (GridController.gridInstance.gridPositions[i - 1, j].biomeBlock != null)
                        {
                            matchPresent = true;
                            break;
                        }
                    }
                }
            }

            if (matchPresent)
            {
                break;
            }
        }

        if (!matchPresent)
        {
            OnLevelLost();
        }
    }

    public void OnLevelSolved()
    {
        
        winScreen.SetActive(true);
    }

    public void OnLevelLost()
    {
        loseScreen.SetActive(true);
    }
}
