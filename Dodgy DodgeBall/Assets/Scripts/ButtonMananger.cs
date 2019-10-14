using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Description:    Changes the color of the currently selected button 
/// Aurhtors:       Connor Young
/// Creation Date:  07/10/19
/// Modified:       09/10/19 
/// </summary>

public class ButtonMananger : MonoBehaviour
{

    [SerializeField] GameObject[] buttons;

    // Update is called once per frame
    void Update()
    {
        //Iterates through the array of buttons changing the color of the "highlighted" color variable giving the chaging color effect 
        foreach (GameObject button in buttons)
        {
            Button currentButton = button.GetComponent<Button>();
            ColorBlock cb = currentButton.colors;
            cb.highlightedColor = Color.Lerp(Color.red, Color.blue, Mathf.PingPong(Time.time, 1));
            currentButton.colors = cb;
        }
    }
}
    