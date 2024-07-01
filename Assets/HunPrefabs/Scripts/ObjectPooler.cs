using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;

// Unity 에디터에서 ObjectPooler를 커스터마이징하기 위한 에디터 스크립트
[CustomEditor(typeof(ObjectPooler))]
public class ObjectPoolerEditor : Editor
{
    // 에디터에서 보여줄 정보 상수
    const string INFO = "풀링한 오브젝트에 다음을 적으세요 \nvoid OnDisable()\n{\n" +
        "    ObjectPooler.ReturnToPool(gameObject);    // 한 객체에 한번만 \n" +
        "    CancelInvoke();    // Monobehaviour에 Invoke가 있다면 \n}";

    public override void OnInspectorGUI()
    {
        // 에디터에 도움말 상자 표시
        EditorGUILayout.HelpBox(INFO, MessageType.Info);
        base.OnInspectorGUI();
    }
}
#endif

public class ObjectPooler : MonoBehaviour
{
    // 싱글톤 인스턴스
    static ObjectPooler inst;
    void Awake() => inst = this;

    [Serializable]
    public class Pool
    {
        public string tag;         // 오브젝트 풀의 태그
        public GameObject prefab;  // 풀에 저장될 프리팹
        public int size;           // 풀의 초기 크기
    }

    [SerializeField] Pool[] pools;                        // 풀 배열
    List<GameObject> spawnObjects;                        // 생성된 오브젝트 목록
    Dictionary<string, Queue<GameObject>> poolDictionary; // 풀 딕셔너리
    readonly string INFO = " 오브젝트에 다음을 적으세요 \nvoid OnDisable()\n{\n" +
        "    ObjectPooler.ReturnToPool(gameObject);    // 한 객체에 한번만 \n" +
        "    CancelInvoke();    // Monobehaviour에 Invoke가 있다면 \n}";

    // 오브젝트를 풀에서 스폰하는 정적 메서드
    public static GameObject SpawnFromPool(string tag, Vector3 position) =>
        inst._SpawnFromPool(tag, position, Quaternion.identity);

    public static GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation) =>
        inst._SpawnFromPool(tag, position, rotation);

    // 오브젝트를 특정 컴포넌트 타입으로 스폰하는 제네릭 메서드
    public static T SpawnFromPool<T>(string tag, Vector3 position) where T : Component
    {
        GameObject obj = inst._SpawnFromPool(tag, position, Quaternion.identity);
        if (obj.TryGetComponent(out T component))
            return component;
        else
        {
            obj.SetActive(false);
            throw new Exception($"Component not found");
        }
    }

    public static T SpawnFromPool<T>(string tag, Vector3 position, Quaternion rotation) where T : Component
    {
        GameObject obj = inst._SpawnFromPool(tag, position, rotation);
        if (obj.TryGetComponent(out T component))
            return component;
        else
        {
            obj.SetActive(false);
            throw new Exception($"Component not found");
        }
    }

    // 모든 풀 오브젝트를 반환하는 정적 메서드
    public static List<GameObject> GetAllPools(string tag)
    {
        if (!inst.poolDictionary.ContainsKey(tag))
            throw new Exception($"Pool with tag {tag} doesn't exist.");

        return inst.spawnObjects.FindAll(x => x.name == tag);
    }

    public static List<T> GetAllPools<T>(string tag) where T : Component
    {
        List<GameObject> objects = GetAllPools(tag);

        if (!objects[0].TryGetComponent(out T component))
            throw new Exception("Component not found");

        return objects.ConvertAll(x => x.GetComponent<T>());
    }

    // 오브젝트를 풀로 반환하는 정적 메서드
    public static void ReturnToPool(GameObject obj)
    {
        if (!inst.poolDictionary.ContainsKey(obj.name))
            throw new Exception($"Pool with tag {obj.name} doesn't exist.");

        inst.poolDictionary[obj.name].Enqueue(obj);
    }

    // 풀 오브젝트 정보 확인을 위한 컨텍스트 메뉴
    [ContextMenu("GetSpawnObjectsInfo")]
    void GetSpawnObjectsInfo()
    {
        foreach (var pool in pools)
        {
            int count = spawnObjects.FindAll(x => x.name == pool.tag).Count;
            Debug.Log($"{pool.tag} count : {count}");
        }
    }

    // 실제 풀에서 오브젝트를 스폰하는 메서드
    GameObject _SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
            throw new Exception($"Pool with tag {tag} doesn't exist.");

        // 큐에 오브젝트가 없으면 새로 추가
        Queue<GameObject> poolQueue = poolDictionary[tag];
        if (poolQueue.Count <= 0)
        {
            Pool pool = Array.Find(pools, x => x.tag == tag);
            var obj = CreateNewObject(pool.tag, pool.prefab);
            ArrangePool(obj);
        }

        // 큐에서 오브젝트를 꺼내서 사용
        GameObject objectToSpawn = poolQueue.Dequeue();
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        objectToSpawn.SetActive(true);

        return objectToSpawn;
    }

    // 시작 시 초기화 작업
    void Start()
    {
        spawnObjects = new List<GameObject>();
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        // 미리 오브젝트를 생성하여 풀에 추가
        foreach (Pool pool in pools)
        {
            poolDictionary.Add(pool.tag, new Queue<GameObject>());
            for (int i = 0; i < pool.size; i++)
            {
                var obj = CreateNewObject(pool.tag, pool.prefab);
                ArrangePool(obj);
            }

            // OnDisable에 ReturnToPool 구현 여부와 중복 구현 검사
            if (poolDictionary[pool.tag].Count <= 0)
                Debug.LogError($"{pool.tag}{INFO}");
            else if (poolDictionary[pool.tag].Count != pool.size)
                Debug.LogError($"{pool.tag}에 ReturnToPool이 중복됩니다");
        }
    }

    // 새로운 오브젝트 생성
    GameObject CreateNewObject(string tag, GameObject prefab)
    {
        var obj = Instantiate(prefab, transform);
        obj.name = tag;
        obj.SetActive(false); // 비활성화 시 ReturnToPool을 호출하므로 Enqueue가 됨
        return obj;
    }

    // 추가된 오브젝트를 정렬하여 리스트에 삽입
    void ArrangePool(GameObject obj)
    {
        bool isFind = false;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (i == transform.childCount - 1)
            {
                obj.transform.SetSiblingIndex(i);
                spawnObjects.Insert(i, obj);
                break;
            }
            else if (transform.GetChild(i).name == obj.name)
                isFind = true;
            else if (isFind)
            {
                obj.transform.SetSiblingIndex(i);
                spawnObjects.Insert(i, obj);
                break;
            }
        }
    }
}
