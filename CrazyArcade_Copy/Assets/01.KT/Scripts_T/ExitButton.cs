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

        // 현재 활성화된 씬을 언로드 함
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        // 다음씬으로 이동함
        PhotonNetwork.LoadLevel("02.ReadyScene");

    }
}
