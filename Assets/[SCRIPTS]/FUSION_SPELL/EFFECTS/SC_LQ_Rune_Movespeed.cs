using Unity.VisualScripting;
using UnityEngine;

public class SC_LQ_Rune_Movespeed : SC_LQ_SpellRune
{

    public float boostpourcen;
    public float boostTime;

    public override void Awake()
    {
        runeName = "Movespeed";
        base.Awake();

        boostpourcen = 20;
        boostTime = 1f;
    }

    public override void Effect()
    {
        base.Effect();

        SC_LQ_BoostMovementSpeed boost = SC_LC_PlayerGlobal.instance.AddComponent<SC_LQ_BoostMovementSpeed>();
        boost.boostpourcen = boostpourcen;
        boost.boostTime = boostTime;


        //Add component qui augmente la vitesse du joueur ???
    }

    public override void TouchEnemy(GameObject enemy)
    {
        base.TouchEnemy(enemy);

        Effect();
        
    }
}
