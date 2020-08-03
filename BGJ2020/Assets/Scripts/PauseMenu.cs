using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused;
    [SerializeField]
    private GameObject pauseMenuUI;
    private AudioManager audioManager;
    private float savedThemePitch;
    private AudioSource theme;
    private AudioSource button;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        theme = audioManager.FindSource("Theme");
        button = audioManager.FindSource("Button");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused == false)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }
    }

    private void Pause()
    {
        savedThemePitch = theme.pitch;
        theme.pitch = audioManager.themePitch;
        button.Play();
        GameIsPaused = true;
        Time.timeScale = 0f;
        pauseMenuUI.SetActive(true);
    }

    public void Resume()
    {
        theme.pitch = savedThemePitch;
        button.Play();
        GameIsPaused = false;
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);
    }

    public void Quit()
    {
        Debug.Log("Quitting Game");
        button.Play();
        Application.Quit();
    }

    public void LoadMenu()
    {
        GameIsPaused = false;
        Time.timeScale = 1f;
        button.Play();
        SceneManager.LoadScene(0);
    }
}
