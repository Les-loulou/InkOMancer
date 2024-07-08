using UnityEngine;

public class SC_LQ_State_Inky_Move : SC_LQ_EnemyState
{
    public float maxOffset = 3;
    Vector3 offset;

    Vector3 dest;

    public override void OnEnterState()
    {
        offset = new Vector3(Random.Range(-maxOffset, maxOffset), 0, Random.Range(-maxOffset, maxOffset));
        dest = SC_LC_PlayerMovements.instance.transform.position + offset;
        agent.destination = dest;
    }

    public override void OnExitState()
    {

    }

    public override void UpdateState()
    {
        agent.destination = dest;


        dest = Vector3.Distance(SC_LC_PlayerMovements.instance.transform.position, transform.position) < 5f ? SC_LC_PlayerMovements.instance.transform.position : SC_LC_PlayerMovements.instance.transform.position + offset;



        //Have to do with player distance most than remaining offset's distance
        if (Vector3.Distance(SC_LC_PlayerMovements.instance.transform.position, transform.position) < 2f)
        {
            transform.LookAt(SC_LC_PlayerMovements.instance.transform.position);

            //Change State
            manager.SwitchState(GetComponent<SC_LQ_State_Inky_Attack>());
        }

    }
}
