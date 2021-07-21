using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Vehicle : LivingEntity,IDamageable
{
    public Transform player;

    public bool vehicleActive;
    bool isInTransition;

    public Transform exitPosition;
    public Transform seatPosition;

    //[Header("Photon")]
    //public PhotonView PV;
    //[Header("UI")]
    //public TextMeshProUGUI nickName;
    public Rigidbody rigid;
    public float Speed;
    public float rotSpeed;

    private LayerMask layer;
    private GameObject hasThrowItem;
    private CinemachineTargetGroup cinemachine;
    private Vector3 moveDirection;
    private Vector3 curPos;

    private int count;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.tag = "Untagged";
        gameObject.layer = 0;
        cinemachine = GameObject.Find("CM TargetGroup1").GetComponent<CinemachineTargetGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (vehicleActive && isInTransition) Exit();
        else if (!vehicleActive && isInTransition) Enter();

        if (vehicleActive) 
        {
            float axisX = Input.GetAxis("Horizontal");
            float axisZ = Input.GetAxis("Vertical");

            moveDirection = axisZ * transform.forward * Speed * Time.deltaTime;
            rigid.MovePosition(rigid.position + moveDirection);
            rigid.rotation = rigid.rotation * Quaternion.Euler(new Vector3(0, axisX * Time.deltaTime * rotSpeed, 0));
            if (Input.GetKeyDown(KeyCode.E))
            {
                isInTransition = true;
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


    void Exit() 
    {
        gameObject.tag = "Untagged";
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
        gameObject.tag = "Player";
        gameObject.layer = 3;
        player.gameObject.SetActive(false);
        player.GetComponent<CharacterController>().enabled = false;
        player.position = Vector3.Lerp(player.position, seatPosition.position, 0.2f);

        if (count == 0)
        {
            cinemachine.RemoveMember(player.transform);
            cinemachine.AddMember(this.transform, 1, 1);
            count=1;
        }
       
        if (player.position == seatPosition.position) 
        {
            isInTransition = false;
            vehicleActive = true;
        }
    }
}
