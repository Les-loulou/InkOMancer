using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SC_LQ_SpellGlobal : MonoBehaviour
{
    public float speed;
    public float acceleration = 1f;
    //public float damage;
    public Transform target;

    public float contactBeforeDestroy = 1;

    public Rigidbody rb;

    private void FixedUpdate()
    {
        rb.linearVelocity = Vector3.Lerp(Vector3.zero, Vector3.Normalize(target.position - transform.position) * speed, acceleration);
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        SetTarget();
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    private void SetTarget()
    {
        //Found an target
        if (target == null)
        {
            Collider[] enemy = Physics.OverlapSphere(transform.position, 100f);

            foreach (Collider collider in enemy)
            {
                if (collider.gameObject.GetComponent<SC_LQ_EnemyGlobal>())
                {
                    target = collider.transform;
                    break;
                }
            }
        }
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
