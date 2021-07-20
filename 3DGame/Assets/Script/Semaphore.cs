using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

public class Semaphore : MonoBehaviour
{
    // Start is called before the first frame update


    public GameObject lineGameObject;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Environment") 
        {
            StartCoroutine(LineSetActive());
        }
    }

    IEnumerator LineSetActive() 
    {
        yield return new WaitForSeconds(0.5f);
        lineGameObject.SetActive(true);
    }
}
