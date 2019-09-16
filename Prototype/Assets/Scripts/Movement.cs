using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float m_speed = 5f;
    public bool m_useImpulse = false;
    private Rigidbody m_rb = null;
    private float m_speedMultiplier = 20f;

    public void Start()
    {
        m_rb = transform.GetComponent<Rigidbody>();
    }

    public void FixedUpdate()
    {
        if(m_useImpulse)
        {
            var multiplier = m_speedMultiplier / 6;
            if(Input.GetKey(KeyCode.D))
            {
                var speed = m_speed * multiplier * Time.deltaTime;
                var movement = new Vector3(speed,0f,0f);
                m_rb.AddForce(movement, ForceMode.Impulse);
            }
            if (Input.GetKey(KeyCode.A))
            {
                var speed = m_speed * multiplier * Time.deltaTime;
                var movement = new Vector3(-speed, 0f, 0f);
                m_rb.AddForce(movement, ForceMode.Impulse);
            }
            if (Input.GetKey(KeyCode.W))
            {
                var speed = m_speed * multiplier * Time.deltaTime;
                var movement = new Vector3(0f, 0f, speed);
                m_rb.AddForce(movement, ForceMode.Impulse);
            }
            if (Input.GetKey(KeyCode.S))
            {
                var speed = m_speed * multiplier * Time.deltaTime;
                var movement = new Vector3(0f, 0f, -speed);
                m_rb.AddForce(movement, ForceMode.Impulse);
            }
        }
        else
        {
            var h = Input.GetAxisRaw("Horizontal");
            var v = Input.GetAxisRaw("Vertical");

            var speed = m_speed * m_speedMultiplier * Time.deltaTime;
            var velocity = new Vector3(v * -speed, 0f, h * speed);
            m_rb.velocity = velocity;
        }
    }
}

