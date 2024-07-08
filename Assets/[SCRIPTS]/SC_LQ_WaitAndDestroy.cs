using UnityEngine;

public class SC_LQ_WaitAndDestroy : MonoBehaviour
{
    [SerializeField] float timebeforeDestroy;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, timebeforeDestroy);
    }

}
