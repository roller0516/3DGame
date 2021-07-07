using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateLock : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject target;
    public Vector3 v;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            this.transform.position = new Vector3(target.transform.position.x + v.x,
            target.transform.position.y + v.y, target.transform.position.z + v.z);
        }
        else
            this.gameObject.SetActive(false);
    }
}
