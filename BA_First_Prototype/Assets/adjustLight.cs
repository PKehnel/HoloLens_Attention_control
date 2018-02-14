using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class adjustLight : MonoBehaviour
{

    // Use this for initialization
    public Color color0 = new Color(00, 1f, 1f, 1f);
    public Color color1 = new Color(1f, 00, 00, 1f);
    public Light lt;
    private bool change = false;
    void Start()
    {
        lt = GetComponent<Light>();
    }
    void Update()
    {

        print("I change the light");
        if (change)
        {
            lt.color = color1;
            change = false;
        }
        else
        {
            lt.color = color0;
            change = true;
        }
    }
}
