/* PistonControl.cs
 * Authors: Leith Merrifield
 * Description: Controls the playing of piston animations
 * Creation: 10/11/2019
 * Modified: 10/11/2019
 */

using UnityEngine;

public class PistonControl : MonoBehaviour
{
    public Animator anim = null;
    public float m_interval = 15.0f;
    public float m_holdInterval = 10.0f; // How long the pistons will stay up
    public bool m_togglePistons = true;

    private float m_count = 0.0f;
    private bool m_playingBool = false; // if the animation is playing/up

    private void Update()
    {
        if (RoundManager.m_isPlaying && m_togglePistons)
        {
            //print("count: " + m_count);
            if (!m_playingBool)
            {
                m_count += Time.deltaTime;
                if ((int)m_count % (int)m_interval == 0 && (int)m_count != 0)
                {
                    anim.SetTrigger("Extend");
                    m_playingBool = true;
                    m_count = 0.0f;
                }
            }
            else
            {
                m_count += Time.deltaTime;
                if ((int)m_count % (int)m_holdInterval == 0 && (int)m_count != 0)
                {
                    anim.SetTrigger("Retract");
                    m_playingBool = false;
                    m_count = 0.0f;
                }
            }
        }
    }

    // If the pistons are up and a round has ended then play the down anim
    private void LateUpdate()
    {
        if ((!RoundManager.m_blueTeamAlive ||
            !RoundManager.m_redTeamAlive) &&
            m_playingBool == true)
        {
            anim.SetTrigger("Down");
            m_count = 0;
            m_playingBool = false;
            print("Yee");
        }
    }
}