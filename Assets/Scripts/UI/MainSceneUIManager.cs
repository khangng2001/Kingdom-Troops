using UnityEngine;
using UnityEngine.UI;

public class MainSceneUIManager : MonoBehaviour
{
    [SerializeField] private Button continueBtn;
    [SerializeField] private Button newGameBtn;
    [SerializeField] private Button exitBtn;

    private void Start()
    {
        continueBtn.onClick.AddListener(() =>
        {
            PlayContinue();
        }); 
        newGameBtn.onClick.AddListener(() =>
        {
            PlayNewGame();
        }); 
        exitBtn.onClick.AddListener(() =>
        {
            ExitGame();
        });

        if (GameManager.Instance.GameData == null) return;
        if (GameManager.Instance.GameData.AppOpenCounts <= 1)
        {
            continueBtn.gameObject.SetActive(false);
        }
    }


    private void PlayNewGame()
    {
        //Reset All Data
        GameManager.Instance.GameData.Reset();

        //Load PlayScene
        //SceneLoader.LoadScene(GameManager.Instance.GameData.CurLevelMap, "");
        StartCoroutine(GameSceneLoading.Instance.LoadChildGame(GameManager.Instance.GameData.CurLevelMap, () =>
        {
            //Play Music in GamePlay
            Debug.Log(">>Play GamePlay music");
        }));
    }

    private void PlayContinue()
    {
        //Load PlayScene
        //SceneLoader.LoadScene(GameManager.Instance.GameData.CurLevelMap, "");
        GameSceneLoading.Instance.LoadChildGame(GameManager.Instance.GameData.CurLevelMap, () =>
        {
            //Play Music in GamePlay
            Debug.Log(">>Play GamePlay music");
        });
    }

    private void ExitGame()
    {
        Debug.Log(">>> QUIT APP");
        Application.Quit();
    }
}
