using UnityEngine;

public class SC_LQ_Player_State_Idle : Player_State
{
    public override void EnterState()
    {
        
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        movement.PlayerSpeed(1);

        mesh.localEulerAngles = Vector3.zero;

        //if (SC_LC_PlayerGlobal.instance.inputs.castSpellPressed == true)
        if(SC_LC_PlayerGlobal.instance.inputs.focusPressed)
        {
            manager.SwitchState(GetComponent<SC_LQ_Player_AimStateSecond>());
        }

    }
}
