using System.Collections;
using UnityEngine;

public class ControlBall : MonoBehaviour
{
    public float maxDistance = 3000f; // �ִ� �̵� �Ÿ� ����
    public float miny = 0;
    public float maxy = 0;

    void Start()
    {
        // ���� ��ġ ����
        transform.position = new Vector3(0f, 22041.1f, 0f);
        StartCoroutine(MoveRandomlyWithinMaxDistance());
    }

    IEnumerator MoveRandomlyWithinMaxDistance()
    {
        while (true)
        {
            // ���� ���� ���� ���� (���� ǥ�鿡�� �����ϰ� ������ ���� ����)
            Vector3 randomDirection = Random.insideUnitSphere;

            // ���� �������� ���� �Ÿ���ŭ �̵�
            Vector3 randomPosition = transform.position + randomDirection * Random.Range(0, maxDistance);

            // �ƽ� �Ÿ� ���� ��ġ�ϵ��� ����
            randomPosition = Vector3.ClampMagnitude(randomPosition, maxDistance);

            // �� ��ġ�� �̵�
            transform.position = randomPosition;
            float posY = Random.Range(miny, maxy);
            transform.position = new Vector3(transform.position.x, posY, transform.position.z);

            yield return new WaitForSeconds(20f); // 5�ʸ��� �̵�
        }
    }
}
