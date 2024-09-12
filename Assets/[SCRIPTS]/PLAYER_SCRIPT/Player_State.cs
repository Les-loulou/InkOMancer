using UnityEngine;

public abstract class Player_State : MonoBehaviour
{
    [HideInInspector] protected SC_LQ_Player_StateManager manager;
    [HideInInspector] protected SC_LC_PlayerMovements movement;
    [HideInInspector] protected Transform mesh;

    private void Start()
    {
        movement = GetComponent<SC_LC_PlayerMovements>();
        mesh = transform.GetChild(0);
        manager = GetComponent<SC_LQ_Player_StateManager>();
    }

    public abstract void EnterState();

    public abstract void ExitState();

    public abstract void UpdateState();

}
