using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

///<summary>
/// Description: Handles the Score needed to win the game, round duration
/// Authors Leith Merrifield
/// Creation Date: 15/10/2019
/// Modified: 15/10/2019
///</summary>

public class RoundManager : MonoBehaviour
{
    [Header("Config")]
    public int m_roundsToWin = 3;
    public int m_roundCooldown = 5;
    public int m_startCooldown = 5;

    [Header("Status")]
    public int m_currentRound = 1;
    public double m_duration = 0.0;
    public int m_blueScore = 0;
    public int m_redScore = 0;
    public bool m_redTeamAlive = true;
    public bool m_blueTeamAlive = true;
    public bool m_isGameOver = false;

    private bool m_roundStart = false;
    private double m_roundDuration;
    public static bool m_isPlaying = false; // static bool to be used globally to pause the game
    private bool m_gameStart = true; // Used to run the initial timer once

    public Text m_redScoreText;
    public Text m_blueScoreText;
    public Text m_RoundTimer; 

    void Start()
    {
        m_gameStart = true; // temp, but the variable will the entry point of a round
    }

    void Update()
    {
        if(m_gameStart)
        {
            StartCoroutine(Cooldown(m_startCooldown));
            m_gameStart = false;
        }

        if (m_isGameOver)
        {
            TeamManager.m_blueTeam.Clear();
            TeamManager.m_redTeam.Clear();
            SceneManager.LoadScene("MainMenuScene");
            // anything relating to the game being over
            // can go here
            return;
        }

        if(m_isPlaying)
        {
            if(!m_roundStart)
            {
                m_roundDuration = 0;
                m_roundStart = true;
            }
            m_roundDuration += Time.deltaTime;
            m_duration = (Math.Floor(m_roundDuration));
            m_RoundTimer.text = m_duration.ToString(); 
            // Checks if a member of the team are still alive
            foreach(var player in TeamManager.m_blueTeam)
            {
                if(player.GetComponent<Player>().m_isAlive)
                {
                    m_blueTeamAlive = true;
                    break;
                }
                else
                    m_blueTeamAlive  = false;
            }
            foreach(var player in TeamManager.m_redTeam)
            {

                if(player.GetComponent<Player>().m_isAlive)
                {
                    m_redTeamAlive = true;
                    break;
                }
                else
                    m_redTeamAlive  = false;
            }

            if(!m_blueTeamAlive)
            {
                m_redScore++;
                m_redScoreText.text = m_redScore.ToString(); 
                if(m_redScore == m_roundsToWin)
                {
                    EndGame();
                    Debug.Log("Red Wins");
                    return;
                    // Game over red wins
                    // m_isGameOver = true
                }
                // restart round
                RestartRound();
            }
            else if(!m_redTeamAlive)
            {
                m_blueScore++;
                m_blueScoreText.text = m_blueScore.ToString(); 
                if(m_blueScore == m_roundsToWin)
                {
                    EndGame();
                    Debug.Log("Blue Wins");
                    return;
                    // Game over blue wins
                    // m_isGameOver = true;
                }
                // restart round
                RestartRound();
            }
        }
    }

    void EndGame()
    {
        m_isGameOver = true;
        m_isPlaying = false;

        foreach (var player in TeamManager.m_redTeam)
        {
            player.GetComponent<Player>().DropBall();
        }
        foreach (var player in TeamManager.m_blueTeam)
        {
            player.GetComponent<Player>().DropBall();
        }


        GetComponent<TeamManager>().Reset();
        GetComponent<BallManager>().ResetBalls();        
    }

    void RestartRound()
    {
        // m_isPlayer determines if the scoring/duration system runs
        m_isPlaying = false;
        StartCoroutine(Cooldown(m_roundCooldown));
        m_currentRound++;
        // duration reset
        m_roundStart = false;


        foreach (var player in TeamManager.m_redTeam)
        {
            player.GetComponent<Player>().DropBall();
        }
        foreach (var player in TeamManager.m_blueTeam)
        {
            player.GetComponent<Player>().DropBall();
        }


        GetComponent<TeamManager>().Reset();
        GetComponent<BallManager>().ResetBalls();        
    }

    IEnumerator Cooldown(int seconds)
    {
        for(int i = seconds; i > 0 ; --i)
        {
            m_RoundTimer.text = String.Format("0:{00}", i.ToString());
            Debug.Log("Starts in: " +  i);
            yield return new WaitForSeconds(1.0f);
        }
        Debug.Log("Go!");
        m_isPlaying = true;
    }
}

