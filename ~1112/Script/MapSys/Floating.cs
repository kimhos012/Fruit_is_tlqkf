using System.Collections;
using UnityEngine;

public class Floating : MonoBehaviour
{

    public float floatingTime;
    public float CurYPos;           //�ִ�� �ö󰡴� Y���� ��

    [Header("��������")]
    public float leftTime;
    public bool sinking;

    Animator animator;

    private void Start()
    {
        leftTime = floatingTime;
        animator = transform.GetChild(0).GetComponent<Animator>();

        StartCoroutine(Float2Water());
    }
    private void FixedUpdate()          //�ִϸ��̼� ����
    {
        if (leftTime < (floatingTime * 0.5))
        {
            animator.SetBool("Shake", true);
        }
        else animator.SetBool("Shake", false);
    }
    IEnumerator Float2Water()
    {
        var waiT = new WaitForSeconds(.02f);

        while (transform.position.y < CurYPos)         //��������
        {
            gameObject.transform.position += new Vector3(0, 0.05f, 0);
            yield return waiT;
        }
        while (true)
        {
            if (sinking)                //�������� ����ɱ������ ���ð�
            {
                leftTime -= Time.deltaTime;
            }
            else
            {
                if (leftTime < floatingTime)
                    leftTime += (Time.deltaTime)/2;
            }
            if (leftTime <= 0)
            {
                for (int i = 0; i < 50; i++)
                {
                    gameObject.transform.position -= new Vector3(0, 0.05f, 0);
                    yield return waiT;
                }
                Destroy(gameObject);
            }
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            sinking = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            sinking = false;
        }
    }
}
