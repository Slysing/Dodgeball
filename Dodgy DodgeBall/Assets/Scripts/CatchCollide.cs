using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

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

    public XboxController controller;

    public bool m_isRedTeam;

    //public const float m_Max_Trigger_Scale;
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
        m_isRedTeam = gameObject.transform.parent.GetComponent<Player>().m_isRedTeam;
    }

    // When an object is colliding with the catch range
    void OnTriggerStay(Collider other)
    {
        if (m_cooldown)
            return;


        if(other.tag == "Neutral Ball")
        {
            if(m_isRedTeam || !m_isRedTeam)
            {
                // other.gameObject.layer == 9 && 
                if (Input.GetKey(KeyCode.Mouse0) || (1.21f * (1.0f - XCI.GetAxis(XboxAxis.RightTrigger, controller)) > 0))
                {
                    m_player.GetComponent<Player>().SetBall(other.gameObject);
                    //Debug.Log("SetBall");

                    if (Input.GetKeyDown(KeyCode.Space) || XCI.GetAxis(XboxAxis.RightTrigger, controller) == 0)
                    {
                        m_lastBall = other.gameObject;
                        StartCoroutine(cooldown());
                    }
                }
            }
        }
        if (other.tag == "Red Ball")
        {
            if (m_isRedTeam)
            {
                // other.gameObject.layer == 9 && 
                if (Input.GetKey(KeyCode.Mouse0) || (1.21f * (1.0f - XCI.GetAxis(XboxAxis.RightTrigger, controller)) > 0))
                {
                    m_player.GetComponent<Player>().SetBall(other.gameObject);
                    //Debug.Log("SetBall");

                    if (Input.GetKeyDown(KeyCode.Space) || XCI.GetAxis(XboxAxis.RightTrigger, controller) == 0)
                    {
                        m_lastBall = other.gameObject;
                        StartCoroutine(cooldown());
                    }
                }
            }
        }
        if (other.tag == "Blue Ball")
        {
            if (!m_isRedTeam)
            {
                // other.gameObject.layer == 9 && 
                if (Input.GetKey(KeyCode.Mouse0) || (1.21f * (1.0f - XCI.GetAxis(XboxAxis.RightTrigger, controller)) > 0))
                {
                    m_player.GetComponent<Player>().SetBall(other.gameObject);
                    //Debug.Log("SetBall");

                    if (Input.GetKeyDown(KeyCode.Space) || XCI.GetAxis(XboxAxis.RightTrigger, controller) == 0)
                    {
                        m_lastBall = other.gameObject;
                        StartCoroutine(cooldown());
                    }
                }
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
