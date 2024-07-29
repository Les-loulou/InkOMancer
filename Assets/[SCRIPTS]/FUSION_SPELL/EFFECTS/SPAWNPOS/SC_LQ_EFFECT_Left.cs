using UnityEngine;

public class SC_LQ_EFFECT_Left : SC_LQ_SpellEffect
{
    public override void Effect()
    {
        
    }

    public override void Awake()
    {
        transform.position += transform.right * -1;
    }
}
