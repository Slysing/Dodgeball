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

    
    public bool m_countDown = false;

    public XboxController m_controller;

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

    private void OnTriggerEnter(Collider other)
    {
        
    }


    // When an object is colliding with the catch range
    void OnTriggerStay(Collider other)
    {
        if (m_cooldown)
            return;


        if(other.tag == "Neutral Ball")
        {
            if(m_isRedTeam)
            {
                // other.gameObject.layer == 9 && 
                if (Input.GetKey(KeyCode.Mouse0) || (1.21f * (1.0f - XCI.GetAxis(XboxAxis.RightTrigger, m_controller)) > 0))
                {
                    // Set timer from 0 seconds upto 5 seconds then set the ball back to Neutral and Have the ball leave the collider
                    

                    //Debug.Log("SetBall");
                    m_player.GetComponent<Player>().SetBall(other.gameObject);


                    // Sets the Neutral colour ball to red
                    other.tag = "Red Ball";
                    other.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red);
                    if (other.tag == "Red Ball")
                    {
                        Debug.Log("Now a Red ball");
                    }

                    

                    



                    if (Input.GetKeyDown(KeyCode.Space) || XCI.GetAxis(XboxAxis.RightTrigger, m_controller) == 0)
                    {
                        m_lastBall = other.gameObject;
                        StartCoroutine(cooldown());
                    }
                }
            }
        }
        if (other.tag == "Neutral Ball")
        {
            if (!m_isRedTeam)
            {
                // other.gameObject.layer == 9 && 
                if (Input.GetKey(KeyCode.Mouse0) || (1.21f * (1.0f - XCI.GetAxis(XboxAxis.RightTrigger, m_controller)) > 0))
                {
                    //Debug.Log("SetBall");
                    m_player.GetComponent<Player>().SetBall(other.gameObject);

                    // Sets the Neutral colour ball to blue
                    other.tag = "Blue Ball";
                    other.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.blue);
                    if (other.tag == "Blue Ball")
                    {
                        Debug.Log("Now a Blue ball");
                    }

                    

                    if (Input.GetKeyDown(KeyCode.Space) || XCI.GetAxis(XboxAxis.RightTrigger, m_controller) == 0)
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
                if (Input.GetKey(KeyCode.Mouse0) || (1.21f * (1.0f - XCI.GetAxis(XboxAxis.RightTrigger, m_controller)) > 0))
                {
                    //Debug.Log("SetBall");
                    m_player.GetComponent<Player>().SetBall(other.gameObject);
                    

                    if (Input.GetKeyDown(KeyCode.Space) || XCI.GetAxis(XboxAxis.RightTrigger, m_controller) == 0)
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
                if (Input.GetKey(KeyCode.Mouse0) || (1.21f * (1.0f - XCI.GetAxis(XboxAxis.RightTrigger, m_controller)) > 0))
                {
                    //Debug.Log("SetBall");
                    m_player.GetComponent<Player>().SetBall(other.gameObject);
                    

                    if (Input.GetKeyDown(KeyCode.Space) || XCI.GetAxis(XboxAxis.RightTrigger, m_controller) == 0)
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
