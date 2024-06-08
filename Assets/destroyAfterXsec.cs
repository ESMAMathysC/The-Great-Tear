using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyAfterXsec : MonoBehaviour
{
    public int seconds = 1;

    private void OnEnable()
    {
        Invoke(nameof(AutoDestroy), seconds);
    }

    private void AutoDestroy()
    {
        Destroy(gameObject);
    }
}

