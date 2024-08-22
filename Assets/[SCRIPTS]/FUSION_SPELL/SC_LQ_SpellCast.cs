using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;

public class SC_LQ_SpellCast : MonoBehaviour
{
    SC_LC_PlayerGlobal player;

    public GameObject spell;

    public List<SO_Rune> runesScriptable ;
    public List<SC_LQ_SpellRune> runes;
    public SC_LQ_SpellRune[] runesActifs = new SC_LQ_SpellRune[2];


    private void Start()
    {
        
        foreach (SO_Rune soRune in runesScriptable)
        {
            runes.Add(soRune.runeScript.GetComponent<SC_LQ_SpellRune>());
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

        foreach (SC_LQ_SpellRune rune in runesActifs)
        {
            currentSpell.AddComponent(rune.GetType());
        }

    }

    public void TouchEnemy()
    {

    }

    public void ChangeRune(SC_LQ_SpellRune oldRune)
    {
        //runesActifs.SetValue(runes[0], ArrayUtility.IndexOf(runesActifs, oldRune));
        //runes.RemoveAt(0);
        //runes.Add(oldRune);

    }

    private void Update()
    {
        //To replace by Click on Enemy
        //CTRL + A
        if (SC_LC_PlayerGlobal.instance.inputs.castSpellPressed == true)
        {
            LaunchSpell();
        }

        foreach (SC_LQ_SpellRune rune in runesActifs)
        {
            //if(rune.currentInk <=0)
            //{
            //    ChangeRune(rune);
            //}
            //break;
        }

    }
}

