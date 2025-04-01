using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteButton : MonoBehaviour
{
    public void Init()
    {
        foreach (Transform building in GameObject.Find("Placed Buildings").transform)
        {
            if (building.GetChild(0).GetComponent<Renderer>().sortingOrder == 2)
            {
                GameObject.Find("Field").GetComponent<BuildingsGrid>().DeleteBuilding(building.gameObject);
                break;
            }
        }
    }
}
