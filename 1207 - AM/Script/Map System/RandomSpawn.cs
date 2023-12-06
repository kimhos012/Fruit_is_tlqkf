using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class RandomSpawn : MonoBehaviour
{
    Vector3 axis;
    Vector3 receieveaxis;
    PhotonView pv;

    public float Max;         //�����ð� ������ ����
    public float Min;
    float Xmax;
    float Zmax;
    float Xmin;
    float Zmin;

    float time;
    int index;
    Vector3 pos;
    Quaternion Rot;

    public GameObject SpawnMinAxis;         //�� ��ü���� XZ��ǥ �ȿ��� �������� �����˴ϴ�. ���̴°� ���ϰ� �ϱ����� �̷� ���¸� ���߽��ϴ�.
    public GameObject SpawnMaxAxis;         //Y��ǥ�� �ʿ��ϸ� �߰��� �����Դϴ�. �����ϴ� ��ü���� 11��3�ϱ��� ���ϸ� ���� ������ ���� �����Դϴ�.
    [Tooltip("�ʿ� ������ �� �ִ� ��ü�� ������ �����մϴ�. 0�̸� ������ ������ �������ϴ�.")]
    [Range(0, 20)]
    public int limitSpawn;

    [Space(10f)]
    [Header("������ ���ǵ�")]
    public GameObject[] SpawnObject;        //������ ���ǵ�
    GameObject[] sCount;                    //������ ������ ����

    private void Start()
    {
        pv = GetComponent<PhotonView>();
        if (SpawnObject.Length == 0) { Debug.Log("�����ҰԾ�� ������"); return; }

        Xmax = SpawnMaxAxis.GetComponent<Transform>().position.x;
        Zmax = SpawnMaxAxis.GetComponent<Transform>().position.z;      //���� �ִ� ��ǥ
        Xmin = SpawnMinAxis.GetComponent<Transform>().position.x;
        Zmin = SpawnMinAxis.GetComponent<Transform>().position.z;      //���� �ּ� ��ǥ

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
            Debug.Log("��ü�� ���� ���Ƽ� �� ��ü��� ��������Ʈ�굵 �װڴ�");
        }
    }
}
