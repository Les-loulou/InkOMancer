using UnityEngine;

public class SC_LQ_EFFECT_GoBack : SC_LQ_SpellEffect
{
    public override void FixedUpdate()
    {
        transform.Translate(transform.rotation * Vector3.back * spell.speed * Time.deltaTime, Space.World);
    }
}
