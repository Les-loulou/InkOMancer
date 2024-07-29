using UnityEngine;

public class SC_LQ_EFFECT_GoRight : SC_LQ_SpellEffect
{

    public override void FixedUpdate()
    {
        transform.Translate(transform.rotation * Vector3.right * spell.speed * Time.deltaTime, Space.World);
    }
}
