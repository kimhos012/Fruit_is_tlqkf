using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class EditUI : MonoBehaviour//, IPunObservable
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
    int[] playerChar;

    public GameObject[] players;

    void Start()
    {
        pv = GetComponent<PhotonView>();
        //Connect UI Setting

        playerHpPer = new Image[playerHpPer.Length];
        playerSkillImage = new Image[playerSkillImage.Length];
        playerCooldownPer = new Image[playerCooldownPer.Length];
        playerName = new Text[playerName.Length];

        for (int i = 0; i < uiNumber.Length; i++)
        {
            playerHpPer[i] = uiNumber[i].transform.GetChild(0).GetChild(0).GetComponent<Image>();
            playerSkillImage[i] = uiNumber[i].transform.GetChild(1).GetComponent<Image>();
            playerCooldownPer[i] = uiNumber[i].transform.GetChild(1).GetChild(0).GetComponent<Image>();
            playerName[i] = uiNumber[i].transform.GetChild(2).GetChild(0).GetComponent<Text>();
        }

    }
    void GetPlayer()
    {

    }

    void FixedUpdate()
    {
        UpdateUI();
        //pv.RPC("UpdateUI", RpcTarget.AllBuffered);
        pv.RPC("UpdateUI", RpcTarget.Others);
    }

    [PunRPC]
    void UpdateUI()
    {
        GetPlayer();
        players = GameObject.FindGameObjectsWithTag("Player");

        /*foreach (GameObject player in players)
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

        }*/

        /*for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<PlayerData>().playerNum == (uiNumber[i + 1] ))
            {
                hpPer = players[i].GetComponent<PlayerData>().hpPer;
                cooldown = players[i].GetComponent<PlayerData>().CoolPer;
                name = players[i].GetComponent<PhotonView>().Controller.NickName.ToString();
                playerChar = players[i].GetComponent<PlayerData>().charNum;
            }
        }*/
        
        for(int i = 0;i < players.Length;i++)
        {
            hpPer = players[i].GetComponent<PlayerData>().hpPer;



            playerHpPer[i].fillAmount = players[i].GetComponent<PlayerData>().hpPer;
            playerCooldownPer[i].fillAmount = players[i].GetComponent<PlayerData>().CoolPer;
            playerName[i].text = players[i].GetComponent<PhotonView>().Controller.NickName.ToString();
            playerChar[i] = players[i].GetComponent<PlayerData>().charNum;

            playerSkillImage[i].sprite = Img[playerChar[i]];
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
