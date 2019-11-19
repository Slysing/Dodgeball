/*   BallIncrease.cs
 *   Authors: Riley Palmer, Leith Merrifield
 *   Description: If the ball falls below minimum speed apply a force
 *   Modified: 09/11/2019
 */

using UnityEngine;

public class BallIncrease : MonoBehaviour
{
    public AudioSource m_audioSource;
    public AudioClip m_collisionSound;
    public float m_bounceAmount = 10.0f;
    public float m_mininumSpeed = 15.0f;
    private Rigidbody m_rb;
    private float m_bounceSpeedMultiplier = 20.0f; // constant multiplier for force
    private Vector3 m_tempVelocity;
    private bool m_runOnce = false;

    private void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
        m_rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (RoundManager.m_pauseGame || RoundManager.m_pauseRound)
        {
            if (!m_runOnce)
            {
                if (m_rb.velocity.magnitude > 0)
                    m_tempVelocity = m_rb.velocity;
                m_rb.isKinematic = true;
                m_rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
                m_runOnce = true;
            }
        }
        else
        {
            if (m_rb.isKinematic)
            {
                m_rb.isKinematic = false;
                m_rb.velocity = m_tempVelocity;
                m_runOnce = false;
                m_rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            }
        }
    }

    public static void Reset()
    {
    }
    private void OnCollisionExit(Collision collider)
    {
        m_audioSource.PlayOneShot(m_collisionSound);
        //Debug.Log("testbounce " + m_rb.velocity.magnitude);

        // If magnitude is less then m_mininumSpeed then apply force using m_bounceAmount
        if (m_rb.velocity.magnitude < m_mininumSpeed)
        {
            m_rb.AddForce(m_rb.velocity * (Time.deltaTime * m_bounceAmount * 20.0f), ForceMode.Impulse);
        }
    }
}