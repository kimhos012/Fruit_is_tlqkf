using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveOption : MonoBehaviour
{
    public static float masterVol,fxVol,musicVol;






    void Awake() => DontDestroyOnLoad(this);
}
