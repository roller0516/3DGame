using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
public class LivingEntity : MonoBehaviourPunCallbacks,IDamageable
{
    public float MaxHp;
    public float hp { get; protected set; }
    public bool dead { get; protected set; }

    public event Action onDeath;

    protected virtual void OnEnable() 
    {
        dead = false;
        hp = MaxHp;
    }

    public virtual void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        hp -= damage;
        if (hp <= 0 && !dead)
            Die();
    }
    public virtual void Die() 
    {
        if (onDeath != null) 
        {
            onDeath();
        }
        dead = true;
    }

}
