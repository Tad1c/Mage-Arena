using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSetup : MonoBehaviour
{
    void Start()
    {
        var virtualCamObj = GameObject.FindGameObjectWithTag("VirtualCamera");
        if (virtualCamObj != null)
        {
            MidpointCalculator midpointCalculator = virtualCamObj.GetComponent<MidpointCalculator>();
            midpointCalculator.player = transform;
        }

    }

}
