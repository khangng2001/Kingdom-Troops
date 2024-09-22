using System;
using System.Collections;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameData GameData { get; set; }

    private void Start()
    {
        //LoadGameData();
        StartCoroutine(LoadingGame());
    }

    private IEnumerator LoadingGame()
    {
        //Loading Data
        bool loadingFinish = false;
        StartCoroutine(LoadingGameConfigs(b => { loadingFinish = true; }));

        yield return new WaitUntil(() => loadingFinish);

        //Load StartGame function
        GameSceneLoading.Instance.LoadingStartGame();
        yield return null;

        /*//WE START FROM THE MAIN SCENE -> Dont need to Load this Scene again*/
        //Loading MainScene
        //SceneLoader.LoadScene(StringConstants.MAIN, "");
        bool loadGameRoomFinish = false;
        GameSceneLoading.Instance.LoadRoom(StringConstants.MAIN, b => { loadGameRoomFinish = true; });
        yield return new WaitUntil(() => loadGameRoomFinish);
    }

    private IEnumerator LoadingGameConfigs(Action<bool> loadDone)
    {
        LoadGameData();

        yield return null;

        loadDone.Invoke(true);
    }

    private void LoadGameData()
    {
        GameData = DataManager.Instance.LoadGame();
    }
}
