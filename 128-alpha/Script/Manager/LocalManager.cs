using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalManager : MonoBehaviour
{
    public static int MaxRound = 3;

    public static float MasterVol = 0f;
    public static float MusicVol = 0f;
    public static float SfxVol = 0f;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
