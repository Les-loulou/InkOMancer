using UnityEngine;

public class SC_LQ_Player_StateManager : MonoBehaviour
{

    [SerializeField] Player_State currentState;



    void Start()
    {
        currentState.EnterState();
    }

    void Update()
    {
        currentState.UpdateState();
    }

    public void SwitchState(Player_State newState)
    {
        currentState.ExitState();

        currentState = newState;

        currentState.EnterState();
    }
}
