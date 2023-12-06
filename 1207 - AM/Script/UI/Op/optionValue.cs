using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class optionValue : MonoBehaviour
{
    public int optionNum;       //설정 번호
    [Space(10f)]
    [Range(0, 100)]

    public float Value;

    public Slider slider;
    public Text text;

    public void ValueButton(bool Add)
    {
        Value = Add == true ? Value++ : Value--;            //Add == true 면 Value++ 아니면 Value--
    }

    private void Start()
    {
        Debug.Log(optionNum.ToString() + " , " + Value);
        slider.onValueChanged.AddListener(OnValueChange);

        slider.value = Value/100;
    }
    void OnValueChange(float changeValue)
    {
        Value = (float)Math.Truncate(changeValue * 100);
        text.text = Value.ToString();
    }
}
