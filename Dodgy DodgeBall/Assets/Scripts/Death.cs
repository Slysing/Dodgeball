using UnityEngine;

public class Death : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ball")
        {
            gameObject.SetActive(false);
        }
    }
}