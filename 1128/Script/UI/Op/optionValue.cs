using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class optionValue : MonoBehaviour
{
    public int optionNum;       //���� ��ȣ
    [Space(10f)]
    [Range(0 , 10)]
    public int Value;

    public void ValueUpdate(bool Add)
    {
        Value = Add == true ? Value++ : Value--;            //Add == true �� Value++ �ƴϸ� Value--
    }
}
