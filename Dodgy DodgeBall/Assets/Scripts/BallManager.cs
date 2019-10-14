using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    public bool m_active = false;
    public int m_spawnInterval = 15;
    public GameObject m_ball = null;
    public List<GameObject> m_spawnPoints = new List<GameObject>();

    private bool m_isSpawning = false;
    private int m_currentBallCount = 0;
    private List<GameObject> m_ballPool = new List<GameObject>();
    private Vector3 m_ballPoolPosition = new Vector3(999f,999f,999f);
    private int m_ballPoolSize = 100;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(m_ball, "Ball prefab needed in BallManager Script");
        GameObject pool = new GameObject();
        pool.name = "Ball Pool";

        for(int i = 0; i < m_ballPoolSize; ++i)
        {
            var newBall = Instantiate(m_ball,pool.transform,false);
            newBall.SetActive(false);
            m_ballPool.Add(newBall);
        }
        if(m_active)
            StartCoroutine(BallSpawner());
    }

    // Update is called once per frame
    void Update()
    {
        if(!m_active && m_isSpawning)
        {
            StopCoroutine(BallSpawner());
        }
        else if(m_active && !m_isSpawning)
        {
            StartCoroutine(BallSpawner());
        }

        if(Input.GetKeyDown(KeyCode.P))
        {
            RemoveBall(m_ballPool[2]);
        }
    }

    void RemoveBall(GameObject ball)
    {
        ball.SetActive(false);
        m_currentBallCount--;

        for(int i = 0; i < m_currentBallCount; ++i)
        {
            if(!m_ballPool[i].activeSelf)
            {
                int count = i;
                while(m_ballPool[count + 1].activeSelf)
                {
                    var temp = m_ballPool[count + 1];
                    m_ballPool[count + 1] = m_ballPool[count];
                    m_ballPool[count] = temp;
                    count++;
                }
            }
        }
    }

    // ball spawner will spawn a set of balls every m_spawnInterval and only spawn if there is no player or ball in the way
    // instead of instantiating and destroy setup an object pool
    IEnumerator BallSpawner()
    {
        m_isSpawning = true;
        while(true)
        {
            foreach(var point in m_spawnPoints)
            {
                var tempBall = m_ballPool[m_currentBallCount];
                tempBall.SetActive(true);
                tempBall.transform.position = point.transform.position;
                m_currentBallCount++;

                var colliders = Physics.OverlapSphere(point.transform.position,.5f);
                int ballCount = 0;
                foreach(var obj in colliders)
                {
                    if(obj.gameObject.layer == 9)
                    {
                        ballCount++;
                    }
                    else if(obj.gameObject.layer == 10)
                    {
                        ballCount++;
                    }
                }

                if(ballCount > 1)
                {
                    tempBall.transform.position = m_ballPoolPosition;
                    tempBall.SetActive(false);
                    m_currentBallCount--;
                }
            }
            for(int i = m_spawnInterval; i > 0; i--)
            {
                Debug.Log(i);
                yield return new WaitForSeconds(1f);
            }

            if(m_spawnInterval == 0)
            {
                yield return new WaitForSeconds(m_spawnInterval);
            }
        }
    }


    void OnDrawGizmos()
    {
        foreach(var point in m_spawnPoints)
        {
            Gizmos.DrawWireSphere(point.transform.position,.5f);
        }
    }

}
