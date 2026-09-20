using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pauseMenuScript : MonoBehaviour
{
    [SerializeField] private GameObject m_Options;
    [SerializeField] private GameObject[] m_PauseMenuButtons;
    
    public void Back()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
    public void Settings()
    {
        for (int i = 0; i < m_PauseMenuButtons.Length; i++)
        {
            m_PauseMenuButtons[i].SetActive(false);
            m_Options.SetActive(true);
        }
    }
    public void Quit()
    {
        Application.Quit();
    }
}
