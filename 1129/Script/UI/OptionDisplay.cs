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


    public void ButtonChange(int optionNum)             //��ư �� �����ϱ�
    {
        for(int i = 0;i<optionButton.Length;i++)
        {
            optionButton[i].GetComponent<Image>().color = new Color(1,1,1,1);            //�ϴ� ��� ����â�� �� ����
        }

        optionButton[optionNum].GetComponent<Image>().color = new Color(0.7f, 0.7f, 0.7f, 1);         //������ �ɼ�â�� ����
    }

    public void ViewOption(int optionNum)              //�������� �ɼ� ���̱�
    {
        for (int i = 0; i < optionButton.Length; i++)
        {
            optionLine[i].SetActive(false);
        }

        optionLine[optionNum].SetActive(true);
    }

    public void SaveOption()            //�ɼ��� ����        ����� ����1�� �ִ� �ڽ�Obj�� ��ũ��ƮoptionValue�� Value�� optionNum�� ���ϰ� �װſ� ���� LocalManager���� �Ҹ��� ������
    {

    }
    public void ResetOption()
    {

    }
}
