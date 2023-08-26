using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSound : MonoBehaviour
{
    public AudioClip mapSound;
    private AudioClip gameStartSound;

    private void Start()
    {
        gameStartSound = Resources.Load<AudioClip>("01.Sounds/game_start");
        AudioManager.instance.PlayOneShot(gameStartSound);
        AudioManager.instance.PlayMusicLoop(mapSound);
    }
}
