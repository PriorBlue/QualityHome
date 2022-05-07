using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    public BoxCollider2D Box;
    public RectTransform Rect;

    private void OnValidate()
    {
        Fit();
    }

    private void Reset()
    {
        Box = GetComponent<BoxCollider2D>();
        Rect = GetComponent<RectTransform>();

        Fit();
    }

    private void Awake()
    {
        Fit();
    }

    private void Fit()
    {
        Box.size = new Vector2(
            Screen.width,
            Rect.sizeDelta.y
        );

        Box.offset = new Vector2(
            0f,
            Rect.sizeDelta.y * 0.5f
        );
    }
}
