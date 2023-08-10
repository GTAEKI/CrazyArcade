using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    // Start is called before the first frame update
    public void StartSceneChange()
    {
        SceneManager.LoadScene("ReadyScene_J");
    }

    public void ReadySceneChange()
    {
        SceneManager.LoadScene("PlayScene_J");
    }

    public void PlaySceneChange()
    {
        SceneManager.LoadScene("StartScene_J");
    }

}
