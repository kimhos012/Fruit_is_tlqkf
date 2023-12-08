using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Effects;

public class OptionDisplay : MonoBehaviour
{
    public GameObject[] optionLine;
    public GameObject[] settingValue;

    public void SwitchOption(int number)
    {
        for(int i=0; i<optionLine.Length; i++)
        {
            optionLine[i].SetActive(false);
        }
        optionLine[number].SetActive(true);

        GetSettingValue(number);
        LoadOption();
    }

    private void Start()
    {
        settingValue = new GameObject[4];
        SwitchOption(0);
    }

    void GetSettingValue(int number)
    {
        for(int i=0;i<4;i++)
        {
            settingValue[i] = optionLine[number].transform.GetChild(i).gameObject;
        }
    }
    public void LoadOption()
    {
        foreach (GameObject o in settingValue)
        {
            o.GetComponent<optionValue>().BackUpSetting();
        }
    }

    public void SaveOption()
    {
        foreach(GameObject o in settingValue)
        {
            o.GetComponent<optionValue>().SaveSetting();
        }
    }
}
