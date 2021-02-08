using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GameObjectAct
{
    //快捷键控制游戏对象的开关 alt + `
    [MenuItem("Tools/Custom/Active GameObject &q")]
    public static void ActiveGameObject()
    {
        GameObject go = Selection.activeGameObject;
        if (go == null) return;
        bool isActive = !go.activeSelf;
        go.SetActive(isActive);
    }


  
}
