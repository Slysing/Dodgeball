using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSound : MonoBehaviour
{
    public AudioSource bounce;
    public AudioClip collisionSound;

    void SetRef()
    {
        bounce = GetComponent<AudioSource>(); 
    }
    // Start is called before the first frame update
    void Start() 
    {
        SetRef();
    }
    private void Reset()
    {
        SetRef();
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
