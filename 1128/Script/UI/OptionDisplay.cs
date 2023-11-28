using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionDisplay : MonoBehaviour
{
    public GameObject[] optionButton;
    public GameObject[] optionLine;

    private void Start()
    {
        //ButtonChange(0);
        //ViewOption(0);
    }


    public void ButtonChange(int optionNum)             //버튼 색 변경하기
    {
        for(int i = 0;i<optionButton.Length;i++)
        {
            optionButton[i].GetComponent<Image>().color = new Color(1,1,1,1);            //일단 모든 설정창을 끈 다음
        }

        optionButton[optionNum].GetComponent<Image>().color = new Color(0.7f, 0.7f, 0.7f, 1);         //설정한 옵션창을 띄우기
    }

    public void ViewOption(int optionNum)              //값에따른 옵션 보이기
    {
        for (int i = 0; i < optionButton.Length; i++)
        {
            optionLine[i].SetActive(false);
        }

        optionLine[optionNum].SetActive(true);
    }

    public void SaveOption()            //옵션을 저장        방법은 설정1에 있는 자식Obj에 스크립트optionValue의 Value와 optionNum을 구하고 그거에 따라 LocalManager에서 소리가 조절됨
    {

    }
    public void ResetOption()
    {

    }
}
