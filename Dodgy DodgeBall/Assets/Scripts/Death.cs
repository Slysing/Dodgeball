using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Ball")
        {
            gameObject.SetActive(false);
        }
    }

}
