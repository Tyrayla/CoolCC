using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    public void TutorialPlayGame()
    {
        SceneManager.LoadSceneAsync("MainLevel");
    }

}
