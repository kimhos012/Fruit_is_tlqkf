using UnityEngine;

public class RandomSpawn : MonoBehaviour
{
    Vector3 axis;

    public float Max;         //�����ð� ������ ����
    public float Min;
    float Xmax;
    float Zmax;
    float Xmin;
    float Zmin;

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
        if (SpawnObject.Length == 0) { Debug.Log("�����ҰԾ�� ������"); return; }

        Xmax = SpawnMaxAxis.GetComponent<Transform>().position.x;
        Zmax = SpawnMaxAxis.GetComponent<Transform>().position.z;      //���� �ִ� ��ǥ
        Xmin = SpawnMinAxis.GetComponent<Transform>().position.x;
        Zmin = SpawnMinAxis.GetComponent<Transform>().position.z;      //���� �ּ� ��ǥ

        InvokeRepeating("Spawn", 3, Random.Range(Min, Max));
    }
    void Spawn()                //�����۾�
    {
        sCount = GameObject.FindGameObjectsWithTag("MapObs");
        if (sCount.Length < limitSpawn || limitSpawn == 0)
        {

            axis = new Vector3(Random.Range(Xmin, Xmax), gameObject.transform.position.y, Random.Range(Zmin, Zmax));

            int randomIndex = Random.Range(0, SpawnObject.Length);
            Instantiate(SpawnObject[randomIndex], axis, Quaternion.identity);
            Debug.Log("asdasd");

        }
    }
}
