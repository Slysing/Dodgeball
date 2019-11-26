/* SelectManager
 * Author:      Connor Young
 * Description: Gets input from users to select the team which they would like to play on
 * Creation:    21/10/19
 * Modified:    25/11/19
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XboxCtrlrInput;
using UnityEngine.Animations;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public enum State
{
    LEFT,
    RIGHT,
    MIDDLE
}

public class SelectManager : MonoBehaviour
{
    //Used for the allocation of player location and sprites----------------------
    [Header("Location and Sprites")]
    public List<GameObject> m_players = new List<GameObject>();
    public List<Vector3> m_playerPosition = new List<Vector3>();
    public List<GameObject> m_locations = new List<GameObject>();
    public List<Sprite> m_sprites = new List<Sprite>();
    //----------------------------------------------------------------------------

    //Controller allocation-------------------------------------------------------
    [Header("Controllers")]
    public List<XboxController> m_controllers = new List<XboxController>();
    private static bool m_didQueryNumOfCtrlrs = false;
    //----------------------------------------------------------------------------

    //Reference to the ButtonManager script and buttons---------------------------
    [Header("Script Refernce")]
    public ButtonMananger m_bmReference;
    public GameObject m_play;
    //----------------------------------------------------------------------------

    //Static variables------------------------------------------------------------
    [HideInInspector]public List<State> m_states = new List<State>();
    [HideInInspector] public bool m_startGame = false;
    //----------------------------------------------------------------------------

    //Public Objects--------------------------------------------------------------
    [Header("Public Variables")]
    public Animator m_mainAnimator;
    public GameObject m_mainMenuCanvas = null;
    public GameObject m_teamSelectCanvas = null;
    public float m_delayTime = 0;
    public EventSystem m_mainEventSystem = null;
    public Button m_startGameButton = null;
    //----------------------------------------------------------------------------

    // Start is called before the first frame update
    private void Start()
    {
        #region Controller Set Up
        if (!m_didQueryNumOfCtrlrs)
        {
            m_didQueryNumOfCtrlrs = true;

            int queriedNumberOfCtrlrs = XCI.GetNumPluggedCtrlrs();
            if (queriedNumberOfCtrlrs == 1)
            {
                Debug.Log("Only " + queriedNumberOfCtrlrs + " Xbox controller plugged in.");
            }
            else if (queriedNumberOfCtrlrs == 0)
            {
                Debug.Log("No Xbox controllers plugged in!");
            }
            else
            {
                Debug.Log(queriedNumberOfCtrlrs + " Xbox controllers plugged in.");
            }

            XCI.DEBUG_LogControllerNames();
        }
        #endregion
        foreach(GameObject player in m_players)
        {
            Vector3 temp = new Vector3();
            temp = player.transform.position;
            m_playerPosition.Add(temp);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        //Varaibles used for getting input for the left joystick on each controller 
        #region Joystick input 
        float p1LeftStick = XCI.GetAxis(XboxAxis.LeftStickX, m_controllers[0]);
        float p2LeftStick = XCI.GetAxis(XboxAxis.LeftStickX, m_controllers[1]);
        float p3LeftStick = XCI.GetAxis(XboxAxis.LeftStickX, m_controllers[2]);
        float p4LeftStick = XCI.GetAxis(XboxAxis.LeftStickX, m_controllers[3]);
        #endregion


        #region Input
        int blue = 0;                                                               //Count for each team 
        int red = 0;
        
        if (XCI.GetDPadDown(XboxDPad.Left, m_controllers[0]) || p1LeftStick < 0)    //Input for each player 
        {
            m_players[0].transform.position = m_locations[0].transform.position;    //Setting the location for the sprite depending on the input of the user
            m_states[0] = State.LEFT;                                               //Saving the team of the player to be carried across to the game 
        }
        if (XCI.GetDPadDown(XboxDPad.Right, m_controllers[0]) || p1LeftStick > 0)   //Repeated process for the other side 
        {
            m_players[0].transform.position = m_locations[1].transform.position;
            m_states[0] = State.RIGHT;
        }
        if (XCI.GetDPadDown(XboxDPad.Left, m_controllers[1]) || p2LeftStick < 0)
        {
            m_players[1].transform.position = m_locations[2].transform.position;    //Repeated for P2 and so forth 
            m_states[1] = State.LEFT;
        }
        if (XCI.GetDPadDown(XboxDPad.Right, m_controllers[1]) || p2LeftStick > 0)
        {
            m_players[1].transform.position = m_locations[3].transform.position;
            m_states[1] = State.RIGHT;
        }
        if (XCI.GetDPadDown(XboxDPad.Left, m_controllers[2]) || p3LeftStick < 0)
        {
            m_players[2].transform.position = m_locations[4].transform.position;
            m_states[2] = State.LEFT;
        }
        if (XCI.GetDPadDown(XboxDPad.Right, m_controllers[2]) || p3LeftStick > 0)
        {
            m_players[2].transform.position = m_locations[5].transform.position;
            m_states[2] = State.RIGHT;
        }
        if (XCI.GetDPadDown(XboxDPad.Left, m_controllers[3]) || p4LeftStick < 0)
        {
            m_players[3].transform.position = m_locations[6].transform.position;
            m_states[3] = State.LEFT;
        }
        if (XCI.GetDPadDown(XboxDPad.Right, m_controllers[3]) || p4LeftStick > 0)
        {
            m_players[3].transform.position = m_locations[7].transform.position;
            m_states[3] = State.RIGHT;
        }
        #endregion

        //Counting how many players on each team,
        //used to make sure that the teams are vaild to start the game 
        for (int i = 0; i < 4; i++)
        {
            switch (m_states[i])
            {
                case State.LEFT:
                    red++;
                    break;

                case State.RIGHT:
                    blue++;
                    break;
            }

            //Allocts the stripe for each player depending on whether they 
            // are the first or second person on each team 
            if (m_states[i] != State.MIDDLE)
            {
                if (m_states[i] == State.LEFT && red == 1)                      //If the user is on the left and the first on the team
                    m_players[i].GetComponent<Image>().sprite = m_sprites[0];   //then it will be allocated the first team icon 
                else if (m_states[i] == State.LEFT && red == 2)
                    m_players[i].GetComponent<Image>().sprite = m_sprites[2];   //If they are the second then they will get the second sprite 
                else if (m_states[i] == State.RIGHT && blue == 1)
                    m_players[i].GetComponent<Image>().sprite = m_sprites[1];   //Repeate for the blue team 
                else if (m_states[i] == State.RIGHT && blue == 2)
                    m_players[i].GetComponent<Image>().sprite = m_sprites[3];
            }
        }

        //Checking of the teams are valid 
        if (red == 1 && blue == 1)
            m_play.SetActive(true);
        else if (red == 2 && blue == 2)
            m_play.SetActive(true);
        else
            m_play.SetActive(false);

        //If the teams are valid and any player presses A then the game will begin 
        if (m_play.activeInHierarchy)
        {
            if (XCI.GetButton(XboxButton.A, m_controllers[0]) ||
                XCI.GetButton(XboxButton.A, m_controllers[1]) ||
                XCI.GetButton(XboxButton.A, m_controllers[2]) ||
                XCI.GetButton(XboxButton.A, m_controllers[3]))
            {
                m_startGame = true;
            }
        }

        //At any point any player can press be to return to the main menue 
        if ((XCI.GetButton(XboxButton.B, m_controllers[0]) ||
            XCI.GetButton(XboxButton.B, m_controllers[1]) ||
            XCI.GetButton(XboxButton.B, m_controllers[2]) ||
            XCI.GetButton(XboxButton.B, m_controllers[3])) && !m_bmReference.m_isTranslating)
        {
            m_mainMenuCanvas.SetActive(true);
            m_bmReference.SelectScreenBack();
            m_startGameButton.Select();

            // IF BACK BUTTON IS PRESSED, SET THE PLAYERS BACK TO THE MIDDLE
            int count = 0;
            foreach(GameObject player in m_players)
            {
                player.GetComponent<Image>().sprite = m_sprites[4];
                player.transform.position = m_playerPosition[count];
                m_states[count] = State.MIDDLE;
                count++;
            }
        }


    }
}