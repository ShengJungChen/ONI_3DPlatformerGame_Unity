using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour
{
    public Button instrcutions;
    private AudioSource audioSource;
    public AudioClip open;
    public AudioClip close;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    public void ShowInstructions()
    {
        instrcutions.GetComponent<Animator>().SetTrigger("Open");
        audioSource.PlayOneShot(open);
    }

    public void CloseInstructions()
    {
        instrcutions.GetComponent<Animator>().SetTrigger("Close");
        audioSource.PlayOneShot(close);
    }
    
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
