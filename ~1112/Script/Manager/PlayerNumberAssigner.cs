using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PlayerNumberAssigner : MonoBehaviourPunCallbacks
{
    private static readonly int MaxPlayers = 4;
    public int MaxP;
    int assignedNumber;


    public override void OnJoinedRoom()
    {
        AssignPlayerNumber(PhotonNetwork.LocalPlayer);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AssignPlayerNumber(newPlayer);
    }

    private void AssignPlayerNumber(Player player)
    {
        // ���� �뿡 �ִ� �÷��̾� ��
        int currentPlayerCount = PhotonNetwork.CurrentRoom.PlayerCount;

        // �ִ� �÷��̾� ���� �ʰ����� �ʴ� ��쿡�� ��ȣ �Ҵ�
        if (currentPlayerCount <= MaxPlayers)
        {
            assignedNumber = currentPlayerCount - 1; // 0���� �����ϵ��� ��ȣ �ο�
            player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "PlayerNumber", assignedNumber } });

            Debug.Log($"{player.NickName} assigned to Player Number {assignedNumber}");
        }
        else
        {
            Debug.LogWarning("Room is already full.");
        }


    }
}