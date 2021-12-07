using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour
{

    public float rotateSpeed = 80f;
    public Transform axisTransform;


    private void FixedUpdate()
    {
        this.transform.RotateAround(axisTransform.position , Vector3.up, rotateSpeed * Time.deltaTime );
    }




}
