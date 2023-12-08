using Photon.Pun;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerData : MonoBehaviour , IPunObservable
{
    [Header("플레이어 설정 값(수정금지)")]
    public int playerNum;
    
    public CharType charactorType;
    [Space(10f)]
    public readonly int maxHp = 120;
    [Range(0, 300)]
    public int Hp;
    public float[] skillCool;
    public float curCool;

    [Space(20f)]

    //다른 스크립트에 공유되는 변수들
    //------------------------------
    public int gameScore = 0;
    public float playerSpeed = 5;
    public float gravity = 10;
    public float jumpPower = 9.8f;
    public Animator animator;
    [HideInInspector] public float hpPer;       //체력 퍼센트
    [HideInInspector] public float CoolPer;     //쿨다운 퍼센트
    public int charNum;       //캐릭터 고유번호
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
            case CharType.포도:
                charNum = 0;
                break;
            case CharType.체리:
                charNum = 1;
                break;
            case CharType.귤:
                charNum = 2;
                break;
            case CharType.양파:
                charNum = 3;
                break;
            case CharType.고추:
                charNum = 4;
                break;
            case CharType.고구마:
                charNum = 5;
                break;
        }
        //------------------------------UI표시값 설정
        hpPer = (float)Hp / maxHp;
        CoolPer = (float)curCool / skillCool[charNum];
        //------------------------------쿨다운
        if (curCool > 0)
        {
            curCool--;
        }
        //------------------------------애니메이션 컨트롤러 교체
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
        //=------------------------------------HP관리
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
            // 데이터를 전송하는 측인 경우
            // 동기화할 데이터를 stream에 씁니다.
            stream.SendNext(Hp);
            stream.SendNext(curCool);
            stream.SendNext(gameScore);
            stream.SendNext(diee);
            stream.SendNext(charactorType);
            stream.SendNext(playerNum);
        }
        else
        {
            // 데이터를 수신하는 측인 경우
            // stream에서 데이터를 읽고 동기화합니다.
            receiveHp = (int)stream.ReceiveNext();
            receivecurCool = (float)stream.ReceiveNext();
            receivegameScore = (int)stream.ReceiveNext();
            receivediee = (bool)stream.ReceiveNext();
            receivecharactorType = (CharType)stream.ReceiveNext();
            receiveplayerNum = (int)stream.ReceiveNext();
        }
    }
}

public enum CharType        //캐릭터 종류
{
    포도,
    체리,
    귤,
    양파,
    고추,
    고구마
}