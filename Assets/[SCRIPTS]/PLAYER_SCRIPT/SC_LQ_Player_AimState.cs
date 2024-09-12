using System.Collections;
using UnityEngine;

public class SC_LQ_Player_AimState : Player_State
{

    public Transform target;
    [SerializeField] Transform visualTarget;

    public float delaySwitchTarget = 0.2f;
    bool canChangeFocus = true;
    public void SearchEnemy()
    {
        //Found an target
        if (target == null)
        {
            Collider[] enemy = Physics.OverlapSphere(transform.position, SC_LC_PlayerGlobal.instance.playerStats.radiusAttack);
            float nearestDistance = Mathf.Infinity;

            if (enemy.Length > 0)
            {
                foreach (Collider collider in enemy)
                {
                    if (collider.gameObject.GetComponent<SC_LQ_EnemyGlobal>())
                    {

                        float distance = Vector3.Distance(transform.position, collider.gameObject.transform.position);

                        if (distance < nearestDistance)
                        {
                            target = collider.transform;
                            nearestDistance = distance;
                        }
                    }
                }
            }
        }
    }

    public System.Action OnEnterState;
    public override void EnterState()
    {
        SC_LQ_ProcessVolumeChange.instance.SetVignette(0.35f, 0.5f);
        transform.GetChild(2).gameObject.SetActive(true);

        SearchEnemy();

        OnEnterState?.Invoke();
    }

    public System.Action OnExitState;
    public override void ExitState()
    {
        SC_LQ_ProcessVolumeChange.instance.SetVignette(.2f, 1);
        target = null;

        OnExitState?.Invoke();
    }

    public void ChangeTarget(Vector3 direction)
    {

        //Found an target
        if (target == null)
            return;

        StartCoroutine(WaitCdTarget());
        Transform oldTarget = target;

        Collider[] enemy = Physics.OverlapSphere(oldTarget.position, SC_LC_PlayerGlobal.instance.playerStats.radiusAttack);
        float nearestDistance = Mathf.Infinity;

        if (enemy.Length > 0)
        {
            foreach (Collider collider in enemy)
            {
                if (collider.gameObject.GetComponent<SC_LQ_EnemyGlobal>() && collider.gameObject != oldTarget.gameObject)
                {

                    bool isAllow = true;

                    if (collider.transform.position.x < oldTarget.transform.position.x && direction.x > 0)
                    {
                        isAllow = false;
                    }

                    if (collider.transform.position.x > oldTarget.transform.position.x && direction.x < 0)
                    {
                        isAllow = false;
                    }

                    if (collider.transform.position.z < oldTarget.transform.position.z && direction.y > 0)
                    {
                        isAllow = false;
                    }

                    if (collider.transform.position.z > oldTarget.transform.position.z && direction.y < 0)
                    {
                        isAllow = false;
                    }



                    if (isAllow == true)
                    {

                        Vector3 dir = direction;

                        Vector3 test1 = new Vector3(oldTarget.position.x * dir.x, 0, oldTarget.transform.position.z * dir.y);
                        Vector3 test2 = new Vector3(collider.transform.position.x * dir.x, 0, collider.transform.position.z * dir.y);

                        float distance = Vector3.Distance(oldTarget.transform.position, collider.transform.position);

                        if (distance < nearestDistance)
                        {
                            target = collider.transform;
                            nearestDistance = distance;
                        }
                    }
                }
            }
        }
    }

    public IEnumerator WaitCdTarget()
    {
        canChangeFocus = false;
        yield return new WaitForSeconds(delaySwitchTarget);
        canChangeFocus = true;
    }

    public override void UpdateState()
    {

        //Check exit state ET CHANGER CE PUTAIN DE BOUTON DE MERDE
        if (!SC_LC_PlayerGlobal.instance.inputs.focusPressed)
        {
            manager.SwitchState(GetComponent<SC_LQ_Player_State_Idle>());
        }

        movement.PlayerSpeed(0.6f);

        if (target == null)
        {
            SearchEnemy();
            return;
        }

        if (Vector3.Distance(transform.position, target.transform.position) > SC_LC_PlayerGlobal.instance.playerStats.radiusAttack || target.GetComponent<SC_LC_EnemyHealth>().health <= 0)
        {
            target = null;
            SearchEnemy();
            return;
        }

        transform.LookAt(target.position);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

        if (SC_LC_PlayerGlobal.instance.inputs.attackPressed == true)
        {
            GameObject spell = GetComponent<SC_LQ_SpellCast>().LaunchSpell();
            spell.GetComponent<SC_LQ_SpellGlobal>().SetTarget(target);
        }

        if (SC_LC_PlayerGlobal.instance.inputs.changeFocusPressed == true && canChangeFocus == true)
        {
            ChangeTarget(SC_LC_PlayerGlobal.instance.inputs.focusDirection);
        }

    }
}
