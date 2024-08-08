using System;
using Unity.VisualScripting;
using UnityEngine;


public class SC_LQ_SpellEffect : MonoBehaviour
{

    //public int indexEffect;
    public SC_LQ_SpellGlobal spell;
    public virtual void Effect()
    {
        
    }

    /// <summary>
    /// Function Call output and start next runes
    /// </summary>
    public virtual void Output()
    {

    }

    public virtual void Awake()
    {
        //spell = GetComponent<SC_LQ_SpellGlobal>();
        //indexEffect = GetComponentIndex() - 5;

        spell = GetComponent<SC_LQ_SpellGlobal>();
    }

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
        
    }

    public virtual void OnCollisionEnter(Collision collision)
    {
        
    }


}
