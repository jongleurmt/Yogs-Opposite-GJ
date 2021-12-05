using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrowingControll : MonoBehaviour
{
    public float rotationSpeed = 1;
    public float TrowPower = 5;

    public GameObject Box;
    public Transform HoldingPoint;

    // Update is called once per frame
    void Update()
    {
        float HorizontalRotation = Input.GetAxis("Horizontal");
        float VerticalRotation = Input.GetAxis("Vertical");

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles +
            new Vector3(0, HorizontalRotation * rotationSpeed, VerticalRotation * rotationSpeed));
    
        if(Input.GetKeyDown(KeyCode.Space))
        {
            GameObject createBox = Instantiate(Box, HoldingPoint.position, HoldingPoint.rotation);
            createBox.GetComponent<Rigidbody>().velocity = HoldingPoint.transform.up * TrowPower; // trowing system
        }
    }
}
