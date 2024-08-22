using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SC_LQ_SpellCast : MonoBehaviour
{
    SC_LC_PlayerGlobal player;

    public GameObject shape;

    public List<BranchEffect> Branch;

    public GameObject spellBranch;


    public void LaunchSpell()
    {
        GameObject currentSpell = Instantiate(shape, transform.position, transform.rotation);//Instantiate spell Shape

        
        foreach (BranchEffect branch in Branch)
        {
            
            GameObject Gobranch = Instantiate(spellBranch, currentSpell.transform);    //Create GameObject 
            SC_LQ_Spell_Branch myBranch = Gobranch.AddComponent<SC_LQ_Spell_Branch>(); //and add Branch Component

            for (int i = 0; i < branch.Runes.Count; i++)
            {
                //Instantiate empty GameObject and Add SpellEffect
                Component ActualRune = Gobranch.AddComponent(branch.Runes[i].GetType()); // Add component on Gameobject's Child
                myBranch.branch.SetRunesList(myBranch);

                //myBranch.branch.Runes.Add(ActualRune); //Add to the branch's list the new spell effect

                #region COMMENTARY
                //Don't work
                ////Set Previous and Next Spell Effect
                //
                //if (effect.effects.Count > i + 1)
                //{
                //    newEffect.GetType(SC_LQ_SpellEffect).nextEffect = newEffect.GetComponent<SC_LQ_SpellEffect>().nextEffect = effect.effects[i + 1];
                //}
                //if (i > 0)
                //{
                //    newEffect.GetComponent<SC_LQ_SpellEffect>().previousEffect = effect.effects[i - 1];
                //}
                //
                ////newSpellBranch.AddComponent(effectSpell.GetType());
                #endregion
            }

            myBranch.FinishAddRunes();
        }
    }

    private void Update()
    {
        //CTRL + A
        if (SC_LC_PlayerGlobal.instance.inputs.castSpellPressed == true)
        {
            LaunchSpell();
        }

    }
}

