using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SC_LQ_DealDamageZone : MonoBehaviour
{
    [SerializeField] float damageZone;
    [SerializeField] LayerMask layermaskDamaged;


    void OnEnable()
    {
        Collider[] hited = Physics.OverlapSphere(transform.position, damageZone, layermaskDamaged);

        foreach (Collider hit in hited)
        {
            //To remplace by a component less specific than player, more like "Ally life"
            if(hit.GetComponent<SC_LC_Player>())
            {
                print("A toucher le joueur");
            }
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, damageZone);
    }
}
