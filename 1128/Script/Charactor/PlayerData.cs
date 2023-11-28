using Photon.Pun;
using System.Collections;
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
    public bool isKnockback;
    public Animator animator;
    [HideInInspector] public float hpPer;       //ü�� �ۼ�Ʈ
    [HideInInspector] public float CoolPer;     //��ٿ� �ۼ�Ʈ
    public int charNum;       //ĳ���� ������ȣ
    public bool diee;

    Mesh mesh;
    PhotonView pv;

    private void Start()
    {
        pv = GetComponent<PhotonView>();
        mesh = GetComponent<Mesh>();
        Hp = maxHp;
        StartCoroutine(UpdateVariation());
        CooldownReset();
        switch(playerNum)
        {
            case 0:
                playerNum = PhotonNetwork.CurrentRoom.PlayerCount; break;

            case -1:
                return;
        }
        DontDestroyOnLoad(gameObject);
    }
    private IEnumerator UpdateVariation()
    {
        if(pv.IsMine)
        {
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
        }
        yield return new WaitForSeconds(0.05f);

        StartCoroutine(UpdateVariation());
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
            stream.SendNext(gameScore);
            stream.SendNext(diee);
            stream.SendNext(charactorType);
        }
        else
        {
            // �����͸� �����ϴ� ���� ���
            // stream���� �����͸� �а� ����ȭ�մϴ�.
            gameScore = (int)stream.ReceiveNext();
            diee = (bool)stream.ReceiveNext();
            charactorType = (CharType)stream.ReceiveNext();
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