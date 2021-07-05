using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class Player : MonoBehaviourPunCallbacks,IPunObservable
{
    public Rigidbody rigid;
    public Animator ani;
    public PhotonView PV;
    public Text nickName;
    Vector3 movement;
    Vector3 curPos;
    void Start()
    {
        nickName.text = PV.IsMine ? PhotonNetwork.NickName : PV.Owner.NickName;
        nickName.color = PV.IsMine ? Color.green : Color.red;
    }

    

    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        if (PV.IsMine)
        {
            float axisX = Input.GetAxis("Horizontal");
            float axisZ = Input.GetAxis("Vertical");
            Vector3 velocity = new Vector3(axisX, 0, axisZ) * 10;
            rigid.velocity = velocity;
            print(rigid.velocity);
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
}
