using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class optionValue : MonoBehaviour
{
    public int optionNum;       //설정 번호     0부터 마스터,음악,SF볼륨,키바인딩,빛나는달걀
    [Space(10f)]
    [Range(0, 100)]

    public float Value;
    public Slider slider;
    public Text text;

    public AudioMixer audioMixer;

    private void Start()
    {
        Debug.Log(optionNum.ToString() + " , " + Value);
        slider.onValueChanged.AddListener(OnValueChange);
        BackUpSetting();
    }
    void OnValueChange(float changeValue)                   //슬라이더식
    {
        Value = (Mathf.Log10(changeValue) * 20);                //0 ~ -inf
        text.text = (Math.Round(changeValue , 2) * 100).ToString();
    }
    public void ValueButton(bool Add)
    {
        Value = Add == true ? Value++ : Value--;             //버튼식
    }

    /*public void SetLevel(float changeValue)
    {
        switch(optionNum)
        {
            case 0:
                Value = (Mathf.Log10(changeValue) * 20);
                break;
            case 1:
                Value = (Mathf.Log10(changeValue) * 20);
                break;
            case 2:
                Value = (Mathf.Log10(changeValue) * 20);
                break;
        }
        
    }*/
    public void BackUpSetting()
    {
        switch(optionNum)
        {
            case 0:
                Value = LocalManager.MasterVol;
                break;
            case 1:
                Value = LocalManager.MusicVol;
                break;
            case 2:
                Value = LocalManager.SfxVol;
                break;

        }
    }

    public void SaveSetting()
    {
        switch (optionNum)
        {
            case 0:
                audioMixer.SetFloat("Master", Value);
                LocalManager.MasterVol = Value;
                break;
            case 1:
                audioMixer.SetFloat("Music", Value);
                LocalManager.MusicVol = Value;
                break;
            case 2:
                audioMixer.SetFloat("Sfx", Value);
                LocalManager.SfxVol = Value;
                break;
        }
    }
}
