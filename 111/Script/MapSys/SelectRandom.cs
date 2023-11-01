using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectRandom : MonoBehaviour
{

    public Image Select1 , Select2 , Select3 , Select4 , Select5;

    int Map1, Map2, Map3, Map4, Map5;

    public void Start()
    {
        Map1 = Random.Range(0, 5);
        Map2 = Random.Range(0, 5);
        Map3 = Random.Range(0, 5);
        Map4 = Random.Range(0, 5);
        Map5 = Random.Range(0, 5);

        MultiplayerSystem.MapN = (Map1 + Map2  + Map3  + Map4  + Map5).ToString();
    }
}
