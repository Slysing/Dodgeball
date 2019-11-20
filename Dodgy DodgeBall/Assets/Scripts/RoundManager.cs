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

    public static bool m_pauseRound = false;

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
    public float m_roundDuration;

    public static bool m_isPlaying = false; // static bool to be used globally to pause the game
    private bool m_roundStart = false;
    private bool m_gameStart = true; // Used to run the initial timer once
    public bool m_resetBallOnce = false;

    public float m_endScreenTimer = 5.0f;
    public TextMeshProUGUI m_redScoreText;
    public TextMeshProUGUI m_blueScoreText;
    public TextMeshProUGUI m_RoundTimer;
    public GameObject m_pausePanel;

    public GameObject m_endPanel;
    public TextMeshProUGUI m_endText;

    public Image m_RoundBeginImage;

    public Sprite m_RoundBeginSprint3;
    public Sprite m_RoundBeginSprint2;
    public Sprite m_RoundBeginSprint1;

    private float m_gameStartTimer = 3;

    public Animator m_anim = null;
    float m_nextRoundTimer = 5f;

    private void Start()
    {
        m_gameStart = true; // temp, but the variable will the entry point of a round
        m_isPlaying = false;
        m_RoundBeginImage.enabled = true;
        m_gameStartTimer = m_startCooldown;
    }

    private void Update()
    {
        if (m_redScore == m_roundsToWin)
        {
            EndGame();
            Debug.Log("Red Wins");
            return;
            // Game over red wins
            // m_isGameOver = true
        }

        if (m_blueScore == m_roundsToWin)
        {
            EndGame();
            Debug.Log("Blue Wins");
            return;
            // Game over blue wins
            // m_isGameOver = true;
        }

        // If the game is paused dont run the update
        if (Input.GetKeyDown(KeyCode.P) || XCI.GetButtonDown(XboxButton.Start))
        {
            m_pauseGame = m_pauseGame == true ? false : true;
        }

        if (m_pauseGame)
        {
            m_pausePanel.SetActive(true);
            return;
        }
        else
        {
            if (m_pausePanel.activeSelf)
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
            return;

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
            // If blue team is NOT alive
            if (!m_blueTeamAlive)
            {
                m_pauseRound = true;
                m_nextRoundTimer -= Time.deltaTime;
                m_anim.SetBool("FlashRed", true);
                ResetBalls();

                if((int)m_nextRoundTimer == 0)
                {
                    m_redScore++;
                    m_redScoreText.text = m_redScore.ToString();
                    // restart round
                    RestartRound();
                    m_anim.SetBool("FlashRed", false);
                }
            }
            else if (!m_redTeamAlive)
            {
                m_pauseRound = true;
                m_nextRoundTimer -= Time.deltaTime;
                m_anim.SetBool("FlashBlue", true);
                ResetBalls();

                if ((int)m_nextRoundTimer == 0)
                {
                    m_blueScore++;
                    m_blueScoreText.text = m_blueScore.ToString();
                    RestartRound();
                    m_anim.SetBool("FlashBlue", false);
                }                 
            }
        }
    }

    // will reset all the balls onces
    private void ResetBalls()
    {
        if(!m_resetBallOnce)
        {
            GetComponent<BallManager>().ResetBalls();
            m_resetBallOnce = true; 
        }
    }

    // runs once at the end of the game
    private void EndGame()
    {
        if(m_resetBallOnce)
        {
            return;            
        }
        else
        {
            m_resetBallOnce = true;
        }

        m_isGameOver = true;
        m_isPlaying = false;

        StartCoroutine(EndGameScreen());

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

        GetComponent<BallManager>().ResetBalls();
        GetComponent<TeamManager>().EndGame();
    }

    // at the end of each round this should run
    // resetting all needed variables and timers
    // along with the players and balls
    private void RestartRound()
    {
        m_nextRoundTimer = 5f;
        m_resetBallOnce = false;
        m_pauseRound = false;

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
        GetComponent<PistonControl>().Reset();
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

    private IEnumerator EndGameScreen()
    {
        m_endPanel.SetActive(true);

        if (m_blueTeamAlive)
            m_endText.text = "Blue Wins!";
        else
            m_endText.text = "Red Wins!";

        yield return new WaitForSeconds(m_endScreenTimer);
        m_resetBallOnce = false;
        SceneManager.LoadScene(0);
    }
}