using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Integer2Image : MonoBehaviour
{

    public Sprite a;
    public Sprite b;
    public Sprite c;
    public Sprite d;
    public Sprite e;
    public Sprite f;

    [HideInInspector]
    public int Count;

    Image img;
    private void Start()
    {
        img = this.GetComponent<Image>();
    }
    void Update()
    {


        switch (Count)
        {

            case 0:
                img.sprite = a;
                break;
            case 1:
                img.sprite = b;
                break;
            case 2:
                img.sprite = c;
                break;
            case 3:
                img.sprite = d;
                break;
            case 4:
                img.sprite = e;
                break;
            case 5:
                img.sprite = f;
                break;
        }
    }
}
