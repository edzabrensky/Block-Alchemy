using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ServiceLocator : Singleton<ServiceLocator>
{
    private Transform playerPosition;

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Assert.IsNotNull(player, "Player was not found");
        this.playerPosition = player.transform;
    }

    public Vector3 GetPlayerPosition()
    {
        return this.playerPosition.position;
    }
}