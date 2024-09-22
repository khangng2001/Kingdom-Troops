using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneLoading : Singleton<GameSceneLoading>
{
    [SerializeField] private GameObject loadingCanvas;

    private GameData gameData;

    private string sceneLoaded = string.Empty;

    public void LoadingStartGame()
    {
        gameData = GameManager.Instance.GameData;
    }

    public void LoadRoom(string sceneName, Action<bool> loadDone)
    {
        StartCoroutine(LoadRoomScene(sceneName, loadDone));
    }

    private IEnumerator LoadRoomScene(string sceneName, Action<bool> loadFinished = null)
    {
        yield return new WaitForSeconds(0.5f);

        Debug.Log(">>> LOAD SCENE: " +  sceneName);

        StartCoroutine(LoadSceneCoroutine(sceneName, sceneName, () =>
        {
            LoadSceneFinishCallback(loadFinished);
        }));
    }

    private IEnumerator LoadSceneCoroutine(string path, string name, Action onComplete = null)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(path, LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        //Unload Scene we dont need
        if (!string.IsNullOrEmpty(sceneLoaded))
        {
            AsyncOperation asyncUnload = null;

            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (scene.name.Contains(sceneLoaded))
                {
                    asyncUnload = SceneManager.UnloadSceneAsync(scene);
                    break;
                }
            }

            while (asyncUnload != null && !asyncUnload.isDone)
            {
                yield return null;
            }
        }


        sceneLoaded = name;

        onComplete?.Invoke();
    }

    private void LoadSceneFinishCallback(Action<bool> loadFinish)
    {
        loadFinish?.Invoke(true);
    }

    public IEnumerator LoadChildGame(string sceneName, Action finish)
    {
        loadingCanvas.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        LoadRoom(sceneName, b =>
        {
            loadingCanvas.SetActive(false);
            finish?.Invoke();
        });
    }
}