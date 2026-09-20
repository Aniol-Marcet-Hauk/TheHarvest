using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pauseMenuScript : MonoBehaviour
{
    [SerializeField] private GameObject options;
    [SerializeField] private GameObject[] pauseMenuButtons;
    
    public void Back()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
    public void Settings()
    {
        for (int i = 0; i < pauseMenuButtons.Length; i++)
        {
            pauseMenuButtons[i].SetActive(false);
            options.SetActive(true);
        }
    }
    public void Quit()
    {
        Application.Quit();
    }
}
