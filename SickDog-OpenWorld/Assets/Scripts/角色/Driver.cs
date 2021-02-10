using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Driver : Player
{
    SphereTrigger st;

    protected override void Awake()
    {
        base.Awake();
        st = transform.GetComponentInChildren<SphereTrigger>();
    }


    //Debug.DrawRay(transform.position, transform.right*100,Color.red);
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        Debug.Log("按下了e     ");
    //        //Ray myRay = new Ray(transform.position,transform.right);
    //        RaycastHit hit;
    //        if (Physics.Raycast(transform.position, transform.right,out hit))
    //        {
    //            Debug.Log("检测到物体"+hit.transform.name);
    //            if (hit.transform.tag=="car")
    //            {
    //                if (allCollicion.Contains(hit.transform))
    //                {
    //                    Debug.Log("开始移动");
    //                    nav.SetDestination(hit.transform.position);
    //                }
    //            }
    //        }


    //    }


}
