using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISys : MonoBehaviour
{
    public GameObject CharC;
    public GameObject Option;
    public GameObject MainMenu;         //Array�� ���ϴ� ���� = ���ϰ� �����
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
    #region �ٸ� ��ũ��Ʈ���� �ϴ� �������� ���⼭ �۵��մϴ�.
    public void CharToggle(bool Yes)
    {
        CharC.SetActive(Yes);
    }
    //ĳ���� ��ȯ Toggle

    public void SettingToggle(bool Yes)
    {
        Option.SetActive(Yes);
    }
    //���� Toggle

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
