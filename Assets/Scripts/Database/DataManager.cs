using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private GameData gameData;

    private static DataManager instance;

    public static DataManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("DataManager", typeof(DataManager));
                instance = go.GetComponent<DataManager>();

                if (Application.isPlaying)
                {
                    DontDestroyOnLoad(instance.gameObject);
                }

                instance.Init();
            }

            return instance;
        }
    }

    private GameData Init()
    {
        try
        {
            if (gameData == null)
            {
                if (File.Exists(StringConstants.GAME_DATA_FILENAME))
                    gameData = JsonDataHandler.ReadFromJson<GameData>(StringConstants.GAME_DATA_FILENAME);
                else
                    Debug.Log("GameData not found! Start a new game");
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }

        if (gameData == null)
            gameData = new GameData();

        return gameData;
    }

    public void SaveGame()
    {
        JsonDataHandler.SaveToJson(gameData, StringConstants.GAME_DATA_FILENAME);
        Debug.Log("SAVE GAME");
    }

    public GameData LoadGame() 
    { 
        if (gameData == null)
            Init();

        gameData?.Init();

        return gameData;
    }
}
