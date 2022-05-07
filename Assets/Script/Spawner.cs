using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("References")]
    public Transform Canvas;

    [Header("Settings")]
    public Vector2 Size;
    public float Delay = 5f;

    [Header("Tiles")]
    public List<GameObject> Tiles = new List<GameObject>();

    private GameObject randTile;
    private Vector3 randPosition;
    private float lastSpawn;

    private void Start()
    {
        lastSpawn = Time.time;
    }

    private void Update()
    {
        if (Time.time >= lastSpawn + Delay)
        {
            lastSpawn = Time.time;

            randTile = Tiles[Random.Range(0, Tiles.Count)];
            randPosition = new Vector3(Random.Range(-Size.x, Size.x), Random.Range(-Size.y, Size.y), 0f);

            var tile = Instantiate(randTile, transform.position + randPosition, Quaternion.identity, Canvas);

            tile.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-20f, 20f), 100f);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, Size * 2f);
    }
}
