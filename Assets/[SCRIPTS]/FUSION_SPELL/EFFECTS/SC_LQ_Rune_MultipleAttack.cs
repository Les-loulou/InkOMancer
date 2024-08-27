using System.Collections;
using UnityEngine;

public class SC_LQ_Rune_MultipleAttack : SC_LQ_SpellRune
{

     int numberAttack = 3;

    public override void Awake()
    {
        runeName = "caca";
        base.Awake();
    }

    public override void Start()
    {
        base.Start();
        Effect();
    }

    public override void Effect()
    {
        base.Effect();

        StartCoroutine(EffectAndWait());
    }

    public IEnumerator EffectAndWait()
    {

        for (int i = 1; i < numberAttack; i++)
        {
            GameObject newGo = Instantiate(gameObject, SC_LC_PlayerGlobal.instance.transform.position, SC_LC_PlayerGlobal.instance.transform.rotation);
            Destroy(newGo.GetComponent<SC_LQ_Rune_MultipleAttack>());

            newGo.GetComponent<SC_LQ_SpellGlobal>().SetTarget(spell.target);
            yield return new WaitForSeconds(0.2f / numberAttack);
        }
    }


}

