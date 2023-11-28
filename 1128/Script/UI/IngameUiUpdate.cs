using Photon.Pun;
using Photon.Realtime;
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

    void Start()
    {
        pv = GetComponent<PhotonView>();
        //Connect UI Setting

        playerHpPer = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        playerSkillImage = transform.GetChild(1).GetComponent<Image>();
        playerCooldownPer = transform.GetChild(1).GetChild(0).GetComponent<Image>();
        playerName = transform.GetChild(2).GetChild(0).GetComponent<Text>();


    }
    void GetPlayer()
    {

    }

    void Update()
    {
        UpdateUI();
        //pv.RPC("UpdateUI", RpcTarget.AllBuffered);
        pv.RPC("UpdateUI", RpcTarget.Others);
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

            if (playerNum == PlayerNumber)
            {
                CurrentPlayer = player;
            }
        }
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}
