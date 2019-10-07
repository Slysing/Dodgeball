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

    void Start()
    {
        if(m_player == null)
        {
            Debug.LogAssertion("You need to set a player for the Catch Collide script \n" +
                                "Name: " + gameObject.name +
                                ", Parent: " + transform.parent.name);
        }   
    }

    // When an object is colliding with the catch range
    void OnTriggerStay(Collider other)
    {
        if(m_cooldown)
            return;
        
        if(other.gameObject.layer == 9 && Input.GetKey(KeyCode.Mouse0))
        {
            m_player.GetComponent<Player>().SetBall(other.gameObject);

            if(Input.GetKeyDown(KeyCode.Space))
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
