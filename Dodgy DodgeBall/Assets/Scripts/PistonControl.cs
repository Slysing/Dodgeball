/* PistonControl.cs
 * Authors: Leith Merrifield
 * Description: Controls the playing of piston animations
 * Creation: 10/11/2019
 * Modified: 10/11/2019
 */

using System.Collections.Generic;
using UnityEngine;

public class PistonControl : MonoBehaviour
{
    public Animator m_anim = null;
    public List<Animator> m_animLights = new List<Animator>();
    public float m_interval = 15.0f;
    public float m_holdInterval = 10.0f; // How long the pistons will stay up
    public float m_timeBetweenAnims = 1.0f;
    public bool m_togglePistons = true;

    private bool m_extend = false;
    private bool m_lower = false;

    private float m_count = 0.0f;
    private bool m_playingBool = false; // if the animation is playing/up
    private bool m_runOnce = false;
    private float m_time;

    private void Update()
    {
        // If the game is paused dont run the update
        if (RoundManager.m_pauseGame)
        {
            m_anim.enabled = false;
            foreach (Animator light in m_animLights)
            {
                light.enabled = false;
            }
            m_runOnce = true;
            return;
        }
        else
        {
            if (m_runOnce)
            {
                foreach (Animator light in m_animLights)
                {
                    light.enabled = true;
                }
                m_anim.enabled = true;
                m_runOnce = false;
            }
        }

        if (m_extend && (Time.time - m_time) > m_timeBetweenAnims)
        {
            m_anim.SetTrigger("Extend");
            m_extend = false;
        }
        else if (m_lower && (Time.time - m_time) > m_timeBetweenAnims)
        {
            m_anim.SetTrigger("Retract");
            m_lower = false;
        }

        if (RoundManager.m_isPlaying && m_togglePistons)
        {
            //print("count: " + m_count);
            if (!m_playingBool)
            {
                m_count += Time.deltaTime;
                if ((int)m_count % (int)m_interval == 0 && (int)m_count != 0)
                {
                    m_time = Time.time;
                    //StartCoroutine(m_raiseCo);
                    //m_anim.SetTrigger("Extend");

                    foreach (Animator piston in m_animLights)
                    {
                        piston.SetTrigger("Lights");
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
                    m_time = Time.time;
                    //StartCoroutine(m_lowerCo);
                    foreach (Animator piston in m_animLights)
                    {
                        piston.SetTrigger("Lights");
                    }
                    m_lower = true;
                    m_playingBool = false;
                    m_count = 0.0f;
                }
            }
        }
    }

    // If the pistons are up and a round has ended then play the down anim
    private void LateUpdate()
    {
        if (RoundManager.m_pauseGame)
            return;
    }

    public void Reset()
    {
        print("Yes");
        if (m_playingBool == true)
        {
            m_anim.SetTrigger("Retract");
            foreach (Animator piston in m_animLights)
            {
                piston.SetTrigger("Lights");
            }

            m_playingBool = false;
        }
        m_extend = false;
        m_lower = false;
        m_count = 0.0f;
        m_time = 0.0f;
    }
}