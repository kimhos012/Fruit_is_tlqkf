using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISys : MonoBehaviour
{

    public GameObject CharC;

    public void EnterChar()
    {
        CharC.SetActive(true);
    }
    public void ExitChar()
    {
        CharC.SetActive(false);
    }
}
