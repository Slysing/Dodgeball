using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonMananger : MonoBehaviour
{
    //public Button PlayButton;
    //private GameObject OptionsMenu;

    // Start is called before the first frame update
    void Start()
    {
        //MainMenu = GameObject.FindGameObjectWithTag("MainMenu");
        //OptionsMenu = GameObject.FindGameObjectWithTag("OptionsMenu");
        //OptionsMenu.SetActive(false);
        //MainMenu.SetActive(true);

        //PlayButton = GetComponent<Button>(); 

    }

    [SerializeField] GameObject[] buttons;

    // Try attacking a script with this function in it to the seperate buttons and see if that does anything
    // link: https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnMouseEnter.html
    private void OnMouseOver()
    {
        // Change colour
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject button in buttons)
        {
            Button currentButton = button.GetComponent<Button>();
            ColorBlock cb = currentButton.colors;
            cb.highlightedColor = Color.Lerp(Color.red, Color.blue, Mathf.PingPong(Time.time, 1));
            currentButton.colors = cb;
        }
        Debug.Log("FUCK");
        //Debug.Log(active.name); 

    }

    private void OnMouseEnter()
    {
        Debug.Log("SHIT");
        
    }


    private void OnMouseExit()
    {
        GameObject active = EventSystem.current.currentSelectedGameObject;
        if (active != null)
        {
            //Debug.Log("FUCK");
            Button currentButton = active.GetComponent<Button>();
            ColorBlock cb = currentButton.colors;
            cb.normalColor = Color.black;
        }
    }




    //public void Play()
    //{
    //    MainMenu.SetActive(false);  
    //}

    //public void Options()
    //{
    //    MainMenu.SetActive(false);
    //    OptionsMenu.SetActive(true); 
    //}

    //public void OptionsBack()
    //{
    //    MainMenu.SetActive(true);
    //    OptionsMenu.SetActive(false); 
    //}
}

public interface IPointerEnterHandler : IEventSystemHandler
{
    
}