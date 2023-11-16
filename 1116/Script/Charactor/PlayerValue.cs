using UnityEngine;
using Photon.Pun;
public class PlayerValue : MonoBehaviour
{
    public int mapScore;
    public int number;
    [HideInInspector]
    public string NickName;
    private void Awake() => DontDestroyOnLoad(gameObject);
    void LateUpdate()
    {
        if(NickName == null)
        {
            NickName = GetComponent<PhotonView>().Controller.NickName;
        }

        if(number==0)
        {
            number = PhotonNetwork.PlayerList.Length;
        }
    }
}
