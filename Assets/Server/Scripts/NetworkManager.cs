﻿using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{

    public static NetworkManager instance;

    public GameObject playerPrefab;
    public List<GameObject> projectilePrefab;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
    }

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;
        
        Server.Start(50, 26950);
    }

    private void OnApplicationQuit()
    {
        Server.Stop();
    }

    public Player InstanciatePlayer()
    {
        return Instantiate(playerPrefab, Vector3.zero, Quaternion.identity).GetComponent<Player>();
    }

    [CanBeNull]
    public Projectile InstanciateProjectile(Transform origin, int type)
    {
        if (!projectilePrefab[type])
            return null;
        
        var obj = Instantiate(projectilePrefab[type], origin.position, Quaternion.identity).GetComponent<Projectile>();
        obj.type = type;
        
        return obj;
    }

}
