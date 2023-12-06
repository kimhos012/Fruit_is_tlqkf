using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class RandomSpawn : MonoBehaviour
{
    Vector3 axis;
    Vector3 receieveaxis;
    PhotonView pv;

    public float Max;         //생성시간 랜덤값 지정
    public float Min;
    float Xmax;
    float Zmax;
    float Xmin;
    float Zmin;

    float time;
    int index;
    Vector3 pos;
    Quaternion Rot;

    public GameObject SpawnMinAxis;         //이 물체들의 XZ좌표 안에서 랜덤값이 결정됩니다. 보이는걸 편하게 하기위해 이런 형태를 취했습니다.
    public GameObject SpawnMaxAxis;         //Y좌표는 필요하면 추가할 예정입니다. 생성하는 물체들이 11월3일기준 상하를 넣을 이유가 없기 때문입니다.
    [Tooltip("맵에 존재할 수 있는 물체의 갯수를 조정합니다. 0이면 갯수에 제한이 없어집니다.")]
    [Range(0, 20)]
    public int limitSpawn;

    [Space(10f)]
    [Header("스폰할 물건들")]
    public GameObject[] SpawnObject;        //생성할 물건들
    GameObject[] sCount;                    //생성한 물건을 감지

    private void Start()
    {
        pv = GetComponent<PhotonView>();
        if (SpawnObject.Length == 0) { Debug.Log("스폰할게없어서 개삐짐"); return; }

        Xmax = SpawnMaxAxis.GetComponent<Transform>().position.x;
        Zmax = SpawnMaxAxis.GetComponent<Transform>().position.z;      //스폰 최대 좌표
        Xmin = SpawnMinAxis.GetComponent<Transform>().position.x;
        Zmin = SpawnMinAxis.GetComponent<Transform>().position.z;      //스폰 최소 좌표

        StartCoroutine(SpawnManager());
    }

    IEnumerator SpawnManager()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            pos = new Vector3(Random.Range(Xmin, Xmax), transform.position.y, Random.Range(Zmin, Zmax));
            time = Random.Range(Min, Max);
            var varTime = new WaitForSeconds(time);
            index = Random.Range(0,SpawnObject.Length);
            Vector3 Vector2Quat = new Vector3(0, Random.Range(0, 360), 0);
            Rot = Quaternion.Euler(Vector2Quat);
            yield return varTime;

            pv.RPC("Spawn", RpcTarget.AllBuffered, pos, index, Rot);
        }
        yield return null;
    }

    [PunRPC]
    public void Spawn(Vector3 position , int spawnIndex , Quaternion rotation)
    {
        GameObject[] objCount = GameObject.FindGameObjectsWithTag("MapObs");
        if(objCount.Length < limitSpawn || limitSpawn == 0)
        {
            Instantiate(SpawnObject[spawnIndex], position, rotation);
        }
        else
        {
            Debug.Log("물체가 존나 많아서 이 물체들로 에베레스트산도 쌓겠다");
        }
    }
}
