using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class IngameUiUpdate : MonoBehaviour//, IPunObservable
{
    PhotonView pv;

    //Set Numb
    //public int PlayerNumber;

    //SetPlayerNum2Color;
    public GameObject[] uiNumber;

    Image[] playerHpPer;
    float hpPer;

    Image[] playerCooldownPer;
    float cooldown;

    Text[] playerName;
    new string name;

    Image[] playerSkillImage;
    public Sprite[] Img;
    int playerChar;

    public GameObject CurrentPlayer;

    void Start()
    {
        pv = GetComponent<PhotonView>();
        //Connect UI Setting

        for (int i = 0; i < uiNumber.Length; i++)
        {
            playerHpPer[i] = transform.GetChild(0).GetChild(0).GetComponent<Image>();
            playerSkillImage[i] = transform.GetChild(1).GetComponent<Image>();
            playerCooldownPer[i] = transform.GetChild(1).GetChild(0).GetComponent<Image>();
            playerName[i] = transform.GetChild(2).GetChild(0).GetComponent<Text>();
        }
    }
    void GetPlayer()
    {

    }

    void FixedUpdate()
    {
        UpdateUI();
        //pv.RPC("UpdateUI", RpcTarget.AllBuffered);
        //pv.RPC("UpdateUI", RpcTarget.All);
    }

    [PunRPC]
    void UpdateUI()
    {
        GetPlayer();
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {
            PlayerData playerData = player.GetComponent<PlayerData>();
            int playerNum = playerData.playerNum;

            hpPer = playerData.hpPer;
            cooldown = playerData.CoolPer;
            name = player.GetComponent<PhotonView>().Controller.NickName.ToString();
            playerChar = playerData.charNum;

            playerHpPer[playerNum].fillAmount = hpPer;
            playerCooldownPer[playerNum].fillAmount = cooldown;
            playerName[playerNum].text = name;
            playerSkillImage[playerNum].sprite = Img[playerChar];

            Debug.Log(playerData.playerNum+" "+"Name : " + name + " Health : " + hpPer);

        }
    }

    /*public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(hpPer);
            stream.SendNext(cooldown);
            stream.SendNext(name);
        }
        else
        {
            hpPer = (float)stream.ReceiveNext();
            cooldown = (float)stream.ReceiveNext();
            name = (string)stream.ReceiveNext();
        }
    }*/
}
