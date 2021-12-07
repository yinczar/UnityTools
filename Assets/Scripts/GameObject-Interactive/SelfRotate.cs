using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfRotate : MonoBehaviour
{

    public float rotateSpeed = 20f;


    private void FixedUpdate()
    {

        this.transform.Rotate(Vector3.up * Time.deltaTime * rotateSpeed);
    }






}
