using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallIncrease : MonoBehaviour
{
    public AudioSource bounce;
    public AudioClip collisionSound;

    void SetRef()
    {
        bounce = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
    }
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {

        SetRef();
    }
    private void Reset()
    {
        SetRef();
    }
    void OnCollisionExit(Collision _c)
    {
        if (_c.gameObject.tag == "Wall")
        {
            bounce.PlayOneShot(collisionSound);
               Debug.Log("testbounce " + rb.velocity.magnitude);
            // change the value after magnitude within the f eg "15f" to alter the speed increase from the projectile
            if (rb.velocity.magnitude < 15f)
            rb.AddForce(rb.velocity, ForceMode.VelocityChange);
        }
    }

    

    // Update is called once per frame
    void Update() { }

}
