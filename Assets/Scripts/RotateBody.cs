using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBody : MonoBehaviour
{
    public float speed = 4f;
    void Update()
    {

        transform.Rotate(0,90f * speed * Time.deltaTime,0);
    }
}
