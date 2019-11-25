/*
 * Author:      Connor Young
 * Description: Keeps track of the current holder and owner of the ball 
 * Creation:    18/11/19
 * Modified:    20/11/19
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{

    public GameObject m_owner = null;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }
}
