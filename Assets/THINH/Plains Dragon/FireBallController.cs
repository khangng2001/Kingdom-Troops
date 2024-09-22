using UnityEngine;

public class FireBallController : MonoBehaviour
{
    public DragonFire DragonFire;

    public float Speed;
    public float Damage;

    public float TimeBeforeDeactive;
    public float TimeRunning;
    private void OnEnable()
    {
        TimeRunning = TimeBeforeDeactive;
    }

    private void Update()
    {
        if (TimeRunning <= 0)
        {
            transform.gameObject.SetActive(false);
            return;
        }

        TimeRunning -= Time.deltaTime;

        transform.position += transform.forward * Speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Give Damage To Player
        }
        else
        {
            DragonFire.FireTrace(other.ClosestPoint(this.transform.position));
        }

        transform.gameObject.SetActive(false);
    }
    
}
