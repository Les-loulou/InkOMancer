using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SC_LQ_SpellCast : MonoBehaviour
{
    SC_LC_PlayerGlobal player;

    public GameObject spell;

    public List<SO_Rune> runesScriptable;
    public List<SO_Rune> runes;
    public SO_Rune[] runesActifs = new SO_Rune[2];


    private void Start()
    {

        foreach (SO_Rune soRune in runesScriptable)
        {
            runes.Add(soRune);
            soRune.ResetStats();
        }

        //Active Runes
        for (int i = 0; i < 2; i++)
        {
            runesActifs.SetValue(runes[0], i);
            runes.RemoveAt(0);
            runes.Add(runesActifs[i]);
        }

    }

    public void LaunchSpell()
    {
        GameObject currentSpell = Instantiate(spell, transform.position, transform.rotation);//Instantiate spell Shape

        foreach (SO_Rune rune in runesActifs)
        {
            //rune.AddRune(currentSpell);
            //print()
            Component compo = currentSpell.AddComponent(rune.runeScript.GetComponent<SC_LQ_SpellRune>().GetType());
            compo = rune.runeScript.GetComponent<SC_LQ_SpellRune>();
        }
    }

    public void TouchEnemy()
    {

    }

    public void ChangeRune(SO_Rune oldRune)
    {
        print("changerune");
        runesActifs.SetValue(runes[0], ArrayUtility.IndexOf(runesActifs, oldRune));
        runes.RemoveAt(0);
        runes.Add(oldRune);
        oldRune.ResetStats();

    }

    private void Update()
    {
        //To replace by Click on Enemy
        //CTRL + A
        if (SC_LC_PlayerGlobal.instance.inputs.castSpellPressed == true)
        {
            LaunchSpell();
        }

        foreach (SO_Rune rune in runesActifs)
        {
            if (rune.remainingInk <= 0)
            {
                ChangeRune(rune);
            }
            break;
        }

    }
}

