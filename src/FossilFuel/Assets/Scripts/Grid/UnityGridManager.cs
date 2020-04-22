﻿using Assets.Scripts.Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityGridManager : MonoBehaviour
{


    [SerializeField, Tooltip("The prefab for a grid piece")]
    private GameObject gridPiecePrefab;

    private List<GridMovableNode> nodeList;

    private List<GameObject> gridPieceList;

    [SerializeField, Tooltip("The depth/width in grid pieces of the grid")]
    private int gridDepth, gridWidth;

    [SerializeField, Tooltip("The prefab for character one")]
    private GameObject characterOnePrefab;

    [SerializeField, Tooltip("The prefab for character two")]
    private GameObject characterTwoPrefab;

    [SerializeField, Tooltip("The spawn points for each character. Adding or removing from this will add or remove characters. Teams alternate.")]
    private List<Vector2Int> charStartingPositions;

    [HideInInspector]
    public List<GameObject> ActiveCharsGO;

    [HideInInspector]
    public List<GameObject> AllCharsGO;

    private TurnManager turnMgr;

    private float gridPieceD, gridPieceW;

    // Start is called before the first frame update
    void Start()
    {
        StartCreateGrid();

        StartPlaceCharacters();

        Camera.main.orthographicSize = (gridDepth > gridWidth) ? gridDepth * 1.5f : gridWidth * 1.5f;
    }

    private void StartCreateGrid()
    {
        nodeList = new List<GridMovableNode>();
        gridPieceList = new List<GameObject>();

        gridPieceD = 2.55f; // depth (z-scale) of a grid piece
        gridPieceW = 2.05f; // width (x-scale) of a grid piece

        this.gameObject.transform.position = new Vector3(-(gridPieceW * gridWidth * 0.5f), 0f, -(gridPieceD * gridDepth * 0.5f));

        for (int i = 0; i < gridDepth; i++)
        {
            for (int j = 0; j < gridWidth; j++)
            {
                GameObject currentGridPiece;
                GridMovableNode currentNode = new GridMovableNode(j, i);

                nodeList.Add(currentNode);

                gridPieceList.Add(currentGridPiece = Instantiate(gridPiecePrefab, this.transform.position + new Vector3(((0.5f * gridPieceW) + (j * gridPieceW)), 0f, (0.5f * gridPieceD) + (i * gridPieceD)), Quaternion.identity));

                currentGridPiece.GetComponentInChildren<MovableNodeContainer>().AssignedNode = currentNode;
            }
        }

        foreach (GridMovableNode n1 in nodeList)
        {
            foreach (GridMovableNode n2 in nodeList)
            {
                n2.MatchUpNeighbors(n1);
            }
        }
    }

    private void StartPlaceCharacters()
    {
        turnMgr = TurnManager.Instance;

        turnMgr.InitCharacterList();

        ActiveCharsGO = new List<GameObject>();
        AllCharsGO = new List<GameObject>();

        bool teamOne = true;
        int charNum = 1;

        // This list, populated manually in inspector, determines how many dinos to spawn and what their positions will be.
        foreach(Vector2Int pos in charStartingPositions)
        {
            GameObject currentCharGO;
            Character currentChar;

            // Spawn a dino on a team, alternating which team's dino to spawn with a bool. Also create an abstracted "character" in the turn manager
            if (teamOne)
            {
                currentCharGO = Instantiate(characterOnePrefab, GetNodeContainer(GetNode(pos.x, pos.y)).gameObject.transform.position, Quaternion.identity);

                currentChar = turnMgr.CreateActiveCharacter(1, currentCharGO);
            }
            else
            {
                currentCharGO = Instantiate(characterTwoPrefab, GetNodeContainer(GetNode(pos.x, pos.y)).gameObject.transform.position, Quaternion.identity);

                currentChar = turnMgr.CreateActiveCharacter(2, currentCharGO);
            }

            currentCharGO.GetComponent<CharacterGridMovement>().GridPosX = pos.x;
            currentCharGO.GetComponent<CharacterGridMovement>().GridPosZ = pos.y;

            currentCharGO.GetComponent<UnityCharacterTurnInfo>().CharacterNumber = charNum;

            if (!teamOne) charNum++; // Increment character number after we've given the current number to both sides

            ActiveCharsGO.Add(currentCharGO); // Add to list for public reference
            AllCharsGO.Add(currentCharGO);

            teamOne = !teamOne; // Alternate teams
        }

        // Get the ball rolling and start index 0's turn after we're done building the lists
        turnMgr.StartTurnByNumber(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Gets a node at abstracted position x, z in the node list. Defaults to null.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public GridMovableNode GetNode(int x, int z)
    {
        GridMovableNode returnNode = null; // default to null

        foreach(GridMovableNode n in nodeList)
        {
            if (n.XValue == x && n.ZValue == z)
            {
                returnNode = n;

                return returnNode;
            }
        }

        return returnNode;
    }

    /// <summary>
    /// Gets the node container script of a node, allowing Unity things to be done with it.
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public MovableNodeContainer GetNodeContainer(GridMovableNode node)
    {
        MovableNodeContainer returnContainer = gridPieceList[0].GetComponentInChildren<MovableNodeContainer>();

        foreach(GameObject cont in gridPieceList)
        {
            if(cont.GetComponentInChildren<MovableNodeContainer>().AssignedNode == node)
            {
                returnContainer = cont.GetComponentInChildren<MovableNodeContainer>();
            }
        }

        return returnContainer;
    }

    public bool CheckIfNodeOccupied(GridMovableNode queryNode)
    {
        foreach (GameObject c in ActiveCharsGO)
        {
            if (c.GetComponent<CharacterGridMovement>().CurrentNode == queryNode)
            {
                return true;
            }
        }

        return false;
    }

    public GameObject GetOccupantOfNode(GridMovableNode queryNode)
    {
        foreach(GameObject c in ActiveCharsGO)
        {
            if (c.GetComponent<CharacterGridMovement>().CurrentNode == queryNode)
            {
                return c;
            }
        }

        return null;
    }
}
