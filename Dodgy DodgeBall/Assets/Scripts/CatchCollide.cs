using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
/// Description: Handles ball entering player catch range
/// Authors Leith Merrifield
/// Creation Date: 07/10/2019
/// Modified: 07/10/2019
///</summary>


public class CatchCollide : MonoBehaviour
{
    public GameObject m_player = null;
    private GameObject m_lastBall = null;
    private bool m_cooldown = false;

    private int m_pNumber = 1;

   //public int m_PlayerControllerNumber;

    void Start()
    {
        if (m_player == null)
        {
            Debug.LogAssertion("You need to set a player for the Catch Collide script \n" +
                                "Name: " + gameObject.name +
                                ", Parent: " + transform.parent.name);
        }
    }

    public void Update()
    {
        m_pNumber = gameObject.transform.parent.GetComponent<Player>().m_PlayerControllerNumber;
    }

    // When an object is colliding with the catch range
    void OnTriggerStay(Collider other)
    {
        if (m_cooldown)
            return;
        
        if(other.gameObject.layer == 9 && Input.GetKey(KeyCode.Mouse0) || Input.GetAxis("P" + m_pNumber + " Right Trigger") > 0 && other.tag == "Ball")
        {
            m_player.GetComponent<Player>().SetBall(other.gameObject);
            //Debug.Log("SetBall");

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetAxis("P" + m_pNumber + " Left Trigger") > 0)
            { 
                m_lastBall = other.gameObject;
                StartCoroutine(cooldown());
            }
        }
    }

    IEnumerator cooldown()
    {
        m_cooldown = true;
        yield return new WaitForSeconds(.5f);
        m_cooldown = false;
    }
    
}
