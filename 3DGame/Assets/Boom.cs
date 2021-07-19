using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom : MonoBehaviour
{
    public float damage = 100;
    public ParticleSystem boomParticle;
    public SphereCollider collider;
    void Start()
    {
        collider.enabled = false;
        StartCoroutine(BoomCoroutine());
    }
    IEnumerator BoomCoroutine() 
    {
        yield return new WaitForSeconds(3);
        Instantiate(boomParticle,transform.position,Quaternion.Euler(Vector3.zero));
        collider.enabled = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "") 
        {

        }
    }

    // Update is called once per frame
}
