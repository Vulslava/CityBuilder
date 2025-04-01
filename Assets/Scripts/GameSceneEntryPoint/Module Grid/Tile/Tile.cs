using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color baseColor, offsetColor;
    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] private bool isClosed;

    public void Init(bool isOffset)
    {
        renderer.color = isOffset ? offsetColor : baseColor;
    }
}
