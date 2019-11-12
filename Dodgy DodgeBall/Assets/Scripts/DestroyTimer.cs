using UnityEngine;

public class DestroyTimer : MonoBehaviour
{
    public float timer = 3;

    // Update is called once per frame
    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Destroy(gameObject);
        }
    }
}