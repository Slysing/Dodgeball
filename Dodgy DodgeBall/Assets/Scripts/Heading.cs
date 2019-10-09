using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Heading : MonoBehaviour
{
    public Image HeadingImage;

    float TimeLeft;
    Color TargetColour; 
    

    // Start is called before the first frame update
    void Start()
    {
        TargetColour = new Color(1.0f, 0.0f, 0.0f); 
    }

    // Update is called once per frame
    void Update()
    {
        //HeadingImage.color = Color.Lerp(Color.red, Color.blue, Mathf.PingPong(Time.time, 1));

        if (TimeLeft <= Time.deltaTime)
        {
            HeadingImage.color = TargetColour;

            TargetColour = new Color(Random.value, Random.value, Random.value);
            TimeLeft = 1;
        }
        else
        {
            HeadingImage.color = Color.Lerp(HeadingImage.color, TargetColour, Time.deltaTime / TimeLeft);

            TimeLeft -= Time.deltaTime;
        }
    }
}
