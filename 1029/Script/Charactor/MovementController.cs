using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;

public class MovementController : MonoBehaviour , IPunObservable
{
    //Char Movement
    private CharacterController controller;
    private PhotonView pv;          
    public bool isG = true;             //��������
    public bool Lobby = false;          //�κ� �ִ� �÷��̾�� ��ų�� ����� �� ����.
    private Vector3 moveDir;

    [Space(10f)]

    public GameObject normalAtt;
    public GameObject jumpAtt;

    [Space(10f)]

    [Header("CanSwitchValue")]
    public float PlayerSpeed = 5;
    public float JumpPower = 10f;
    public float gravity = 9.8f;

    [Space(10f)]
    
    [Header("Detect Floor Ray Setting")]
    public Transform Ground;
    public float RayLine = 0.1f;

    bool IsknockBack;                   //true�� ������, ��ų ��Ȱ��ȭ
    [HideInInspector]
    public float damping = 1.0f;               //Debuff�� �� ���� ����
    private float Yaxis;
    private float v => Input.GetAxis("Vertical");       //InputManager
    private float h => Input.GetAxis("Horizontal");

    private Vector3 receivePos;     //Photon only
    private Quaternion receiveRot;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        pv = GetComponent<PhotonView>();
        moveDir = Vector3.zero;

        Yaxis = 1;
    }


    void Update()
    {
        if (pv.IsMine)      //�����̸� ������, �ƴϸ� ��ġ �����ϱ�
        {
            OnGround();
            if (!IsknockBack)
            {//�����̴� ����� KnockBack�������� �ƴ���
                MovingMan();
                if(!Lobby)
                {
                    Attack();
                    PushungSaRaRak();
                }
            }
            else
                Debug.Log("U R knockBacked! U can't Move");
        }
        else
        {
            transform.SetPositionAndRotation(Vector3.Lerp(transform.position, receivePos, Time.deltaTime * damping),
            Quaternion.Slerp(transform.rotation, receiveRot, Time.deltaTime * damping));
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)        //������ ������ ����, �ƴԳ� �۽�
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
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        cameraForward.y = 0.0f;
        cameraRight.y = 0.0f;

        moveDir = (cameraForward * v) + (cameraRight * h);
        moveDir = new Vector3(moveDir.x, 0, moveDir.z);

        if (moveDir != Vector3.zero)        //�����϶����� ������ ����
        {
            transform.rotation = Quaternion.Euler(0, Mathf.Atan2(h, v) * Mathf.Rad2Deg, 0);
        }

        if (isG)            //Charactor Controller�� isGround�� �ſ� ���̿����ϴ�.
        {
            Yaxis = 0;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Yaxis += JumpPower;
            }
        }
        else
        {
            Yaxis -= gravity * Time.deltaTime;
        }
        

        controller.Move((PlayerSpeed * Time.deltaTime * moveDir) + (Time.deltaTime * Yaxis * Vector3.up));      //������

    }


    void PushungSaRaRak()          //����
    {
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {

        }
    }

    void Attack()       //�������� ������ Movement����, ���� ����� CharactorSkill���� �ٷ�ϴ�
    {                   //������, Ű������ ���� ���� ������ �� ��ũ��Ʈ���� �մϴ�.
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (!isG)
            {

            }
            else
            {

            }
        }
        else if (Input.GetKeyDown(KeyCode.D))               //��ų ���
        {

            this.GetComponent<CharactorSkill>().UseSkill();
        }
        else
        {
            Debug.Log("U can use Skill.");
        }
    }

    #region Effects

    void Debuff(string DebuffList)
    {

    }
    void OnGround()
    {
        int layerMask = (-1) - (1 << LayerMask.NameToLayer("Player"));
        RaycastHit hit;

        Debug.DrawRay(Ground.position, -transform.up, Color.red, RayLine);      //sphereTrack���� ��ü ����
        if (Physics.Raycast(Ground.position, -transform.up, out hit, RayLine))
        {
            isG = true;
        }
        else
        {
            isG = false;
        }
    }
    #endregion
}
