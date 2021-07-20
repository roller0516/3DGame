using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowItem : MonoBehaviour
{
    private Transform forward;
    private Transform parent;
    private Rigidbody rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        forward = GameObject.FindWithTag("Player").GetComponent<Transform>();
        parent = GameObject.Find("ThrowPosition").GetComponent<Transform>();
        transform.SetParent(parent);
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.detectCollisions = false;
        rigidbody.useGravity = false;
        transform.localPosition = new Vector3(0, 0, 0);
    }
    void Update() 
    {
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }
    // Update is called once per frame
    public void ReleaseMe() 
    {
        transform.rotation = parent.transform.rotation;
        rigidbody.detectCollisions = true;
        transform.parent = null;
        rigidbody.useGravity = true;
        rigidbody.AddForce(forward.forward * 10000);
        rigidbody.AddForce(forward.up * 10000);
    }
}
