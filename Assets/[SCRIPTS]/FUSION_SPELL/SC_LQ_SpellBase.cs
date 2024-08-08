using System;
using System.Collections.Generic;
using UnityEngine;

public class SC_LQ_SpellBase : MonoBehaviour
{
    SC_LC_PlayerGlobal player;

    public GameObject shape;

    public List<BranchEffect> Branch;


    public void LaunchSpell()
    {
        player = SC_LC_PlayerGlobal.instance;

        GameObject currentSpell = Instantiate(shape, transform.position, transform.rotation);

        //Add branch's Components
        foreach (BranchEffect effect in Branch)
        {
            //Trouver un moyen de se souvenir des effets avant et après ? 
            foreach (SC_LQ_SpellEffect effectSpell in effect.effects)
            {
                currentSpell.AddComponent(effectSpell.GetType());
            }
        }
    }

    private void Update()
    {
        if (player.inputs.castSpellPressed == true)
        {
            LaunchSpell();
        }
    }
}
[Serializable]
public class BranchEffect
{
    public List<SC_LQ_SpellEffect> effects;
}
