using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.Vehicles.Car;

public class RandomSelect : MonoBehaviour
{
    public string[] mapList = { "BooAuk", "NamB", "SinkDeah" };
    public GameObject selectimage;
    [Space(10f)]
    public GameObject[] mapPos;
    public int min;
    public int max;

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
    int currentInt;

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
            ap = Random.Range(min, max);
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
        zoomMap.SetActive(false);
    }
    [PunRPC]
    public void RandomWaitCount(int righTime)
    {
        StartCoroutine(SelectRandom(righTime));

    }
    IEnumerator SelectRandom(int righTime)
    {

        var wait = new WaitForSeconds(0.2f);
        int Zero = 0;
        Debug.Log("선택하는맵");
        while(righTime > 0)
        {
            Debug.Log("Lefttime" + righTime);
            if (Zero < mapList.Length - 1)
            {
                Zero++;
            }
            else
            {
                Zero = 0;
            }
            selectimage.GetComponent<RectTransform>().anchoredPosition = mapPos[Zero].GetComponent<RectTransform>().anchoredPosition;            
            Debug.Log(Zero);
            righTime--;
            yield return wait;
        }
        string currentScene = SceneManager.GetActiveScene().name;
        switch(currentScene)
        {
            case "BooAuk":
                currentInt = 0;
                break;
            case "NamB":
                currentInt = 1;
                break;
            case "SinkDeah":
                currentInt = 2;
                break;
        }
        if (currentInt == Zero)         //맵이 이전과 같으면
        {
            Debug.Log("같은맵+1한다");
            if (Zero < mapList.Length - 1)
            {
                Zero++;
            }
            else
            {
                Zero = 0;
            }
            selectimage.GetComponent<RectTransform>().anchoredPosition = mapPos[Zero].GetComponent<RectTransform>().anchoredPosition;
            Debug.Log(Zero);
        }

        yield return new WaitForSeconds(1f);
        Debug.Log("이제곧시작");
        if(PhotonNetwork.IsMasterClient)
        {
            Debug.Log("MapNumber" + Zero + " " + mapList[Zero]);
            pv.RPC("ShowMap" , RpcTarget.AllBuffered , Zero);
            yield return new WaitForSeconds(3f);
            PhotonNetwork.LoadLevel(mapList[Zero]);
            zoomMap.SetActive(false);
            uiSys.RandomToggle(false);
        }
        else
        {
            yield return new WaitForSeconds(3f);
            zoomMap.SetActive(false);
            uiSys.RandomToggle(false);
        }
        

    }
    [PunRPC]
    public void ShowMap(int numb)
    {
        zoomMap.SetActive(true);
        zoomText.text = text[numb];
        zoomImage.sprite = image[numb];
        IM.sprite = mm[numb];
        
    }
}
