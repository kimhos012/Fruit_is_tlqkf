using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchColor : MonoBehaviour
{
    private Image toggleColor;
    private Toggle m_Toggle;

    private void Start()
    {
        toggleColor = transform.GetChild(0).GetComponent<Image>();

        m_Toggle = GetComponent<Toggle>();

        m_Toggle.onValueChanged.AddListener(delegate
            {
                ToggleValueChanged();
            }
        );
        toggleColor.color = new Color(0, 1, 0, 1);
    }



    //Output the new state of the Toggle into Text when the user uses the Toggle
    void ToggleValueChanged()
    {
        if(m_Toggle.isOn)
        {
            toggleColor.color = new Color(0, 1, 0, 1);
            Debug.Log("True");
        }
        else
        {
            toggleColor.color = new Color(1, 0, 0, 1);
            Debug.Log("False");
        }
    }
}
