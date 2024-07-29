using UnityEngine;
using UnityEngine.Animations;

public class SC_LQ_State_Inky_Move : SC_LQ_EnemyState
{
    public float maxOffset = 3;
    Vector3 offset;

    Vector3 dest;

    public override void OnEnterState()
    {
        agent.enabled = true;

        offset = new Vector3(Random.Range(-maxOffset, maxOffset), 0, Random.Range(-maxOffset, maxOffset));
        dest = SC_LC_PlayerMovements.instance.transform.position + offset;
        agent.destination = dest;

        animator.SetTrigger("Move");
    }

    public override void OnExitState()
    {

    }

    public override void UpdateState()
    {
        agent.destination = dest;

        //if close enough to player position + offset or if player is close : change my navmesh destination to player's position
        if (dest != SC_LC_PlayerMovements.instance.transform.position)
        {
            dest = Vector3.Distance(agent.destination, transform.position) < 1f || Vector3.Distance(SC_LC_PlayerMovements.instance.transform.position, transform.position) < 3f ? SC_LC_PlayerMovements.instance.transform.position : SC_LC_PlayerMovements.instance.transform.position + offset;
        }



        //if very close to player's position, change body rotation to look at player and switch to "Charge State"
        if (Vector3.Distance(SC_LC_PlayerMovements.instance.transform.position, transform.position) < 2f)
        {
            transform.LookAt(SC_LC_PlayerMovements.instance.transform.position);

            //Change State
            manager.SwitchState(GetComponent<SC_LQ_State_Inky_Charge>());
        }

    }
}
