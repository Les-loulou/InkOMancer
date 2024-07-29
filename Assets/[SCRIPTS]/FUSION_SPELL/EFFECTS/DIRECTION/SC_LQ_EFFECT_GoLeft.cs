using UnityEngine;

public class SC_LQ_EFFECT_GoLeft : SC_LQ_SpellEffect
{

    public override void FixedUpdate()
    {
        transform.Translate(transform.rotation * Vector3.left * spell.speed * Time.deltaTime, Space.World);
    }
}
