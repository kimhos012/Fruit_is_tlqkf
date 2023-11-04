using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSkillImg : MonoBehaviour
{
    public Sprite Select0;
    public Sprite Select1;
    public Sprite Select2;
    public Sprite Select3;
    public Sprite Select4;

    [Space(10f)]

    public int MaxPick = 5;
    public int MinPick = 1;

    [HideInInspector]
    public int PickWea;
    Image IMG;

    void Start()
    {
        IMG = this.GetComponent<Image>();
        PickWea = 1;
    }

    // Update is called once per frame
    void Update()
    {
        switch (PickWea)
        {
            case 1:
                IMG.sprite = Select0;
                break;
            case 2:
                IMG.sprite = Select1;
                break;
            case 3:
                IMG.sprite = Select2;
                break;
            case 4:
                IMG.sprite = Select3;
                break;
            case 5:
                IMG.sprite = Select4;
                break;
            
        }
    }

    public void Next()
    {
        if(PickWea < MaxPick)
        { PickWea++; }
    }
    public void Previous()
    {
        if(PickWea > MinPick)
        { PickWea--; }
    }
}
