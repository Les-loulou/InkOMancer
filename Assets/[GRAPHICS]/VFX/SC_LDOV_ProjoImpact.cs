using UnityEngine;
using UnityEngine.VFX;

public class SC_LDOV_ProjoImpact : MonoBehaviour
{
    public Transform targetPos;
    private VisualEffect fx;
    public string targetTag;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fx = GetComponent<VisualEffect>();
        fx.SetVector3("Space", transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = Vector3.Lerp(transform.position, targetPos.position, Time.deltaTime*3);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.CompareTag(targetTag))
        {
            fx.SetBool("IsAlive", false);
            fx.SendEvent("OnHit");
            gameObject.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            print("shoot");

            other.gameObject.GetComponent<Animator>().SetTrigger("Damage");
        }
    }

}
