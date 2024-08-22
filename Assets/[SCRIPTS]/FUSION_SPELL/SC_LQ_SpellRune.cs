using UnityEngine;


public class SC_LQ_SpellRune : MonoBehaviour
{

    //public int indexEffect;
    [HideInInspector] public SC_LQ_SpellGlobal spell;

    public SO_Rune runeSO;

    public float currentInk;
    public float costInk;
    public float damage;

    //Faire s'abonner la fonction Effect quand on veut déclencher les effets ???
    public virtual void Effect()
    {

        //Faire spawn une créature amie
        //Faire bruler la cible (Add Component)
    }

    public virtual void TouchEnemy(GameObject enemy)
    {
        //Call Touch Enemy in my parents
        SC_LC_PlayerGlobal.instance.GetComponent<SC_LQ_SpellCast>().TouchEnemy();
    }

    public virtual void Awake()
    {
        spell = transform.root.GetComponent<SC_LQ_SpellGlobal>();
        spell.MyCollisionEnter += OnCollisionEnter;
        spell.MyTriggerEnter += OnTriggerEnter;

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
