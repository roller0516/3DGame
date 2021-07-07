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
    public GameObject TEST;
    public Animator ani;
    public PhotonView PV;
    public TextMeshProUGUI nickName;
    public float Speed;
    public float gravity = -9.8f;
    private CinemachineTargetGroup cinemachine;
    public CharacterController characterController;
    private Vector3 moveDirection;
    Vector3 curPos;
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
        if (Input.GetMouseButtonDown(0)) 
        {
            Vector3 LevelUp = Camera.main.WorldToScreenPoint(transform.position);
        }

        if (PV.IsMine)
        {
            float axisX = Input.GetAxis("Horizontal");
            float axisZ = Input.GetAxis("Vertical");

            if (characterController.isGrounded == false) 
            {
                moveDirection.y += gravity * Time.deltaTime;
            }

            moveDirection = new Vector3(axisX,moveDirection.y, axisZ);

            characterController.Move(moveDirection);

            ani.SetFloat("X", axisX);
            ani.SetFloat("Y", axisZ);

            Rotate();
        }
        else if ((transform.position - curPos).sqrMagnitude >= 100) transform.position = curPos;
        else transform.position = Vector3.Lerp(transform.position, curPos, Time.deltaTime * 10);
    }
    void Run() 
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Speed = 5;
            ani.SetBool("Run", true);
        }
        else
            ani.SetBool("Run", false);
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
}
