using System.Collections;
using UnityEngine;

public class SC_LQ_Storm : MonoBehaviour
{

    public float radius;
    public float damage;

    public float time;
   
    void Start()
    {

        StartCoroutine(StormEffect());
        StartCoroutine(WaitAndDestroy());
    }

    public IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(time); 

        Destroy(gameObject);
    }

    IEnumerator StormEffect()
    {
        yield return new WaitForSeconds(1);

        Collider[] touched = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider collider in touched)
        {
            SC_LC_EnemyHealth health = collider.GetComponent<SC_LC_EnemyHealth>();
            if (health != null)
            {
                health.Damage(-damage);
            }
        }

        StartCoroutine(StormEffect());
    }
}
