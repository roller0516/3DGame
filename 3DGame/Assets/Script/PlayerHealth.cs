using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : LivingEntity
{
    public Slider hpSlider;
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
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        base.OnDamage(damage, hitPoint, hitDirection);
        //hpSlider.value -= hp;
    }
    public override void Die()
    {
        base.Die();

        //hpSlider.gameObject.SetActive(false);

        ani.SetTrigger("Die");

        playerMovement.enabled = false;
        playerShooter.enabled = false;
    }
    private void OnTriggerEnter(Collider other) // æ∆¿Ã≈€
    {
        if (!dead) 
        {

        }
    }
}
