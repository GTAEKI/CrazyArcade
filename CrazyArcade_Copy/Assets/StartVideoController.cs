using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartVideoController : MonoBehaviour
{
    public GameObject loginScene_BGM;

    private Color firstColor;
    private Color endColor;
    private Color blackColor;
    private Image image;

    private float setTime = 0.0f;
    private float endTime = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        firstColor = new Color(1, 1, 1, 1); //일반컬로 255로 쓰고싶으면 Color 32
        endColor = new Color(1, 1, 1, 0);
        blackColor = new Color(0, 0, 0, 1);
        image = GetComponent<Image>();

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
        
        yield return new WaitForSeconds(3.0f);
        loginScene_BGM.SetActive(true);

        setTime = 0.0f;

        while (setTime < endTime)
        {
            setTime += Time.deltaTime;
            float time = Mathf.Clamp01(setTime / endTime);
            image.color = Color.Lerp(firstColor, endColor, time);
            yield return null;
        }

        Destroy(gameObject);
    }
}
