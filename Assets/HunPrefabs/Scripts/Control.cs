using UnityEngine;
using System.Collections;
using UnityEngine.VFX;

public class Control : MonoBehaviour
{
    public delegate Vector3 UFOBodyDelegate();
    private UFOBodyDelegate ufobodyCallback;
    public UFOBodyDelegate UFOBodyCallback
    {
        set { ufobodyCallback = value; }
    }
    public UFOBody[] ufobody;
    public Boid[] boids;
    public static Vector3 centreOfMass = Vector3.zero;
    public static Vector3 globalVelocity = Vector3.zero;
    public GameObject COMSphere;
    public GameObject controlBall;
    public GameObject enemy;
    public UFOBullet bullet;

    public static int numberOfBoids = 10;

    public float centreOfMassController;
    public float distanceController;
    public float velocityController;
    public float controLBallController;

    public static GameObject physBoid;
    public GameObject boidBody;

    Vector3 tempVector;
    float maxProxyValue = 1000;

    Vector3 c1;
    Vector3 c2;
    Vector3 c3;
    Vector3 c4;

    public static float maxDistanceFromOrigin = 10000f;

    public float sensitivity = 10;
    public float heightRot = 0;
    public float yRot = 0;
    private bool fiver = false;

    public float minDistanceToControlBall = 5f;
    public float maxDistanceToControlBall = 10f;
    public float minDistanceToEnemy = 50f;
    public float maxDistanceToEnemy = 100f;

    private WaitForSeconds shootSpeed = new WaitForSeconds(1f);
    private VisualEffect[] vf;

    void Start()
    {
        physBoid = boidBody;

        boids = new Boid[numberOfBoids];
        vf = new VisualEffect[numberOfBoids];
        ufobody = new UFOBody[numberOfBoids];

        for (int b = 0; b < numberOfBoids; b++)
        {
            boids[b] = new Boid(b);
            //Debug.Log(boids[b].body.name);
            ufobody[b] = boids[b].body.gameObject.GetComponent<UFOBody>();

            if (boids[b].shootPoint != null)
            {
                VisualEffect effect = boids[b].shootPoint.GetComponentInChildren<VisualEffect>();
                if (effect != null)
                {
                    vf[b] = effect;
                    vf[b].Stop();
                }
            }
        }
    }

    public void SetMat(Material mat)
    {
        foreach(Boid boid in boids) 
        {
            Renderer[] boidmat = boid.body.GetComponentsInChildren<Renderer>();
            boidmat[2].material = mat;
            boidmat[5].material = mat;
        }

    }


    void Update()
    {
        UpdateConstants(); // 상수 업데이트
        if (COMSphere != null)
        {
            COMSphere.transform.position = centreOfMass; // 질량 중심 Sphere 위치 업데이트
        }

        // 모든 보이드 업데이트
        for (int b = 0; b < numberOfBoids; b++)
        {
            if (boids[b] == null || boids[b].body == null)
            {
                continue;
            }

            if (!boids[b].setParent)
            {
                boids[b].body.transform.SetParent(transform);
                
                boids[b].setParent = true;
            }

            if (ufobody[b].hp <= 10)
            {
                if (shootSpeed == new WaitForSeconds(1))
                    shootSpeed = new WaitForSeconds(0.5f);
                if (!fiver)
                    fiver = true;
            }

            if (ufobody[b].hp <= 0)
            {
                boids[b].DestroyBoid();
                RespawnBoid(b);
                continue; // 보이드가 파괴된 경우 다음 보이드로 넘어감
            }

            c1 = PercievedCentreOfMass(b); // 인지된 질량 중심 계산
            c2 = PercievedVelocity(b); // 인지된 전역 속도 계산
            c3 = BoidRepulsion(b); // 보이드 추방 계산
            c4 = IsCollision(b); // 충돌 계산

            float intensity = c3.magnitude;
            if (intensity > maxProxyValue) maxProxyValue = intensity;
            float dotProduct = Vector3.Dot((enemy.transform.position - boids[b].position).normalized, boids[b].velocity.normalized);
            float distanceToEnemy = Vector3.Distance(boids[b].position, enemy.transform.position);

            if (0.3f < dotProduct && dotProduct < 0.7f && distanceToEnemy < maxDistanceToEnemy)
            {
                boids[b].enemy = true;
            }
            else if (distanceToEnemy > maxDistanceToEnemy + 100)
            {
                boids[b].enemy = false;
                boids[b].isBullet = false;
                vf[b].Stop();
                StopCoroutine(BulletIn(b));
            }

            if (boids[b].enemy)
            {
                Vector3 followEnemyVec = FollowEnemy(b);
                if (distanceToEnemy < maxDistanceToEnemy - 50)
                {
                    if (!boids[b].isBullet)
                    {
                        StartCoroutine(BulletIn(b));
                    }

                    followEnemyVec = -(enemy.transform.position - boids[b].position).normalized;
                }
                else if (distanceToEnemy > minDistanceToEnemy + 100)
                {
                    followEnemyVec = (enemy.transform.position - boids[b].position).normalized;
                }

                boids[b].velocity = fiver ? (boids[b].velocity + c1 + c2 + c3 + c4 + followEnemyVec) * 0.5f : (boids[b].velocity + c1 + c2 + c3 + c4 + followEnemyVec) * 0.1f;
                SmoothLookAt(boids[b], enemy.transform.position); // 부드럽게 회전하도록 수정
                boids[b].UpdateBoid();
            }

            if (!boids[b].enemy)
            {
                Vector3 followBallVec = FollowBall(b);
                float distanceToControlBall = Vector3.Distance(boids[b].position, controlBall.transform.position);

                if (distanceToControlBall < minDistanceToControlBall)
                {
                    followBallVec = -(controlBall.transform.position - boids[b].position).normalized * controLBallController;
                }
                else if (distanceToControlBall > maxDistanceToControlBall)
                {
                    followBallVec = (controlBall.transform.position - boids[b].position).normalized * controLBallController;
                }
                else
                {
                    followBallVec = Vector3.zero;
                }

                boids[b].velocity = fiver ? (boids[b].velocity + c1 + c2 + c3 + c4 + followBallVec) * 2f : (boids[b].velocity + c1 + c2 + c3 + c4 + followBallVec);
                SmoothLookAt(boids[b], controlBall.transform.position); // 부드럽게 회전하도록 수정
                boids[b].UpdateBoid();
            }
        }
    }

