/* PistonControl.cs
 * Authors: Leith Merrifield
 * Description: Controls the playing of piston animations
 * Creation: 10/11/2019
 * Modified: 10/11/2019
 */

using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class PistonControl : MonoBehaviour
{
    public Animator m_anim = null;
    public List<Animator> m_animLights = new List<Animator>();
    public float m_interval = 15.0f;
    public float m_holdInterval = 10.0f; // How long the pistons will stay up
    public bool m_togglePistons = true;

    private bool m_extend = false;
    private float m_count = 0.0f;
    private bool m_playingBool = false; // if the animation is playing/up
    private bool m_runOnce = false;

    private void Update()
    {
        // If the game is paused dont run the update
        if(RoundManager.m_pauseGame)
        {
            m_anim.enabled = false;
            foreach(Animator light in  m_animLights)
            {
                light.enabled = false;
            }
            m_runOnce = true;
            return;
        }
        else
        {
            if(m_runOnce)
            {
                foreach(Animator light in  m_animLights)
                {
                    light.enabled = true;
                }
                m_anim.enabled = true;
                m_runOnce = false;
            }
        }

        if (RoundManager.m_isPlaying && m_togglePistons)
        {
            //print("count: " + m_count);
            if (!m_playingBool)
            {
                m_count += Time.deltaTime;
                if ((int)m_count % (int)m_interval == 0 && (int)m_count != 0)
                {
                    StartCoroutine(PistonRaise());

                    foreach(Animator piston in m_animLights)
                    {
                        piston.SetTrigger("Extend");     
                    }     
                    m_extend = true;
                    m_playingBool = true;
                    m_count = 0.0f;
                }
            }
            else
            {
                m_count += Time.deltaTime;
                if ((int)m_count % (int)m_holdInterval == 0 && (int)m_count != 0)
                {
                    StartCoroutine(PistonLower());
                    m_anim.SetTrigger("Retract");     
                    foreach(Animator piston in m_animLights)
                    {
                        piston.SetTrigger("Extend");     
                    }                   
                    m_extend = false;
                    m_playingBool = false;
                    m_count = 0.0f;
                }
            }
        }
    }

    // If the pistons are up and a round has ended then play the down anim
    private void LateUpdate()
    {
        if(RoundManager.m_pauseGame)
            return;

        if ((!RoundManager.m_blueTeamAlive ||
            !RoundManager.m_redTeamAlive) &&
            m_playingBool == true)
        {
            StartCoroutine(PistonLower());
            foreach(Animator piston in m_animLights)
            {
                piston.SetTrigger("Extend");     
            }       

            m_count = 0;
            m_playingBool = false;
            print("Yee");
        }
    }

    private IEnumerator PistonRaise()
    {
        yield return new WaitForSeconds(1.0f);
        m_anim.SetTrigger("Extend");
    }
    private IEnumerator PistonLower()
    {
        yield return new WaitForSeconds(1.0f);
        m_anim.SetTrigger("Retract");
    }
}