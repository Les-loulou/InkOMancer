using System.Collections;
using UnityEngine;

public class SC_LQ_State_Inky_Attack : SC_LQ_EnemyState
{

    [HideInInspector] public GameObject currentdecals;
    [SerializeField] public GameObject inkExplosion;
    [SerializeField] float increaser;

    [SerializeField] public float timeWaitAfterExplode;

    public override void OnEnterState()
    {
        OnJump += FeedbackJump;

        agent.enabled = false;
        currentdecals = GetComponent<SC_LQ_State_Inky_Charge>().currentDecals;

        Jump();

        increaser = Vector3.Distance(transform.position, currentdecals.transform.position) * Time.deltaTime * 3;
    }

    public override void OnExitState()
    {
        OnJump -= FeedbackJump;
    }

    public override void UpdateState()
    {
        if (currentdecals != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(currentdecals.transform.position.x, transform.position.y, currentdecals.transform.position.z), increaser);
        }
    }

    public System.Action OnJump;
    public void Jump()
    {
        OnJump?.Invoke();
    }

    public void FeedbackJump()
    {
        animator.SetTrigger("Attack");

        //Instantiate dirt ground VFX to inky's jump ?
    }

    public void Explode()
    {
        Instantiate(inkExplosion, transform.position, transform.rotation);
        Destroy(currentdecals);

        StartCoroutine(WaitAfterExplosion());
    }

    public IEnumerator WaitAfterExplosion()
    {

        yield return new WaitForSeconds(timeWaitAfterExplode);

        manager.SwitchState(GetComponent<SC_LQ_State_Inky_Move>());
    }

}
