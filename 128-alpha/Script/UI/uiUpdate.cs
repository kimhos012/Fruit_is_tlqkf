using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class uiUpdate : MonoBehaviourPun
{
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

                yield break;
            }
        }
        if (Player == null)
        {
            this.gameObject.SetActive(false);
        }
        yield return null;
    }

    void UpdateUI()
    {
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
        //Debug.Log("Name : " + name + " Health : " + hpPer);

    }
}
