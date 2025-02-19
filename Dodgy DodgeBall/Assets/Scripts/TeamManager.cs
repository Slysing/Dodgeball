﻿/* TeamManager.cs
 * Authors:     Leith Merrifield and Connor Young 
 * Description: Handles creation of teams and spawning/resetting of players
 * Creation:    
 * Modified:   25/11/19
 */

using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    [Header("Team Allocations")]
    public static List<GameObject> m_blueTeam = new List<GameObject>();
    public static List<GameObject> m_redTeam = new List<GameObject>();

    [Header("Prefabs")]
    public GameObject m_redplayer1Prefab = null;
    public GameObject m_redplayer2Prefab = null;
    public GameObject m_blueplayer1Prefab = null;
    public GameObject m_blueplayer2Prefab = null;

    [Header("Spawn Locations")]
    public List<GameObject> m_blueSpawns = new List<GameObject>();
    public List<GameObject> m_redSpawns = new List<GameObject>();

    [Header("Materials")]
    public Material m_redMaterial = null;
    public Material m_blueMaterial = null;

    ///<summary>
    /// team: 1 = blue team,  2 = red team
    ///</summary>
    private void AddPlayer(int team, GameObject player)
    {
        switch (team)
        {
            case 1:
                m_blueTeam.Add(player);
                player.GetComponent<MeshRenderer>().material = m_blueMaterial;
                break;

            case 2:
                m_redTeam.Add(player);
                player.GetComponent<MeshRenderer>().material = m_redMaterial;
                break;

            default:
                Debug.LogError("Adding player to a nonexistent team");
                break;
        }
    }

    ///<summary> Resets spawns/isAlive </summary>
    public void Reset()
    {
        int idx = 0;
        foreach (var player in m_blueTeam)
        {
            player.transform.position = m_blueSpawns[idx].transform.position;
            player.GetComponent<Player>().m_isAlive = true;
            player.SetActive(true);
            idx++;
        }
        idx = 0;
        foreach (var player in m_redTeam)
        {
            player.transform.position = m_redSpawns[idx].transform.position;
            player.GetComponent<Player>().m_isAlive = true;
            player.SetActive(true);
            idx++;
        }
    }
    
    public void EndGame()
    {
        m_blueTeam.Clear();
        m_redTeam.Clear();
    }

    private void Awake()
    {

        // --- TESTING PURPOSE --- 
        // This only here in place of a team select
        //m_redTeam.Add(Instantiate(m_redplayer1Prefab, m_redSpawns[0].transform.position, Quaternion.identity));
        //m_redTeam.Add(Instantiate(m_redplayer2Prefab, m_redSpawns[1].transform.position, Quaternion.identity));

        //m_blueTeam.Add(Instantiate(m_blueplayer1Prefab, m_blueSpawns[0].transform.position, Quaternion.identity));
        //m_blueTeam.Add(Instantiate(m_blueplayer2Prefab, m_blueSpawns[1].transform.position, Quaternion.identity));

        // --- FOR MAIN GAME ---
        #region and Prefab allocation
        ////Bool to whether a player has joined the red or blue team 
        bool foundBlue = false;
        bool foundRed = false;

        //Loops through players to assign them to the right prefab on the right team 
        for (int i = 0; i < StaticVariables.m_playerStates.Length; i++)
        {
            //If the player is on the right team (blue)
            if (StaticVariables.m_playerStates[i] == State.RIGHT)
            {
                //If no one has been asigned to the team yet 
                if (!foundBlue)
                {
                    //Since the current loop corresonds with the player I use the index to find the right controller 
                    switch (i)
                    {
                        //This will assign the first prefab the to correct controller 
                        case 0:
                            m_blueplayer1Prefab.GetComponent<Player>().m_controller = XboxCtrlrInput.XboxController.First;
                            m_blueTeam.Add(Instantiate(m_blueplayer1Prefab, m_blueSpawns[0].transform.position, Quaternion.identity));
                            break;
                        case 1:
                            m_blueplayer1Prefab.GetComponent<Player>().m_controller = XboxCtrlrInput.XboxController.Second;
                            m_blueTeam.Add(Instantiate(m_blueplayer1Prefab, m_blueSpawns[0].transform.position, Quaternion.identity));
                            break;
                        case 2:
                            m_blueplayer1Prefab.GetComponent<Player>().m_controller = XboxCtrlrInput.XboxController.Third;
                            m_blueTeam.Add(Instantiate(m_blueplayer1Prefab, m_blueSpawns[0].transform.position, Quaternion.identity));
                            break;
                        case 3:
                            m_blueplayer1Prefab.GetComponent<Player>().m_controller = XboxCtrlrInput.XboxController.Fourth;
                            m_blueTeam.Add(Instantiate(m_blueplayer1Prefab, m_blueSpawns[0].transform.position, Quaternion.identity));
                            break;
                    }
                    //The first blue player has been found 
                    foundBlue = true;
                }
                else
                {
                    switch (i)
                    {
                        //Repeated for the 2nd prefab
                        case 0:
                            m_blueplayer2Prefab.GetComponent<Player>().m_controller = XboxCtrlrInput.XboxController.First;
                            m_blueTeam.Add(Instantiate(m_blueplayer2Prefab, m_blueSpawns[1].transform.position, Quaternion.identity));
                            break;
                        case 1:
                            m_blueplayer2Prefab.GetComponent<Player>().m_controller = XboxCtrlrInput.XboxController.Second;
                            m_blueTeam.Add(Instantiate(m_blueplayer2Prefab, m_blueSpawns[1].transform.position, Quaternion.identity));
                            break;
                        case 2:
                            m_blueplayer2Prefab.GetComponent<Player>().m_controller = XboxCtrlrInput.XboxController.Third;
                            m_blueTeam.Add(Instantiate(m_blueplayer2Prefab, m_blueSpawns[1].transform.position, Quaternion.identity));
                            break;
                        case 3:
                            m_blueplayer2Prefab.GetComponent<Player>().m_controller = XboxCtrlrInput.XboxController.Fourth;
                            m_blueTeam.Add(Instantiate(m_blueplayer2Prefab, m_blueSpawns[1].transform.position, Quaternion.identity));
                            break;
                    }
                }
            }
            //Repeated if the player is of the left team (red)
            if (StaticVariables.m_playerStates[i] == State.LEFT)
            {
                if (!foundRed)
                {
                    switch (i)
                    {
                        case 0:
                            m_redplayer1Prefab.GetComponent<Player>().m_controller = XboxCtrlrInput.XboxController.First;
                            m_redTeam.Add(Instantiate(m_redplayer1Prefab, m_redSpawns[0].transform.position, Quaternion.identity));
                            break;
                        case 1:
                            m_redplayer1Prefab.GetComponent<Player>().m_controller = XboxCtrlrInput.XboxController.Second;
                            m_redTeam.Add(Instantiate(m_redplayer1Prefab, m_redSpawns[0].transform.position, Quaternion.identity));
                            break;
                        case 2:
                            m_redplayer1Prefab.GetComponent<Player>().m_controller = XboxCtrlrInput.XboxController.Third;
                            m_redTeam.Add(Instantiate(m_redplayer1Prefab, m_redSpawns[0].transform.position, Quaternion.identity));
                            break;
                        case 3:
                            m_redplayer1Prefab.GetComponent<Player>().m_controller = XboxCtrlrInput.XboxController.Fourth;
                            m_redTeam.Add(Instantiate(m_redplayer1Prefab, m_redSpawns[0].transform.position, Quaternion.identity));
                            break;
                    }

                    foundRed = true;
                }
                else
                {

                    switch (i)
                    {
                        case 0:
                            m_redplayer2Prefab.GetComponent<Player>().m_controller = XboxCtrlrInput.XboxController.First;
                            m_redTeam.Add(Instantiate(m_redplayer2Prefab, m_redSpawns[1].transform.position, Quaternion.identity));
                            break;
                        case 1:
                            m_redplayer2Prefab.GetComponent<Player>().m_controller = XboxCtrlrInput.XboxController.Second;
                            m_redTeam.Add(Instantiate(m_redplayer2Prefab, m_redSpawns[1].transform.position, Quaternion.identity));
                            break;
                        case 2:
                            m_redplayer2Prefab.GetComponent<Player>().m_controller = XboxCtrlrInput.XboxController.Third;
                            m_redTeam.Add(Instantiate(m_redplayer2Prefab, m_redSpawns[1].transform.position, Quaternion.identity));
                            break;
                        case 3:
                            m_redplayer2Prefab.GetComponent<Player>().m_controller = XboxCtrlrInput.XboxController.Fourth;
                            m_redTeam.Add(Instantiate(m_redplayer2Prefab, m_redSpawns[1].transform.position, Quaternion.identity));
                            break;
                    }
                }
            }
        }
        #endregion
    }
}