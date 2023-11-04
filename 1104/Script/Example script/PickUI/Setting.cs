using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setting : MonoBehaviour
{

    public int Char;
    public int Weapon;
    public int Skill;


    public GameObject FindChar;
    public GameObject FindWeapon;
    public GameObject FindSkill;
    public GameObject UI;
    [Space(10f)]
    public GameObject readyImg;

    void Start()
    {
        readyImg = GameObject.Find("PickedImg");
    }

    
    void Update()
    {
        
    }

    public void FightReady()
    {
        UI.GetComponent<PickCharactor>().TogglePick = false;
        UI.GetComponent<PickCharactor>().OneClick = false;

        Char = FindChar.GetComponent<PickCharactor>().Char;
        Weapon = FindWeapon.GetComponent<WeaponSkillImg>().PickWea;
        Skill = FindSkill.GetComponent<WeaponSkillImg>().PickWea;

        readyImg.transform.GetChild(0).GetComponent<Integer2Image>().Count = Char;
        readyImg.transform.GetChild(1).GetComponent<Integer2Image>().Count = Weapon;
        readyImg.transform.GetChild(2).GetComponent<Integer2Image>().Count = Skill;

    }
}
