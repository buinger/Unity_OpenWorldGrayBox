using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMake : MonoBehaviour
{

    public float stardYvalue = 0;
    public GameObject model;
    public Transform container;


    public Transform tran1;
    public Transform tran2;

    public int hangCount = 6;
    public int lieCount = 3;

  


    private List<RangeRectangle> allRanges = new List<RangeRectangle>();


    private float hangDis;
    private float lieDis;

    [ContextMenu("刷物体")]
    void DoingAll()
    {
        GetAllRectRange();
        Making();
    }
    



    // Start is called before the first frame update
    void Making()
    {
       
        foreach (RangeRectangle rr in allRanges)
        {

            Debug.Log(rr.zuoxia+"----"+rr.youshang );
            float zuoxiaX = rr.zuoxia.x;
            float zuoxiaZ = rr.zuoxia.z;

            float youshangX = rr.youshang.x;
            float youshangZ = rr.youshang.z;

            Vector3 aimPos = new Vector3
                (Random.Range(zuoxiaX, youshangX), 
                stardYvalue, Random.Range(zuoxiaZ, youshangZ));


            GameObject newModel =Instantiate(model);
            newModel.transform.position = aimPos;
            newModel.transform.parent = container;
        }

    }


    void GetAllRectRange()
    {

        allRanges.Clear();

        //hangDis = tran1.position.x - tran2.position.x;
        //lieDis = tran1.position.z - tran2.position.z;


        hangDis =  tran2.position.x- tran1.position.x;
        lieDis = tran2.position.z- tran1.position.z ;


        float oneRectangleWidth = hangDis / lieCount;
        float oneRectangleHeight = lieDis / hangCount;

        for (int hang = 0; hang < hangCount-1; hang++)
        {


            for (int lie = 0; lie < lieCount; lie++)
            {


                float ponit1_x = tran1.position.x + lie * oneRectangleWidth;
                float ponit1_z = tran1.position.z + hang * oneRectangleHeight;

                //float ponit2_x = ponit1_x + oneRectangleWidth * (lie + 1);
                // float ponit2_z = ponit1_z + oneRectangleWidth * (hang + 1);

                float ponit2_x = ponit1_x + oneRectangleWidth;
                 float ponit2_z = ponit1_z + oneRectangleWidth ;



                RangeRectangle rangeRect = new RangeRectangle();
                rangeRect.zuoxia = new Vector3(ponit1_x, 0, ponit1_z);
                rangeRect.youshang = new Vector3(ponit2_x, 0, ponit2_z);

                allRanges.Add(rangeRect);
            }


        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }




}




class RangeRectangle
{
  public Vector3 zuoxia;
   public Vector3 youshang;
}
