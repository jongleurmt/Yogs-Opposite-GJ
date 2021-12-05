using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjects : MonoBehaviour
{
    public GameObject[] items;
    public Transform maxX, minX, maxZ, minZ, topY;
    public int minAmount, maxAmount;
    public int offset;

    private void Start()
    {
        spawnItems();
    }

    public void spawnItems()
    {
        int amount = Random.Range(minAmount, maxAmount);

        for (int i = 0; i < amount; i++)
        {
            int itemRand = Random.Range(0, items.Length);

            float randX = Random.Range(minX.position.x + offset, maxX.position.x - offset);
            float randZ = Random.Range(minZ.position.z + offset, maxZ.position.z - offset);

            Instantiate(items[itemRand], new Vector3(randX, topY.position.y, randZ), Quaternion.identity);
        }
    }
}
