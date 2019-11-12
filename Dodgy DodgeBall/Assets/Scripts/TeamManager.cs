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

 private void Start()
    {
        // This only here in place of a team select
        //m_redTeam.Add(Instantiate(m_redplayer1Prefab, m_redSpawns[0].transform.position, Quaternion.identity));
        //m_redTeam.Add(Instantiate(m_redplayer2Prefab, m_redSpawns[1].transform.position, Quaternion.identity));

        //m_blueTeam.Add(Instantiate(m_blueplayer1Prefab, m_blueSpawns[0].transform.position, Quaternion.identity));
        //m_blueTeam.Add(Instantiate(m_blueplayer2Prefab, m_blueSpawns[1].transform.position, Quaternion.identity));

        //m_blueTeam[0].GetComponent<Player>().m_PlayerControllerNumber = 2;
        ////m_blueTeam[0].GetComponent<Player>().gameObject.tag = "BlueTeam";
        //m_blueTeam[0].transform.Find("Catch_Range").gameObject.tag = "RedTeam";
        ////m_blueTeam[1].GetComponent<Player>().m_PlayerControllerNumber = 2;
        //m_redTeam[0].GetComponent<Player>().m_PlayerControllerNumber = 1;
        //m_redTeam[0].transform.Find("Catch_Range").gameObject.tag = "RedTeam";

        //m_redTeam[1].GetComponent<Player>().m_PlayerControllerNumber = 4;
        //m_blueTeam[0].GetComponent<MeshRenderer>().material = m_blueMaterial;
        //m_redTeam[0].GetComponent<MeshRenderer>().material = m_redMaterial;

        bool foundBlue = false;
        bool foundRed = false;
        for (int i = 0; i < StaticVariables.m_playerStates.Length; i++)
        {
            if (StaticVariables.m_playerStates[i] == State.RIGHT)
            {
                if (!foundBlue)
                {
                    m_blueTeam.Add(Instantiate(m_blueplayer1Prefab, m_blueSpawns[0].transform.position, Quaternion.identity));
                    foundBlue = true;
                }
                else
                    m_blueTeam.Add(Instantiate(m_blueplayer2Prefab, m_blueSpawns[1].transform.position, Quaternion.identity));
            }
            if (StaticVariables.m_playerStates[i] == State.LEFT)
            {
                if (!foundRed)
                {
                    m_redTeam.Add(Instantiate(m_redplayer1Prefab, m_redSpawns[0].transform.position, Quaternion.identity));
                    foundRed = true;  
                }
                else
                    m_redTeam.Add(Instantiate(m_redplayer2Prefab, m_redSpawns[1].transform.position, Quaternion.identity));

            }
        }
        //{
        //    if(playerlist[i] == LEFT) //blue
        //    {
        //        if (!foundBlue)
        //        {
        //            m_blueTeam.Add(Instantiate(m_blueplayer1Prefab, m_blueSpawns[i].transform.position, Quaternion.identity));
        //            foundBlue = true;
        //        }
        //        else
        //        {
        //            //other prefab
        //        }
        //    }

        //    if (playerlist[i] == Right) //red
        //    {
        //        if (!foundBlue)
        //        {
        //            m_blueTeam.Add(Instantiate(m_blueplayer1Prefab, m_blueSpawns[i].transform.position, Quaternion.identity));
        //            foundBlue = true;
        //        }
        //        else
        //        {
        //            //other prefab
        //        }
        //    }
        //}

        //if (StaticVariables.m_p1Blue)
        //    m_blueTeam.Add(Instantiate(m_blueplayer1Prefab, m_blueSpawns[0].transform.position, Quaternion.identity));
        //else
        //    m_redTeam.Add(Instantiate(m_redplayer1Prefab, m_redSpawns[0].transform.position, Quaternion.identity));
        //if (StaticVariables.m_p2Blue)
        //    m_blueTeam.Add(Instantiate(m_blueplayer2Prefab, m_blueSpawns[1].transform.position, Quaternion.identity));
        //else
        //    m_redTeam.Add(Instantiate(m_redplayer2Prefab, m_redSpawns[1].transform.position, Quaternion.identity));


    }
}