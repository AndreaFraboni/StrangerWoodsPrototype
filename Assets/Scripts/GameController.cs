using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem.XR;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    private static GameController _instance;

    [System.Serializable]
    public class PlayerData
    {
        public string UserName;
        public int HighScore;
        public int BestTime;
    }
    PlayerData mPlayerData = new PlayerData();

    private bool isPlaying = true;
    public bool IsPlaying { get { return isPlaying; } }
    private bool isPaused = false;
    public bool IsPaused { get { return isPaused; } }

    public int CurrentScore = 0;
    public int Lives = 5;

    private float CountDown = 300f;// Durata del timer in secondi = 5 minuti

    private float tempoTrascorso;

    public UIController UIController;

    // Proprietà per accedere all'istanza singleton del GameController
    public static GameController Instance
    {
        get
        {
            // Se non esiste già un'istanza, cerca nel gioco
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameController>();

                // Se non è stato trovato, crea un nuovo oggetto GameController
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("GameController");
                    _instance = singletonObject.AddComponent<GameController>();
                    singletonObject.tag = "GameController"; // Assegna il tag "GameController"
                }
            }

            return _instance;
        }
    }

    // Metodo chiamato quando il giocatore entra in contatto con un nemico
    public void PlayerCollidedWithEnemy()
    {
        Debug.Log("Il giocatore ha colpito un nemico!");
        // Puoi aggiungere qui la logica per gestire l'evento quando il giocatore colpisce un nemico
    }

    // Assicura che l'istanza singleton non venga distrutta durante il cambio di scene
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Se esiste già un'istanza singleton, distruggi questo oggetto
            Destroy(gameObject);
        }

        tempoTrascorso = CountDown;
        LoadSaveDataPlayer(); // Carica o Salva/Crea DATA Player
        isPlaying = true;
        isPaused = false;
        Time.timeScale = 1;


    }

    private void Start()
    {
    }

    private void Update()
    {

    }
    //*******************************************************************************************************//
    //************************* FIXED UPDATE ****************************************************************//
    //*******************************************************************************************************//
    private void FixedUpdate()
    {
        tempoTrascorso -= Time.deltaTime;
        Timer();

        if (tempoTrascorso <= 0)
        {
            // Metti il gameover
            tempoTrascorso = 0;
            Invoke("GameOver", 1);
        }

    }

    //*******************************************************************************************************//
    //************************* TIMER ***********************************************************************//
    //*******************************************************************************************************//
    private void Timer()
    {
        int secondiTrascorsi = Mathf.FloorToInt(tempoTrascorso);
        UIController.TimerText.text = "Countdown : " + secondiTrascorsi + "s";
    }

    //*******************************************************************************************************//
    //************************* PLAYER ADD SCORE ************************************************************//
    //*******************************************************************************************************//
    public void AddScore(int _value)
    {
        CurrentScore += _value;
        if (CurrentScore > mPlayerData.HighScore) { mPlayerData.HighScore = CurrentScore; }
        UIController.UpdateScoreText(CurrentScore);
    }

    //*******************************************************************************************************//
    //************************* PLAYER LIVE LOST ************************************************************//
    //*******************************************************************************************************//
    public void LiveLost()
    {
        // lose life
        Lives = Lives - 1;
        UIController.UpdateLives(Lives);

        if (Lives < 0)
        {
            Invoke("GameOver", 1);
        }
    }

    //*******************************************************************************************************//
    //************************* LEVEL FINISHED **************************************************************//
    //*******************************************************************************************************//
    public void HideLevelUp()
    {
        UIController.HideWinLevel();
        Destroy(gameObject);
        SceneManager.LoadScene("MainMenu");
    }
    public void LevelFinished()
    {
        Debug.Log("LEVEL FINISHED !!!");

        YouWinLevel();
    }
    public void YouWinLevel()
    {
        UIController.ShowWinLevel();
        Invoke("HideLevelUp", 4);
    }

    //*******************************************************************************************************//
    //************************* GAMEOVER ********************************************************************//
    //*******************************************************************************************************//
    public void GameOverGameExitButton()
    {
        UIController.HideGameOver();
        Destroy(gameObject);
        SavePlayerData();
        SceneManager.LoadScene("MainMenu");
    }

    public void ReStartGameOverBUtton()
    {
        UIController.HideGameOver();
        Destroy(gameObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GameOver()
    {
        isPlaying = false;
        isPaused = true;
        Time.timeScale = 0;
        if (CurrentScore > mPlayerData.HighScore) { mPlayerData.HighScore = CurrentScore; }
        UIController.ShowGameOver();
    }

    //*******************************************************************************************************//
    //************************* PAUSEGAME *******************************************************************//
    //*******************************************************************************************************//
    public void OnPause(InputAction.CallbackContext context)
    {
        isPlaying = false;
        isPaused = true;
        Time.timeScale = 0;
        UIController.ShowPausePanel();
    }

    public void UnPauseGame()
    {
        isPaused = false;
        Time.timeScale = 1;
        isPlaying = true;
        UIController.HidePausePanel();
    }

    public void PauseGameExitButton()
    {
        UIController.HidePausePanel();
        isPlaying = false;
        isPaused = true;
        Destroy(gameObject);
        SavePlayerData();
        SceneManager.LoadScene("MainMenu");
    }

    //**************************************************************************************************//
    //******************************** LOAD_SAVE_PLAYER_DATA *******************************************//
    //**************************************************************************************************//
    public void LoadSaveDataPlayer()
    {
        string saveFile = Application.persistentDataPath + "/gamedata.json";

        if (File.Exists(saveFile))
        {
            //Debug.Log("FILE EXISTS !!!");
            string loadPlayerData = File.ReadAllText(saveFile);
            mPlayerData = JsonUtility.FromJson<PlayerData>(loadPlayerData);
        }
        else
        {
            mPlayerData.UserName = "Player0";
            mPlayerData.HighScore = 0;
            //Debug.Log("FILE DOES NOT EXISTS !!!");
            string json = JsonUtility.ToJson(mPlayerData);
            File.WriteAllText(saveFile, json);
        }

    }

    public void SavePlayerData()
    {
        string saveFile = Application.persistentDataPath + "/gamedata.json";
        string json = JsonUtility.ToJson(mPlayerData);
        File.WriteAllText(saveFile, json);
    }

}



