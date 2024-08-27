using System.Collections;
using UnityEngine;

public class SC_LQ_Rune_TargetMore : SC_LQ_SpellRune
{
    public int numberTarget = 5;


    public override void Awake()
    {
        runeName = "TargetMore";
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
        //Duplicate myself, make sur their targets are different from mine and remove this
        for (int i = 0; i < numberTarget; i++)
        {
            GameObject newGo = Instantiate(gameObject, SC_LC_PlayerGlobal.instance.transform.position, SC_LC_PlayerGlobal.instance.transform.rotation);
            Destroy(newGo.GetComponent<SC_LQ_Rune_TargetMore>());

            Collider[] enemy = Physics.OverlapSphere(transform.position, 100f);

            int test = i + 2;
            foreach (Collider collider in enemy)
            {
                if (collider.gameObject.GetComponent<SC_LQ_EnemyGlobal>())
                {
                    test--;

                    if (test == 0)
                    {
                        newGo.GetComponent<SC_LQ_SpellGlobal>().SetTarget(collider.transform);
                        break;
                    }
                }
            }

            yield return new WaitForSeconds(0.025f);
        }
    }
}


