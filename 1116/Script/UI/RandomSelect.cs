using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Xml.Serialization;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class RandomSelect : MonoBehaviour
{
    public string[] mapList = { "NamB", "BooAuk", "SinkDeah" };
    public RectTransform selectimage;
    [Space(10f)]
    public GameObject[] mapPos;
    float Xpos;

    private void Start()
    {
        Debug.Log(mapList[0]);
        StartCoroutine(SelectRandom());
        Xpos = selectimage.anchoredPosition.x;
        Debug.Log(Xpos);
    }

    IEnumerator SelectRandom()
    {
        var wait = new WaitForSeconds(1f);
        while (true)
        {
            Debug.Log("ChangePos");
            for (int i = 0; i < mapPos.Length; i++)
            {
                Xpos = selectimage.GetComponent<RectTransform>().anchoredPosition.x; = mapPos[i].GetComponent<RectTransform>().anchoredPosition.x;
                yield return wait;
            }
            yield return wait;
        }
    }
}
