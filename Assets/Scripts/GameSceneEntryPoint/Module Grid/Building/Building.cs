using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public Vector2Int size = Vector2Int.one;
    public Renderer mainRenderer;

    public void SetTransperent(bool available)
    {
        mainRenderer.sortingOrder = 2;

        if (available)
        {
            mainRenderer.material.color = Color.green;
        }
        else
        {
            mainRenderer.material.color = Color.red;
        }
    }

    public void SetNormal()
    {
        mainRenderer.material.color = Color.white;
        mainRenderer.sortingOrder = 1;
    }

    private void OnDrawGizmosSelected()
    {
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                if ((x + y) % 2 == 0)
                {
                    Gizmos.color = new Color(1, 1, 0, 0.3f);
                }
                else
                {
                    Gizmos.color = new Color(0, 1, 1, 0.3f);
                }

                Gizmos.DrawCube(transform.position + new Vector3(x, y, 0), new Vector3(1, 1, 0.1f));
            }
        }
    }

    public void Init()
    {
        for (int i = 0; i < GameObject.Find("Buildings").transform.childCount; i++)
        {
            Building child = GameObject.Find("Buildings").transform.GetChild(i).gameObject.GetComponent<Building>();
            child.SetNormal();
        }

        for (int i = 0; i < GameObject.Find("Placed Buildings").transform.childCount; i++)
        {
            Building child = GameObject.Find("Placed Buildings").transform.GetChild(i).gameObject.GetComponent<Building>();
            child.SetNormal();
        }

        SetTransperent(true);
    }
}
