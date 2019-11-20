/* BallManager.cs
 * Authors: Leith Merrifield
 * Description: Controls the spawing of balls on specific spawn points
 * Modified: 10/11/2019
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BallManager : MonoBehaviour
{
    public bool m_active = false;
    public int m_spawnInterval = 15;
    public GameObject m_ball = null;
    public List<GameObject> m_spawnPoints = new List<GameObject>();

    private bool m_isSpawning = false;
    private int m_currentBallCount = 0;
    private List<GameObject> m_ballPool = new List<GameObject>();
    private Vector3 m_ballPoolPosition = new Vector3(999f, 999f, 999f);
    private int m_ballPoolSize = 200;
    private static System.Random rng = new System.Random();

    public Material m_redMaterial = null;
    public Material m_blueMaterial = null;
    public Material m_greenMaterial = null;
    

    // Start is called before the first frame update
    private void Start()
    {
        Debug.Assert(m_ball, "Ball prefab needed in BallManager Script");
        GameObject pool = new GameObject();
        pool.name = "Ball Pool";

        for (int i = 0; i < m_ballPoolSize; ++i)
        {
            var newBall = Instantiate(m_ball, pool.transform, false);
            newBall.SetActive(false);
            m_ballPool.Add(newBall);
        }
        if (m_active)
        {
            StartCoroutine("BallSpawner");
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (RoundManager.m_pauseGame)
            return;

        if (!m_active && m_isSpawning)
        {
            StopCoroutine("BallSpawner");
        }
        else if (m_active && !m_isSpawning)
        {
            StartCoroutine("BallSpawner");
        }
    }

    // removes specific ball and reorganises the pool
    private void RemoveBall(GameObject ball)
    {
        ball.SetActive(false);
        m_currentBallCount--;

        for (int i = 0; i < m_currentBallCount; ++i)
        {
            if (!m_ballPool[i].activeSelf)
            {
                int count = i;
                while (m_ballPool[count + 1].activeSelf)
                {
                    var temp = m_ballPool[count + 1];
                    m_ballPool[count + 1] = m_ballPool[count];
                    m_ballPool[count] = temp;
                    count++;
                }
            }
        }
    }

    public void ResetBalls()
    {
        print("Imma reset now");
        Shuffle(ref m_ballPool);

        foreach (GameObject ball in m_ballPool)
        {
            ball.SetActive(false);
            ball.transform.position = m_ballPoolPosition;
            ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
            ball.tag = "Neutral Ball";
            ball.gameObject.layer = 9;
            ball.GetComponent<MeshRenderer>().material = m_greenMaterial;
        }
        m_currentBallCount = 0;
        StopCoroutine("BallSpawner");
        StartCoroutine("BallSpawner");
    }

    // ball spawner will spawn a set of balls every m_spawnInterval and only spawn if there is no player or ball in the way
    // instead of instantiating and destroy setup an object pool
    private IEnumerator BallSpawner()
    {
        m_isSpawning = true;
        while (true)
        {
            // loops through all the spawn points
            foreach (var point in m_spawnPoints)
            {
                // activates the ball
                var tempBall = m_ballPool[m_currentBallCount];
                tempBall.SetActive(true);
                tempBall.transform.position = point.transform.position;
                m_currentBallCount++;

                // Checks if there is already a ball or player
                // on the spawn point
                var colliders = Physics.OverlapSphere(point.transform.position, .5f);
                int ballCount = 0; // used to grab whats inside the sphere
                foreach (var obj in colliders)
                {
                    if (obj.tag == "Neutral Ball")
                    {
                        ballCount++;
                    }
                    else if (obj.gameObject.layer == 10)
                    {
                        ballCount++;
                    }
                }

                // sends the ball back to the pool
                if (ballCount > 1)
                {
                    tempBall.transform.position = m_ballPoolPosition;
                    tempBall.SetActive(false);
                    m_currentBallCount--;
                }
            }
            for (int i = m_spawnInterval; i > 0; i--)
            {
                // if the game is in a paused state then this will run
                // basically resuming the corroutine every 1 second
                // to check if the game has started yet
                while (RoundManager.m_pauseGame)
                {
                    yield return null;
                }
                if (!RoundManager.m_isPlaying)
                {
                    i++;
                    yield return new WaitForSeconds(1f);
                    continue;
                }
                yield return new WaitForSeconds(1f);
            }

            if (m_spawnInterval == 0)
            {
                yield return new WaitForSeconds(m_spawnInterval);
            }
        }
    }

    private void OnDrawGizmos()
    {
        foreach (var point in m_spawnPoints)
        {
            Gizmos.DrawWireSphere(point.transform.position, .5f);
        }
    }

    // shuffles a passed in list
    public static void Shuffle<T>(ref List<T> list)
    {
        int number = list.Count;
        while(number > 1)
        {
            number--;
            int next = rng.Next(number + 1);
            T value = list[next];
            list[next]= list[number];
            list[number] = value;
        }
    }
}