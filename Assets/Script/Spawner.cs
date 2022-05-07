using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Spawner : MonoBehaviour
{
    public static Spawner Instance;

    [Header("References")]
    public Transform Canvas;
    public TextMeshProUGUI PhaseName;
    public TextMeshProUGUI PhaseCounter;
    public TextMeshProUGUI Title;
    public TextMeshProUGUI Description;
    public AreaEffector2D AreaEffector;
    public AudioSource Audio;
    public GameObject Border;

    public Button RateButton;
    public Button NextButton;
    public GameObject Popup;

    [Header("Settings")]
    public Vector2 Size;
    public float Delay = 5f;

    [Header("Tiles")]
    public List<PhaseInfo> Phases = new List<PhaseInfo>();

    [System.Serializable]
    public class PhaseInfo
    {
        public PhaseData Phase;
    }

    private int currPhase;
    private CategoryInfo randCategory;
    private Tile randTile;
    private Vector3 randPosition;
    private float lastSpawn;
    private float endSpawn;

    private List<Tile> tiles = new List<Tile>();
    private List<CategoryInfo> categories = new List<CategoryInfo>();

    [System.Serializable]
    public class CategoryInfo
    {
        public CategoryData Category;
        public int tilesLeft;
    }

    public PhaseData GetCurrentPhase()
    {
        return Phases[currPhase].Phase;
    }

    private void Awake()
    {
        Instance = this;
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    private void Start()
    {
        lastSpawn = Time.time;

        SetPhase(0);

        RateButton.gameObject.SetActive(false);
        NextButton.gameObject.SetActive(false);

        AreaEffector.enabled = false;
    }

    private void Update()
    {
        if (categories.Count >= 1)
        {
            if (Time.time >= lastSpawn + Delay)
            {
                lastSpawn = Time.time;

                SpawnTile();
            }
        }
        else
        {
            if (Time.time - endSpawn > Phases[currPhase].Phase.Delay)
            {
                endSpawn = Time.time;

                if (Phases[currPhase].Phase.ShowPopup == true)
                {
                    NextButton.gameObject.SetActive(true);
                }
                else
                {
                    RateButton.gameObject.SetActive(true);
                }

                if (Phases[currPhase].Phase.BakeTiles == true)
                {
                    foreach (var tile in tiles)
                    {
                        tile.Body.bodyType = RigidbodyType2D.Static;
                        tile.Outline.enabled = false;
                        tile.enabled = false;
                    }

                    tiles.Clear();
                }

                if (Phases[currPhase].Phase.WindStrength > 0f)
                {
                    Border.SetActive(true);
                }

                AreaEffector.forceMagnitude = 0f;
                AreaEffector.enabled = false;
            }
        }
    }

    public void NextPhase()
    {
        if (currPhase < Phases.Count - 1)
        {
            SetPhase(currPhase + 1);
        }
    }

    private void SetPhase(int phase)
    {
        currPhase = phase;

        foreach (var category in Phases[currPhase].Phase.Categories)
        {
            categories.Add(new CategoryInfo() { Category = category.Category, tilesLeft = Random.Range(category.MinCount, category.MaxCount + 1) });
        }

        Popup.SetActive(Phases[currPhase].Phase.ShowPopup);

        if (currPhase < Phases.Count)
        {
            PhaseName.text = Phases[currPhase].Phase.Name;
            PhaseCounter.text = $"Phase {currPhase + 1}";

            RateButton.gameObject.SetActive(false);
            NextButton.gameObject.SetActive(false);

            Title.text = Phases[currPhase].Phase.Title;
            Description.text = Phases[currPhase].Phase.Description;
        }

        if (Phases[currPhase].Phase.WindStrength > 0f)
        {
            AreaEffector.forceMagnitude = Phases[currPhase].Phase.WindStrength;
            AreaEffector.enabled = true;

            Border.SetActive(false);
        }

        if (Phases[currPhase].Phase.Sound != null)
        {
            Audio.Stop();
            Audio.clip = Phases[currPhase].Phase.Sound;
            Audio.Play();
        }
        else
        {
            Audio.Stop();
        }
    }

    private void SpawnTile()
    {
        randCategory = categories[Random.Range(0, categories.Count)];

        randCategory.tilesLeft--;

        randTile = randCategory.Category.Tiles[Random.Range(0, randCategory.Category.Tiles.Count)];
        randPosition = new Vector3(Random.Range(-Size.x, Size.x), Random.Range(-Size.y, Size.y), 0f);

        if (randCategory.tilesLeft <= 0)
        {
            categories.Remove(randCategory);
        }

        var tile = Instantiate(randTile, transform.position + randPosition, Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.forward), Canvas);

        tile.Body.velocity = new Vector2(Random.Range(-20f, 20f), 100f);

        tiles.Add(tile);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, Size * 2f);
    }
}