    void SmoothLookAt(Boid boid, Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - boid.body.transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            boid.body.transform.rotation = Quaternion.Slerp(boid.body.transform.rotation, targetRotation, Time.deltaTime * sensitivity);
        }
    }


    private IEnumerator BulletIn(int boidID)
    {
        boids[boidID].isBullet = true;
        while (true)
        {
            yield return shootSpeed;

            if (boids[boidID] == null || boids[boidID].body == null)
            {
                yield break;
            }

            if (boids[boidID].isBullet && boids[boidID].enemy)
            {
                Vector3 shootDirection = (enemy.transform.position - boids[boidID].shootPoint.transform.position).normalized;
                Quaternion shootRotation = Quaternion.LookRotation(shootDirection);
                ObjectPooler.SpawnFromPool("UFOBullet", boids[boidID].shootPoint.transform.position, boids[boidID].body.transform.rotation);
                vf[boidID].Play();

            }
        }
    }


    void UpdateConstants()
    {
        tempVector = Vector3.zero;
        for (int j = 0; j < numberOfBoids; j++)
        {
            if (boids[j].body != null)
            {
                tempVector += boids[j].position;
            }
        }
        centreOfMass = tempVector / numberOfBoids;
        tempVector = Vector3.zero;

        for (int b = 0; b < numberOfBoids; b++)
        {
            if (boids[b].body != null)
            {
                tempVector += boids[b].velocity;
            }
        }
        tempVector = tempVector / numberOfBoids;
        globalVelocity = tempVector;
    }

    public Vector3 FollowEnemy(int boidID)
    {
        return (enemy.transform.position - boids[boidID].position) * controLBallController * reverseSigmoid(Vector3.Distance(boids[boidID].position, enemy.transform.position));
    }

    public Vector3 FollowBall(int boidID)
    {
        return (controlBall.transform.position - boids[boidID].position) * controLBallController * reverseSigmoid(Vector3.Distance(boids[boidID].position, controlBall.transform.position));
    }

    public Vector3 BoidRepulsion(int boidID)
    {
        Vector3 returnVect = Vector3.zero;

        for (int b = 0; b < numberOfBoids; b++)
        {
            if ((b != boidID) && (boids[b].body != null) && ((boids[boidID].position - boids[b].position).magnitude < distanceController))
            {
                returnVect = returnVect - (boids[b].position - boids[boidID].position);
            }
        }
        return returnVect;
    }

    public Vector3 IsCollision(int boidID)
    {
        Vector3 returnVect = Vector3.zero;
        Boid boid = boids[boidID];
        UFOBody UFOCollision = boid.body.GetComponent<UFOBody>();

        if (UFOCollision != null && UFOCollision.isCol && ufobodyCallback != null)
        {
            Vector3 colPos = ufobodyCallback.Invoke();
            returnVect = returnVect - (colPos - boid.position);
        }

        return returnVect;
    }

    public Vector3 OtherCollisionPos(Vector3 pos)
    {
        return pos;
    }

    public Vector3 PercievedCentreOfMass(int boidID)
    {
        Vector3 COMP = (centreOfMass - (boids[boidID].position / numberOfBoids));
        return (COMP - boids[boidID].position) * centreOfMassController;
    }

    public Vector3 PercievedVelocity(int boidID)
    {
        return velocityController * (globalVelocity - (boids[boidID].velocity / numberOfBoids));
    }

    float reverseSigmoid(float x)
    {
        return 1 / Mathf.Exp(x / Mathf.Abs(distanceController));
    }
    void RespawnBoid(int boidID)
    {
        // 보이드를 리스폰하는 메서드입니다.
        // 필요에 따라 리스폰 로직을 커스터마이즈할 수 있습니다.
        // 여기서는 기본적으로 보이드를 초기화하고 원점에 위치시키는 예시를 보여줍니다.
        if(boids[boidID].body.activeSelf == false)
        {
            ufobody[boidID].hp = 40;
            boids[boidID].body.SetActive(true);
            boids[boidID].body.transform.position = new Vector3(Random.Range(0,10000), Random.Range(21000, 23000), Random.Range(0, 10000));
        }
            
    }

}
