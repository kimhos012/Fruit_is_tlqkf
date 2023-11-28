using Photon.Pun;
using UnityEngine;

public class Attack : MonoBehaviour
{
    PlayerData playerData;
    PhotonView pv;

    public GameObject[] pos;

    [Space(20f)]

    //Atk


    //Skill
    [Header("�÷��̾� ��ų ����")]
    public GameObject[] playerSkill;        //�� ü �� �� �� ����
    [Tooltip("�� ǥâ�� �������� �����մϴ�")]
    public float sureken_angle = 25;
    public GameObject[] toggleSkill;
    public GameObject normalAtk;



    private void Start()
    {
        playerData = GetComponent<PlayerData>();
        pv = GetComponent<PhotonView>();
    }
    public void ReceiveAttack(bool AtkType , int charNum)
    {
        if(!AtkType)
        {
            pv.RPC("Normal", RpcTarget.All);
        }
        else
        {
            pv.RPC("cignature", RpcTarget.All , charNum);
        }
    }


    [PunRPC]
    public void Normal()
    {
        normalAtk.SetActive(true);
    }
    [PunRPC]
    public void cignature(int charNum)
    {
        switch (charNum)
        {
            case 0:

                //GrapeSkill();
                pv.RPC("GrapeSkill", RpcTarget.AllBuffered);
                break;
            case 1:

                //CherrySkill();
                pv.RPC("CherrySkill", RpcTarget.AllBuffered);
                break;
            case 2:

                //MandarinSkill();
                pv.RPC("MandarinSkill", RpcTarget.AllBuffered);
                break;
            case 3:

                //OnionSkill();
                pv.RPC("OnionSkill", RpcTarget.AllBuffered);
                break;
            case 4:

                //PepperSkill();
                pv.RPC("PepperSkill", RpcTarget.AllBuffered);
                break;
            case 5:

                //SweetpotatoSkill();
                pv.RPC("SweetpotatoSkill", RpcTarget.AllBuffered);
                break;

        }
    }
    #region
    [PunRPC]
    void GrapeSkill()
    {
        Instantiate(playerSkill[0], pos[0].GetComponent<Transform>().position, pos[0].GetComponent<Transform>().rotation);
    }

    [PunRPC]
    void CherrySkill()
    {
        Quaternion rotation = pos[0].GetComponent<Transform>().rotation;
        Vector3 rot = pos[0].GetComponent<Transform>().eulerAngles;
        rot.y += sureken_angle * 2;
        for (int i = 0; i < 3; i++)
        {
            rot.y -= sureken_angle;
            Quaternion a = Quaternion.Euler(rot);
            Instantiate(playerSkill[1], pos[0].GetComponent<Transform>().position, a);
        }
    }

    [PunRPC]
    void MandarinSkill()
    {
        Instantiate(playerSkill[2], pos[2].GetComponent<Transform>().position, pos[2].GetComponent<Transform>().rotation);
    }

    [PunRPC]
    void OnionSkill()       //�� 2���� ��ų�� �÷��̾ ���󰡱� ������ Player�ȿ� ������
    {                       //���� SetActive���� ����
        toggleSkill[0].SetActive(true);
    }
    [PunRPC]
    void PepperSkill()
    {
        toggleSkill[1].SetActive(true);
    }

    [PunRPC]
    void SweetpotatoSkill()
    {
        Quaternion rotation = pos[1].GetComponent<Transform>().rotation;

        Vector3 rot = pos[1].GetComponent<Transform>().eulerAngles;
        rot.z += 90;
        Quaternion a = Quaternion.Euler(rot);

        Instantiate(playerSkill[5], pos[1].GetComponent<Transform>().position, a);
        //None
    }
    #endregion
}
