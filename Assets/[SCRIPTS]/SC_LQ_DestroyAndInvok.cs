using UnityEngine;

public class SC_LQ_DestroyAndInvok : MonoBehaviour
{
    public GameObject explosion;


    public void DestroyAndInvok()
    {
        Instantiate(explosion, transform.GetChild(0).position, transform.GetChild(0).rotation);
        Destroy(gameObject);
    }

}
