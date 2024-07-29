using System;
using Unity.VisualScripting;
using UnityEngine;


public class SC_LQ_SpellEffect : MonoBehaviour
{

    public int indexEffect;
    public SC_LQ_SpellGlobal spell;
    public virtual void Effect()
    {

    }

    public virtual void Awake()
    {
        spell = GetComponent<SC_LQ_SpellGlobal>();
        indexEffect = GetComponentIndex() - 5;
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
