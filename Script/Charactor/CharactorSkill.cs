using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactorSkill : MonoBehaviour
{
    public CharType charactorType;

    private void Start()
    {
        this.gameObject.transform.GetChild(1).transform.GetChild(0).gameObject.SetActive(false);
        this.gameObject.transform.GetChild(1).transform.GetChild(1).gameObject.SetActive(false);
        this.gameObject.transform.GetChild(1).transform.GetChild(2).gameObject.SetActive(false);

        Check();
    }

    public void Check()
    {
        switch(charactorType)
        {
            case CharType.°íÃß:
                this.gameObject.transform.GetChild(1).transform.GetChild(0).gameObject.SetActive(true);
                this.gameObject.transform.GetChild(1).transform.GetChild(1).gameObject.SetActive(false);
                this.gameObject.transform.GetChild(1).transform.GetChild(2).gameObject.SetActive(false);
                break;
            case CharType.¾çÆÄ:
                this.gameObject.transform.GetChild(1).transform.GetChild(0).gameObject.SetActive(false);
                this.gameObject.transform.GetChild(1).transform.GetChild(1).gameObject.SetActive(true);
                this.gameObject.transform.GetChild(1).transform.GetChild(2).gameObject.SetActive(false);
                break;
            case CharType.±Ö:
                this.gameObject.transform.GetChild(1).transform.GetChild(0).gameObject.SetActive(false);
                this.gameObject.transform.GetChild(1).transform.GetChild(1).gameObject.SetActive(false);
                this.gameObject.transform.GetChild(1).transform.GetChild(2).gameObject.SetActive(true);
                break;
        }
    }
    public int skillCooldown;

}
public enum CharType
{
    °íÃß,
    ¾çÆÄ,
    ±Ö
}