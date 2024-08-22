using UnityEngine;

public class SC_LQ_Rune_Storm : SC_LQ_SpellRune
{
    private void Awake()
    {
        base.Awake();

        currentInk = 10;
        costInk = 1;
        damage = 1;
    }
}
