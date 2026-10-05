using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerCamerasDoutor : MonoBehaviour
{
    public Camera cameraInterna, cameraExterna;
    public Vector3 cenarioMin, cenarioMax;

    // Update is called once per frame
    void Update()
    {
        bool foraDosLimites =
            (cameraExterna.transform.position.x < cenarioMin.x) ||
            (cameraExterna.transform.position.z < cenarioMin.z) ||
            (cameraExterna.transform.position.x > cenarioMax.x) ||
            (cameraExterna.transform.position.z > cenarioMax.z);

        cameraInterna.gameObject.SetActive(foraDosLimites);            
        cameraExterna.gameObject.SetActive(!foraDosLimites);
    }
}
