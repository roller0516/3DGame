using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoItem : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") 
        {
            Gun gun = other.gameObject.GetComponentInChildren<Gun>();

            gun.ammoRemain += 100;

            this.gameObject.SetActive(false);
        }
    }
}
