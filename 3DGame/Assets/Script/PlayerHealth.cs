using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : LivingEntity
{
    PlayerMovement playerMovement;
    PlayerShooter playerShooter;
    Animator ani;
    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerShooter = GetComponent<PlayerShooter>();
        ani = GetComponent<Animator>();

    }
    protected override void OnEnable()
    {
        base.OnEnable();
        playerShooter.enabled = true;
        playerMovement.enabled = true;


    }
}
