using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjects : MonoBehaviour
{
    //public GameObject[] items;
    public Transform maxX, minX, maxZ, minZ, topY;
    public int minAmount, maxAmount;
    public int offset;


    ObjectPooler objectPooler;

    private void Start()
    {
        objectPooler = ObjectPooler.Instance;

        spawnItems();
    }

    public void spawnItems()
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
}
