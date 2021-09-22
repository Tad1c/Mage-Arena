using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSetup : MonoBehaviour
{
    public Transform lookAtThis;

    void Start()
    {
        var groupObj = GameObject.FindGameObjectWithTag("GroupTarget");
        if (groupObj != null)
        {
            // MidpointCalculator midpointCalculator = virtualCamObj.GetComponent<MidpointCalculator>();
            // midpointCalculator.player = transform;
            CinemachineTargetGroup cinemachineGroup = groupObj.GetComponent<CinemachineTargetGroup>();
            cinemachineGroup.m_Targets[1].target = transform;
        }

    }

}
