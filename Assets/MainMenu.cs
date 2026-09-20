using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    [SerializeField] private Texture2D cursor;

    [SerializeField] private GameObject mainMenu, optionsMenu;
    private void Start()
    {
        if(GameManager.gameManager != null)
        {
            Destroy(GameManager.gameManager.PLAYER.transform.parent.gameObject);
            Destroy(GameManager.gameManager.gameObject);
        }
        
        Cursor.visible = true;
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.ForceSoftware);
    }
    public void Play()
    {
        SceneManager.LoadScene(1);
    }
    public void OpenOptions()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void CloseOptions()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }
}
