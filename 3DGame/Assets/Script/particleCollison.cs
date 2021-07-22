using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleCollison : MonoBehaviour
{
    public GameObject owner;
    float damage;
    public ParticleSystem hitParticle;
    private void Start()
    {
        if (owner.tag == "Mechanic") 
        {
            if (owner.GetComponent<Mechanic>().bullet[0])
                damage = owner.GetComponent<Mechanic>().normaldamage;
            else
                damage = owner.GetComponent<Mechanic>().missiledamage;
        }
        if (owner.tag == "Player")
            damage = GameObject.Find("ShootFX").GetComponent<Gun>().damage;
        if (owner.tag == "Turret")
            damage = owner.GetComponent<Turret>().damage;
    }
    private void OnParticleCollision(GameObject other)
    {
        RaycastHit hit;
        if (other.tag == "Enemy")
        {
            IDamageable target = other.GetComponent<IDamageable>();
            Vector3 hitPoint = other.GetComponent<Collider>().ClosestPoint(transform.position);
            Vector3 hitNormal = transform.position - other.transform.position;
            if (target != null)
            {
                target.OnDamage(damage, hitPoint, hitNormal);
            }
        }
        else if (other.tag == "Environment") 
        {
            if (Physics.Raycast(transform.position, transform.forward, out hit, 10))
            {
                if(hitParticle)
                    Instantiate(hitParticle, hit.point, transform.rotation);
            }
        }
        gameObject.SetActive(false);
    }
}
