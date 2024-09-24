using System;
using UnityEngine;
using UnityEngine.UIElements;

public class SC_LQ_Rune_Lifesteal : SC_LQ_SpellRune
{
    public GameObject gameTouched;

    public float pourcentageSteal;

    public override void Awake()
    {
        runeName = "Lifesteal";
        base.Awake();

        //OnTouchEnemy += Effect;
    }

    private void OnDestroy()
    {
        //OnTouchEnemy -= Effect;
    }

    public override void Effect()
    {
        base.Effect();

        //Heal PV d'un montant pourcentage du lifeSteal
        //SC_LC_PlayerGlobal.instance.GetComponent<player>
    }


    public override void TouchEnemy(GameObject enemy)
    {
        gameTouched = enemy;
        base.TouchEnemy(enemy);

        Effect();
    }
}
