using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class uiUpdate : MonoBehaviour
{
    PhotonView pv;
    //Set Numb
    public int PlayerNumber;

    Image playerHpPer;
    float hpPer;

    Image playerCooldownPer;
    float cooldown;

    Text playerName;
    new string name;

    GameObject playerSkillImage;
    public Sprite[] Simg;
    public int playerChar;

    GameObject Player;

    void Start()
    {
        pv = GetComponent<PhotonView>();
    }

    void FindPlayer()
    {
        GameObject[] findPs = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject P in findPs)
        {
            int detect = P.GetComponent<PlayerData>().playerNum;
            if (detect == PlayerNumber)
            {
                Player = P;

                UpdateUI();
                pv.RPC("UpdateUI", RpcTarget.Others);

                break;
            }
            else
            {

            }
        }
    }
    void FixedUpdate()
    {
        FindPlayer();
        pv.RPC("FindPlayer", RpcTarget.Others);
    }
    [PunRPC]
    void UpdateUI()
    {
        //Connect UI Setting
        playerHpPer = gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Image>();
        playerSkillImage = gameObject.transform.GetChild(1).gameObject;
        playerCooldownPer = gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Image>();
        playerName = gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).GetComponent<Text>();
        //Connect 2 Player
        hpPer = Player.GetComponent<PlayerData>().hpPer;
        cooldown = Player.GetComponent<PlayerData>().CoolPer;
        name = Player.GetComponent<PhotonView>().Controller.NickName.ToString();
        playerChar = Player.GetComponent<PlayerData>().charNum;

        //ShowBar
        playerHpPer.fillAmount = hpPer;
        playerCooldownPer.fillAmount = cooldown;
        playerName.text = name;
        playerSkillImage.GetComponent<Image>().sprite = Simg[playerChar];
        Debug.Log("Name : " + name + " Health : " + hpPer);

        //Switch Char 2 skillNum

        
        /*switch (playerChar)
        {
            case "0":
                playerSkillImage.GetComponent<SkillImage>().UpdateImape(0);
                break;
            case "1":
                playerSkillImage.GetComponent<SkillImage>().UpdateImape(1);
                break;
            case "2":
                playerSkillImage.GetComponent<SkillImage>().UpdateImape(2);
                break;
            case "3":
                playerSkillImage.GetComponent<SkillImage>().UpdateImape(3);
                break;
            case "4":
                playerSkillImage.GetComponent<SkillImage>().UpdateImape(4);
                break;
            case "5":
                playerSkillImage.GetComponent<SkillImage>().UpdateImape(5);
                break;
        }*/
    }
}
