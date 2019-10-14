using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    public bool m_active = false;
    public int m_spawnInterval = 15;
    public GameObject m_ball = null;
    public List<GameObject> m_spawnPoints = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(m_ball, "Ball prefab needed in BallManager Script");

        StartCoroutine(BallSpawner());
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    // ball spawner will spawn a set of balls every m_spawnInterval and only spawn if there is no player or ball in the way
    // instead of instantiating and destroy setup an object pool
    IEnumerator BallSpawner()
    {
        while(true)
        {
            Debug.Log("Spawn");
            foreach(var point in m_spawnPoints)
            {
                var tempBall = Instantiate(m_ball,point.transform.position, Quaternion.identity);
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
                        ballCount += 2;
                    }
                }

                if(ballCount > 2)
                {
                    Destroy(tempBall);
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
