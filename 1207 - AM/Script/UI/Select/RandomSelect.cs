using JetBrains.Annotations;
using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RandomSelect : MonoBehaviour
{
    public string[] mapList = { "BooAuk", "NamB", "SinkDeah" };
    public GameObject selectimage;
    [Space(10f)]
    public GameObject[] mapPos;
    public int min;
    public int max;

    bool select;
    int mapNum;
    [Space(10f)]
    public GameObject zoomMap;

    public Text zoomText;
    [TextArea] public string[] text;
    public Image zoomImage;
    public Sprite[] image;
    [Space(10f)]
    public Image IM;
    public Sprite[] mm;

    int ap;

    PhotonView pv;
    public UISys uiSys;



    private void Start()
    {
        zoomMap.SetActive(false);

        pv = GetComponent<PhotonView>();
        if (uiSys == null)
        {
            uiSys = FindObjectOfType<UISys>();
        }
    }
    public void Pick()
    {
        uiSys.MainToggle(false);


        pv.RPC("RandomPick", RpcTarget.AllBuffered);

        if (PhotonNetwork.IsMasterClient)
        {
            ap = Random.Range(min, max + 1);
            pv.RPC("RandomWaitCount", RpcTarget.AllBuffered, ap);
        }

        Debug.Log("PickTheMap");
    }

    [PunRPC]
    public void RandomPick()
    {
        uiSys.MainToggle(false);
        uiSys.OptionToggle(false);
        uiSys.CharToggle(false);
        uiSys.RandomToggle(true);
        select = false;
        zoomMap.SetActive(false);
    }
    [PunRPC]
    public void RandomWaitCount(int righTime)
    {
        StartCoroutine(SelectRandom(righTime));

    }
    IEnumerator SelectRandom(int righTime)
    {
        int i = 0;


        var wait = new WaitForSeconds(0.2f);
        while (!select)
        {
            if (i < mapList.Length)
            {
                i++;
            }
            else
            {
                i = 1;
            }
            selectimage.GetComponent<RectTransform>().anchoredPosition = mapPos[i - 1].GetComponent<RectTransform>().anchoredPosition;
            
            if(righTime < 1)
            {
                select = true;
            }
            else
            {
                righTime--;
                yield return wait;
            }
        }
        yield return new WaitForSeconds(1f);
        Debug.Log("ÀÌÁ¦°ð½ÃÀÛ");
        mapNum = i - 1;
        Debug.Log("MapNumber" + mapNum + " " + mapList[mapNum]);
        zoomMap.SetActive(true);
        zoomText.text = text[Random.Range(1, text.Length)];
        zoomImage.sprite = image[mapNum];
        IM.sprite = mm[mapNum];
        yield return new WaitForSeconds(3f);
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(mapList[mapNum]);
        }
        zoomMap.SetActive(false);
        uiSys.RandomToggle(false);
    }
}
