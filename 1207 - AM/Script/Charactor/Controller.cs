using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour, IPunObservable
{
    PlayerData playerData;
    Attack atk;
    Mesh mesh;        //�÷��̾� �ִϸ��̼� ����
    CapsuleCollider cpCollider;

    public bool isGround;
    public bool isBind;
    public bool isLobby = false;
    public GameObject OverHead;
    //Char
    CharacterController characterController;
    PhotonView pv;

    public GameObject bindObj;
    public float rayLine = 0.1f;
    public float damping = 2f;
    public ParticleSystem onJump;

    //Private Variation
    private float Verti => Input.GetAxis("Vertical");       //InputManager
    private float Horiz => Input.GetAxis("Horizontal");
    private Vector3 receivePos;
    private Quaternion receiveRot;
    private float receiveCol;


    public Vector3 moveDir;
    private float Yaxis;
    public float speed = 1;        //�÷��̾� ������ ���
    private void Start()
    {
        //�ڽ��� Obj���ִ� ��ũ��Ʈ ����
        playerData = GetComponent<PlayerData>();
        atk = GetComponent<Attack>();
        mesh = GetComponent<Mesh>();
        cpCollider = GetComponent<CapsuleCollider>();
        moveDir = Vector3.zero;
        pv = GetComponent<PhotonView>();
        characterController = GetComponent<CharacterController>();
        speed = 1;
    }

    private void Update()
    {

        //isLobby = SceneManager.GetActiveScene().name == "MainMenu" == true ? true : false;
        if(SceneManager.GetActiveScene().name == "MainMenu" || SceneManager.GetActiveScene().name == "Final")
        {
            isLobby = true;
            OverHead.SetActive(false);
        }
        else
        {
            isLobby = false;
            OverHead.SetActive(true);
        }

        //GetData
        if (pv.IsMine)
        {
            GroundDetect();
            if (playerData.isKnockback) { }  //�и���
            else if (playerData.diee) { }
            else if (isBind)                //���̸�
            {
                //bindObj.SetActive(true);
                pv.RPC("displayBind", RpcTarget.AllBuffered, true);
                attack();
            }
            else
            {                               //������ ����
                //bindObj.SetActive(false);
                pv.RPC("displayBind", RpcTarget.AllBuffered , false);
                MovingMan();                //Move
                attack();
            }
        }
        else
        {               //���Źޱ� , �ٸ� �÷��̾�
            transform.position = Vector3.Lerp(transform.position, receivePos, damping);
            transform.rotation = Quaternion.Slerp(transform.rotation, receiveRot, damping);
            playerData.curCool = receiveCol;
            //playerData.Hp = receiveHp;
        }
        //---------------------------
        if (playerData.diee)
        {

            cpCollider.enabled = !playerData.diee;
            atk.enabled = !playerData.diee;
        }
    }
    [PunRPC]
    public void displayBind(bool Yes)
    {
        bindObj.SetActive(Yes);
    }
    void attack()
    {
        if (!isLobby)
        {
            if (Input.GetKeyDown(KeyCode.D) && playerData.curCool <= 0)         //Skill
            {

                //atk.cignature(playerData.charNum);
                //pv.RPC("atk.cignature", RpcTarget.All, playerData.charNum);

                atk.ReceiveAttack(true, playerData.charNum);

                playerData.CooldownReset();
                mesh.AtkAnimation();
            }
            else if (Input.GetKeyDown(KeyCode.S))    //Attack
            {
                //atk.Normal();
                //pv.RPC("atk.Normal", RpcTarget.All);

                atk.ReceiveAttack(false, 0);

                mesh.AtkAnimation();

            }
        }
    }
    void MovingMan()            //����ȭ �ʿ�--
    {
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        cameraForward.y = 0.0f;
        cameraRight.y = 0.0f;

        moveDir = (cameraForward * Verti) + (cameraRight * Horiz);
        moveDir = new Vector3(moveDir.x, 0, moveDir.z) * speed;

        if (moveDir != Vector3.zero)        //�����϶����� ������ ����
        {
            transform.rotation = Quaternion.Euler(0, Mathf.Atan2(Horiz, Verti) * Mathf.Rad2Deg, 0);
        }

        if (isGround)            //Charactor Controller�� isGround�� �ſ� ���̿����ϴ�.
        {
            Yaxis = 0;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                pv.RPC("HoiYa", RpcTarget.All);
                mesh.JumpAnmation();
                Yaxis += playerData.jumpPower;
            }
        }
        else
        {
            Yaxis -= playerData.gravity * Time.deltaTime;
        }


        characterController.Move((Time.deltaTime * playerData.playerSpeed * (moveDir)) + (Time.deltaTime * Yaxis * Vector3.up));      //������
    }
    [PunRPC]
    public void hoiYa()
    {
        Vector3 ontrans = transform.position;
        ontrans.y -= 1;

        Instantiate(onJump, ontrans, Quaternion.identity);
    }
    void GroundDetect()
    {
        int layerMask = (-1) - (1 << LayerMask.NameToLayer("Proj"));

        RaycastHit hit;

        if (Physics.Raycast(transform.position, -transform.up, out hit, rayLine, layerMask))
        {
            isGround = true;
        }
        else
        {
            isGround = false;
        }
    }

    [PunRPC]
    public void SaSaSak(int KNpower)      //�˹�
    {
        if(KNpower == 1)
        {
            this.transform.localPosition += new Vector3(0, 0, 5);
        }
        else if(KNpower == 2)
        {
            this.transform.localPosition += new Vector3(0, 0, 10);
        }
        playerData.isKnockback = false;
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)        //������ ������ ����, �ƴԳ� �۽�
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(playerData.curCool);
            //stream.SendNext(playerData.Hp);

        }
        else
        {
            receivePos = (Vector3)stream.ReceiveNext();
            receiveRot = (Quaternion)stream.ReceiveNext();
            receiveCol = (float)stream.ReceiveNext();
            //receiveHp = (int)stream.ReceiveNext();
        }
    }
}
