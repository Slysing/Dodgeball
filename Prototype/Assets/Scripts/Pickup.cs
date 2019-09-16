using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class Pickup : MonoBehaviour
{
    public bool m_holdingBall = false;
    public GameObject m_hand = null;
    public float m_throwStrength = 5f;
    private bool m_coolDown = false;
    private void Awake()
    {
        if(m_hand == null)
        {
            m_hand = transform.gameObject;
        }
    }

    private void Update()
    {
        Vector3 direction = new Vector3();
        Vector3 mouse = Input.mousePosition;
        Ray castPoint = Camera.main.ScreenPointToRay(mouse);
        RaycastHit hit;
        if (Physics.Raycast(castPoint, out hit, Mathf.Infinity))
        {
            transform.LookAt(hit.point);
            transform.eulerAngles = new Vector3(0.0f,transform.eulerAngles.y, 0f);
            direction = hit.point - transform.position;
        }


        var collisions = Physics.OverlapSphere(transform.position, 5f);
        GameObject ball = null;
        foreach(var i in collisions)
        {
            if(i.tag == "Ball")
            {
                if(Input.GetKey(KeyCode.Mouse0) && m_coolDown == false)
                {
                    ball = i.gameObject;
                    i.gameObject.transform.position = m_hand.transform.position;
                    i.gameObject.transform.rotation = m_hand.transform.rotation;
                    m_holdingBall = true;
                }
                else
                {
                    m_holdingBall = false;
                }
            }
        }

        if (m_holdingBall && ball != null)
        {
            if(Input.GetKey(KeyCode.Space) && m_coolDown == false)
            {
                m_coolDown = true;
                Thread t = new Thread(new ThreadStart(cooldownThread));
                t.Start();
                var rb = ball.GetComponent<Rigidbody>();
                var force = direction.normalized * m_throwStrength;
                rb.AddForce(force, ForceMode.Impulse);
            }
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 5f);
    }

    public void cooldownThread()
    {
        for(float i = 1; i < 11; ++i)
        {
            Debug.Log("Cooldown: " + i / 10);
            Thread.Sleep(100);
        }
        m_coolDown = false;
        return;
    }
    private IEnumerator cooldown()
    {

        yield return null;
    }
}
