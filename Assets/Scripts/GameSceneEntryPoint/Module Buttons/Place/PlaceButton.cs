using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEditor;
using UnityEngine;

public class PlaceButton : MonoBehaviour
{
    private Building[] buildingsPrefab;

    public void GetBuildings(Building[] buildingsMass)
    {
        buildingsPrefab = buildingsMass;
    }

    public void Init()
    {
        foreach (Transform building in GameObject.Find("Buildings").transform)
        {
            if (building.GetChild(0).GetComponent<Renderer>().sortingOrder == 2)
            {
                foreach (var buildingPrefab in buildingsPrefab)
                {
                    if (building.GetComponent<Building>().name.Split("(Clone)")[0] == buildingPrefab.name)
                    {
                        building.GetComponent<Building>().SetNormal();
                        GameObject.Find("Field").GetComponent<BuildingsGrid>().StartPlacingBuilding(buildingPrefab);
                        break;
                    }
                }
            }
        }
    }
}
