using UnityEngine;
using UnityEngine.UI;
using XboxCtrlrInput;

public class TeamSelect : MonoBehaviour
{
    private static bool m_didQueryNumOfCtrlrs = false;

    public static int m_team = 0;

    public XboxController m_controller;

    public ButtonMananger m_bmRefernce;

    public GameObject m_left;
    public GameObject m_right;

    public Sprite m_blue;
    public Sprite m_red;

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
    }

    // Update is called once per frame
    private void Update()
    {
        if (XCI.GetDPadDown(XboxDPad.Left, m_controller))
        {
            transform.position = m_left.transform.position;
            GetComponent<Image>().sprite = m_blue;
            m_team = 1;
        }
        if (XCI.GetDPadDown(XboxDPad.Right, m_controller))
        {
            transform.position = m_right.transform.position;
            GetComponent<Image>().sprite = m_red;
            m_team = 2;
        }
    }
}