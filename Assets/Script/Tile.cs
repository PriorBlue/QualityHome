using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tile : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("References")]
    public Image Image;
    public RectTransform Rect;
    public Rigidbody2D Body;
    public TargetJoint2D Target;

    [Header("Settings")]
    public float FadeIn = 1f;
    public float DragStrength = 10f;
    public float MaxMagnitude = 200f;
    public float MaxAngularMagnitude = 10f;

    private float spawnTime;
    private float currFade;

    private bool isDragging;

    private void Reset()
    {
        if (Image == null)
        {
            Image = GetComponent<Image>();
        }

        if (Rect == null)
        {
            Rect = GetComponent<RectTransform>();
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

        Target.enabled = false;
    }

    private void FixedUpdate()
    {
        if (isDragging)
        {
            Target.target = Input.mousePosition;
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

        Vector2 result;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(Rect, Input.mousePosition, null, out result);

        Target.anchor = result;
        Target.enabled = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;

        Target.anchor = Vector2.zero;
        Target.enabled = false;

        if (Body.velocity.magnitude > MaxMagnitude)
        {
            Body.velocity = Body.velocity.normalized * MaxMagnitude;
        }

        if (Body.angularVelocity > MaxAngularMagnitude)
        {
            Body.angularVelocity = MaxAngularMagnitude;
        }
    }
}
