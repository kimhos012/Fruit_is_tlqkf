using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;

public class MovementController : MonoBehaviour , IPunObservable
{
    private CharacterController controller;
    private PhotonView pv;
    public Vector3 moveDir;


    public float PlayerSpeed = 5;
    public float JumpPower = 10f;
    public float damping = 10.0f;

    float v => Input.GetAxis("Vertical");
    float h => Input.GetAxis("Horizontal");

    private Vector3 receivePos;
    private Quaternion receiveRot;

    private int CharC; 
    void Start()
    {
        controller = GetComponent<CharacterController>();

        pv = GetComponent<PhotonView>();
        moveDir = Vector3.zero;
    }


    void Update()
    {
        if (pv.IsMine)
        {
            MovingMan();
        }
        else
        {
            transform.SetPositionAndRotation(Vector3.Lerp(transform.position, receivePos, Time.deltaTime * damping),
            Quaternion.Slerp(transform.rotation, receiveRot, Time.deltaTime * damping));
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            receivePos = (Vector3)stream.ReceiveNext();
            receiveRot = (Quaternion)stream.ReceiveNext();
        }
    }

    void MovingMan()
    {
        if(controller.isGrounded)
        {
            Vector3 cameraForward = Camera.main.transform.forward;
            Vector3 cameraRight = Camera.main.transform.right;

            cameraForward.y = 0.0f;
            cameraRight.y = 0.0f;

            //카메라에 따른 이동 방향 설정
            moveDir = (cameraForward * v) + (cameraRight * h);

            moveDir = new Vector3(moveDir.x, 0, moveDir.z);

            //angler
            if (moveDir != Vector3.zero)
            {
                transform.rotation = Quaternion.Euler(0, Mathf.Atan2(h, v) * Mathf.Rad2Deg, 0);
            }
            moveDir *= PlayerSpeed * damping;

            Debug.Log("UcanJump");
            if (Input.GetKeyDown(KeyCode.Space))     //SampleCode--
            {
                moveDir.y += JumpPower;
                Debug.Log("Jump");
            }
        }
        moveDir.y += Physics.gravity.y * Time.deltaTime;
        controller.Move(moveDir * Time.deltaTime);




    }

}
