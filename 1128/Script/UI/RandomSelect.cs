using Photon.Pun;
using System.Collections;
using UnityEngine;

public class RandomSelect : MonoBehaviour
{
    public string[] mapList = { "NamB", "BooAuk", "SinkDeah" };
    public GameObject selectimage;
    [Space(10f)]
    public GameObject[] mapPos;
    Vector3 Xpos;
    public float min;
    public float max;

    bool select;
    int mapNum;
    [Space(10f)]
    public GameObject randomSelectDisplay;
    public GameObject zoomMap;
    PhotonView pv;
    private void Start()
    {
        pv = GetComponent<PhotonView>();
        randomSelectDisplay.SetActive(false);
    }
    public void Pick() => pv.RPC("RandomPick", RpcTarget.AllBuffered);

    [PunRPC]
    public void RandomPick()
    {
        randomSelectDisplay.SetActive(true);
        select = false;
        zoomMap.SetActive(false);
        StartCoroutine(SelectRandom());
        StartCoroutine(RandomWaitCount());
    }

    IEnumerator RandomWaitCount()
    {
        yield return new WaitForSeconds(Random.Range(min, max));
        select = true;
    }
    IEnumerator SelectRandom()
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
            Debug.Log(i);
            selectimage.GetComponent<RectTransform>().anchoredPosition = mapPos[i - 1].GetComponent<RectTransform>().anchoredPosition;
            yield return wait;
        }

        yield return new WaitForSeconds(2f);

        Debug.Log("¿Ã¡¶∞Ω√¿€");
        mapNum = i - 1;
        Debug.Log("MapNumber" + mapNum + " " + mapList[mapNum]);
        zoomMap.SetActive(true);

        yield return new WaitForSeconds(3f);
        PhotonNetwork.LoadLevel(mapList[mapNum]);
    }
}
