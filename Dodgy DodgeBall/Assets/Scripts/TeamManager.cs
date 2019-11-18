/* TeamManager.cs
 * Authors: Leith Merrifield
 * Description: Handles creation of teams and spawning/resetting of players
 * Creation:
 * Modifield:
 */

using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    public static List<GameObject> m_blueTeam = new List<GameObject>();
    public static List<GameObject> m_redTeam = new List<GameObject>();
    public GameObject m_redplayer1Prefab = null;
    public GameObject m_redplayer2Prefab = null;

    public GameObject m_blueplayer1Prefab = null;
    public GameObject m_blueplayer2Prefab = null;
    public List<GameObject> m_blueSpawns = new List<GameObject>();
    public List<GameObject> m_redSpawns = new List<GameObject>();

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

    private void Start()
    {
        // This only here in place of a team select
        //m_redTeam.Add(Instantiate(m_redplayer1Prefab, m_redSpawns[0].transform.position, Quaternion.identity));
        //m_redTeam.Add(Instantiate(m_redplayer2Prefab, m_redSpawns[1].transform.position, Quaternion.identity));

        //m_blueTeam.Add(Instantiate(m_blueplayer1Prefab, m_blueSpawns[0].transform.position, Quaternion.identity));
        //m_blueTeam.Add(Instantiate(m_blueplayer2Prefab, m_blueSpawns[1].transform.position, Quaternion.identity));

        bool foundBlue = false;
        bool foundRed = false;
        for (int i = 0; i < StaticVariables.m_playerStates.Length; i++)
        {
            if (StaticVariables.m_playerStates[i] == State.RIGHT)
            {
                if (!foundBlue)
                {
                    switch (i)
                    {
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
                    foundBlue = true;
                }
                else
                {
                    switch (i)
                    {
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
    }
}