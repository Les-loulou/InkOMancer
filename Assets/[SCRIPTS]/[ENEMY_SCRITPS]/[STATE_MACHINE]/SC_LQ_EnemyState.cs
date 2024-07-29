using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public abstract class SC_LQ_EnemyState : MonoBehaviour
{
    //Set Manager & Agent
    [HideInInspector] public SC_LQ_EnemyStateManager manager;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Animator animator;
    


    public virtual void Awake()
    {
        manager = GetComponent<SC_LQ_EnemyStateManager>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    public abstract void OnEnterState();
    public abstract void UpdateState();
    public abstract void OnExitState();

}
