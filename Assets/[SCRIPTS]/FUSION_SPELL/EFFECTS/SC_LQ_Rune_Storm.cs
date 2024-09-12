using UnityEngine;

public class SC_LQ_Rune_Storm : SC_LQ_SpellRune
{
    public override void Awake()
    {
        runeName = "Storm";
        base.Awake();
    }

    public override void Effect()
    {
        base.Effect();

       GameObject myPrefab = Resources.Load<GameObject>("StormGO");

        // Step 3: Check if the prefab was loaded successfully
        if (myPrefab != null)
        {
            // Step 4: Instantiate the prefab
            Instantiate(myPrefab, transform.position, transform.rotation);
        }
    }

    public override void TouchEnemy(GameObject enemy)
    {
        base.TouchEnemy(enemy);
        Effect();
    }
}
