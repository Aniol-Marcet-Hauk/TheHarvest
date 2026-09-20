using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class goToNextRoom : MonoBehaviour
{
   
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            //DontDestroyOnLoad(GameManager.gameManager.PLAYER.transform.parent);
            GameManager.gameManager.loadingScene.SetActive(true);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
