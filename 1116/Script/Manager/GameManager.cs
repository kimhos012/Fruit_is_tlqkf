using Photon.Pun;
using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private void Start()
    {
        //Create();
        SetPlayerPos();   
    }
    [PunRPC]
    void SetPlayerPos()
    {
        // ���� ��ġ ������ �迭�� ����
        Transform[] points = GameObject.Find("Spawnpoint").GetComponentsInChildren<Transform>();


        GameObject[] Player = GameObject.FindGameObjectsWithTag("Player");
        for(int i=0;i<Player.Length; i++)
        {
            
            Player[i].transform.position = points[i].position;
            
        }
    }
 
}