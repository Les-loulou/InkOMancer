using UnityEngine;
using UnityEngine.Animations;

public class SC_LQ_State_Inky_Move : SC_LQ_EnemyState
{
    SC_LC_PlayerGlobal player;

    [SerializeField] float maxOffset = 3;
    [SerializeField] float detectPlayer = 3.5f;
    Vector3 offset;

    Vector3 dest;

    public override void OnEnterState()
    {
        player = SC_LC_PlayerGlobal.instance;

		agent.enabled = true;

        offset = new Vector3(Random.Range(-maxOffset, maxOffset), 0, Random.Range(-maxOffset, maxOffset));
        dest = player.transform.position + offset;
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
        if (dest != player.transform.position)
        {
            dest = Vector3.Distance(agent.destination, transform.position) < 1f || Vector3.Distance(player.transform.position, transform.position) < detectPlayer +1 ? player.transform.position : player.transform.position + offset;
        }



        //if very close to player's position, change body rotation to look at player and switch to "Charge State"
        if (Vector3.Distance(player.transform.position, transform.position) < detectPlayer)
        {
            transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));

            //Change State
            manager.SwitchState(GetComponent<SC_LQ_State_Inky_Charge>());
        }

    }
}
