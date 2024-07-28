using System.Collections;
using UnityEngine;

public class SC_LQ_State_Inky_Charge : SC_LQ_EnemyState
{

    [SerializeField] float WaitBeforeAttack;
    [SerializeField] GameObject decalsExplosion;

    [HideInInspector] public GameObject currentDecals;
    Vector3 posExplode;

    public override void OnEnterState()
    {
        OnCharge += StartCharge;

        //Stop the movement
        agent.enabled = false;
        StartCoroutine(Charge());

    }

    public override void OnExitState()
    {
        OnCharge -= StartCharge;
    }

    public override void UpdateState()
    {

    }

    public void StartCharge()
    {
        posExplode = SC_LC_PlayerMovements.instance.transform.position + Vector3.down * 0.5f;
        currentDecals = Instantiate(decalsExplosion, posExplode, Quaternion.Euler(90, 0, 0));

        animator.SetTrigger("Charge");
    }

    public System.Action OnCharge;
    public IEnumerator Charge()
    {
        OnCharge?.Invoke();

        yield return new WaitForSeconds(WaitBeforeAttack);
    }

    public void EndCharge()
    {
        //Switch to attack State
        manager.SwitchState(GetComponent<SC_LQ_State_Inky_Attack>());
    }
}
