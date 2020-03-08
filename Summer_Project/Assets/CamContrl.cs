using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamContrl : MonoBehaviour
{
    float mX, mY;
    public float SpeedX, SpeedY;
    Vector3 cPosition,tPosition;
    public GameObject Target;
    public float DistanceV, DistanceH;
    Quaternion cRotation,tRotation;
    public float camRO;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0);
     
            tPosition = tRotation * (-transform.forward * 10 + transform.up * 2) + Target.transform.position;
            transform.position = tPosition;
        

    }
    public void Reset()
    {
       // transform.rotation = Quaternion.LookRotation(Target.transform.forward)*Quaternion.Euler(camRO,0,0);
        //transform.position = (-Target.transform.forward * 10 + Target.transform.up * 2) + Target.transform.position;
    }
}
