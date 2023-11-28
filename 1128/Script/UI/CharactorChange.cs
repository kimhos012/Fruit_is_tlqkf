using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System;
using System.Threading;
using Photon.Realtime;

public class CharactorChange : MonoBehaviour
{
    public Image charName;
    public Sprite[] nameSprite;
    [Space(10f)]

    public Text titleText;      //스킬 제목
    public string[] TItle;

    [Space(10f)]
    
    public Image skillImage;    //스킬 이미지
    public Sprite[] Image;

    [Space(10f)]
    
    public Text loreText;       //스킬 설명 
    [TextArea]
    public string[] Lore;


    public void ValueChange(int playerC)
    {
        charName.sprite = nameSprite[playerC];
        titleText.text = TItle[playerC];
        loreText.text = Lore[playerC];
        skillImage.sprite = Image[playerC];
    }


    public void ChangeCharWithButtonNum(string a)           
    {
        GameObject[] p = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in p)
        {
            if (player.GetComponent<PhotonView>().IsMine)
            {
                CharType charType;
                bool success = Enum.TryParse(a, out charType);          //인터넷의 산물

                if (success)
                {
                    player.GetComponent<PlayerData>().charactorType = charType; //캐릭터 변경
                    //ValueChange(player.GetComponent<PlayerData>().charNum);     //설명 변경

                }
                else
                {
                    Debug.LogError("Could not convert " + a + " to CharType.");
                }
            }
        }
    }
}
