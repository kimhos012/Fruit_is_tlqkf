using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionDisplay : MonoBehaviour
{
    public GameObject[] optionButton;
    public GameObject[] optionLine;

    optionValue value;

    int optionNum = 0;

    private void Start()
    {
        ButtonChange();
        ViewOption();

        //load OptionBar

    }


    public void ButtonChange()             //버튼 색 변경하기
    {
        for(int i = 0;i<optionButton.Length;i++)
        {
            optionButton[i].GetComponent<Image>().color = new Color(1,1,1,1);            //일단 모든 설정창을 끈 다음
        }

        optionButton[optionNum].GetComponent<Image>().color = new Color(0.7f, 0.7f, 0.7f, 1);         //설정한 옵션창을 띄우기
        ViewOption();
    }

    public void ViewOption()              //값에따른 옵션 보이기
    {
        for (int i = 0; i < optionButton.Length; i++)
        {
            optionLine[i].SetActive(false);
        }

        optionLine[optionNum].SetActive(true);

        LoadSetting();
    }
    public void LoadSetting()
    {
        foreach(GameObject option in optionLine)
        {
            optionValue value = option.GetComponent<optionValue>();

        }

    }
    public void SaveOption()            //옵션을 저장        방법은 설정1에 있는 자식Obj에 스크립트optionValue의 Value와 optionNum을 구하고 그거에 따라 LocalManager에서 소리가 조절됨
    {
        if(optionNum == 0)
        {
            LocalManager.MasterVol = values[0].Value;
            LocalManager.MusicVol = values[1].Value;            //하려던거
            LocalManager.SfxVol = values[2].Value;
        }
    }
    public void ResetOption()
    {

    }
}
