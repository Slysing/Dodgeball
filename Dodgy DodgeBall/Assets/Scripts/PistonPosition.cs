using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistonPosition : MonoBehaviour
{
    float xPos;
    float zPos;
    // Start is called before the first frame update
    void Start()
    {
        xPos = transform.position.x;
        zPos = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(xPos,transform.position.y,zPos);
        //print(transform.position.x);
    }
}
