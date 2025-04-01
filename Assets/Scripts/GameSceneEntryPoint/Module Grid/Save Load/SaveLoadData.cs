using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveLoadData : MonoBehaviour
{
    [System.Serializable]
    public class GridSizeData
    {
        public int x;
        public int y;
    }

    [System.Serializable]
    public class BuildingData
    {
        public string name;
        public int xPosition;
        public int yPosition;
        public int xSize;
        public int ySize;
    }

    [System.Serializable]
    public class GridListData
    {
        public List<BuildingData> buildingData = new List<BuildingData>();
    }

    public GridSizeData gridSizeData = null;
    public GridListData gridListData = null;

    public void AddGridSize(Vector2Int gridSize)
    {
        gridSizeData.x = gridSize.x;
        gridSizeData.y = gridSize.y;
    }

    public void UpdateGrid(Building[,] gridBuildings)
    {
        GridListData tempGridListData = new GridListData();

        for (int x = 0; x < gridSizeData.x; x++)
        {
            for (int y = 0; y < gridSizeData.y; y++)
            {
                if (gridBuildings[x, y] != null)
                {
                    BuildingData newBuildingData = new BuildingData();
                    newBuildingData.name = gridBuildings[x, y].name.Split("(Clone)")[0];
                    newBuildingData.xPosition = (int)gridBuildings[x, y].gameObject.transform.position.x;
                    newBuildingData.yPosition = (int)gridBuildings[x, y].gameObject.transform.position.y;
                    newBuildingData.xSize = gridBuildings[x, y].size.x;
                    newBuildingData.ySize = gridBuildings[x, y].size.y;

                    if (tempGridListData.buildingData.Count == 0 || IsNewBuildingData(newBuildingData, tempGridListData))
                    {
                        tempGridListData.buildingData.Add(newBuildingData);
                    }
                }
            }
        }

        gridListData.buildingData = tempGridListData.buildingData;
    }

    private bool IsNewBuildingData(BuildingData newBuildingData, GridListData tempGridListData)
    {
        foreach (var building in tempGridListData.buildingData)
        {
            if (building.name == newBuildingData.name && building.xPosition == newBuildingData.xPosition && building.yPosition == newBuildingData.yPosition && building.xSize == newBuildingData.xSize && building.ySize == newBuildingData.ySize)
            {
                return false;
            }
        }

        return true;
    }

    public void Save()
    {
        BinaryFormatter formatter = new BinaryFormatter();

        using (FileStream fs = new FileStream(Application.dataPath + "/Data/data.dat", FileMode.Create))
        {
            formatter.Serialize(fs, gridSizeData);
            formatter.Serialize(fs, gridListData);
        }
    }

    public void Load()
    {
        if (File.Exists(Application.dataPath + "/Data/data.dat"))
        {
            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream fs = new FileStream(Application.dataPath + "/Data/data.dat", FileMode.Open))
            {
                gridSizeData = (GridSizeData)formatter.Deserialize(fs);
                gridListData = (GridListData)formatter.Deserialize(fs);
            }
        }
    }

    public Building[,] LoadBuildings(GameObject[] buildings)
    {
        Building[,] grid = new Building[gridSizeData.x, gridSizeData.y];

        foreach (var building in gridListData.buildingData)
        {
            bool isFirst = true;

            foreach (var buildingPlace in buildings)
            {
                if (building.name == buildingPlace.name)
                {
                    GameObject newBuilding = null;

                    for (int x = building.xPosition; x < building.xPosition + building.xSize; x++)
                    {
                        for (int y = building.yPosition; y < building.yPosition + building.ySize; y++)
                        {
                            if (isFirst)
                            {
                                newBuilding = Instantiate(buildingPlace, new Vector3(x, y, 10), Quaternion.identity, GameObject.Find("Placed Buildings").transform);
                                isFirst = false;
                            }

                            grid[x, y] = newBuilding.GetComponent<Building>();
                        }
                    }

                    break;
                }
            }
        }

        return grid;
    }
}
