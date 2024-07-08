using System.Collections;
using UnityEngine;

public class SC_LQ_State_Inky_Attack : SC_LQ_EnemyState
{

    [SerializeField] float WaitBeforeAttack;
    [SerializeField] GameObject inkyAttack;

    [SerializeField] GameObject decalsExplosion;

    public override void OnEnterState()
    {
        //Stop the movement
        agent.enabled = false;
        StartCoroutine(WaitAndInvoke());
    }

    public override void OnExitState()
    {

    }

    public override void UpdateState()
    {

    }

    public IEnumerator WaitAndInvoke()
    {
        //Instantiate decals
        GameObject decals = Instantiate(decalsExplosion, (transform.position + Vector3.down *1)+ (transform.rotation * Vector3.forward * 1.5f), Quaternion.Euler(90,0,0));

        yield return new WaitForSeconds(WaitBeforeAttack);

        Instantiate(inkyAttack, transform.position, transform.rotation);
        Destroy(gameObject);
    }

}
