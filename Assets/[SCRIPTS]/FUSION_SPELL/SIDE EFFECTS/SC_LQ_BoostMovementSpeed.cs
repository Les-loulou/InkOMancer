using System.Collections;
using UnityEngine;

public class SC_LQ_BoostMovementSpeed : MonoBehaviour
{
    public float boostTime;
    public float boostpourcen;
    private void Start()
    {
        StartCoroutine(SpeedBoost());
    }

    public IEnumerator SpeedBoost()
    {
        SC_LC_PlayerMovements player = GetComponent<SC_LC_PlayerMovements>();

        player.moveSpeed += GetComponent<SC_LC_PlayerGlobal>().playerStats.movementSpeed * boostpourcen * 0.01f;

        yield return new WaitForSeconds(boostTime);

        player.moveSpeed -= GetComponent<SC_LC_PlayerGlobal>().playerStats.movementSpeed * boostpourcen * 0.01f;
        Destroy(this);
    }
}
