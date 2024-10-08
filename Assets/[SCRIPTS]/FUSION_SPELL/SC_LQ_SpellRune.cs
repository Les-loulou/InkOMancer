using UnityEngine;


public class SC_LQ_SpellRune : MonoBehaviour
{

    //public int indexEffect;
    [HideInInspector] public SC_LQ_SpellGlobal spell;

    public SO_Rune runeSO;

    [HideInInspector] public string runeName;


    //Faire s'abonner la fonction Effect quand on veut d�clencher les effets ???
    public virtual void Effect()
    {


        //Faire spawn une cr�ature amie
        //Faire bruler la cible (Add Component)
    }

    public System.Action OnTouchEnemy;
    public virtual void TouchEnemy(GameObject enemy)
    {
        //Call Touch Enemy in my parents
        SC_LC_PlayerGlobal.instance.GetComponent<SC_LQ_SpellCast>().TouchEnemy();
    }

    public void FoundRune()
    {
        foreach (SO_Rune rune in SC_LC_PlayerGlobal.instance.GetComponent<SC_LQ_SpellCast>().runesScriptable)
        {
            if (rune.runeName == runeName)
            {
                runeSO = rune;
                break;
            }
        }
    }

    public virtual void Awake()
    {
        FoundRune();

        spell = transform.root.GetComponent<SC_LQ_SpellGlobal>();
        spell.MyCollisionEnter += OnCollisionEnter;
        spell.MyTriggerEnter += OnTriggerEnter;

        runeSO.CostRune();

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

    }

    public virtual void FixedUpdate()
    {

    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<SC_LQ_EnemyGlobal>())
        {
            TouchEnemy(other.gameObject);
        }
    }

    public virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<SC_LQ_EnemyGlobal>())
        {
            TouchEnemy(collision.gameObject);
        }
    }
    #endregion

}
