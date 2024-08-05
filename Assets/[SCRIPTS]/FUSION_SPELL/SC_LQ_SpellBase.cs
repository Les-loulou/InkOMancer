using System.Collections.Generic;
using UnityEngine;

public class SC_LQ_SpellBase : MonoBehaviour
{
    SC_LC_PlayerGlobal player;

    public GameObject shape;

    public List<SC_LQ_SpellEffect> effects;


    public void LaunchSpell()
    {
		player = SC_LC_PlayerGlobal.instance;

		GameObject currentSpell = Instantiate(shape, transform.position, transform.rotation);

        //Ajouter des Components en fonction de ma liste
        foreach (SC_LQ_SpellEffect effect in effects)
        {
            currentSpell.AddComponent(effect.GetType());
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
