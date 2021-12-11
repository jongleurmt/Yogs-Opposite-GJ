using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnObjects : MonoBehaviour
{
    [SerializeField]
    private Transform[] m_LaunchPoints = { };
    
    //public GameObject[] items;
    public Transform maxX, minX, maxZ, minZ, topY;
    public int minAmount, maxAmount;
    public int offset;

    [SerializeField]
    private int m_ItemsPerSpawn = 2;

    [SerializeField]
    private float m_SpawnTimer = 5f;

    private float _Timer;

    ObjectPooler objectPooler;

    private void Start()
    {
        objectPooler = ObjectPooler.Instance;
        //spawnItems();
    }

    void Update()
    {
        _Timer += Time.deltaTime;

        if (_Timer >= m_SpawnTimer)
        {
            SpawnItem();
            _Timer = 0f;
        }
    }

    private void SpawnItems()
    {
        int amount = Random.Range(minAmount, maxAmount);

        for (int i = 0; i < amount; i++)
        {
            int itemRand = Random.Range(0, objectPooler.pools.Count);

            float randX = Random.Range(minX.position.x + offset, maxX.position.x - offset);
            float randZ = Random.Range(minZ.position.z + offset, maxZ.position.z - offset);

            Vector3 newPosition = new Vector3(randX, topY.position.y, randZ);

            //randomli selects an object from the pool but it wants the tag name as in stings(objectPooler.pools[itemRand].tag)
            objectPooler.SpawnFromPool(objectPooler.pools[itemRand].tag, newPosition, Quaternion.identity);

            //Instantiate(items[itemRand], new Vector3(randX, topY.position.y, randZ), Quaternion.identity);
        }
    }

    private void SpawnItem()
    {
        List<Transform> availableLaunchPoints = new List<Transform>(m_LaunchPoints);
        for (int i = 0; i < m_ItemsPerSpawn; i++)
        {
            int pointIndex = Random.Range(0, availableLaunchPoints.Count);
            int itemIndex = Random.Range(0, objectPooler.pools.Count);

            Transform point = availableLaunchPoints[pointIndex];

            GameObject item = objectPooler.SpawnFromPool(objectPooler.pools[itemIndex].tag, point.position, point.rotation);
            item.GetComponent<ObjectEffect>()?.Throw();

            availableLaunchPoints.RemoveAt(pointIndex);
        }
    }
}
