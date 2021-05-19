using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [SerializeField]
    string mouseHoverSound = "ButtonHover";
    [SerializeField]
    string buttonPressSound = "ButtonPress";
    AudioManager audioManager;
    private void Start()
    {
        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.LogError("Freak Out, where is audio");
        }
    }
    public void Quit()
    {
        audioManager.PlaySound(buttonPressSound);
        Debug.Log("applicationQuit");
        Application.Quit();

    }

    
    public void Retry()
    {
        audioManager.PlaySound(buttonPressSound);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnMouseOver()
    {
        audioManager.PlaySound(mouseHoverSound);
    }
}
