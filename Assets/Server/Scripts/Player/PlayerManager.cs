using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private Player _player;
    private IHealth _healthManager;

    private void Awake()
    {
        _player = GetComponent<Player>();
        _healthManager = GetComponent<IHealth>();
    }

    public void PlayerDead()
    {
        _player.Controller.useGravity = false;
        transform.position = new Vector3(0f, 25f,0f);
        
        ServerSend.PlayerPosition(_player);
        
        
        //Start Respawn countdown
        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        yield return  new WaitForSeconds(3f);
          _player.Controller.useGravity = true;
        _healthManager.Respawn();
        
        ServerSend.PlayerRespawn(_player);
    }
}
