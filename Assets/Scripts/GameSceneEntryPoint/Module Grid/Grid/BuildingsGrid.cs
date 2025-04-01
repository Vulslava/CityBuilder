using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class BuildingsGrid : MonoBehaviour
{
    public Vector2Int gridSize = new Vector2Int(10, 10);
    public Building[,] grid;
    private Building followBuilding;
    private Camera mainCamera;
    private SaveLoadData saveLoadData;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (followBuilding != null)
        {
            Vector2 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
            int x = Mathf.RoundToInt(worldPosition.x);
            int y = Mathf.RoundToInt(worldPosition.y);
            followBuilding.transform.position = new Vector3(x, y, 10);
            bool available = true;

            if (x < 0 || x > gridSize.x - followBuilding.size.x || y < 0 || y > gridSize.y - followBuilding.size.y)
            {
                available = false;
            }

            if (available && IsPlaceBuilding(x, y))
            {
                available = false;
            }

            followBuilding.SetTransperent(available);
            if (available && Input.GetMouseButtonDown(0))
            {
                PlaceBuilding(x, y);
            }
        }
    }

    public void StartPlacingBuilding(Building buildingPrefab)
    {
        if (followBuilding != null)
        {
            Destroy(followBuilding.gameObject);
        }

        followBuilding = Instantiate(buildingPrefab, GameObject.Find("Placed Buildings").transform);
    }

    private bool IsPlaceBuilding(int placeX, int placeY)
    {
        for (int x = 0; x < followBuilding.size.x; x++)
        {
            for (int y = 0; y < followBuilding.size.y; y++)
            {
                if (grid[placeX + x, placeY + y] != null)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void PlaceBuilding(int placeX, int placeY)
    {
        for (int x = 0; x < followBuilding.size.x; x++)
        {
            for (int y = 0; y < followBuilding.size.y; y++)
            {
                grid[placeX + x, placeY + y] = followBuilding;
            }
        }

        saveLoadData.UpdateGrid(grid);
        followBuilding.SetNormal();
        followBuilding = null;
    }

    public void DeleteBuilding(GameObject building)
    {
        for (int x = 0; x < building.GetComponent<Building>().size.x; x++)
        {
            for (int y = 0; y < building.GetComponent<Building>().size.y; y++)
            {
                grid[(int)building.transform.position.x + x, (int)building.transform.position.y + y] = null;
            }
        }

        saveLoadData.UpdateGrid(grid);
        Destroy(building);
    }

    public void GetData(Vector2Int newGridSize, Building[,] newGrid, SaveLoadData dataManager)
    {
        gridSize = newGridSize;
        grid = newGrid;
        saveLoadData = dataManager;
    }
}
