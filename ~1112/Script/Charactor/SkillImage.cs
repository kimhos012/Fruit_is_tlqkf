using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillImage : MonoBehaviour
{
    public int SkillNum;

    public Sprite[] charSkillImg;
    Image Img;

    private void Start()
    {
        Img = GetComponent<Image>();
    }
    private void Update()
    {
        Img.sprite = charSkillImg[SkillNum]; 
    }
}
