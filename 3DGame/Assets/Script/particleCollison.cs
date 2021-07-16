using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleCollison : MonoBehaviour
{
    Gun gun;
    public ParticleSystem hitParticle;
    private void Start()
    {
        gun = GameObject.Find("ShootFX").GetComponent<Gun>();
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
                target.OnDamage(gun.damage, hitPoint, hitNormal);
            }
        }
        else if (other.tag == "Environment") 
        {
            if (Physics.Raycast(transform.position, transform.forward, out hit, 10))
            {
                Instantiate(hitParticle, hit.point, transform.rotation);
            }
        }
        gameObject.SetActive(false);
    }
}
