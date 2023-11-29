
using Photon.Pun;
using UnityEngine;

public class Mesh : MonoBehaviour
{
    PlayerData playerData;
    Controller controller;
    Animator animator;
    public GameObject[] PlayerMesh;
    PhotonView pv;
    private void OnDisable()
    {
        this.GetComponent<Mesh>().enabled = true;
    }
    private void Start()
    {
        controller = GetComponent<Controller>();
        playerData = GetComponent<PlayerData>();
        pv = GetComponent<PhotonView>();
        for(int i = 0; i < PlayerMesh.Length; i++) { PlayerMesh[i].SetActive(false); }
    }
    int chNum = 0;
    private void Update()
    {
        UpdateMesh();
        //pv.RPC("UpdateMesh", RpcTarget.Others);
    }
    CharType ct;
    [PunRPC]
    void UpdateMesh()
    {
        if(ct != playerData.charactorType)
        {
            for (int i = 0; i < PlayerMesh.Length; i++) { PlayerMesh[i].SetActive(false); }
            switch (playerData.charactorType)
            {
                case CharType.포도:
                    PlayerMesh[0].SetActive(true);
                    animator = PlayerMesh[0].GetComponent<Animator>();
                    break;

                case CharType.체리:
                    PlayerMesh[1].SetActive(true);
                    animator = PlayerMesh[1].GetComponent<Animator>();
                    break;

                case CharType.귤:
                    PlayerMesh[2].SetActive(true);
                    animator = PlayerMesh[2].GetComponent<Animator>();
                    break;

                case CharType.양파:
                    PlayerMesh[3].SetActive(true);
                    animator = PlayerMesh[3].GetComponent<Animator>();
                    break;

                case CharType.고추:
                    PlayerMesh[4].SetActive(true);
                    animator = PlayerMesh[4].GetComponent<Animator>();
                    break;

                case CharType.고구마:
                    PlayerMesh[5].SetActive(true);
                    animator = PlayerMesh[5].GetComponent<Animator>();
                    break;
            }
            ct = playerData.charactorType;
        }
        ///Amimation editer
        MoveAnimation(controller.moveDir != Vector3.zero);
    }

    public void MoveAnimation(bool move)
    {
        animator.SetBool("Move", move);
    }
    public void AtkAnimation()
    {
        animator.SetTrigger("Atk");
    }
    public void JumpAnmation()
    {
        animator.SetTrigger("Jump");
    }

}
