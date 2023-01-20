using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{

    private bool isPaused = false;
    private UIController ui;
    public GameObject canvas;
    private AudioSource audioSource;
    public AudioClip pauseAudio;
    public AudioClip unPauseAudio;

    void Start()
    {
        ui = canvas.GetComponent<UIController>();
        audioSource = GetComponent<AudioSource>();
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void PauseGame()
    {
        isPaused = !isPaused;
        ui.isPaused = isPaused;

        if (isPaused)
            {
                Time.timeScale = 0;
                audioSource.PlayOneShot(pauseAudio);
            }
            else
            {
                Time.timeScale = 1;
                audioSource.PlayOneShot(unPauseAudio);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
