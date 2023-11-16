using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISys : MonoBehaviour
{
    public GameObject CharC;
    public GameObject Option;
    public GameObject MainMenu;         //Array로 안하는 이유 = 편하게 볼라고
    public GameObject NameManu;
    public GameObject RandomMenu;

    private void Start()
    {
        CharC.SetActive(false);
        Option.SetActive(false);
        MainMenu.SetActive(false);
        RandomMenu.SetActive(false);
        NameManu.SetActive(true);
    }
    #region 다른 스크립트에서 하는 설정값은 여기서 작동합니다.
    public void CharToggle(bool Yes)
    {
        CharC.SetActive(Yes);
    }
    //캐릭터 변환 Toggle

    public void SettingToggle(bool Yes)
    {
        Option.SetActive(Yes);
    }
    //설정 Toggle

    public void MainToggle(bool Yes)
    {
        MainMenu.SetActive(Yes);
    }
    //
    public void RandomToggle(bool Yes)
    {
        RandomMenu.SetActive(Yes);
    }
    //
    public void NameToggle(bool Yes)
    {
        NameManu.SetActive(Yes);
    }
    //
    public void OptionToggle(bool Yes)
    {
        Option.SetActive(Yes);
    }
    #endregion
}
