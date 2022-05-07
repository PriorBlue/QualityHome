using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "QualityHome/Category")]
public class CategoryData : ScriptableObject
{
    [Header("Settings")]
    public List<AudioClip> PickupSound = new List<AudioClip>();
    public List<AudioClip> DropSound = new List<AudioClip>();
    public List<AudioClip> WooshSound = new List<AudioClip>();

    [Header("Tiles")]
    public List<Tile> Tiles = new List<Tile>();
}
