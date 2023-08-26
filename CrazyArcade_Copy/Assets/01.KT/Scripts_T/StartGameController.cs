using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartGameController : MonoBehaviour
{
    //public GameObject loginScene_BGM;

    public AudioClip loginScene_BGM;
    public AudioClip logoSound;

    private Color firstColor;
    private Color endColor;
    private Color blackColor;
    private Image image;
    private float setTime = 0.0f;
    private float endTime = 1.0f;

    void Start()
    {
        firstColor = new Color(1, 1, 1, 1); //일반컬로 255로 쓰고싶으면 Color 32
        endColor = new Color(1, 1, 1, 0);
        blackColor = new Color(0, 0, 0, 1);
        image = GetComponent<Image>();
        AudioManager.instance.PlayOneShot(logoSound);

        StartCoroutine(StartVideo());

    }

    IEnumerator StartVideo()
    {
        while (setTime < endTime)
        {
            setTime += Time.deltaTime;
            float time = Mathf.Clamp01(setTime / endTime);
            image.color = Color.Lerp(blackColor, firstColor, time);
            yield return null;
        }

        StartCoroutine(EndVideo());
    }

    IEnumerator EndVideo()
    {
        
        yield return new WaitForSeconds(1.5f);

        setTime = 0.0f;

        while (setTime < endTime)
        {
            if(setTime > 0.3f)
            {
                AudioManager.instance.PlayMusicLoop(loginScene_BGM);
            }

            setTime += Time.deltaTime;
            float time = Mathf.Clamp01(setTime / endTime);
            image.color = Color.Lerp(firstColor, endColor, time);
            yield return null;
        }

        Destroy(gameObject);
    }
}
