/* Heading.cs
 * Authors: Connor Young
 * Description: Changes the colour of the background and heading
 * Creation: 07/09/2019
 * Modified: 08/09/2019
 */

using UnityEngine;
using UnityEngine.UI;

/// <summary> Changes the color of the back ground and heading </summary>
public class Heading : MonoBehaviour
{
    public Image HeadingImage;
    private float TimeLeft;
    private Color TargetColour;

    // Start is called before the first frame update
    private void Start()
    {
        //The initial color the image will be
        TargetColour = new Color(1.0f, 0.0f, 0.0f); //Set to red
    }

    // Update is called once per frame
    private void Update()
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