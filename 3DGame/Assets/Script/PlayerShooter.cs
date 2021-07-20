using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerShooter : MonoBehaviour
{
    public Gun gun;
    public Transform gunPivot;
    public Transform leftHandMount;
    public Transform rightHandMount;
    private PlayerMovement playermove;
    private PhotonView PV;

    bool reload;

    Animator ani;
    // Start is called before the first frame update
    void Awake()
    {
        ani = GetComponent<Animator>();
        PV = GetComponent<PhotonView>();
        playermove = GetComponent<PlayerMovement>();
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
        if (PV.IsMine) 
        {
            if (playermove.playerStats == playerStats.roll || playermove.playerStats == playerStats.run)
                return;
            ani.SetBool("Reloading", reload);
            if (Input.GetMouseButton(0))
            {
                gun.Fire();
                ani.SetBool("Run", false);
                if(gun.magAmmo > 0)
                    ani.SetTrigger("Shoot");
            }
            else if (Input.GetKeyDown(KeyCode.R)) 
            {
                reload = true;
                gun.Reload();
                StartCoroutine(Reloading()); 
                playermove.SetRightHand();
            }
        }
    }
    IEnumerator Reloading() 
    {
        while (ani.GetCurrentAnimatorStateInfo(1).normalizedTime < 1) 
        {
            yield return null;
        }

        reload = false;
        //yield return new WaitForSeconds(reloadTime);
        //ani.SetBool("Reloading", false);
    }
}
