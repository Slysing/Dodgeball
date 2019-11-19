/* GameSettings.cs
 * Authors: Leith Merrifield
 * Description: Grabs and applies the settings from playerprefs
 * Creaiton: 10/11/2019
 * Modified: 11/11/2019
 */

using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public PistonControl m_pistonControl = null;
    public AudioSource m_audioControl = null;

    private void Start()
    {
        if (m_pistonControl == null)
            m_pistonControl = gameObject.GetComponent<PistonControl>();
        GetSettings();
    }

    // Grabs settings from player prefs and applies them accordingly
    public void GetSettings()
    {
        m_pistonControl.m_togglePistons = PlayerPrefs.GetInt("PistonsToggle") == 1 ? true : false;

        if (PlayerPrefs.GetInt("MusicToggle") == 0)
        {
            m_audioControl.mute = true;
            m_audioControl.Pause();
        }
        else
        {
            m_audioControl.mute = false;
            m_audioControl.UnPause();
        }

        m_audioControl.volume = PlayerPrefs.GetFloat("Volume");
    }
}