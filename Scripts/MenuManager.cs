using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    string hoverOverSound = "ButtonHover";
    [SerializeField]
    string pressOverSound = "ButtonPress";
    AudioManager audioManager;
    private void Start()
    {
        audioManager = AudioManager.instance;
        if(audioManager == null)
        {

            Debug.Log("Mo audio Manager");
        }
    }
    public void StartGame()
    {
        audioManager.PlaySound(pressOverSound);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void QuitGame()
    {
        audioManager.PlaySound(pressOverSound);
        Application.Quit();
    }

    public void OnMouseOver()
    {
        audioManager.PlaySound(hoverOverSound);
    }
}
