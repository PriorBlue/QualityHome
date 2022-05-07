using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "QualityHome/Phase")]
public class PhaseData : ScriptableObject
{
    [Header("Info")]
    public string Name;
    public string Title;
    [Multiline]
    public string Description;

    [Header("Settings")]
    public bool ShowPopup;
    public float WindStrength = 0f;
    public float Delay = 1f;
    public AudioClip Sound;
    public bool BakeTiles = false;

    [Header("Categories")]
    public List<CategoryInfo> Categories = new List<CategoryInfo>();

    [System.Serializable]
    public class CategoryInfo
    {
        public CategoryData Category;
        public int MinCount = 1;
        public int MaxCount = 1;
    }
}
