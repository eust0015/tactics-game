﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CellManagerSO", menuName = "ScriptableObjects/Manager/CellManagerSO")]
[System.Serializable] 
public class CellManagerSO : ScriptableObject, ISerializationCallbackReceiver
{
    #region Fields

    [Header("Fields")]
    [SerializeField] private int gridWidth;
    [SerializeField] private int gridHeight;
    [SerializeField] private CellModel[] cellModelList;
    [SerializeField] private CellBattleController[] cellBattleControllerList;

    #endregion
    #region Events
    #endregion
    #region Properties

    public int GridWidth
    {
        get
        {
            return gridWidth;
        }

        set
        {
            gridWidth = value;
        }
    }

    public int GridHeight
    {
        get
        {
            return gridHeight;
        }

        set
        {
            gridHeight = value;
        }
    }

    public CellModel[] CellModelList { get => cellModelList; private set => cellModelList = value; }
    public CellBattleController[] CellBattleControllerList { get => cellBattleControllerList; private set => cellBattleControllerList = value; }

    #endregion
    #region Event Properties
    #endregion
    #region Methods

    public void OnAfterDeserialize()
    {
        Clear();
    }

    //public void OnEnable()
    //{

    //}

    //public void OnDestroy()
    //{

    //}

    public CellModel GetCellModel(int x, int y)
    {
        int listNumber = x + GridHeight * y;
        return CellModelList[listNumber];
    }

    public void SetCellModel(CellModel cell, int x, int y)
    {
        int listNumber = x + GridHeight * y;
        CellModelList[listNumber] = cell;
    }

    public void RemoveCellModel(int x, int y)
    {
        int listNumber = x + GridHeight * y;
        CellModelList[listNumber] = null;
    }

    public CellBattleController GetCellBattleController(int x, int y)
    {
        int listNumber = x + GridHeight * y;
        return CellBattleControllerList[listNumber];
    }

    public void SetCellBattleController(CellBattleController cell, int x, int y)
    {
        int listNumber = x + GridHeight * y;
        CellBattleControllerList[listNumber] = cell;
    }

    public void RemoveCellBattleController(int x, int y)
    {
        int listNumber = x + GridHeight * y;
        CellBattleControllerList[listNumber] = null;
    }

    public void Clear()
    {
        GridWidth = 0;
        GridHeight = 0;
        CellModelList = null;
        CellBattleControllerList = null;
    }

    public void CreateBattleCells(CellMapSO cellMap)
    {
        Debug.Log("CellManagerSO - Create Cells");
        GridWidth = cellMap.GridWidth;
        GridHeight = cellMap.GridHeight;
        CellModelList = new CellModel[GridWidth * GridHeight];
        CellBattleControllerList = new CellBattleController[GridWidth * GridHeight];
        int count = 0;
        for (int x = 0; x < GridWidth; x++)
        {
            for (int y = 0; y < GridHeight; y++)
            {
                InstantiatePrefab(x, y, cellMap.PrefabArray[count]);
                count++;
            }
        }
        // Set adjacent paths
        CalculateAdjacentPaths();

    }

    public void InstantiatePrefab(int x, int y, CellBattleController prefab)
    {
        Vector3 position = new Vector3((float)x, (float)y, 0.0f);
        CellBattleController controller = (CellBattleController)Object.Instantiate(prefab, position, Quaternion.identity);
        CellModel model = controller.Model;
        model.CellGridPositionX = x;
        model.CellGridPositionY = y;
        model.CellName = model.CellName + " " + x + "," + y;
        controller.transform.gameObject.name = model.CellName;
        controller.Initialise(model);
    }

    public void CalculateAdjacentPaths()
    {
        Debug.Log("CellManagerSO - CalculateAdjacentCells");
        int pathX;
        int pathY;
        foreach (CellModel cell in CellModelList)
        {
            pathX = cell.CellGridPositionX;
            pathY = cell.CellGridPositionY;
            //Check above
            if (pathY < GridHeight - 1)
            {
                cell.AddAdjacentCell(GetCellModel(pathX, pathY + 1));
            }
            //Check down
            if (pathY > 0)
            {
                cell.AddAdjacentCell(GetCellModel(pathX, pathY - 1));
            }
            //Check right
            if (pathX < GridWidth - 1)
            {
                cell.AddAdjacentCell(GetCellModel(pathX + 1, pathY));
            }
            //Check left
            if (pathX > 0)
            {
                cell.AddAdjacentCell(GetCellModel(pathX - 1, pathY));
            }
        }
    }

    public void OnBeforeSerialize()
    {
    }

    //public void Clear()
    //{
    //    Debug.Log("CellManagerSO - Destroy");
    //    foreach (CellBattleController cell in Cells)
    //    {
    //        Destroy(cell.View.gameObject);
    //    }
    //}

    public bool CheckIfTwoCellsAreAdjacent(int x1, int y1, int x2, int y2)
    {
        bool isAdjacent = false;

        if (x1 == x2 && y1 == y2 - 1) //Check above
        {
            isAdjacent = true;

        }
        else if (x1 == x2 && y1 == y2 + 1) //Check down
        {
            isAdjacent = true;
        }
        else if (x1 == x2 - 1 && y1 == y2) //Check right
        {
            isAdjacent = true;
        }
        else if (x1 == x2 + 1 && y1 == y2) //Check left
        {
            isAdjacent = true;
        }

        return isAdjacent;
    }
    
    #endregion
}
