using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    public void resume()
    {
        GameManager.instance.stateUnpaused();

    }

    public void restart()
    {
        //RJM this is a really bad restart method because it reloads the entire scene from scratch
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameManager.instance.stateUnpaused();
    }

    public void quit()
    {
        Application.Quit();
    }

    public void playerRespawn()
    {
        GameManager.instance.playerScript.spawnPlayer();
        GameManager.instance.stateUnpaused();

    }

    public void LoadLevel(int level) //this func is to load scenes
    {
        SceneManager.LoadScene(level);
    }
}
