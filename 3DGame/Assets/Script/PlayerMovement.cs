using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Cinemachine;

public class PlayerMovement : MonoBehaviourPunCallbacks, IPunObservable
{
    public Rigidbody rigid;
    public Animator ani;
    public PhotonView PV;
    public Text nickName;
    private CinemachineTargetGroup cinemachine;
    Vector3 movement;
    Vector3 curPos;
    void Start()
    {
        nickName.text = PV.IsMine ? PhotonNetwork.NickName : PV.Owner.NickName;
        nickName.color = PV.IsMine ? Color.green : Color.red;
        cinemachine = GameObject.Find("CM TargetGroup1").GetComponent<CinemachineTargetGroup>();
        cinemachine.AddMember(this.transform,1,1);
    }

    

    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        if (PV.IsMine)
        {
            float axisX = Input.GetAxis("Horizontal");
            float axisZ = Input.GetAxis("Vertical");
            Vector3 velocity = new Vector3(axisX, 0, axisZ);
            ani.SetFloat("X", velocity.x * 10);
            ani.SetFloat("Y", velocity.z * 10);
            rigid.velocity = velocity.normalized;
            Rotate();
        }
        else if ((transform.position - curPos).sqrMagnitude >= 100) transform.position = curPos;
        else transform.position = Vector3.Lerp(transform.position, curPos, Time.deltaTime * 10);
    }
    void Rotate() 
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.up, Vector3.zero);
        float distance;

        if (ground.Raycast(ray, out distance)) 
        {
            Vector3 point = ray.GetPoint(distance);
            transform.LookAt(point);
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
