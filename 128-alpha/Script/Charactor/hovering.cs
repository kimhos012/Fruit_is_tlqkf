using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class hovering : MonoBehaviour
{
    PlayerData playerData;

    MeshRenderer meshRenderer;
    public Material[] mat;
    private void Start()
    {
        playerData = transform.GetComponentInParent<PlayerData>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    void FixedUpdate()
    {
        transform.localEulerAngles += new Vector3(0, 1, 0);
        if(playerData.playerNum > 0)
            meshRenderer.material = mat[playerData.playerNum - 1];
    }
}
