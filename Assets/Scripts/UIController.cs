using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoSingleton<UIController>
{
    public TMP_Text TimerText;
    public TMP_Text ScoreText;
    public List<GameObject> LifeIcons = new List<GameObject>();

    public GameObject PauseGamePanel;
    public GameObject GameOverPanel;
    public GameObject LevelUP;

    private void Awake()
    {
    }

    private void Start()
    {

    }

    public void UpdateScoreText(int _value)
    {
        ScoreText.text = "SCORE : " + _value.ToString();
    }

    public void UpdateLives(int lives)
    {
        // Assicurati che LifeIcons sia stato inizializzato e che abbia abbastanza elementi per rappresentare tutte le vite
        if (LifeIcons != null && LifeIcons.Count >= lives)
        {
            // Disattiva le icone delle vite extra (se ce ne sono)
            for (int i = 0; i < LifeIcons.Count; i++)
            {
                if (i >= lives)
                {
                    LifeIcons[i].SetActive(false);
                }
                else
                {
                    LifeIcons[i].SetActive(true);
                }
            }
        }
        else
        {
            Debug.LogWarning("LifeIcons non inizializzato o non contiene abbastanza elementi.");
        }
    }
    public void ShowGameOver()
    {
        GameOverPanel.SetActive(true);
    }

    public void HideGameOver()
    {
        GameOverPanel.SetActive(false);
    }

    public void ShowPausePanel()
    {
        PauseGamePanel.SetActive(true);
    }

    public void HidePausePanel()
    {
        PauseGamePanel.SetActive(false);
    }
    public void OpenSettingsPanel()
    {

    }

    public void ShowWinLevel()
    {
        LevelUP.SetActive(true);
    }
    public void HideWinLevel()
    {
        LevelUP.SetActive(false);
    }

}
