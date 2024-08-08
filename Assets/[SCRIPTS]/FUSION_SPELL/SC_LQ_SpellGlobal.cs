using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SC_LQ_SpellGlobal : MonoBehaviour
{
    public float speed;
    public float damage;
    public bool isDestroyingToContact;

    public Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
}
