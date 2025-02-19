﻿/* ButtonManager.cs
 * Authors: Connor Young, Leith Merrifled
 * Description: Changes colour of buttons/handles click events for the menu buttons and saving of settings
 * Creation: 07/10/2019
 * Modified: 11/11/2019
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ButtonMananger : MonoBehaviour
{
    [SerializeField] private GameObject[] m_colourButtons = null;
    private List<AnimationClip> m_animationClips = new List<AnimationClip>();
    [SerializeField] public bool m_isTranslating = false;
    private Color m_defaultTextColour = new Color();
    private bool m_runColourOnce = false;

    public Animator m_menuAnim = null;

    [Header("Start Button")]
    public Button m_startButton = null;
    public GameObject m_selectCanvas = null;

    [Header("Settings Menu")]
    public GameObject m_settingsPanel = null;

    [SerializeField] private Button m_settingsButton = null;
    [SerializeField] private Button m_settingsBackButton = null;
    public Toggle m_pistonToggle = null;
    public Toggle m_musicToggle = null;
    public Slider m_volumeSlider = null;

    //[SerializeField] private List<GameObject> m_toggles = new List<GameObject>();
    private int m_settingsClipIndex = 0;

    [Header("Controls Menu")]
    [SerializeField] private Button m_controlButton = null;

    [SerializeField] private GameObject m_controlPanel = null;
    [SerializeField] private Button m_controlBackButton = null;

    [Header("Credits Button")]
    [SerializeField] private Button m_creditsButton = null;

    [Header("Exit Button")]
    [SerializeField] private Button m_exitButton;

    public EventSystem m_currentEventSystem;
    [HideInInspector] public bool m_startClicked;

    [Header("Main Menu Canvas")]
    public AudioSource m_menuMusic = null;
    [SerializeField] private GameObject m_mainMenuCanvas = null;
    public float m_deactivateDelayTime = 0;
    public float m_activateDelayTime = 0;
    [SerializeField] private GameObject m_teamSelectCanvas = null;

    

    private void Start()
    {
        m_currentEventSystem.GetComponent<EventSystem>();
        m_settingsButton.onClick.AddListener(SettingsClick);
        m_settingsBackButton.onClick.AddListener(SettingsBackClick);
        m_startButton.onClick.AddListener(StartGameClick);
        m_controlButton.onClick.AddListener(ControlsClick);
        m_controlBackButton.onClick.AddListener(ControlBackClick);
        m_creditsButton.onClick.AddListener(CreditsClick);

        // runs for the first ever launch of the game on the device
        // otherwise load previous settings
        if (PlayerPrefs.GetInt("FIRSTOPEN", 1) == 1)
        {
            m_musicToggle.isOn = true;
            m_pistonToggle.isOn = true;
            m_volumeSlider.value = 0.2f;
            PlayerPrefs.SetInt("FIRSTOPEN", 0);
        }
        else
        {
            m_volumeSlider.value = PlayerPrefs.GetFloat("Volume", 0.2f);
            m_musicToggle.isOn = PlayerPrefs.GetInt("MusicToggle", 1) == 1 ? true : false;
            m_pistonToggle.isOn = PlayerPrefs.GetInt("PistonToggle", 1) == 1 ? true : false;

            m_menuMusic.volume = m_volumeSlider.value;

            if (PlayerPrefs.GetInt("MusicToggle", 1) == 0)
            {
                m_menuMusic.mute = true;
                m_menuMusic.Pause();
            }
            else
            {
                m_menuMusic.mute = false;
                m_menuMusic.UnPause();
            }
        }

        RuntimeAnimatorController ac = m_menuAnim.runtimeAnimatorController;    //Get Animator controller
        for (int i = 0; i < ac.animationClips.Length; i++)                 //For all animations
        {
            m_animationClips.Add(ac.animationClips[i]);
            if (m_animationClips[i].name == "Settings") // saves index of setting animationclip
                m_settingsClipIndex = i;
        }
    }

    // Reasoning behind using events instead of unity editor was that i to add a restriction to when you can press a button
    private void SettingsClick()
    {
        if (!m_isTranslating)
        {
            m_menuAnim.SetTrigger("ToggleSettings");
            for (int i = 0; i < m_animationClips.Count; i++)
            {
                if (m_animationClips[i].name == "Settings")
                {
                    StartCoroutine(CameraTranslate(m_animationClips[i]));
                    m_isTranslating = true;
                    m_settingsPanel.SetActive(true);
                    m_settingsBackButton.Select();
                    break;
                }
            }
        }
    }

    // applies settings to prefs and translates back to main menu
    private void SettingsBackClick()
    {
        if (!m_isTranslating)
        {
            m_isTranslating = true;

            //ApplySettings();

            StartCoroutine(CameraTranslate(m_animationClips[m_settingsClipIndex]));
            m_menuAnim.SetTrigger("ToggleSettings");
            m_settingsPanel.SetActive(false);
            m_settingsButton.Select();
        }
    }

    // will wait for length of animationclip
    public IEnumerator CameraTranslate(AnimationClip animClip)
    {
        float tempTime = animClip.length;
        yield return new WaitForSeconds(tempTime);
        m_isTranslating = false;
    }


    private void ApplySettings()
    {
        int piston = m_pistonToggle.isOn ? 1 : 0;
        int music = m_musicToggle.isOn ? 1 : 0;
        float volume = m_volumeSlider.value;
        m_menuMusic.volume = volume;

        if (music == 0)
        {
            m_menuMusic.mute = true;
            m_menuMusic.Pause();
        }
        else
        {
            m_menuMusic.mute = false;
            m_menuMusic.UnPause();
        }

        PlayerPrefs.SetInt("PistonToggle", piston);
        PlayerPrefs.SetInt("MusicToggle", music);
        PlayerPrefs.SetFloat("Volume", volume);
    }

    private void ControlsClick()
    {
        if (!m_isTranslating)
        {
            m_menuAnim.SetTrigger("ToggleSettings");
            for (int i = 0; i < m_animationClips.Count; i++)
            {
                if (m_animationClips[i].name == "Settings")
                {
                    StartCoroutine(CameraTranslate(m_animationClips[i]));
                    m_isTranslating = true;
                    m_controlPanel.SetActive(true);
                    m_controlBackButton.Select();
                    break;
                }
            }
        }
    }

    private void ControlBackClick()
    {
        if (!m_isTranslating)
        {
            m_isTranslating = true;

            ApplySettings();

            StartCoroutine(CameraTranslate(m_animationClips[m_settingsClipIndex]));
            m_menuAnim.SetTrigger("ToggleSettings");
            m_controlPanel.SetActive(false);
            m_controlButton.Select();
        }
    }

    private void StartGameClick()
    {
        if (!m_isTranslating)
        {
            m_menuAnim.SetTrigger("ToggleStart");
            for (int i = 0; i < m_animationClips.Count; i++)
            {
                if (m_animationClips[i].name == "CharacterSelect")
                {
                    StartCoroutine(CameraTranslate(m_animationClips[i]));
                    m_isTranslating = true;
                    m_selectCanvas.SetActive(true);
                    StartCoroutine(DelayedDeactivate(m_mainMenuCanvas));
                    m_currentEventSystem.SetSelectedGameObject(null); 
                    break;
                }
            }
        }
    }

    public void SelectScreenBack()
    {
        if (!m_isTranslating)
        {
            m_menuAnim.SetTrigger("ToggleMainMenu");
            for (int i = 0; i < m_animationClips.Count; i++)
            {
                if (m_animationClips[i].name == "CharacterSelect")
                {
                    m_isTranslating = true;
                    StartCoroutine(CameraTranslate(m_animationClips[i]));
                    StartCoroutine(DelayedDeactivate(m_teamSelectCanvas));
                    //m_startButton.Select();
                    break;
                }
            }
        }
    }
    
    private void CreditsClick()
    {
        SceneManager.LoadScene(2);
    }

    // Update is called once per frame
    private void Update()
    {
        //m_menuMusic.volume = m_volumeSlider.value;
        ApplySettings();
        //Iterates through the array of buttons changing the color of the "highlighted" color variable giving the chaging color effect
        foreach (GameObject button in m_colourButtons)
        {
            if(button.transform.childCount > 0)
            {
                
                Transform child = button.transform.Find("Text");
                TextMeshProUGUI text = child.gameObject.GetComponent<TextMeshProUGUI>();
                if (!m_runColourOnce)
                {
                    m_defaultTextColour = text.color;
                    m_runColourOnce = true;
                }
                Color rainbow = Color.Lerp(Color.red, Color.blue, Mathf.PingPong(Time.time, 1));

                if (EventSystem.current.currentSelectedGameObject == button &&
                    button.name != "Volume")
                    text.color = rainbow;
                else
                    text.color = m_defaultTextColour;

                if(button.name == "Volume")
                {
                    Transform Knob = button.transform.Find("Handle Slide Area/Handle");

                    if (EventSystem.current.currentSelectedGameObject == button)
                        Knob.gameObject.GetComponent<Image>().color = Color.blue;
                    else
                        Knob.gameObject.GetComponent<Image>().color = Color.black;

                }
                continue;
            }

            Button currentButton = button.GetComponent<Button>();
            ColorBlock cb = currentButton.colors;
            cb.highlightedColor = Color.Lerp(Color.red, Color.blue, Mathf.PingPong(Time.time, 1));
            currentButton.colors = cb;
        }
    }



    private IEnumerator DelayedDeactivate(GameObject turnOff)
    {
        yield return new WaitForSeconds(m_deactivateDelayTime);

        turnOff.SetActive(false); 
    }
}