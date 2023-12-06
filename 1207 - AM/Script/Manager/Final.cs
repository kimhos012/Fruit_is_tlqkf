using Photon.Pun;
using UnityEngine;

public class Final : MonoBehaviour
{
    ShowScoreBorad showPlayer;
    Transform[] Pos;

    private void Update()
    {
        Pos = GameObject.Find("Spawnpoint").GetComponentsInChildren<Transform>();

        GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject pl in player)
        {
            PhotonView pv = pl.GetComponent<PhotonView>();
            if(pv.IsMine)
            {
                pl.transform.position = Pos[1].position;
                pl.transform.rotation = Pos[1].rotation;
            }
            else
            {
                pl.transform.position = Pos[0].position;
                pl.transform.rotation = Pos[0].rotation;
            }
        }
    }
}
