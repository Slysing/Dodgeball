﻿/* CatchCollide.cs
 * Authors:     Leith Merrifield, Boran Baykan and Connor Young
 * Description: Handles ball entering player catch range
 * Creation:    07/10/2019
 * Modified:    25/11/2019
 */

using System.Collections;
using UnityEngine;
using XboxCtrlrInput;

public class CatchCollide : MonoBehaviour
{
    public GameObject m_player = null;
    private GameObject m_lastBall = null;
    [SerializeField] private bool m_cooldown = false;

    public bool m_countDown = false;

    public XboxController m_controller;

    public Material m_redMaterial = null;
    public Material m_blueMaterial = null;
    public Material m_greenMaterial = null;

    public PLAYER_TEAM m_team;

    private Color m_testColour = Color.green;

    private void Start()
    {
        if (m_player == null)
        {
            Debug.LogAssertion("You need to set a player for the Catch Collide script \n" +
                                "Name: " + gameObject.name +
                                ", Parent: " + transform.parent.name);
        }
        m_controller = gameObject.transform.parent.GetComponent<Player>().m_controller;
        m_team = gameObject.transform.parent.GetComponent<Player>().m_currentTeam;
    }

    // When an object is colliding with the catch range
    private void OnTriggerStay(Collider other)
    {
        if (m_cooldown || RoundManager.m_pauseGame || RoundManager.m_pauseRound)
            return;
        if (other.tag == "Red Ball" ||
           other.tag == "Blue Ball" ||
           other.tag == "Neutral Ball")
        {
            //If the ball does not have a current owner and if the player is not currently holding a ball 
            if (other.GetComponent<BallBehaviour>().m_owner == null && m_player.GetComponent<Player>().m_holdingBall == false)
            {
                
                // Set timer from 0 seconds upto 5 seconds then set the ball back to Neutral and Have the ball leave the collider

                MeshRenderer ballMesh = other.GetComponent<MeshRenderer>();
                // sets the ball's team accordinly and if the ball is of the opposing colour dont pick up
                switch (m_team)
                {
                    case PLAYER_TEAM.TEAM_BLUE:
                        //if (other.tag == "Red Ball")
                        //    return;
                        other.tag = "Blue Ball";
                        other.gameObject.layer = 20;
                        other.GetComponent<MeshRenderer>().material = m_blueMaterial;
                        other.GetComponent<BallBehaviour>().m_owner = gameObject;
                        break;

                    case PLAYER_TEAM.TEAM_RED:
                        //if (other.tag == "Blue Ball")
                        //    return;
                        other.tag = "Red Ball";
                        other.gameObject.layer = 19;
                        other.GetComponent<MeshRenderer>().material = m_redMaterial;
                        other.GetComponent<BallBehaviour>().m_owner = gameObject;
                        break;
                }
                // checks the colour and sets if the colour is different
                // prevents setting the colour every loop
                //if (ballMesh.material != m_testColour)
                //    ballMesh.material = m_testColour;

                m_player.GetComponent<Player>().SetBall(other.gameObject);

                //Debug.Log("Now a " + other.tag);

            }

        }
        if (Input.GetKeyDown(KeyCode.Space) || XCI.GetAxis(XboxAxis.RightTrigger, m_controller) > 0)
        {
            print(m_controller);
            m_lastBall = other.gameObject;
            StartCoroutine(cooldown());
            //Sets the ball owner to null when thrown so it can be caught
            
            try
            {

                other.GetComponent<BallBehaviour>().m_owner = null;
            }
            catch
            {
                // shh
            }
            //m_player.GetComponent<Player>().m_holdingBall = false;  
        }
    }

    public void OnTriggerExit(Collider other)
    {
    }
    public void ResetCooldown()
    {
        m_cooldown = false;
    }

    private IEnumerator cooldown()
    {
        m_cooldown = true;
        yield return new WaitForSeconds(.2f);
        m_cooldown = false;
    }
}