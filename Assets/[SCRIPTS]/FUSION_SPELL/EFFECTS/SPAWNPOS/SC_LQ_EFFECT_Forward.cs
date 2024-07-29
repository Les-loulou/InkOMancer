using UnityEngine;

public class SC_LQ_EFFECT_Forward : SC_LQ_SpellEffect
{
    public override void Effect()
    {
        
    }

    public override void Awake()
    {
        transform.position += transform.forward;
    }
}
