using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float m_speed = 5f;
    public bool m_WASD = false;
    private Rigidbody m_rb = null;
    private float m_speedMultiplier = 20f;

    public void Start()
    {
        m_rb = transform.GetComponent<Rigidbody>();
    }

    public void FixedUpdate()
    {
        var speed = m_speed * m_speedMultiplier * Time.deltaTime;
        var velocity = new Vector3();
        if(m_WASD)
        {
            if(Input.GetKey(KeyCode.D))
            {
                velocity.z = 1 * speed;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                velocity.z = -1 * speed;
            }

            if (Input.GetKey(KeyCode.W))
            {
                velocity.x = -1 * speed;
            }
            else if(Input.GetKey(KeyCode.S))
            {
                velocity.x = 1 * speed; 
            }
        }
        else
        {
            if(Input.GetKey(KeyCode.RightArrow))
            {
                velocity.z = 1 * speed;
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                velocity.z = -1 * speed;
            }

            if (Input.GetKey(KeyCode.UpArrow))
            {
                velocity.x = -1 * speed;
            }
            else if(Input.GetKey(KeyCode.DownArrow))
            {
                velocity.x = 1 * speed; 
            }
        }
        m_rb.velocity = velocity;
        /*
        else
        {
            var h = Input.GetAxisRaw("Horizontal");
            var v = Input.GetAxisRaw("Vertical");

            var speed = m_speed * m_speedMultiplier * Time.deltaTime;
            var velocity = new Vector3(v * -speed, 0f, h * speed);
            m_rb.velocity = velocity;
        }
         */
        
    }
}

