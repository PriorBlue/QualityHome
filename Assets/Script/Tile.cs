using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tile : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector]
    public CategoryData Category;

    [Header("References")]
    public Image Image;
    public RectTransform Rect;
    public Rigidbody2D Body;
    public TargetJoint2D Target;
    public Outline Outline;
    public AudioSource Audio;

    [Header("Settings")]
    public float FadeIn = 1f;
    public float DragStrength = 10f;
    public float MaxMagnitude = 200f;
    public float MaxAngularMagnitude = 10f;

    private float spawnTime;
    private float currFade;

    private bool isDragging;
    private bool isOver;
    private float lastWoosh;

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

        if (Outline == null)
        {
            Outline = GetComponent<Outline>();
        }
    }

    private void Start()
    {
        spawnTime = Time.time;

        Image.color = new Color(1f, 1f, 1f, 0f);

        Target.enabled = false;

        Outline.enabled = false;
    }

    private void FixedUpdate()
    {
        if (isDragging)
        {
            Target.target = Input.mousePosition;
        }

        if (Body.angularVelocity > MaxAngularMagnitude)
        {
            Body.angularVelocity = MaxAngularMagnitude;
        }
        else if (Body.angularVelocity < -MaxAngularMagnitude)
        {
            Body.angularVelocity = -MaxAngularMagnitude;
        }

        if (isDragging && Body.velocity.magnitude >= 1000f && Time.time >= lastWoosh + 0.5f)
        {
            lastWoosh = Time.time;

            Audio.clip = Category.WooshSound[Random.Range(0, Category.WooshSound.Count)];
            Audio.Play();
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
        if (Spawner.Instance.GetCurrentPhase().BakeTiles == true)
        {
            return;
        }

        isDragging = true;

        Vector2 result;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(Rect, Input.mousePosition, null, out result);

        Target.anchor = result;
        Target.enabled = true;

        if (Category.PickupSound.Count >= 1)
        {
            Audio.clip = Category.PickupSound[Random.Range(0, Category.PickupSound.Count)];
            Audio.Play();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (Spawner.Instance.GetCurrentPhase().BakeTiles == true)
        {
            return;
        }

        isDragging = false;

        Target.anchor = Vector2.zero;
        Target.enabled = false;

        if (Body.velocity.magnitude > MaxMagnitude)
        {
            Body.velocity = Body.velocity.normalized * MaxMagnitude;
        }

        if (isOver == false)
        {
            Outline.enabled = false;
        }

        if (Category.DropSound.Count >= 1)
        {
            Audio.clip = Category.DropSound[Random.Range(0, Category.DropSound.Count)];
            Audio.Play();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Spawner.Instance.GetCurrentPhase().BakeTiles == true)
        {
            return;
        }

        isOver = true;
        Outline.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Spawner.Instance.GetCurrentPhase().BakeTiles == true)
        {
            return;
        }

        isOver = false;

        if (isDragging == false)
        {
            Outline.enabled = false;
        }
    }
}
