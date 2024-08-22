using System.Collections;
using UnityEngine;

public class SC_LQ_EFFECT_Duplicata : SC_LQ_SpellRune
{
    //If is the first component to be had, wait until all component has been place a instantiate same gameObject

    //If not, remove all follow component and instantiate same shape and add ComponentS

    //public override void Start()
    //{
    //    base.Start();
    //
    //    if (indexEffect == 0)
    //    {
    //        Effect();
    //    }
    //}
    //
    //public override void Effect()
    //{
    //    base.Effect();
    //    StartCoroutine(WaitDuplicata());
    //}
    //
    //public IEnumerator WaitDuplicata()
    //{
    //    yield return new WaitForSeconds(0.01f);
    //    Destroy(this);
    //    Instantiate(gameObject, transform.position, Quaternion.identity);
    //}

}
