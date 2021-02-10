using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereTrigger : MonoBehaviour
{
    [HideInInspector]
    public List<Transform> triggeredObjects = new List<Transform>();


    private void OnEnable()
    {
        triggeredObjects.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!triggeredObjects.Contains(other.transform))
        {
            triggeredObjects.Add(other.transform);
        }
    }



    private void OnTriggerExit(Collider other)
    {


        if (triggeredObjects.Contains(other.transform))
        {
            triggeredObjects.Remove(other.transform);
        }


    }
}
