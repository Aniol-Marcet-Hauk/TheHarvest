using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    [SerializeField] private Texture2D m_Cursor;

    [SerializeField] private GameObject m_MainMenu, m_OptionsMenu;
    private void Start()
    {
        if(GameManager.gameManager != null)
        {
            Destroy(GameManager.gameManager.PLAYER.transform.parent.gameObject);
            Destroy(GameManager.gameManager.gameObject);
        }
        
        Cursor.visible = true;
        Cursor.SetCursor(m_Cursor, Vector2.zero, CursorMode.ForceSoftware);
    }
    public void Play()
    {
        SceneManager.LoadScene(1);
    }
    public void OpenOptions()
    {
        m_MainMenu.SetActive(false);
        m_OptionsMenu.SetActive(true);
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void CloseOptions()
    {
        m_MainMenu.SetActive(true);
        m_OptionsMenu.SetActive(false);
    }
}
