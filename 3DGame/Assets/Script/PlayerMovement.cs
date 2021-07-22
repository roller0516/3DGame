using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Cinemachine;
using TMPro;

public enum playerStats 
{
    idle,
    run,
    roll,
    useItem
}
public class PlayerMovement : MonoBehaviourPunCallbacks, IPunObservable
{
    [Header("AniMation")]
    public Animator ani;
    [Header("Photon")]
    public PhotonView PV;
    [Header("UI")]
    public TextMeshProUGUI nickName;
    [Header("Colider")]
    public CharacterController characterController;
    [Header("PlayerStats")]
    public float Speed;
    public float gravity = 9.8f;
    public playerStats playerStats;
    [Header("WeaPonPivot")]
    public GameObject Weapon;
    public Transform leftHand;
    public Transform rightHand;
    [Header("ThrowItem")]
    public GameObject[] throwItem;

    private GameObject hasThrowItem;
    private CinemachineTargetGroup cinemachine;
    private Vector3 moveDirection;
    private Vector3 curPos;

    public GameObject HasThrowItem { get => hasThrowItem; set => hasThrowItem = value; }

    void Start()
    {
        characterController.GetComponent<CharacterController>();
        nickName.text = PV.IsMine ? PhotonNetwork.NickName : PV.Owner.NickName;
        nickName.color = PV.IsMine ? Color.green : Color.red;
        cinemachine = GameObject.Find("CM TargetGroup1").GetComponent<CinemachineTargetGroup>();
        cinemachine.AddMember(this.transform,1,1);
        gravity = 9.8f;
    }

    void Update()
    {
        if (PV.IsMine)
        {
            float axisX = Input.GetAxis("Horizontal");
            float axisZ = Input.GetAxis("Vertical");

            if (characterController.isGrounded == false)
            {
                moveDirection.y -= gravity * Time.deltaTime;
            }
           
            moveDirection = new Vector3(axisX * Time.deltaTime * Speed, moveDirection.y, axisZ * Time.deltaTime * Speed);

            characterController.Move(moveDirection);
            
            if (Input.GetKeyDown(KeyCode.Space)&& playerStats != playerStats.roll)
            {
                transform.eulerAngles = Vector3.up * Mathf.Atan2(moveDirection.x, moveDirection.z)* Mathf.Rad2Deg;
                Roll();
            }

            ani.SetFloat("X", axisX);
            ani.SetFloat("Y", axisZ);


            if (Input.GetKeyDown(KeyCode.G))
            {
                CreateThrowItem(throwItem[0]);
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                playerStats = playerStats.run;
                transform.eulerAngles = Vector3.up * Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
                ani.SetBool("Run", true);
                Speed = 5;
            }
            else if(playerStats != playerStats.roll)
            {
                playerStats = playerStats.idle;
                ani.SetBool("Run", false);
                Speed = 3;
            }

            Rotate();
        }
        else if ((transform.position - curPos).sqrMagnitude >= 100) transform.position = curPos;
        else transform.position = Vector3.Lerp(transform.position, curPos, Time.deltaTime * 10);
    }
   
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
        }
        else 
        {
            curPos = (Vector3)stream.ReceiveNext();
        }
    }
    void Rotate()
    {
        if (playerStats == playerStats.roll || playerStats == playerStats.run)
            return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.up, transform.position);
        float distance;

        if (ground.Raycast(ray, out distance))
        {
            Vector3 point = ray.GetPoint(distance);
            transform.LookAt(new Vector3(point.x, transform.position.y, point.z));
        }
    }
    public void CreateThrowItem(GameObject prefab) 
    {
        HasThrowItem = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        ani.SetTrigger("Throw");
        Weapon.transform.SetParent(leftHand);
    }
    public void Throw() 
    {
        HasThrowItem.GetComponent<ThrowItem>().ReleaseMe();
        HasThrowItem = null;
    }
    public void SetRightHand() 
    {
        Weapon.transform.SetParent(rightHand);
    }
    void Roll() 
    {
        playerStats = playerStats.roll; 
        ani.SetTrigger("Roll");
        Speed *= 1.2f;
        Invoke("RollEnd",1.0f);
    }
    void RollEnd() 
    {
        playerStats = playerStats.idle;
    }

    void UseItem() 
    {

    }

}
