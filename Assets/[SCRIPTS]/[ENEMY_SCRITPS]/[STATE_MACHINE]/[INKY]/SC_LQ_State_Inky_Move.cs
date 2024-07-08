using UnityEngine;

public class SC_LQ_State_Inky_Move : SC_LQ_EnemyState
{
    public float maxOffset = 3;
    Vector3 offset;

    public override void OnEnterState()
    {
        offset = new Vector3(Random.Range(-maxOffset, maxOffset), 0, Random.Range(-maxOffset, maxOffset));
    }

    public override void OnExitState()
    {

    }

    public override void UpdateState()
    {
        agent.destination = SC_LC_PlayerMovements.instance.transform.position + offset;

        //Have to do with player distance most than remaining offset's distance
        if(agent.remainingDistance < 0.1f) //Vector3.Distance(SC_LC_PlayerMovements.instance.transform.position, transform.position < 1f)
        {
            print("Change d'état");
            //Change State
        }

    }
}
