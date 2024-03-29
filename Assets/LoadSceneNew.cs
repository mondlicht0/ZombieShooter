using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneNew : MonoBehaviour
{
    public void LoadGameplay()
    {
        SceneManager.LoadScene(3);
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(1);
    }
}
