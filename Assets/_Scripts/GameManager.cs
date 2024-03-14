using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject player;
    public GameObject pauseMenu;
    public GameObject replayMenu;
    public TextMeshProUGUI replayWaveNumber;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        ResumeGame();
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("EnergyBall"), LayerMask.NameToLayer("Enemy"));
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void StopGame()
    {
        ToggleCursor(true);
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        ToggleCursor(false);
        Time.timeScale = 1;
    }
    public void Pause()
    {
        StopGame();
        pauseMenu.SetActive(true);
    }
    public void UnPause()
    {
        ResumeGame();
        pauseMenu.SetActive(false);
    }
    public void GameOver()
    {

        StopGame();
        replayMenu.SetActive(true);
    }
    public void Replay()
    {
        replayWaveNumber.text = WaveManager.instance.GetCurrentWaveNumber().ToString();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void ToggleCursor(bool isVisible)
    {
        Cursor.visible = isVisible;
        Cursor.lockState = isVisible ? CursorLockMode.None : CursorLockMode.Locked;
    }
    public void TogglePause()
    {
        if (Time.timeScale == 0)
        {
            UnPause();
        }
        else
        {
            Pause();
        }
    }

}
