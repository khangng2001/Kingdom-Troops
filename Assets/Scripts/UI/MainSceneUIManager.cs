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
        if (GameManager.Instance.GameData.NotPlayedEver)
        {
            continueBtn.gameObject.SetActive(false);
        }
    }

    private void PlayNewGame()
    {
        //Reset All Data
        GameManager.Instance.GameData.Reset();

        GameManager.Instance.GameData.UpdateNotPlayedEver(false);

        //Load PlayScene
        //SceneLoader.LoadScene(GameManager.Instance.GameData.CurLevelMap, "");
        StartCoroutine(GameSceneLoading.Instance.LoadChildGame(GameManager.Instance.GameData.CurLevelMap, () =>
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }));
    }

    private void PlayContinue()
    {
        //Load PlayScene
        //SceneLoader.LoadScene(GameManager.Instance.GameData.CurLevelMap, "");
        StartCoroutine(GameSceneLoading.Instance.LoadChildGame(GameManager.Instance.GameData.CurLevelMap, () =>
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }));
    }

    private void ExitGame()
    {
        Debug.Log(">>> QUIT APP");
        Application.Quit();
    }
}