using UnityEngine;

public class SC_LQ_EnemyStateManager : MonoBehaviour
{

    public SC_LQ_EnemyState currentState;

    void Start()
    {
        currentState.OnEnterState();
    }

    void Update()
    {
        currentState.UpdateState();
    }

    public void SwitchState(SC_LQ_EnemyState newState)
    {
        currentState.OnExitState();

        currentState = newState;

        currentState.OnEnterState();
    }
}
