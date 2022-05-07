using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tile : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("References")]
    public Image Image;
    public Rigidbody2D Body;

    [Header("Settings")]
    public float FadeIn = 1f;
    public float DragStrength = 10f;
    public float MaxMagnitude = 200f;

    private float spawnTime;
    private float currFade;

    private bool isDragging;

    private void Reset()
    {
        if (Image == null)
        {
            Image = GetComponent<Image>();
        }

        if (Body == null)
        {
            Body = GetComponent<Rigidbody2D>();
        }
    }

    private void Start()
    {
        spawnTime = Time.time;

        Image.color = new Color(1f, 1f, 1f, 0f);
    }

    private void FixedUpdate()
    {
        if (isDragging)
        {
            Body.velocity = (Input.mousePosition - transform.position).normalized * Vector3.Distance(Input.mousePosition, transform.position) * DragStrength;
        }
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

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;

        //Body.centerOfMass = new Vector2(100f, 100f);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;

        if (Body.velocity.magnitude > MaxMagnitude)
        {
            Body.velocity = Body.velocity.normalized * MaxMagnitude;
        }
    }
}
