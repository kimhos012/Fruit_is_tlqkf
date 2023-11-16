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
    public GameObject zoomMap;

    private void Start()
    {
        select = false;

        zoomMap.SetActive(false);

        StartCoroutine(SelectRandom());
        Invoke("Pick", Random.Range(min, max));
    }

    IEnumerator SelectRandom()
    {
        int i = 0;
        var wait = new WaitForSeconds(0.2f);
        while (!select)
        {
            if (i< mapList.Length)
            {
                selectimage.GetComponent<RectTransform>().anchoredPosition = mapPos[i].GetComponent<RectTransform>().anchoredPosition;
                i++;
            }
            else
            {
                i = 0;
                selectimage.GetComponent<RectTransform>().anchoredPosition = mapPos[i].GetComponent<RectTransform>().anchoredPosition;
            }
            yield return wait;
        }
        Invoke("SelectMap", 2f);
        yield return null;
    }
    void Pick() => select = true;
    void SelectMap()
    {
        Debug.Log(mapNum);

        zoomMap.SetActive(true);
        Invoke("ChangeMap", 3f);
    }
    void ChangeMap()
    {
        string map;
        map = mapList[mapNum + 1];
        PhotonNetwork.LoadLevel(map);
    }
}
