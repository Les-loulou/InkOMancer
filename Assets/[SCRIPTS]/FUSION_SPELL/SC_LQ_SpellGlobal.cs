using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SC_LQ_SpellGlobal : MonoBehaviour
{
    public float speed;
    public float damage;

    public float contactBeforeDestroy = 1;

    public Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public System.Action<Collision> MyCollisionEnter;
    private void OnCollisionEnter(Collision collision)
    {
        MyCollisionEnter?.Invoke(collision);

        Contact(collision.gameObject);
    }

    public System.Action<Collider> MyTriggerEnter;
    private void OnTriggerEnter(Collider other)
    {
        MyTriggerEnter?.Invoke(other);

        Contact(other.gameObject);
    }


    public void Contact(GameObject contact)
    {

        if (contact.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            contactBeforeDestroy--;
            if (contactBeforeDestroy <= 0)
            {
                Destroy(gameObject);

            }
        }
    }

}
