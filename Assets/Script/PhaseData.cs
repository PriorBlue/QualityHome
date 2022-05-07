using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "QualityHome/Phase")]
public class PhaseData : ScriptableObject
{
    public string Name;
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
