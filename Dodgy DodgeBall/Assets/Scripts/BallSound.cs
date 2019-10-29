using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSound : MonoBehaviour
{
    public AudioSource bounce;
    public AudioClip collisionSound;
    // Start is called before the first frame update
    void Start() 
    {
        bounce = GetComponent<AudioSource>(); 
    }

    void OnCollisionEnter(Collision _c)
    {
        if (_c.gameObject.tag == "Wall")
        {
            bounce.PlayOneShot(collisionSound);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
