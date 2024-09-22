using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    private static string targetScene;
    private static string music;

    public static void LoadScene(string targetScene, string music)
    {
        SceneLoader.targetScene = targetScene;
        SceneLoader.music = music;

        SceneManager.LoadScene(StringConstants.LOADING);
    }

    public static void SceneLoaderCallback()
    {
        Debug.Log(">>> LOAD SCENE: " + targetScene);

        SceneManager.LoadScene(targetScene);

        //Play Music
        Debug.Log(">> Play Music: ");
    }
}