using UnityEngine;
using UnityEngine.VFX;

public class SC_LDOV_Shoot : MonoBehaviour
{
    public Transform ennemiePos;
    public GameObject projo;
    public float speed;
    private Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) 
        {
            GameObject shooted = Instantiate(projo, transform.position, Quaternion.identity);

            VisualEffect fx = shooted.GetComponent<VisualEffect>();
            fx.SetVector3("Space", transform.position);

            rb = shooted.GetComponent<Rigidbody>();
            rb.AddForce(shooted.transform.forward*speed, ForceMode.Impulse);

            //shooted.GetComponent<SC_LDOV_ProjoImpact>().targetPos = ennemiePos;

        }
    }
}
