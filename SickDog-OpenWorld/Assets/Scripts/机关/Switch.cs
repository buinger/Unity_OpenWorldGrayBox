using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Switch : MonoBehaviour
{

    public UnityEvent swithOnEvent;
    public UnityEvent swithOffEvent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="player")
        {
            swithOnEvent.Invoke();
        }
        Debug.Log("sdfsdfsd");
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "player")
        {
            swithOffEvent.Invoke();
        }
    }
}
