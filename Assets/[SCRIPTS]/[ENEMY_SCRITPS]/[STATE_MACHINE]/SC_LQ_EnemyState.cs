using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public abstract class SC_LQ_EnemyState : MonoBehaviour
{
    //Set Manager & Agent
    [HideInInspector] public SC_LQ_EnemyStateManager manager;
    [HideInInspector] public NavMeshAgent agent;

    public virtual void Start()
    {
        manager = GetComponent<SC_LQ_EnemyStateManager>();
        agent = GetComponent<NavMeshAgent>();
    }

    public abstract void OnEnterState();
    public abstract void UpdateState();
    public abstract void OnExitState();

}
