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

    [Header("Debug")]
    public List<CategoryInfo> Categories = new List<CategoryInfo>();

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
        if (Time.time >= lastSpawn + Delay)
        {
            lastSpawn = Time.time;

            SpawnTile();
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
            Categories.Add(new CategoryInfo() { Category = category.Category, tilesLeft = Random.Range(category.MinCount, category.MaxCount + 1) });
        }

        if (currPhase < Phases.Count - 1)
        {
            PhaseName.text = Phases[currPhase].Phase.Name;
            PhaseCounter.text = $"Phase {currPhase + 1}";

            RateButton.gameObject.SetActive(false);
        }
        else
        {
            PhaseName.text = "Rating";
            PhaseCounter.text = $"Phase {currPhase + 2}";

            RateButton.gameObject.SetActive(true);
        }
    }

    private void SpawnTile()
    {
        if (Categories.Count == 0)
        {
            RateButton.gameObject.SetActive(true);

            return;
        }

        randCategory = Categories[Random.Range(0, Categories.Count)];

        randCategory.tilesLeft--;

        randTile = randCategory.Category.Tiles[Random.Range(0, randCategory.Category.Tiles.Count)];
        randPosition = new Vector3(Random.Range(-Size.x, Size.x), Random.Range(-Size.y, Size.y), 0f);

        if (randCategory.tilesLeft <= 0)
        {
            Categories.Remove(randCategory);
        }

        var tile = Instantiate(randTile, transform.position + randPosition, Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.forward), Canvas);

        tile.Body.velocity = new Vector2(Random.Range(-20f, 20f), 100f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, Size * 2f);
    }
}
