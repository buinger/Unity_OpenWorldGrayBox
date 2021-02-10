using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootFlag : MonoBehaviour
{
    public bool couldJump;
    // Start is called before the first frame update
    void Start()
    {
        couldJump = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag != "player")
        {
            couldJump = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag != "player")
        {
            couldJump = false;
        }


    }
}
