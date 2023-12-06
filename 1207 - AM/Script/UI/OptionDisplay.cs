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


    public void ButtonChange()             //��ư �� �����ϱ�
    {
        for(int i = 0;i<optionButton.Length;i++)
        {
            optionButton[i].GetComponent<Image>().color = new Color(1,1,1,1);            //�ϴ� ��� ����â�� �� ����
        }

        optionButton[optionNum].GetComponent<Image>().color = new Color(0.7f, 0.7f, 0.7f, 1);         //������ �ɼ�â�� ����
        ViewOption();
    }

    public void ViewOption()              //�������� �ɼ� ���̱�
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
    public void SaveOption()            //�ɼ��� ����        ����� ����1�� �ִ� �ڽ�Obj�� ��ũ��ƮoptionValue�� Value�� optionNum�� ���ϰ� �װſ� ���� LocalManager���� �Ҹ��� ������
    {
        if(optionNum == 0)
        {
            LocalManager.MasterVol = values[0].Value;
            LocalManager.MusicVol = values[1].Value;            //�Ϸ�����
            LocalManager.SfxVol = values[2].Value;
        }
    }
    public void ResetOption()
    {

    }
}
