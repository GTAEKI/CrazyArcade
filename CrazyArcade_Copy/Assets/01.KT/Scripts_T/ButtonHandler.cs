using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    // Reference to the button
    private Button myButton;
    private AudioClip clickSound;

    private void Start()
    {
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