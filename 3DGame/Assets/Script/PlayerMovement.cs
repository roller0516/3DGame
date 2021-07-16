using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Cinemachine;
using TMPro;

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
    public float gravity = -9.8f;
    [Header("Pivot")]
    public GameObject Weapon;
    public Transform leftHand;
    public Transform rightHand;
    [Header("ThrowItem")]
    public GameObject[] throwItem;

    private GameObject hasThrowItem;
    private CinemachineTargetGroup cinemachine;
    private Vector3 moveDirection;
    private Vector3 curPos;

    void Start()
    {
        characterController.GetComponent<CharacterController>();
        nickName.text = PV.IsMine ? PhotonNetwork.NickName : PV.Owner.NickName;
        nickName.color = PV.IsMine ? Color.green : Color.red;
        cinemachine = GameObject.Find("CM TargetGroup1").GetComponent<CinemachineTargetGroup>();
        cinemachine.AddMember(this.transform,1,1);
        gravity = -9.8f;
    }
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (PV.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                ItemManager.instance.SpawnItem("AmmoPack", Vector3.zero, Quaternion.identity);
            }
            if (Input.GetKeyDown(KeyCode.Space)) 
            {
                ani.SetTrigger("Roll");
            }
            if (Input.GetKeyDown(KeyCode.G))
            {
                CreateThrowItem(throwItem[0]);
                ani.SetTrigger("Throw");
                Weapon.transform.SetParent(leftHand);
            }
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
            {
                ani.SetBool("Run", true);
                Speed = 5;
            }
            else 
            {
                ani.SetBool("Run", false);
                Speed = 3;
            }

            float axisX = Input.GetAxis("Horizontal");
            float axisZ = Input.GetAxis("Vertical");

            if (characterController.isGrounded == false) 
            {
                moveDirection.y += gravity * Time.deltaTime;
            }

            moveDirection = new Vector3(axisX * Time.deltaTime * Speed, moveDirection.y, axisZ * Time.deltaTime * Speed);

            characterController.Move(moveDirection);

            ani.SetFloat("X", axisX);
            ani.SetFloat("Y", axisZ);

            Rotate();
        }
        else if ((transform.position - curPos).sqrMagnitude >= 100) transform.position = curPos;
        else transform.position = Vector3.Lerp(transform.position, curPos, Time.deltaTime * 10);
    }
    void Rotate() 
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.up,transform.position);
        float distance;

        if (ground.Raycast(ray, out distance)) 
        {
            Vector3 point = ray.GetPoint(distance);
            transform.LookAt(new Vector3(point.x,transform.position.y,point.z));
        }
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
    void CreateThrowItem(GameObject prefab) 
    {
        hasThrowItem = Instantiate(prefab, Vector3.zero, Quaternion.identity);
    }
    public void Throw() 
    {
        hasThrowItem.GetComponent<ThrowItem>().ReleaseMe();
        hasThrowItem = null;
    }
    public void SetRightHand() 
    {
        Weapon.transform.SetParent(rightHand);
    }
}
