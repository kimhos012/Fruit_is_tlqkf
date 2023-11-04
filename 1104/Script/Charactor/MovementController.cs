using UnityEngine.SceneManagement;
using UnityEngine;
using Photon.Pun;
using UnityEngine.EventSystems;

public class MovementController : MonoBehaviour , IPunObservable
{
    
    

    public GameObject normalAtt;
    public GameObject jumpAtt;
    public ParticleSystem SunpoEffect;
    [Space(10f)]

    [Header("CanSwitchValue")]
    public float PlayerSpeed = 5;       //�ӵ�
    public float JumpPower = 10f;       //��������
    public float gravity = 9.8f;        //�߷�
    public GameObject bindingObj;
    [Space(10f)]
    [Range(0,10)]
    //�ڷ���Ʈ
    public float telepostDis = 3;
    public float telepostCooldown = 5;
    float curCol;

    [Space(10f)]
    //������
    [Header("Detect Floor Ray Setting")]
    public Transform Ground;
    public float RayLine = 0.1f;

    [Space(10f)]

    //Char Movement
    private CharacterController controller;
    private PhotonView pv;
    public bool isG = true;             //��������
    public bool Lobby = false;          //�κ� �ִ� �÷��̾�� ��ų�� ����� �� ����.
    private Vector3 moveDir;

    //----------------------------�÷��̾� ���� ���� �Լ�-----------------------------------//
    [HideInInspector]
    public bool isknockBack;   
    [HideInInspector]               //true�� ������, ��ų ��Ȱ��ȭ
    public bool isBind;
    [HideInInspector]
    public float defaultspeed = 1f;        //�̵��ӵ��� �����
    


    [HideInInspector]
    public float damping = 0.5f;               //Debuff�� �� ���� ����
    private float Yaxis = 1;
    private float v => Input.GetAxis("Vertical");       //InputManager
    private float h => Input.GetAxis("Horizontal");

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
        if (pv.IsMine)      //�����̸� ������, �ƴϸ� ��ġ �����ϱ�
        {
            OnGround();
            if (!isknockBack && !isBind)        //�����̴� ����� KnockBack�������� �ƴ���
            {                           
                defaultspeed = 1f;
                bindingObj.SetActive(false);

                MovingMan();
                if(!Lobby)                      //�κ񿡼��� ������ �ȵ���
                {
                    Attack();
                    PushungSaRaRak();
                }
            }
            else if(isknockBack && !isBind)     //�˹� ��
            {

            }
            else if(isBind)                     //���� ��
            {
                defaultspeed = 0;
                bindingObj.SetActive(true);
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, receivePos, damping);
            transform.rotation = Quaternion.Slerp(transform.rotation, receiveRot, damping);
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
        controller.Move((Time.deltaTime * PlayerSpeed * (moveDir) * (defaultspeed)) + (Time.deltaTime * Yaxis * Vector3.up));      //������

    }



    void PushungSaRaRak()          //����
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
            
        }
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
}
