using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAround : MonoBehaviour
{
    float spinSpeed =55f;
    Rigidbody rb;
    bool spinAround;
    Quaternion stopRotation;
    float stopTime = 2f;
    float time = 0f;
    void Start()
    {
        //stopRotation = Quaternion.Euler(0f, 0f, 0f);
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //if( spinAround )
        //{
        //    gameObject.transform.Rotate(0f, 15f, 0f, Space.Self);
        //    time += Time.deltaTime;
        //    if( time >= stopTime )
        //    {
        //        gameObject.transform.rotation = Quaternion.RotateTowards(transform.rotation, stopRotation, Time.deltaTime * -spinSpeed);
        //        //Bu satýrý kaldýrabiliriz ivme azalarak hareket etmiyor gibi
        //        spinAround = false;
        //        time = 0f;

        //    }

        //}
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if(collision.gameObject.CompareTag("Player"))
        {
            spinAround = true;
        }
    }
    private void FixedUpdate()
    {
        if(spinAround)
        {
            Quaternion deltaRotation = Quaternion.Euler(0f, spinSpeed * Time.fixedDeltaTime, 0f);
            rb.MoveRotation(rb.rotation * deltaRotation);
            time += Time.fixedDeltaTime;
            if(time >= stopTime)
            {
                spinAround = false;
                time = 0f;
            }
        }
    }
}
