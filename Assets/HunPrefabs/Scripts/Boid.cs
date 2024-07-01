using UnityEngine;
using System.Collections;

public class Boid
{
    Vector3 currentVelocity; // 현재 속도 벡터
    public GameObject body; // 보이드의 실제 게임 오브젝트
    public Vector3 velocity = Vector3.zero; // 보이드의 속도 벡터
    public int ID; // 보이드의 고유 ID
    public bool enemy = false;
    public ShootPoint shootPoint; // 총알 발사 위치
    public bool isBullet = false;

    public bool setParent = false;
    Vector3 referencePoint = new Vector3(0, 20000, 0);

    public Boid(int BoidID)
    {
        // 보이드 생성자
        body = (GameObject)Object.Instantiate(Control.physBoid, new Vector3(Random.Range(-5000, 5000f), Random.Range(20041.1f, 23341.1f), Random.Range(-5000, 5000f)), Quaternion.identity);
        ID = BoidID; // 고유 ID 설정
        body.name = "Boid " + ID; // 게임 오브젝트 이름 설정
        velocity = RandomVector(2f); // 초기 랜덤 속도 설정
        ShootPoint ShootPoint = body.GetComponentInChildren<ShootPoint>();
        if (ShootPoint != null)
        {
            shootPoint = ShootPoint;
        }
    }

    public void UpdateBoid()
    {
        // 매 프레임 업데이트 메서드
        currentVelocity = Vector3.Lerp(currentVelocity, velocity, Time.deltaTime * 4);
        if (body != null)
        {
            body.transform.position += currentVelocity * Time.deltaTime; // 실제 위치 이동

            if (velocity.magnitude > 2)
            {
                velocity = velocity.normalized * 2;
            }

            // 원점으로부터의 거리 계산
            Vector3 difference = body.transform.position - referencePoint;
            float distance = difference.magnitude;

            // 최대 거리 초과 시 위치 재조정
            if (distance > Control.maxDistanceFromOrigin)
            {
                Vector3 direction = (body.transform.position - referencePoint).normalized;
                body.transform.position = referencePoint + direction * Control.maxDistanceFromOrigin;
            }

            Debug.DrawLine(body.transform.position, body.transform.position + velocity, Color.red);

            if (!enemy)
            {
                // 목표 방향 계산
                Vector3 targetDirection = (body.transform.position + velocity) - body.transform.position;

                // 부드러운 회전을 위한 보간
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                body.transform.rotation = Quaternion.Slerp(body.transform.rotation, targetRotation, Time.deltaTime);
            }
        }
    }

    public void LookAt(Vector3 enemyPosition)
    {
        if (body != null)
        {
            body.transform.LookAt(enemyPosition); // 적 위치 바라보기
        }
    }

    public Vector3 position
    {
        get
        {
            if (body != null)
            {
                return body.transform.position;
            }
            else
            {
                Debug.LogWarning("Boid body is destroyed or null.");
                return Vector3.zero; // 또는 적절한 기본값 반환
            }
        }
    }

    public void DestroyBoid()
    {
        if (body != null)
        {
           body.SetActive(false);
        }
    }

    Vector3 RandomVector(float minMax)
    {
        return new Vector3(Random.Range(-minMax, minMax), Random.Range(-minMax, minMax), Random.Range(-minMax, minMax));
    }
}
