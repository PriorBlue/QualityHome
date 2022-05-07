using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "QualityHome/Category")]
public class CategoryData : ScriptableObject
{
    [Header("Tiles")]
    public List<Tile> Tiles = new List<Tile>();
}
