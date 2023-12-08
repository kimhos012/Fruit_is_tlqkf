using Photon.Pun;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerData : MonoBehaviour , IPunObservable
{
    [Header("�÷��̾� ���� ��(��������)")]
    public int playerNum;
    
    public CharType charactorType;
    [Space(10f)]
    public readonly int maxHp = 120;
    [Range(0, 300)]
    public int Hp;
    public float[] skillCool;
    public float curCool;

    [Space(20f)]

    //�ٸ� ��ũ��Ʈ�� �����Ǵ� ������
    //------------------------------
    public int gameScore = 0;
    public float playerSpeed = 5;
    public float gravity = 10;
    public float jumpPower = 9.8f;
    public Animator animator;
    [HideInInspector] public float hpPer;       //ü�� �ۼ�Ʈ
    [HideInInspector] public float CoolPer;     //��ٿ� �ۼ�Ʈ
    public int charNum;       //ĳ���� ������ȣ
    public bool diee = false;
    public ParticleSystem onHit;



    Mesh mesh;
    PhotonView pv;
    int currentHp;

    int receiveHp;
    float receivecurCool;
    int receivegameScore;
    bool receivediee;
    CharType receivecharactorType;
    int receiveplayerNum;

    private void Start()
    {
        currentHp = Hp;
        pv = GetComponent<PhotonView>();
        mesh = GetComponent<Mesh>();
        Hp = maxHp;
        StartCoroutine(UpdateVariation());
        CooldownReset();
        DontDestroyOnLoad(gameObject);
    }
    private IEnumerator UpdateVariation()
    {
        if(!pv.IsMine)
        {
            Hp = receiveHp;
            curCool = receivecurCool;
            gameScore = receivegameScore;
            diee = receivediee;
            charactorType = receivecharactorType;
            playerNum = receiveplayerNum;
        }
        switch (charactorType)
        {
            case CharType.����:
                charNum = 0;
                break;
            case CharType.ü��:
                charNum = 1;
                break;
            case CharType.��:
                charNum = 2;
                break;
            case CharType.����:
                charNum = 3;
                break;
            case CharType.����:
                charNum = 4;
                break;
            case CharType.����:
                charNum = 5;
                break;
        }
        //------------------------------UIǥ�ð� ����
        hpPer = (float)Hp / maxHp;
        CoolPer = (float)curCool / skillCool[charNum];
        //------------------------------��ٿ�
        if (curCool > 0)
        {
            curCool--;
        }
        //------------------------------�ִϸ��̼� ��Ʈ�ѷ� ��ü
        animator = mesh.PlayerMesh[charNum].GetComponent<Animator>();
        //--------------------dEad
        if (Hp <= 0)
        {
            Die();
        }
        else
        {
            diee = false;
        }
        //=------------------------------------HP����
        if(currentHp != Hp)
        {
            pv.RPC("AhYa", RpcTarget.All);

            //AhYa();

            currentHp = Hp;
        }
        yield return new WaitForSeconds(0.1f);

        StartCoroutine(UpdateVariation());
    }
    [PunRPC]
    public void AhYa()
    {
        onHit.Play();
    }
    void Die()
    {
        diee = true;
        //gameObject.SetActive(false);
    }

    public void CooldownReset()
    {
        curCool = skillCool[charNum];
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // �����͸� �����ϴ� ���� ���
            // ����ȭ�� �����͸� stream�� ���ϴ�.
            stream.SendNext(Hp);
            stream.SendNext(curCool);
            stream.SendNext(gameScore);
            stream.SendNext(diee);
            stream.SendNext(charactorType);
            stream.SendNext(playerNum);
        }
        else
        {
            // �����͸� �����ϴ� ���� ���
            // stream���� �����͸� �а� ����ȭ�մϴ�.
            receiveHp = (int)stream.ReceiveNext();
            receivecurCool = (float)stream.ReceiveNext();
            receivegameScore = (int)stream.ReceiveNext();
            receivediee = (bool)stream.ReceiveNext();
            receivecharactorType = (CharType)stream.ReceiveNext();
            receiveplayerNum = (int)stream.ReceiveNext();
        }
    }
}

public enum CharType        //ĳ���� ����
{
    ����,
    ü��,
    ��,
    ����,
    ����,
    ����
}