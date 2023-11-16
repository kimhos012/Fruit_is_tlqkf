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
        // 현재 룸에 있는 플레이어 수
        int currentPlayerCount = PhotonNetwork.CurrentRoom.PlayerCount;

        // 최대 플레이어 수를 초과하지 않는 경우에만 번호 할당
        if (currentPlayerCount <= MaxPlayers)
        {
            assignedNumber = currentPlayerCount - 1; // 0부터 시작하도록 번호 부여
            player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "PlayerNumber", assignedNumber } });

            Debug.Log($"{player.NickName} assigned to Player Number {assignedNumber}");
        }
        else
        {
            Debug.LogWarning("Room is already full.");
        }


    }
}