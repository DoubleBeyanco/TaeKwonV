using System.Collections;
using UnityEngine;

public class ControlBall : MonoBehaviour
{
    public float maxDistance = 3000f; // 최대 이동 거리 설정
    public float miny = 0;
    public float maxy = 0;

    void Start()
    {
        // 시작 위치 설정
        transform.position = new Vector3(0f, 22041.1f, 0f);
        StartCoroutine(MoveRandomlyWithinMaxDistance());
    }

    IEnumerator MoveRandomlyWithinMaxDistance()
    {
        while (true)
        {
            // 랜덤 방향 벡터 생성 (구의 표면에서 균일하게 분포된 랜덤 방향)
            Vector3 randomDirection = Random.insideUnitSphere;

            // 랜덤 방향으로 랜덤 거리만큼 이동
            Vector3 randomPosition = transform.position + randomDirection * Random.Range(0, maxDistance);

            // 맥스 거리 내에 위치하도록 조정
            randomPosition = Vector3.ClampMagnitude(randomPosition, maxDistance);

            // 새 위치로 이동
            transform.position = randomPosition;
            float posY = Random.Range(miny, maxy);
            transform.position = new Vector3(transform.position.x, posY, transform.position.z);

            yield return new WaitForSeconds(20f); // 5초마다 이동
        }
    }
}
