using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Realtime;

public class Mechanic : LivingEntity, IDamageable
{
    [Header("Owner")]
    public Transform player;

    public bool vehicleActive;

    [Header("TransForm")]
    public Transform exitPosition;
    public Transform seatPosition;
    public Transform leftWeapon;
    public Transform rightWeapon;
    [Header("Damage")]
    public float normaldamage;
    public float missiledamage;
    [Header("Ridigdbody")]
    public Rigidbody rigid;
    [Header("Speed")]
    public float Speed;
    public float rotSpeed;
    [Header("")]
    public GameObject[] bullet;

    public float NormaltimeBetFire = 0.12f; // 탄알 발사 간격
    public float MissiletimeBetFire = 0.12f; // 탄알 발사 간격

    private bool isInTransition;
    private float NormallastFireTime;
    private float MissilelastFireTime;
    private LayerMask layer;
    private GameObject hasThrowItem;
    private CinemachineTargetGroup cinemachine;
    private Vector3 moveDirection;
    private Vector3 curPos;
    private Animator ani;

    private int count;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
        //gameObject.tag = "Untagged";
        gameObject.layer = 0;
        cinemachine = GameObject.Find("CM TargetGroup1").GetComponent<CinemachineTargetGroup>();
        ani = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (vehicleActive && isInTransition) Exit();
        else if (!vehicleActive && isInTransition) Enter();
        ani.SetBool("VehicleActive", vehicleActive);
        if (vehicleActive)
        {
            float axisX = Input.GetAxis("Horizontal");
            float axisZ = Input.GetAxis("Vertical");

            moveDirection = axisZ * transform.forward * Speed * Time.deltaTime;
            rigid.MovePosition(rigid.position + moveDirection);
            ani.SetBool("Walk", true);
            ani.SetFloat("Vertical", axisZ);
            rigid.rotation = rigid.rotation * Quaternion.Euler(new Vector3(0, axisX * Time.deltaTime * rotSpeed, 0));
            if (Input.GetKeyDown(KeyCode.E))
            {
                isInTransition = true;
            }

            if (Input.GetMouseButton(0))
            {
                NormalShot();
            }
            if (Input.GetMouseButton(1))
            {
                MissileShot();
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
            if (Input.GetKeyDown(KeyCode.E))
            {
                isInTransition = true;
            }
    }
    private void OnTriggerExit(Collider other)
    {

    }

    void NormalShot()
    {
        if (Time.time >= NormallastFireTime + NormaltimeBetFire)
        {
            NormallastFireTime = Time.time;
            Instantiate(bullet[0], leftWeapon.transform.position, rigid.rotation);
        }
    }
    void MissileShot() 
    {
        if (Time.time >= MissilelastFireTime + MissiletimeBetFire)
        {
            MissilelastFireTime = Time.time;
            Instantiate(bullet[1], rightWeapon.transform.position, rigid.rotation);
        }

    }

    void Exit()
    {
        //gameObject.tag = "Untagged";
        gameObject.layer = 0;
        player.gameObject.SetActive(true);
        player.position = exitPosition.position;
        if (count == 1)
        {
            cinemachine.RemoveMember(player.transform);
            cinemachine.AddMember(player.transform, 1, 1);
            count = 0;
        }
        if (player.position == exitPosition.position)
        {
            player.GetComponent<CharacterController>().enabled = true;
            isInTransition = false;
            vehicleActive = false;
        }
    }
    void Enter()
    {
        //gameObject.tag = "vehicle";
        gameObject.layer = 3;
        player.gameObject.SetActive(false);
        player.GetComponent<CharacterController>().enabled = false;
        player.position = Vector3.Lerp(player.position, seatPosition.position, 0.2f);

        if (count == 0)
        {
            cinemachine.RemoveMember(player.transform);
            cinemachine.AddMember(this.transform, 1, 1);
            count = 1;
        }

        if (player.position == seatPosition.position)
        {
            isInTransition = false;
            vehicleActive = true;

        }
    }
}
