using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class RangeActiveTimeline : MonoBehaviour
{
    public PlayableDirector timeline;
    public UnityEvent EventWhenEnd;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponent<Collider>().enabled = false;

            other.GetComponent<PlayerController>().CanMove(false);
            timeline.stopped += (a) =>
            {
                other.GetComponent<PlayerController>().CanMove(true);
                EventWhenEnd?.Invoke();
            };

            if (timeline != null)
            {
                timeline.Play();
            }


        }
    }

    public void LoadScene()
    {
        StartCoroutine(GameSceneLoading.Instance.LoadChildGame(StringConstants.MAIN, () => 
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }));
    }
}
