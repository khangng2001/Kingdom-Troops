using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonFire : MonoBehaviour
{
    [Header("Fire Spawn Point")]
    public List<Transform> UseSkillFirePoint = new List<Transform>();
    public int IndexPoint = 0;

    [Header("Fire Ball")]
    public int FireCount = 0;
    public int FireCountMax = 3;
    public GameObject FireContain;
    public Transform FireBallSpawnPoint;

    [Header("Fire Trace")]
    public GameObject FireTraceContain;

    [Header("Head")]
    public Transform Head;

    public void Fire(Transform target)
    {
        Vector3 directionToPlayer = (target.position + Vector3.up * 1f) - FireBallSpawnPoint.position;
        foreach (Transform child in FireContain.transform)
        {
            if (!child.gameObject.activeSelf)
            {
                GameObject fireBall = child.gameObject;
                fireBall.transform.position = FireBallSpawnPoint.position;
                if (Vector3.Dot(-directionToPlayer, Head.transform.right) > 0)
                {
                    fireBall.transform.rotation = Quaternion.LookRotation(directionToPlayer);
                }
                else
                {
                    fireBall.transform.forward = -FireBallSpawnPoint.right;
                }
                //fireBall.transform.rotation = Quaternion.LookRotation(directionToPlayer);

                fireBall.GetComponent<FireBallController>().DragonFire = this;
                fireBall.SetActive(true);
                break;
            }
        }
        
    }

    public void FireTrace(Vector3 pivot)
    {
        foreach (Transform child in FireTraceContain.transform)
        {
            if (!child.gameObject.activeSelf)
            {
                GameObject fireTrace = child.gameObject;
                fireTrace.transform.position = pivot;
                fireTrace.transform.rotation = Quaternion.identity;
                fireTrace.SetActive(true);
                StartCoroutine(DeactiveAfterTime(2, fireTrace));
                break;
            }
        }
    }

    IEnumerator DeactiveAfterTime(float time, GameObject fireTrace)
    {
        yield return new WaitForSeconds(time);
        fireTrace.SetActive(false);
    }
}
