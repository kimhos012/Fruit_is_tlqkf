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
        // ���� ��ġ ������ �迭�� ����
        Transform[] points = GameObject.Find("Spawnpoint").GetComponentsInChildren<Transform>();
        int idx = UnityEngine.Random.Range(1, points.Length);

        // ��Ʈ��ũ�� ĳ���� ����
        GameObject pLaYeR = PhotonNetwork.Instantiate("Player", points[idx].position, points[idx].rotation, 0);
        pLaYeR.GetComponent<PlayerValue>().number = MultiplayerSystem.assignedNumber;
    }
 
}