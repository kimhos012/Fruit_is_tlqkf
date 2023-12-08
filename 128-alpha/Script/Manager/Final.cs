using Photon.Pun;
using UnityEngine;

public class Final : MonoBehaviourPun
{
    ShowScoreBorad showPlayer;
    Transform[] Pos;

    private void Start()
    {
        //photonView.RPC("FinalDisplay", RpcTarget.AllBuffered);
        FinalDisplay();
    }


    [PunRPC]
    public void FinalDisplay()
    {
        Pos = GameObject.Find("Spawnpoint").GetComponentsInChildren<Transform>();

        GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject pl in player)
        {

            foreach (GameObject plat in player)                    //ÁÂÇ¥ÀÌµ¿
            {
                PlayerData plData = plat.GetComponent<PlayerData>();
                plData.Hp = 120;
                plData.diee = false;
            }
            Mesh mesh = pl.GetComponent<Mesh>();
            mesh.VictoryAnimation(true);

            pl.transform.position = Pos[1].position;
            pl.transform.rotation = Pos[1].rotation;

        }
    }
}
