using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using static GameController;

public class SettingsScript : MonoBehaviour
{
    [System.Serializable]
    public class PlayerData
    {
        public string UserName;
        public int HighScore;
        public int HighLevel;
    }
    PlayerData mPlayerData = new PlayerData();


    public TMP_Text PlayerNameText;
    public TMP_Text HighScoreText;

    public TMP_InputField InputFieldObj;

    public string NewNamePlayer;

    public void Awake()
    {
        InputFieldObj.ActivateInputField();

        LoadSaveDataPlayer();
        PlayerNameText.text = "PLAYER NAME : " + mPlayerData.UserName;
        HighScoreText.text = "HIGH SCORE   : " + mPlayerData.HighScore.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        InputFieldObj.ActivateInputField();
        InputFieldObj.text = "";
        InputFieldObj.caretBlinkRate = 1000;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ReadTextField(string s)
    {
        string input = s;
        Debug.Log(input);

        mPlayerData.UserName = input;
        mPlayerData.HighScore = 0;
        mPlayerData.HighLevel = 1;

        PlayerNameText.text = "PLAYER NAME : " + mPlayerData.UserName;
        HighScoreText.text = "HIGH SCORE  : " + mPlayerData.HighScore.ToString();

        SavePlayerData();

        InputFieldObj.ActivateInputField();
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
