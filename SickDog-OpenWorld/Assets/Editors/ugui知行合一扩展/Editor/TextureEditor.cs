﻿using UnityEditor;
using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;

public class TextureEditor : MonoBehaviour
{

    static Object targetObj;
    static Object[] allTargetObj;
    //[MenuItem("TextureEdit/Edit")]
    //[MenuItem("TextureEdit/Edit _C")]
    //static void EditTexture()
    //{
    //    targetObj = Selection.activeObject;//这个函数可以得到你选中的对象
    //    if (targetObj && targetObj is Texture)
    //    {
    //        string path = AssetDatabase.GetAssetPath(targetObj);
    //        TextureImporter texture = AssetImporter.GetAtPath(path) as TextureImporter;
    //        texture.textureType = TextureImporterType.Sprite;
    //        texture.spritePixelsPerUnit = 1;
    //        texture.filterMode = FilterMode.Trilinear;
    //        texture.mipmapEnabled = false;
    //        texture.textureFormat = TextureImporterFormat.AutomaticTruecolor;
    //        AssetDatabase.ImportAsset(path);


    //    }
    //}
    [MenuItem("TextureEdit/AllEdit %#Z")]
    static void EditTexture2()
    {
        allTargetObj = Selection.objects;//这个函数可以得到你选中的对象
        foreach (Object targetObj in allTargetObj)
        {
            if (targetObj && targetObj is Texture)
            {
                string path = AssetDatabase.GetAssetPath(targetObj);
                TextureImporter texture = AssetImporter.GetAtPath(path) as TextureImporter;
                texture.textureType = TextureImporterType.Sprite;
                texture.spritePixelsPerUnit = 1;
                texture.filterMode = FilterMode.Trilinear;
                texture.mipmapEnabled = false;
                texture.textureFormat = TextureImporterFormat.AutomaticTruecolor;
                AssetDatabase.ImportAsset(path);
            }
        }
    }
    [MenuItem("TextureEdit/CreateImage %&SPACE")]
    static void ImageMake()
    {
        GameObject canvas = GameObject.Find("Canvas");
        if (canvas == null)
        {
            return;
        }
        allTargetObj = Selection.objects;//这个函数可以得到你选中的对象
        foreach (Object targetObj in allTargetObj)
        {
            if (targetObj && targetObj is Texture)
            {
                string path = AssetDatabase.GetAssetPath(targetObj);
                //string rPath = System.Text.RegularExpressions.Regex.Replace(path, "Assets/Resources/", "");
                //Debug.Log(rPath);
                TextureImporter texture = AssetImporter.GetAtPath(path) as TextureImporter;
                if (texture.textureType == TextureImporterType.Sprite)
                {

                    //Sprite s=(Sprite)Resources.Load(rPath);
                    //Object pic = AssetDatabase.LoadAssetAtPath<Sprite>(path)/* Resources.Load("texture/a", typeof(Sprite))*/;
                    //Sprite s = Instantiate(pic) as Sprite;
                    Sprite s = AssetDatabase.LoadAssetAtPath<Sprite>(path);
                    float w = s.rect.width;
                    float h = s.rect.height;
                    Object obj = Resources.Load("model/ImageModel");
                    GameObject gobj = (GameObject)Instantiate(obj, canvas.transform);
                    Image img = gobj.GetComponent<Image>();
                    img.rectTransform.sizeDelta = new Vector2(w, h);
                    img.sprite = s;
                    img.gameObject.name = s.name;
                }
            }
        }
    }
    [MenuItem("TextureEdit/CopyPath %#X")]
    static void CopyPath()
    {
        targetObj = Selection.activeObject;
        string path = AssetDatabase.GetAssetPath(targetObj);
        path=System.Text.RegularExpressions.Regex.Replace(path, "Assets/Resources/", "");
        GUIUtility.systemCopyBuffer = path;

    }
}
