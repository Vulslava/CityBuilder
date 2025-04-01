using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class GameSceneEntryPoint : MonoBehaviour
{
    public Canvas canvas;
    public GameObject[] buildings;
    public GameObject[] buttons;
    public Transform cam;
    public GameObject backgroundMenu;
    public GameObject inputHandler;
    public SaveLoadData saveLoadData;
    public Tile tilePrefab;

    void Awake()
    {
        Instantiate(backgroundMenu);
        Instantiate(inputHandler, canvas.transform);
        saveLoadData.Load();
        GeneratedGrid();
    }

    public void GeneratedGrid()
    {
        Building[,] grid;
        Vector2Int gridSize = GameObject.Find("Field").GetComponent<BuildingsGrid>().gridSize;

        if (saveLoadData.gridSizeData == null || (saveLoadData.gridSizeData.x == 0 && saveLoadData.gridSizeData.y == 0))
        {
            saveLoadData.AddGridSize(gridSize);
            grid = new Building[gridSize.x, gridSize.y];
        }
        else
        {
            gridSize.x = saveLoadData.gridSizeData.x;
            gridSize.y = saveLoadData.gridSizeData.y;
            grid = saveLoadData.LoadBuildings(buildings);
            saveLoadData.UpdateGrid(grid);
        }

        GameObject.Find("Field").GetComponent<BuildingsGrid>().GetData(gridSize, grid, saveLoadData);

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                var spawnedTile = Instantiate(tilePrefab, new Vector3(x, y, 10), Quaternion.identity, GameObject.Find("Field").transform);
                spawnedTile.name = "Tile " + x + " " + y;
                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                spawnedTile.Init(isOffset);
            }
        }

        cam.transform.position = new Vector3((float)gridSize.x / 2 - 0.5f, (float)gridSize.y / 2 - 0.5f, 0);
        CreateMenu(gridSize.x, gridSize.y);
    }

    private void CreateMenu(int width, int height)
    {
        Transform backgroundTransform = GameObject.Find("Background(Clone)").transform;
        backgroundTransform.localScale = new Vector3(backgroundTransform.localScale.x * width, backgroundTransform.localScale.x * width / 5, backgroundTransform.localScale.z);
        backgroundTransform.localPosition = new Vector3(cam.transform.position.x, cam.transform.position.y - height / 2 - 1 - (height % 2 == 0 ? 0 : 0.5f), 10);
        
        PathMenu(width, height, buildings, "Buildings", backgroundTransform);
        PathMenu(width, height, buttons, "Buttons", backgroundTransform);

        var buildingsMass = new Building[buildings.Length];
        for (int i = 0; i < buildings.Length; i++)
        {
            buildingsMass[i] = buildings[i].GetComponent<Building>();
        }
        GameObject.Find("Place Button(Clone)").GetComponent<PlaceButton>().GetBuildings(buildingsMass);
    }

    private void PathMenu(int width, int height, GameObject[] mass, string namePath, Transform backgroundTransform)
    {
        float startPosition = -2;
        float endPosition = 2;
        float step = (endPosition - startPosition) / mass.Length;
        float nowPosition = mass.Length % 2 == 0 ? startPosition + step / 2 : mass.Length / -2 * step;

        for (int i = 0; i < mass.Length; i++)
        {
            Instantiate(mass[i], backgroundTransform.Find(namePath).transform);
            Transform childTransform = backgroundTransform.Find(namePath).transform.GetChild(i).transform;

            if (namePath == "Buildings")
            {
                childTransform.GetChild(0).transform.localScale = new Vector3(0.3f, 0.3f, 1);
                childTransform.localScale *= 4;
                childTransform.transform.GetChild(0).localPosition = Vector3.zero;
            }

            childTransform.localScale = new Vector3(childTransform.localScale.x / width, childTransform.localScale.y, childTransform.localScale.z);
            childTransform.localPosition = new Vector3(nowPosition, childTransform.localPosition.y, childTransform.localPosition.z);
            nowPosition += step;
        }
    }

    private void OnApplicationQuit()
    {
        saveLoadData.Save();
    }
}
