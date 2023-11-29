using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class IngameUiUpdate : MonoBehaviour, IPunObservable
{
    PhotonView pv;

    //Set Numb
    public int PlayerNumber;

    public Image playerHpPer;
    public float hpPer;

    public Image playerCooldownPer;
    public float cooldown;

    public Text playerName;
    new string name;

    public Image playerSkillImage;
    public Sprite[] Img;
    public int playerChar;

    public GameObject CurrentPlayer;


    float rhp;
    float rcl;
    string rname;
    int rpc;

    void Start()
    {
        pv = GetComponent<PhotonView>();

    }
    void GetPlayer()
    {

        if (CurrentPlayer == null)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject player in players)
            {
                PlayerData playerData = player.GetComponent<PlayerData>();
                int playerNum = playerData.playerNum;

                if (playerNum == PlayerNumber)
                {
                    CurrentPlayer = player;
                    return;
                }
            }
        }

    }

    void Update()
    {
        //UpdateUI();
        pv.RPC("UpdateUI", RpcTarget.All);
        //pv.RPC("UpdateUI", RpcTarget.Others);
    }

    [PunRPC]
    void UpdateUI()
    {
        GetPlayer();
        if (CurrentPlayer == null)
            return;
        if(CurrentPlayer.GetComponent<PhotonView>().IsMine)
        {
            //SetUI
            hpPer = CurrentPlayer.GetComponent<PlayerData>().hpPer;
            cooldown = CurrentPlayer.GetComponent<PlayerData>().CoolPer;
            name = CurrentPlayer.GetComponent<PhotonView>().Controller.NickName.ToString();
            playerChar = CurrentPlayer.GetComponent<PlayerData>().charNum;

            playerHpPer.fillAmount = hpPer;
            playerCooldownPer.fillAmount = cooldown;
            playerName.text = name;
            playerSkillImage.sprite = Img[CurrentPlayer.GetComponent<PlayerData>().playerNum];

            Debug.Log(CurrentPlayer.GetComponent<PlayerData>().playerNum + " " + "Name : " + name + " Health : " + hpPer);
        }
        else
        {
            hpPer = rhp;
            cooldown = rcl;
            name = rname;
            playerChar = rpc;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(hpPer);
            stream.SendNext(cooldown);
            stream.SendNext(name);
            stream.SendNext(playerChar);
        }
        else
        {
            rhp = (float)stream.ReceiveNext();
            rcl = (float)stream.ReceiveNext();
            rname = (string)stream.ReceiveNext();
            rpc = (int)stream.ReceiveNext();
        }
    }
}
