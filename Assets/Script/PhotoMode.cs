using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoMode : MonoBehaviour
{
    public GameObject green;
    public GameObject orange;
    public GameObject blue;
    public GameObject pink;


    // Start is called before the first frame update
    void Start()
    {

    }

    void SetSeason(GameObject season)
    {
        season.SetActive(true); 
    }

    
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetSeason(green);
            orange.SetActive(false);
            blue.SetActive(false);
            pink.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetSeason(orange);
            green.SetActive(false);
            blue.SetActive(false);
            pink.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetSeason(blue);
            green.SetActive(false);
            orange.SetActive(false);
            pink.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SetSeason(pink);
            green.SetActive(false);
            orange.SetActive(false);
            blue.SetActive(false);
        }
    }
}
