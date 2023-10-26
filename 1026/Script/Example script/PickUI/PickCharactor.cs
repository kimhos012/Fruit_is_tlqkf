using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickCharactor : MonoBehaviour
{

    #region Char PickImage
    
    
    #endregion
    public GameObject Select2;
    public GameObject Garget;

    [HideInInspector]
    public bool TogglePick = false;
    [HideInInspector]
    public bool OneClick;
    [HideInInspector]
    public int Char;

    void Start()
    {
        Select2.SetActive(false);
        Garget.SetActive(false);
    }


    
    void Update()
    {


        if(TogglePick && OneClick)
        {
            Debug.Log("U Clicked Char..");
            OneClick = false;
            //Find MouseCursor and deploy Garget setting
            Select2.SetActive(true);

            Garget.SetActive(true);
            Garget.GetComponent<RectTransform>().position = Input.mousePosition;        //스크린에 있는 마우스 위치에 가젯 위치 놓기
        }
        else if(!TogglePick && !OneClick)
        {
            Debug.Log("Apple");

            Select2.SetActive(false);
            Garget.SetActive(false);

            OneClick =true;
        }
    }


    public void Click1()
    {
        Char = 0;
        ClickToggle();
    }
    public void Click2()
    {
        Char = 1;
        ClickToggle();
    }
    public void Click3()
    {
        Char = 2;
        ClickToggle();
    }
    public void Click4()
    {
        Char = 3;
        ClickToggle();
    }
    public void Click5()
    {
        Char = 4;
        ClickToggle();
    }
    public void Click6()
    {
        Char = 5;
        ClickToggle();
    }

    public void ClickToggle()
    {
        if(TogglePick) { TogglePick = false; }
        else TogglePick = true;
    }
}
