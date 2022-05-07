using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    [Header("References")]
    public Image Image;

    [Header("Settings")]
    public float FadeIn = 1f;

    private float spawnTime;
    private float currFade;

    private void Start()
    {
        spawnTime = Time.time;

        Image.color = new Color(1f, 1f, 1f, 0f);
    }

    private void Update()
    {
        currFade = (Time.time - spawnTime) / FadeIn;

        if (currFade < 1f)
        {
            Image.color = new Color(1f, 1f, 1f, currFade);
        }
        else
        {
            Image.color = new Color(1f, 1f, 1f, 1f);
        }
    }
}
