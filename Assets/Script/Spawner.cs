using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public AudioSource Music;
    public AudioSource Sound;
    public GameObject Border;
    public GameObject WindLeft;
    public GameObject WindRight;
    public List<Image> Backgrounds = new List<Image>();

    public Button RateButton;
    public Button NextButton;
    public GameObject Popup;

    public BoxCollider2D Container;

    [Header("Settings")]
    public Vector2 Size;
    public float Delay = 5f;
    public List<AudioClip> SpawnSound = new List<AudioClip>();

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

    public List<GameObject> containerTiles = new List<GameObject>();

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

        WindLeft.SetActive(false);
        WindRight.SetActive(false);

        Backgrounds[0].gameObject.SetActive(true);
        Backgrounds[0].color = Color.white;

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
                    if (Phases[currPhase].Phase.ShowPopup == true)
                    {
                        NextButton.gameObject.SetActive(true);
                    }
                    else if (containerTiles.Count == 0)
                    {
                        RateButton.gameObject.SetActive(true);
                    }
                }

                if (Phases[currPhase].Phase.BakeTiles == true)
                {
                    foreach (var tile in containerTiles.ToArray())
                    {
                        Destroy(tile.gameObject);
                    }

                    foreach (var tile in tiles)
                    {
                        tile.Body.bodyType = RigidbodyType2D.Static;
                        tile.Outline.enabled = false;
                        tile.enabled = false;
                    }

                    tiles.Clear();
                }

                if (Phases[currPhase].Phase.WindStrength != 0f)
                {
                    Border.SetActive(true);
                }

                AreaEffector.forceMagnitude = 0f;
                AreaEffector.enabled = false;
            }
        }

        if (Phases[currPhase].Phase.ChangeBackground >= 1)
        {
            Backgrounds[Phases[currPhase].Phase.ChangeBackground].color = Color.Lerp(Backgrounds[Phases[currPhase].Phase.ChangeBackground].color, Color.white, Time.deltaTime * 0.5f);
        }
    }

    public void NextPhase()
    {
        if (currPhase < Phases.Count - 1)
        {
            SetPhase(currPhase + 1);
        }
        else
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }

    public void RestartGame()
    {
        if (currPhase == 0)
            return;

        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private bool ContainerEmpty()
    {
        foreach (Tile tile in tiles)
        {
            if (Container.IsTouching(tile.GetComponent<Collider2D>()))
            {
                return false;
            }
        }

        return true;
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

        if (Phases[currPhase].Phase.WindStrength != 0f)
        {
            AreaEffector.forceMagnitude = Phases[currPhase].Phase.WindStrength;
            AreaEffector.enabled = true;

            Border.SetActive(false);
        }

        if (Phases[currPhase].Phase.WindStrength > 0f)
        {
            WindLeft.SetActive(true);
            WindRight.SetActive(false);
        }
        else if (Phases[currPhase].Phase.WindStrength < 0f)
        {
            WindLeft.SetActive(false);
            WindRight.SetActive(true);
        }
        else
        {
            WindLeft.SetActive(false);
            WindRight.SetActive(false);
        }

        if (Phases[currPhase].Phase.Sound != null)
        {
            Music.Stop();
            Music.clip = Phases[currPhase].Phase.Sound;
            Music.Play();
        }
        else
        {
            Music.Stop();
        }

        if (Phases[currPhase].Phase.Categories.Count >= 1)
        {
            Sound.clip = SpawnSound[Random.Range(0, SpawnSound.Count)];
            Sound.Play();
        }

        if (Phases[currPhase].Phase.ChangeBackground >= 1)
        {
            Backgrounds[Phases[currPhase].Phase.ChangeBackground].gameObject.SetActive(true);
            Backgrounds[Phases[currPhase].Phase.ChangeBackground].color = new Color(1f, 1f, 1f, 0f);
        }
    }

    private void SpawnTile()
    {
        randCategory = categories[Random.Range(0, categories.Count)];

        randCategory.tilesLeft--;

        randTile = randCategory.Category.Tiles[Random.Range(0, randCategory.Category.Tiles.Count)];
        randPosition = new Vector3(Random.Range(-Size.x, Size.x), Random.Range(-Size.y, Size.y), 0f);

        var tile = Instantiate(randTile, transform.position + randPosition, Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.forward), Canvas);

        tile.Body.velocity = new Vector2(Random.Range(-1f, 1f), 10f);

        tile.Category = randCategory.Category;

        tiles.Add(tile);

        if (randCategory.tilesLeft <= 0)
        {
            categories.Remove(randCategory);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, Size * 2f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        containerTiles.Add(collision.gameObject);

        RateButton.gameObject.SetActive(false);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        containerTiles.Remove(collision.gameObject);

        if (containerTiles.Count == 0 && Phases[currPhase].Phase.BakeTiles == false)
        {
            RateButton.gameObject.SetActive(true);
        }
    }
}
