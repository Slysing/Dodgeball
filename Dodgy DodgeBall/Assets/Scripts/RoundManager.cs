/* RoundManager.cs
 * Authors: Leith Merrifield
 * Description: Handles the score needed to win the game and round duration
 * Creation: 15/10/2019
 * Modified: 10/11/2019
 */

using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using XboxCtrlrInput;


public class RoundManager : MonoBehaviour
{
    public static bool m_pauseGame = false;
    [Header("Config")]
    public int m_roundsToWin = 3;

    public int m_roundCooldown = 5;
    public int m_startCooldown = 5;

    [Header("Status")]
    public int m_currentRound = 1;

    public int m_duration = 0;
    public int m_blueScore = 0;
    public int m_redScore = 0;
    public static bool m_redTeamAlive = true;
    public static bool m_blueTeamAlive = true;
    public bool m_isGameOver = false;

    private bool m_roundStart = false;
    public static float m_roundDuration;
    public static bool m_isPlaying = false; // static bool to be used globally to pause the game
    private bool m_gameStart = true; // Used to run the initial timer once

    public TextMeshProUGUI m_redScoreText;
    public TextMeshProUGUI m_blueScoreText;
    public TextMeshProUGUI m_RoundTimer;
    public GameObject m_pausePanel;
    

    public Image m_RoundBeginImage;

    public Sprite m_RoundBeginSprint3;
    public Sprite m_RoundBeginSprint2;
    public Sprite m_RoundBeginSprint1;


    float m_gameStartTimer = 3;

    private void Start()
    {
        m_gameStart = true; // temp, but the variable will the entry point of a round
        m_isPlaying = false;
        m_RoundBeginImage.enabled = true;
        m_gameStartTimer = m_startCooldown;
    }

    private void Update()
    {
        // If the game is paused dont run the update
        if(Input.GetKeyDown(KeyCode.P) || XCI.GetButtonDown(XboxButton.Start) )
        {
            m_pauseGame = m_pauseGame == true ? false : true;
        }

        if(m_pauseGame)
        {
            m_pausePanel.SetActive(true);
            return;
        }
        else
        {
            if(m_pausePanel.activeSelf)
                m_pausePanel.SetActive(false);
        }


        if (m_gameStart)
        {
            m_gameStartTimer -= Time.deltaTime;
            // m_RoundTimer.text = Mathf.Floor(m_gameStartTimer).ToString();
            m_RoundTimer.text = "0:00";

            switch (Mathf.CeilToInt(m_gameStartTimer))
            {
                case 1:
                    m_RoundBeginImage.sprite = m_RoundBeginSprint1;
                    break;
                case 2:
                    m_RoundBeginImage.sprite = m_RoundBeginSprint2;
                    break;
                case 3:
                    m_RoundBeginImage.sprite = m_RoundBeginSprint3;
                    break;
            }


            if (m_gameStartTimer <= 0)
            {

                m_isPlaying = true;
                m_gameStart = false;
                m_RoundBeginImage.enabled = false;
            }
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

        if (m_isPlaying)
        {
            if (!m_roundStart)
            {
                m_roundDuration = 0;
                m_roundStart = true;
            }
            m_roundDuration += Time.deltaTime;


            float t = m_roundDuration;
            string minutes = ((int)t / 60).ToString();
            string seconds = (t % 60).ToString("00");

            //m_RoundTimer.text = String.Format("0:{00}", m_duration.ToString());
            m_RoundTimer.text = minutes + ":" + seconds;

            // Checks if a member of the team are still alive
            foreach (var player in TeamManager.m_blueTeam)
            {
                if (player.GetComponent<Player>().m_isAlive)
                {
                    m_blueTeamAlive = true;
                    break;
                }
                else
                    m_blueTeamAlive = false;
            }
            foreach (var player in TeamManager.m_redTeam)
            {
                if (player.GetComponent<Player>().m_isAlive)
                {
                    m_redTeamAlive = true;
                    break;
                }
                else
                    m_redTeamAlive = false;
            }

            if (!m_blueTeamAlive)
            {
                m_redScore++;
                m_redScoreText.text = m_redScore.ToString();
                if (m_redScore == m_roundsToWin)
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
            else if (!m_redTeamAlive)
            {
                m_blueScore++;
                m_blueScoreText.text = m_blueScore.ToString();
                if (m_blueScore == m_roundsToWin)
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

    private void EndGame()
    {
        m_isGameOver = true;
        m_isPlaying = false;

        foreach (var player in TeamManager.m_redTeam)
        {
            player.GetComponent<Player>().DropBall();
            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        foreach (var player in TeamManager.m_blueTeam)
        {
            player.GetComponent<Player>().DropBall();
            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

        GetComponent<TeamManager>().Reset();
        GetComponent<BallManager>().ResetBalls();
    }

    private void RestartRound()
    {
        // m_isPlayer determines if the scoring/duration system runs
        m_isPlaying = false;
        // StartCoroutine(Cooldown(m_roundCooldown));
        m_currentRound++;
        // duration reset
        m_roundStart = false;

        m_gameStart = true; // temp, but the variable will the entry point of a round
        m_isPlaying = false;
        m_RoundBeginImage.enabled = true;
        m_gameStartTimer = m_startCooldown;

        foreach (var player in TeamManager.m_redTeam)
        {
            player.GetComponent<Player>().DropBall();
            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        foreach (var player in TeamManager.m_blueTeam)
        {
            player.GetComponent<Player>().DropBall();
            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

        GetComponent<TeamManager>().Reset();
        GetComponent<BallManager>().ResetBalls();
    }

    private IEnumerator Cooldown(int seconds)
    {
        for (int i = seconds; i > 0; --i)
        {
            int smollseconds = i % 10;
            int tenSeconds = i / 10;
            int minutes = i / 60;

         //  m_RoundMinutes.text = minutes.ToString();
         //  m_RoundTens.text = tenSeconds.ToString();
         //  m_RoundSeconds.text = smollseconds.ToString();
            yield return new WaitForSeconds(1.0f);
        }
        //m_RoundTimer.text = "Begin!";
        //yield return new WaitForSeconds(.5f);
        m_isPlaying = true;
    }
}