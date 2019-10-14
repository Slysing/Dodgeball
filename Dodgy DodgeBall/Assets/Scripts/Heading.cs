using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Description:    Changes the color of the back ground and heading 
/// Author:         Connor Young
/// Creation Date:  07/09/19
/// Modified:       08/09/19
/// </summary>
/// 
public class Heading : MonoBehaviour
{
    public Image HeadingImage;

    float TimeLeft;
    Color TargetColour; 
    
    // Start is called before the first frame update
    void Start()
    {
        //The initial color the image will be
        TargetColour = new Color(1.0f, 0.0f, 0.0f); //Set to red
    }

    // Update is called once per frame
    void Update()
    {
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
