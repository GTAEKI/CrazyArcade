using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class ExitButton : MonoBehaviour
{
    public void OnExitButton()
    {
        StartCoroutine(Exit());   
    }

    IEnumerator Exit()
    {
        yield return new WaitForSeconds(0.5f);

        // Unload the current scene
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        // Load the next scene
        PhotonNetwork.LoadLevel("02.ReadyScene");

    }
}
