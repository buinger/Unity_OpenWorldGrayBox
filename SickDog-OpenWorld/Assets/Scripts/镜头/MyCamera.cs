using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCamera : MonoBehaviour
{



    // 三级刷新
    void LateUpdate()
    {


        //插值跟进(抖动bug)
        //transform.position = Vector3.Lerp(transform.position, Player.Instance.cameraTemp.position,0.1f);
        //transform.rotation = Quaternion.Lerp(transform.rotation, Player.Instance.cameraTemp.rotation, 0.1f);
        //直接跟进

        if (Player.Instance != null)
        {
            transform.position = Player.Instance.cameraTemp.position;
            transform.rotation = Player.Instance.cameraTemp.rotation;
            //transform.position = Vector3.Lerp(transform.position, Player.Instance.cameraTemp.position, 0.1f);
            //transform.rotation = Quaternion.Lerp(transform.rotation, Player.Instance.cameraTemp.rotation, 0.1f);
        }
    }






}
