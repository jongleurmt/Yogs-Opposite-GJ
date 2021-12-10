using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public Vector3 boardSize = new Vector3(2f, 0.5f, 5f);
    public float spacing = 0.1f;
    public float offsetSize = 0.5f;
    public Vector2 areaSize = new Vector2(30f, 30f);

    // Start is called before the first frame update
    void Start()
    {
        Rect dimensions = new Rect(areaSize.x * -0.5f, areaSize.y * -0.5f, areaSize.x * 0.5f, areaSize.y * 0.5f);
        bool offset = false;
        for (float x = dimensions.x; x < dimensions.width; x += boardSize.x + spacing)
        {
            offset = !offset;
            for (float y = dimensions.y; y < dimensions.height; y += boardSize.z + spacing)
            {
                GameObject board = GameObject.CreatePrimitive(PrimitiveType.Cube);
                board.transform.localScale = boardSize;
                board.transform.position = new Vector3(x, boardSize.y * -0.5f, y + (offset ? offsetSize : 0));
                board.transform.SetParent(transform);

                Collider c = board.GetComponent<Collider>();
                Destroy(c);
            }
        }
    }
}
