using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//누르는 버튼 전체에 들어가는 스크립트
public class ButtonHandler : MonoBehaviour
{
    // 버튼 클릭소리를 위한 변수
    private Button myButton;
    private AudioClip clickSound;

    private void Start()
    {
        //클릭소리에 Resources하위 폴더의 AudioClip중 01.sounds폴더 하위의 click이름을 가진 파일을 불러온다.
        clickSound = Resources.Load<AudioClip>("01.Sounds/click");
        myButton = GetComponent<Button>();
        myButton.onClick.AddListener(ButtonClick);
    }

    // Method to be called when the button is clicked
    private void ButtonClick()
    {
        Debug.Log("Button Clicked!");
        AudioManager.instance.PlayOneShot(clickSound);
        // Add your custom behavior here
    }

    private void OnDestroy()
    {
        // Clean up the event listener when the script is destroyed
        myButton.onClick.RemoveListener(ButtonClick);
    }
}