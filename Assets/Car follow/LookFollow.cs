using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookFollow : MonoBehaviour
{
    public Transform Target;
    public GameObject car;
    public GameObject badcar;
    
    public float Distance_;

    float SpeedBadCar = 5.0f;

       

    // Update is called once per frame
    void Update()
    {

        transform.position = Vector3.Slerp( badcar.transform.position, car.transform.position, 0.01f);

        transform.LookAt(Target.position);
        transform.Translate(0.0f, 0.0f, SpeedBadCar * Time.deltaTime);

    }
}
