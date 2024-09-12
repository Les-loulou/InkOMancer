using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SC_LDOV_Shoot : MonoBehaviour
{
    public Transform ennemiePos;
    public List<GameObject> allprojo;
    public float speed;
    private Rigidbody rb;

    public int firstRuneIndex;

    public int secondtRuneIndex;

    public bool isFusion;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isFusion)
            {
                GameObject shoot = Instantiate(allprojo[firstRuneIndex], transform.position, Quaternion.identity);

                VisualEffect fx = shoot.GetComponent<VisualEffect>();
                fx.SetVector3("Space", transform.position);
                fx.SetInt("DuoRune", firstRuneIndex);

                rb = shoot.GetComponent<Rigidbody>();
                rb.AddForce(shoot.transform.forward * speed, ForceMode.Impulse);
            }
            else
            {
                GameObject shootOne = Instantiate(allprojo[firstRuneIndex], transform.position, Quaternion.identity);
                GameObject shootTwo = Instantiate(allprojo[secondtRuneIndex], transform.position, Quaternion.identity);

                VisualEffect fx1 = shootOne.GetComponent<VisualEffect>();
                fx1.SetVector3("Space", transform.position);
                fx1.SetInt("DuoRune", secondtRuneIndex);

                VisualEffect fx2 = shootTwo.GetComponent<VisualEffect>();
                fx2.SetVector3("Space", transform.position);
                fx2.SetInt("DuoRune", firstRuneIndex);

                rb = shootOne.GetComponent<Rigidbody>();
                rb.AddForce(shootOne.transform.forward * speed, ForceMode.Impulse);

                rb = shootTwo.GetComponent<Rigidbody>();
                rb.AddForce(shootTwo.transform.forward * speed, ForceMode.Impulse);

                //shooted.GetComponent<SC_LDOV_ProjoImpact>().targetPos = ennemiePos;
            }
        

        }
    }
}
