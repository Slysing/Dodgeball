/* StaticVariables
 * Author:      Connor Young
 * Description: Variables to be carried from the main menu to the main game scene 
 * Creation:    21/10/19
 * Modified:    25/11/19
 */
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

public class StaticVariables : MonoBehaviour
{
    //Reference to the Select Manager Script 
    public SelectManager m_smRefernce;

    //Publically accessable list of the players states 
    public static State[] m_playerStates = new State[4];

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        //If the game has been started 
        if (m_smRefernce.m_startGame)
        {
            //Cycle through for the amount of players 
            for (int i = 0; i < m_playerStates.Length; i++)
            {
                //Set the current index state in the static list 
                m_playerStates[i] = m_smRefernce.m_states[i];
            }
            //Once cycled through the complete list of players load the game scene 
            SceneManager.LoadScene(1);
        }
    }
}