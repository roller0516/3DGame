using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    public Gun gun;
    public Transform gunPivot;
    public Transform leftHandMount;
    public Transform rightHandMount;

    Animator ani;
    // Start is called before the first frame update
    void Awake()
    {
        ani = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        gun.gameObject.SetActive(true);
    }
    private void OnDisable()
    {
        gun.gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
            gun.Fire();
        else if (Input.GetKeyDown(KeyCode.R))
            if (gun.Reload())
                ani.SetBool("Reloading", true);
    }
}
