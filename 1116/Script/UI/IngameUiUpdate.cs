using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class IngameUiUpdate : MonoBehaviour
{
    PhotonView pv;
    //Set Numb
    public int PlayerNumber;

    public Image playerHpPer;
    float hpPer;

    public Image playerCooldownPer;
    float cooldown;

    public Text playerName;
    new string name;

    public Text playerScore;
    string score;

    public Image playerSkillImage;
    public string playerChar;
    public int skillNum;

    GameObject Player;

    void Start()
    {
        pv = GetComponent<PhotonView>();
    }
    [PunRPC]
    void FindPlayer()
    {
        GameObject[] findPs = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject P in findPs)
        {
            int detect = P.GetComponent<PlayerValue>().number;
            if (detect == PlayerNumber)
            {
                Player = P;
                break;
            }
            else
            {
                Player = null;
            }
        }
        if (Player == null)
            return;
    }
    void FixedUpdate()
    {
        FindPlayer();
        pv.RPC("FindPlayer", RpcTarget.Others);
        UpdateUI();
        pv.RPC("UpdateUI", RpcTarget.Others);
    }
    [PunRPC]
    void UpdateUI()
    {
        FindPlayer();
        //Connect UI Setting
        playerHpPer = gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Image>();
        playerSkillImage = gameObject.transform.GetChild(1).GetComponent<Image>();
        playerCooldownPer = gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Image>();
        playerName = gameObject.transform.GetChild(2).GetComponent<Text>();
        playerScore = gameObject.transform.GetChild(3).GetComponent<Text>();
        //Connect 2 Player
        hpPer = Player.GetComponent<Damage>().hpPers;
        cooldown = Player.GetComponent<CharactorSkill>().cooldownPer;
        name = Player.GetComponent<PhotonView>().Controller.NickName.ToString();
        score = Player.GetComponent<PlayerValue>().mapScore.ToString();
        playerChar = Player.GetComponent<CharactorSkill>().charactorType.ToString();

        //ShowBar
        playerHpPer.fillAmount = hpPer;
        playerCooldownPer.fillAmount = cooldown;
        playerName.text = name;
        playerScore.text = score;

        //Switch Char 2 skillNum
        switch (playerChar)
        {
            case "포도":
                Player.transform.GetChild(1).GetComponent<SkillImage>().SkillNum = 0;
                break;
            case "체리":
                Player.transform.GetChild(1).GetComponent<SkillImage>().SkillNum = 1;
                Debug.Log("S");
                break;
            case "귤":
                Player.transform.GetChild(1).GetComponent<SkillImage>().SkillNum = 2;
                break;
            case "양파":
                Player.transform.GetChild(1).GetComponent<SkillImage>().SkillNum = 3;
                break;
            case "고추":
                Player.transform.GetChild(1).GetComponent<SkillImage>().SkillNum = 4;
                break;
            case "고구마":
                Player.transform.GetChild(1).GetComponent<SkillImage>().SkillNum = 5;
                break;
        }
    }
}
