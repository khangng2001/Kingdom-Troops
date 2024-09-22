using System.Collections;
using UnityEngine;

public class FireTraceController : MonoBehaviour
{
    public bool CanDamage = true;
    public int Damage = 10;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (CanDamage)
            {
                // Damage
                other.GetComponent<PlayerStatSystem>().TakeDamage(Damage);
                StartCoroutine(CanDamageAfter(0.5f));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        CanDamage = true;
    }

    IEnumerator CanDamageAfter(float time)
    {
        CanDamage = false;
        yield return new WaitForSeconds(time);
        CanDamage = true;
    }
}
