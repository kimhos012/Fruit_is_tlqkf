using UnityEngine;
using Photon.Pun;
public class PlayerValue : MonoBehaviour
{
    public int mapScore;
    public int number;
    private void Awake() => DontDestroyOnLoad(gameObject);

    void LateUpdate()
    {
        if(number==0)
        {
            number = PhotonNetwork.PlayerList.Length;
        }
    }
}
