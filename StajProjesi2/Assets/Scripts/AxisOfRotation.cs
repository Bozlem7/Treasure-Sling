using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisOfRotation : MonoBehaviour
{
    Quaternion rotationA;
    Quaternion rotationB;
    //Quaternion targetRotation;
    Quaternion currentRotation;
    float rotateSpeed = 25f;
    bool rotationNow ;
    // Start is called before the first frame update
    void Start()
    {
        rotationA = Quaternion.Euler(-64f, 0f, 0f);
        rotationB = Quaternion.Euler(-90f, 0f, 0f);
        currentRotation = rotationA;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(rotationNow)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, currentRotation, Time.deltaTime * rotateSpeed);
            if (currentRotation == transform.rotation)
            {
                rotationNow = false;
                currentRotation = (currentRotation == rotationA) ? rotationB : rotationA;
            }


        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            rotationNow = true;
        }
    }
}
