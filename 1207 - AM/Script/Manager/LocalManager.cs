using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalManager : MonoBehaviour
{
    public static int MaxRound = 3;

    public static float MasterVol = 100;
    public static float MusicVol = 100;
    public static float SfxVol = 100;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
