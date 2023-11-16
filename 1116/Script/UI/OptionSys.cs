using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class OptionSys : MonoBehaviour
{
    public GameObject[] OptionNum;
    public GameObject[] OptionButton;
    int maxLength;
    void Start()
    {
        maxLength = OptionNum.Length;
    }
    public void ButtonChange()
    {
        for(int i = 0; i < maxLength; i++)
        {
            OptionNum[i].SetActive(false);
        }
        OptionNum[maxLength].SetActive(true);
    }

    public void SaveOption()
    {

    }
    public void RevertOption()
    {

    }
    public void ResetOPtion()
    {

    }
}
