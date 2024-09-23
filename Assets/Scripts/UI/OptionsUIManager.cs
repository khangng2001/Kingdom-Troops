using UnityEngine;
using UnityEngine.UI;

public class OptionsUIManager : MonoBehaviour
{
    [SerializeField] private Button continueBtn;
    [SerializeField] private Button optionBtn;
    [SerializeField] private Button controlBtn;
    [SerializeField] private Button exitBtn;

    private void Start()
    {
        continueBtn.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
        });
        optionBtn.onClick.AddListener(() =>
        {
            Debug.Log("Open Option UI");
        });
        controlBtn.onClick.AddListener(() =>
        {
            Debug.Log("Open Control UI");
        }); 
        exitBtn.onClick.AddListener(() =>
        {
            DataManager.Instance.SaveGame();

            // Back to MainScene
            StartCoroutine(GameSceneLoading.Instance.LoadChildGame(StringConstants.MAIN, () => {
            }));
        });
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameObject.activeInHierarchy)
                gameObject.SetActive(false);
            else 
                gameObject.SetActive(true);
        }
    }
}
