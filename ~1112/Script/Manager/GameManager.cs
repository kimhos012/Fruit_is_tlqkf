using Photon.Pun;
using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private void Start()
    {
        Create();
        
    }

    void Create()
    {
        // 출현 위치 정보를 배열에 저장
        Transform[] points = GameObject.Find("Spawnpoint").GetComponentsInChildren<Transform>();
        int idx = UnityEngine.Random.Range(1, points.Length);

        // 네트워크상에 캐릭터 생성
        GameObject pLaYeR = PhotonNetwork.Instantiate("Player", points[idx].position, points[idx].rotation, 0);
        pLaYeR.GetComponent<PlayerValue>().number = MultiplayerSystem.assignedNumber;
    }
 
}