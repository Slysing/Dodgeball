using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XboxCtrlrInput;

public enum State
{
    LEFT,
    RIGHT,
    MIDDLE
}

public class SelectManager : MonoBehaviour
{
    public List<GameObject> m_players = new List<GameObject>();

    public List<GameObject> m_locations = new List<GameObject>();

    public List<Sprite> m_sprites = new List<Sprite>();

    public List<XboxController> m_controllers = new List<XboxController>();

    public ButtonMananger m_bmReference;

    private static bool m_didQueryNumOfCtrlrs = false;

    public List<State> m_states = new List<State>();

    public GameObject m_play;

    [HideInInspector] public bool m_startGame = false;

    // Start is called before the first frame update
    private void Start()
    {
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

        for (int i = 0; i < 4; i++)
        {
            m_states.Add(State.MIDDLE);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        int blue = 0;
        int red = 0;

        if (XCI.GetDPadDown(XboxDPad.Left, m_controllers[0]))
        {
            m_players[0].transform.position = m_locations[0].transform.position;
            m_players[0].GetComponent<Image>().sprite = m_sprites[0];
            m_states[0] = State.LEFT;
        }
        if (XCI.GetDPadDown(XboxDPad.Right, m_controllers[0]))
        {
            m_players[0].transform.position = m_locations[1].transform.position;
            m_players[0].GetComponent<Image>().sprite = m_sprites[1];
            m_states[0] = State.RIGHT;
        }
        if (XCI.GetDPadDown(XboxDPad.Left, m_controllers[1]))
        {
            m_players[1].transform.position = m_locations[2].transform.position;
            m_players[1].GetComponent<Image>().sprite = m_sprites[2];
            m_states[1] = State.LEFT;
        }
        if (XCI.GetDPadDown(XboxDPad.Right, m_controllers[1]))
        {
            m_players[1].transform.position = m_locations[3].transform.position;
            m_players[1].GetComponent<Image>().sprite = m_sprites[3];
            m_states[1] = State.RIGHT;
        }
        if (XCI.GetDPadDown(XboxDPad.Left, m_controllers[2]))
        {
            m_players[2].transform.position = m_locations[4].transform.position;
            m_players[2].GetComponent<Image>().sprite = m_sprites[0];
            m_states[2] = State.LEFT;
        }
        if (XCI.GetDPadDown(XboxDPad.Right, m_controllers[2]))
        {
            m_players[2].transform.position = m_locations[5].transform.position;
            m_players[2].GetComponent<Image>().sprite = m_sprites[1];
            m_states[2] = State.RIGHT;
        }
        if (XCI.GetDPadDown(XboxDPad.Left, m_controllers[3]))
        {
            m_players[3].transform.position = m_locations[6].transform.position;
            m_players[3].GetComponent<Image>().sprite = m_sprites[2];
            m_states[3] = State.LEFT;
        }
        if (XCI.GetDPadDown(XboxDPad.Right, m_controllers[3]))
        {
            m_players[3].transform.position = m_locations[7].transform.position;
            m_players[3].GetComponent<Image>().sprite = m_sprites[3];
            m_states[3] = State.RIGHT;
        }

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
        }

        if (red == 1 && blue == 1)
            m_play.SetActive(true);
        else if (red == 2 && blue == 2)
            m_play.SetActive(true);
        else
            m_play.SetActive(false);

        if (m_play.activeInHierarchy)
        {
            if (XCI.GetButton(XboxButton.A, m_controllers[0]))
            {
                m_startGame = true;
            }
        }

        //Debug.Log("REDCOUNT: " + red);
        //Debug.Log("BLUECOUNT: " + blue);
    }
}