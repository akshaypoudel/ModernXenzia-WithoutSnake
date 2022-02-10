using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float speed=4f;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 90f * speed * Time.deltaTime, 90f*speed*Time.deltaTime);
    }
}
