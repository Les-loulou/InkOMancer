using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_LC_Player : MonoBehaviour
{
    [HideInInspector] public SC_LC_PlayerControls controls;

    void Start()
    {
        controls = SC_LC_PlayerControls.instance;
    }
}
