using System;
using System.Drawing;
using UnityEngine;

public class SC_LQ_RuneKnockback : SC_LQ_SpellRune
{

    public LayerMask collisionLayer;

    float knockLenght = 3f;

    public override void Start()
    {
        spell.MyCollisionEnter += OnCollisionEnter;
        spell.MyTriggerEnter += OnTriggerEnter;
    }//Subscribe to Trigger and Collision

    private void OnDestroy()
    {
        spell.MyCollisionEnter -= OnCollisionEnter;
        spell.MyTriggerEnter -= OnTriggerEnter;
    }//UnSubscribe to Trigger and Collision

    public override void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Effect(collision.gameObject);
        }
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Effect(other.gameObject);
        }
    }

    public override void Effect(GameObject knocked)
    {

        base.Effect(knocked);

        if (knocked != null)
        {
            Knock(knocked);
        }
        print("knock ??");

        Output();
    }

    public void Knock(GameObject knocked)
    {
        //Refaire le knock et en faire un state de l'ennemi (Si peut être knock ?)
        Vector3 dir = transform.position - knocked.transform.position;
        dir.y = 0;

        dir = dir.normalized;

        knocked.transform.position += dir * knockLenght;
    }



}
