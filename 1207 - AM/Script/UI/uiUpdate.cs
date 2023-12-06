using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class uiUpdate : MonoBehaviourPun
{
   // PhotonView pv;
    //Set Numb
    public int PlayerNumber;

    Image playerHpPer;
    [SerializeField] float hpPer;

    Image playerCooldownPer;
    [SerializeField] float cooldown;

    Text playerName;
    [SerializeField] new string name;

    GameObject playerSkillImage;
    public Sprite[] Simg;
    public int playerChar;

    Image CharImage;
    public Sprite[] charImage;

    GameObject Player;

    void Start()
    {
        //pv = GetComponent<PhotonView>();
        //UpdateVariation();
        //pv.RPC("UpdateVariation", RpcTarget.All);
    }
    void Update()
    {
        StartCoroutine(FindPlayer());
        //UpdateUI();
    }
    public IEnumerator FindPlayer()
    {
        GameObject[] findPs = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject P in findPs)
        {
            int detect = P.GetComponent<PlayerData>().playerNum;
            if (detect == PlayerNumber)
            {
                Player = P;

                UpdateUI();
                //photonView.RPC("UpdateUI", RpcTarget.All);

                yield break;
            }
        }
        //yield return new WaitForSeconds(0.05f);
        //StartCoroutine(FindPlayer());
        yield return null;
    }

    void UpdateUI()
    {
        //FindPlayer();
        if (Player == null)
        {
            gameObject.SetActive(false);
            return;
        }
        Debug.Log("UpdateUI" + PlayerNumber.ToString());


        //Connect UI Setting
        playerHpPer = transform.GetChild(0)?.GetChild(0).GetComponent<Image>();
        playerSkillImage = transform.GetChild(1)?.gameObject;
        playerCooldownPer = transform.GetChild(1)?.GetChild(0).GetComponent<Image>();
        playerName = transform.GetChild(2)?.GetChild(0).GetComponent<Text>();
        CharImage = transform.GetChild(3)?.GetComponent<Image>();
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
        CharImage.sprite = charImage[playerChar];
        Debug.Log("Name : " + name + " Health : " + hpPer);

    }
}
