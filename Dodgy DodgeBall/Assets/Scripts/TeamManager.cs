using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    public static List<GameObject> m_blueTeam = new List<GameObject>();
    public static List<GameObject> m_redTeam = new List<GameObject>();
    public GameObject m_playerPrefab = null;
    public List<GameObject> m_blueSpawns = new List<GameObject>();
    public List<GameObject> m_redSpawns = new List<GameObject>();

    public Material m_redMaterial = null;
    public Material m_blueMaterial = null;

    ///<summary>
    /// team: 1 = blue team,  2 = red team
    ///</summary>
    void AddPlayer(int team, GameObject player)
    {
        switch(team)
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
        foreach(var player in m_blueTeam)
        {
            player.transform.position = m_blueSpawns[idx].transform.position;  
            player.GetComponent<Player>().m_isAlive = true;
            player.SetActive(true);
            idx++;
        }
        idx = 0;
        foreach(var player in m_redTeam)
        {
            player.transform.position = m_redSpawns[idx].transform.position;
            player.GetComponent<Player>().m_isAlive = true;
            player.SetActive(true);
            idx++;
        }
    }

    void Start()
    {
        // This only here in place of a team select
        m_blueTeam.Add(Instantiate(m_playerPrefab,m_blueSpawns[0].transform.position, Quaternion.identity));
        m_blueTeam.Add(Instantiate(m_playerPrefab,m_blueSpawns[1].transform.position, Quaternion.identity));
        m_redTeam.Add(Instantiate(m_playerPrefab,m_redSpawns[0].transform.position, Quaternion.identity));
        m_redTeam.Add(Instantiate(m_playerPrefab,m_redSpawns[1].transform.position, Quaternion.identity));

        foreach(var i in m_blueTeam)
            i.GetComponent<MeshRenderer>().material = m_blueMaterial;
        foreach(var i in m_redTeam)
            i.GetComponent<MeshRenderer>().material = m_redMaterial;
    }
}
