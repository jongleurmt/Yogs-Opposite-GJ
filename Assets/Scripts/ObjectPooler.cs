using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : Singleton<ObjectPooler>
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    [SerializeField]
    private List<Pool> m_Pools = new List<Pool>();
    public List<Pool> pools => m_Pools;

    [SerializeField]
    private Transform m_ObjectParent = null;

    private Dictionary<string, Queue<GameObject>> m_PoolDictionary = new Dictionary<string, Queue<GameObject>>();

    // Start is called before the first frame update
    void Start()
    {
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                objectPool.Enqueue(obj);
                obj.transform.SetParent(m_ObjectParent);
                obj.SetActive(false);
            }

            m_PoolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!m_PoolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
            return null;
        }

        if (m_PoolDictionary[tag].Count == 0) return null;
        GameObject objectToSpawn = m_PoolDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        m_PoolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

}
