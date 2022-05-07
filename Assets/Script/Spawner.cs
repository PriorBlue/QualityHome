using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    [Header("References")]
    public Transform Canvas;
    public Text PhaseName;
    public Text PhaseCounter;
    public Button RateButton;
    public Text Title;
    public Text Description;

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

    private void Start()
    {
        lastSpawn = Time.time;

        SetPhase(0);

        SpawnTile();

        RateButton.gameObject.SetActive(false);
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
                RateButton.gameObject.SetActive(true);
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

        if (currPhase < Phases.Count)
        {
            PhaseName.text = Phases[currPhase].Phase.Name;
            PhaseCounter.text = $"Phase {currPhase + 1}";

            RateButton.gameObject.SetActive(false);

            Title.text = Phases[currPhase].Phase.Title;
            Description.text = Phases[currPhase].Phase.Description;
        }

        foreach (var tile in tiles)
        {
            tile.Body.bodyType = RigidbodyType2D.Static;
            tile.Outline.enabled = false;
            tile.enabled = false;
        }

        tiles.Clear();
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

        if (categories.Count == 0)
        {
            endSpawn = Time.time;
        }

        var tile = Instantiate(randTile, transform.position + randPosition, Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.forward), Canvas);

        tile.Body.velocity = new Vector2(Random.Range(-20f, 20f), 200f);

        tiles.Add(tile);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, Size * 2f);
    }
}
