using UnityEngine;


public class SC_LQ_SpellRune : MonoBehaviour
{

    //public int indexEffect;
    [HideInInspector] public SC_LQ_SpellGlobal spell;
    [HideInInspector] public SC_LQ_Spell_Branch branch;
    public bool isActif = false;


    public virtual void SetRuneState(bool state)
    {
        isActif = state;
    }

    public SC_LQ_SpellRune GetNextRune()
    {
        int index = branch.branch.Runes.IndexOf(this);

        return index + 1 < branch.branch.Runes.Count ? branch.branch.Runes[index + 1] : null;

    }

    public SC_LQ_SpellRune GetPreviousRune()
    {
        int index = branch.branch.Runes.IndexOf(this);

        return index > 0 ? branch.branch.Runes[index + 1] : null;
    }

    public virtual void Effect(GameObject gameObject)
    {
        if (isActif == false)
        {
            return;
        }
    }

    /// <summary>
    /// Function Call output and start next runes
    /// </summary>
    public virtual void Output()
    {

        //Activer l'effet du sort après moi
        if (GetNextRune() != null)
        {
            GetNextRune().SetRuneState(true);
        }
    }

    public virtual void Awake()
    {
        spell = transform.root.GetComponent<SC_LQ_SpellGlobal>();
        branch = GetComponent<SC_LQ_Spell_Branch>();
    }

    #region FunctionBase
    public virtual void OnEnable()
    {

    }

    public virtual void Start()
    {

    }

    public virtual void Update()
    {
        if (isActif == false)
        {
            return;
        }

    }

    public virtual void FixedUpdate()
    {

    }

    public virtual void OnTriggerEnter(Collider other)
    {

    }

    public virtual void OnCollisionEnter(Collision collision)
    {

    }
    #endregion

}
