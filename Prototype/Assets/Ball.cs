﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update ()
    {
        Debug.Log(rb.velocity.magnitude);
        if (rb.velocity.magnitude < 14)
            rb.AddForce(rb.velocity, ForceMode.VelocityChange);
    }
}
