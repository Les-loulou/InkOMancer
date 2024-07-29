using UnityEngine;

public class SC_LQ_EFFECT_GoForward : SC_LQ_SpellEffect
{

    public override void FixedUpdate()
    {
        transform.Translate(transform.rotation * Vector3.forward * spell.speed * Time.deltaTime, Space.World);
    }
}
