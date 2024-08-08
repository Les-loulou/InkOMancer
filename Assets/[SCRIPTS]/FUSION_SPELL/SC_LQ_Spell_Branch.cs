using System;
using System.Collections.Generic;
using UnityEngine;

public class SC_LQ_Spell_Branch : MonoBehaviour
{
    public BranchEffect branch = new BranchEffect();


}

[Serializable]
public class BranchEffect
{
    public List<SC_LQ_SpellRune> Runes = new List<SC_LQ_SpellRune>();

    //public void SetRunesList(MonoBehaviour mono)
    //{
    //    Runes.Clear();
    //
    //   SC_LQ_SpellRune[] tempoRune  = mono.GetComponents<SC_LQ_SpellRune>();
    //
    //    foreach (SC_LQ_SpellRune rune in tempoRune)
    //    {
    //        Runes.Add(rune);
    //    }
    //}
}
