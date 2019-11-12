using UnityEngine;
using UnityEngine.SceneManagement;

public class StaticVariables : MonoBehaviour
{
    public SelectManager m_smRefernce;

    public static bool m_p1Blue;
    public static bool m_p2Blue;
    public static bool m_p3Blue;
    public static bool m_p4Blue;

    public static State[] m_playerStates = new State[4];

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (m_smRefernce.m_startGame)
        {
            //if (m_smRefernce.m_states[0] == State.LEFT)
            //    m_p1Blue = true;
            //else
            //    m_p1Blue = false;
            //if (m_smRefernce.m_states[1] == State.LEFT)
            //    m_p2Blue = true;
            //else
            //    m_p2Blue = false;
            //if (m_smRefernce.m_states[2] == State.LEFT)
            //    m_p3Blue = true;
            //else
            //    m_p3Blue = false;
            //if (m_smRefernce.m_states[3] == State.LEFT)
            //    m_p4Blue = true;
            //else
            //    m_p4Blue = false;
            for (int i = 0; i < m_playerStates.Length; i++)
            {
                m_playerStates[i] = m_smRefernce.m_states[i];
            }
            SceneManager.LoadScene("Beta");
        }
    }
}