using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoMode : MonoBehaviour
{
    public GameObject green;
    public GameObject orange;
    public GameObject blue;
    public GameObject pink;
    public Spawner spawner;
    public PhaseData finalphase;
    
    
    // Update is called once per frame
    void Update()
    {
        if (spawner.GetCurrentPhase() == finalphase)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                green.SetActive(true);
                orange.SetActive(false);
                blue.SetActive(false);
                pink.SetActive(false);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                orange.SetActive(true);
                green.SetActive(false);
                blue.SetActive(false);
                pink.SetActive(false);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                blue.SetActive(true);
                green.SetActive(false);
                orange.SetActive(false);
                pink.SetActive(false);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                pink.SetActive(true);
                green.SetActive(false);
                orange.SetActive(false);
                blue.SetActive(false);
            }
        }
    }
}
