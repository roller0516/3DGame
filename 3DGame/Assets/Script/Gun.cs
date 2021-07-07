using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Gun : MonoBehaviourPunCallbacks, IPunObservable
{
    public enum State {Ready,Empty,Reloading }
    public State state { get; protected set; }

    public Transform fireTransform;
    public ParticleSystem muzzleFlashEffect;

    private LineRenderer bulletLineRenderer;

    public float damage = 20;
    private float fireDitance = 50;

    public int ammoRemain = 100; //√— ≈∫√¢
    public int magCapacity = 25; // √÷¥Î≈∫√¢
    public int magAmmo;

    public float timeBetFire = 0.12f; // ≈∫æÀ πﬂªÁ ∞£∞›
    public float reloadTime = 1.8f; // ¿Á¿Â¿¸ º“ø‰ Ω√∞£
    private float lastFireTime;

    private void Awake()
    {
        bulletLineRenderer = GetComponent<LineRenderer>();

        bulletLineRenderer.positionCount = 2;

        bulletLineRenderer.enabled = false;
    }
    private void OnEnable()
    {
        magAmmo = magCapacity;

        state = State.Ready;

        lastFireTime = 0;
    }

    public void Fire() 
    {
        if (state == State.Ready && Time.time >= lastFireTime + timeBetFire) 
        {
            lastFireTime = Time.time;

            Shot();
        }
    }
    private void Shot() 
    {
        photonView.RPC("ShotProcessOnServer", RpcTarget.AllBuffered);
        magAmmo--;

        if (magAmmo <= 0)
            state = State.Empty;
    }
    // Update is called once per frame
    [PunRPC]
    private void ShotProcessOnServer() 
    {
        RaycastHit hit;

        Vector3 hitPostion = Vector3.zero;

        if (Physics.Raycast(fireTransform.position, fireTransform.forward, out hit, fireDitance))
        {
            IDamageable target = hit.collider.GetComponent<IDamageable>();

            if (target != null)
            {
                target.OnDamage(damage, hit.point, hit.normal);
            }
            hitPostion = hit.point;
        }
        else
        {
            hitPostion = fireTransform.position + fireTransform.forward * fireDitance;
        }
        StartCoroutine(ShotEffect(hitPostion));

        
    }
    private IEnumerator ShotEffect(Vector3 hitPosition) 
    {

        //muzzleFlashEffect.Play();

        bulletLineRenderer.SetPosition(0, fireTransform.position);

        bulletLineRenderer.SetPosition(1, hitPosition);

        bulletLineRenderer.enabled = true;

        yield return new WaitForSeconds(0.03f);

        bulletLineRenderer.enabled = false;
    }
    public bool Reload() 
    {
        if (state == State.Reloading || ammoRemain <= 0 || magAmmo >= magCapacity)
            return false;
        StartCoroutine(ReloadRoutine());
        return true;
    }
    private IEnumerator ReloadRoutine() 
    {
        state = State.Reloading;

        yield return new WaitForSeconds(reloadTime);

        int ammoToFill = magCapacity - magAmmo;
        if (ammoRemain < ammoToFill) 
        {
            ammoToFill = ammoRemain;
        }

        magAmmo += ammoToFill;
        ammoRemain -= ammoToFill;

        state = State.Ready;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(ammoRemain);
            stream.SendNext(magAmmo);
            stream.SendNext(state);
        }
        else 
        {
            ammoRemain = (int)stream.ReceiveNext();
            magAmmo = (int)stream.ReceiveNext();
            state = (State)stream.ReceiveNext();
        }
    }
}
