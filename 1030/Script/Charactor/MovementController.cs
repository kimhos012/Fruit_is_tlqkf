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
    public bool isG = true;             //점프감지
    public bool Lobby = false;          //로비에 있는 플레이어는 스킬을 사용할 수 없다.
    private Vector3 moveDir;

    [Space(10f)]

    public GameObject normalAtt;
    public GameObject jumpAtt;
    public ParticleSystem SunpoEffect;
    [Space(10f)]

    [Header("CanSwitchValue")]
    public float PlayerSpeed = 5;
    public float JumpPower = 10f;
    public float gravity = 9.8f;

    [Space(10f)]
    [Range(0,10)]
    public float telepostDis = 3;
    public float telepostCooldown = 5;
    float curCol;

    [Space(10f)]

    [Header("Detect Floor Ray Setting")]
    public Transform Ground;
    public float RayLine = 0.1f;

    bool IsknockBack;                   //true면 움직임, 스킬 비활성화
    [HideInInspector]
    public float damping = 0.1f;               //Debuff시 이 값이 변경
    private float Yaxis = 1;
    private float v => Input.GetAxis("Vertical");       //InputManager
    private float h => Input.GetAxis("Horizontal");

    float playerMovementAmpl = 1f;

    private Vector3 receivePos;     //Photon only
    private Quaternion receiveRot;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        pv = GetComponent<PhotonView>();
        moveDir = Vector3.zero;
    }


    void Update()
    {
        if (pv.IsMine)      //로컬이면 움직임, 아니면 위치 설정하기
        {
            OnGround();
            if (!IsknockBack)
            {//움직이는 사람이 KnockBack상태인지 아닌지
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

            transform.position = Vector3.Lerp(transform.position, receivePos, damping);
            transform.rotation = Quaternion.Slerp(transform.rotation, receiveRot, damping);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)        //로컬이 맞으면 수신, 아님녀 송신
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

        if (moveDir != Vector3.zero)        //움직일때마다 각도가 변경
        {
            transform.rotation = Quaternion.Euler(0, Mathf.Atan2(h, v) * Mathf.Rad2Deg, 0);
        }

        if (isG)            //Charactor Controller의 isGround는 매우 똥이였습니다.
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
        controller.Move((Time.deltaTime * PlayerSpeed * (moveDir) * (playerMovementAmpl)) + (Time.deltaTime * Yaxis * Vector3.up));      //움직임

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == ("MandarinFloor(sphere)"))
        {
            playerMovementAmpl = 0.5f;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        playerMovementAmpl = 1f;
    }

    void PushungSaRaRak()          //순보
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && curCol <= 0)
        {
            transform.Translate(Vector3.forward * telepostDis);
            curCol = telepostCooldown;
        }
        else
        {

            curCol -= Time.deltaTime;
        }
    }

    void Attack()       //공통적인 공격은 Movement에서, 전용 기술은 CharactorSkill에서 다룹니다
    {                   //하지만, 키감지를 통한 공격 설정은 이 스크립트에서 합니다.
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (!isG)
            {

            }
            else
            {

            }
        }
        else if (Input.GetKeyDown(KeyCode.D))               //스킬 사용
        {

            this.GetComponent<CharactorSkill>().UseSkill();
        }
        else
        {
            Debug.Log("U can use Skill.");
        }
    }

    #region Effects

    void OnGround()
    {
        int layerMask = (-1) - (1 << LayerMask.NameToLayer("Player"));
        RaycastHit hit;

        Debug.DrawRay(Ground.position, -transform.up, Color.red, RayLine);      //sphereTrack으로 교체 예정
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
