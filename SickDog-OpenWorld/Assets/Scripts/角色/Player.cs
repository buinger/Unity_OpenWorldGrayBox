using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    //************************************所有字段*****************************************
    public static Player Instance;
    //设置跑力道
    [Range(1, 1000)]
    public float runForce = 20;
    //设置走力道
    private float walkForce;
    //设置跳跃力道
    [Range(0, 100)]
    public float JumpForce = 100;
    //视角控制的速度
    public float mouseSpeed = 100f;
    //------------------------------------------------------------------------------
    //因为玩家状态可能叠加，这里用链表存储当前状态，如果链表长度为0默认状态为idel
    private List<PlayerState> nowStates = new List<PlayerState>();
    //方向输入值
    float hValue = 0;
    float vValue = 0;
    //重力组件
    Rigidbody rigidBody;
    //跳跃判定对象
    FootFlag ff;
    //角色零件
    Transform parts;
    //镜头参考
    [HideInInspector]
    public Transform cameraTemp;



    //ai相关字段
    //是否为ai状态
    public bool isAi = true;
    //是否为自动寻路ai状态
    public bool isNavAiStutas = false;
    //最大和最小cd（用于刷新ai行为）
    public float aiResetCdMin = 2f;
    public float aiResetCdMax = 10f; 
    //------------------------------------------------------------------------------
    //普通ai行为刷新cd
    private float aiResetCd;
    //上次普通ai行为刷新经过时间
    private float aiPassTime = 0;
    //自动寻路组件
    private NavMeshAgent myNavMeshAgent;



    //*************************************************************************************

    // 一级初始化
    protected virtual void Awake()
    {

        //ai初始化
        myNavMeshAgent = transform.GetComponent<NavMeshAgent>();
        aiResetCd = Random.Range(aiResetCdMin, aiResetCdMax);
        aiPassTime = 0;
        hValue = Random.Range(-1f, 1f);
        vValue = Random.Range(-1f, 1f);

        // 控制初始化
        walkForce = runForce / 2;
        parts = transform.Find("Parts");
        cameraTemp = transform.Find("CameraTemp");
        rigidBody = transform.GetComponent<Rigidbody>();
        ff = transform.GetComponentInChildren<FootFlag>();


        //实现单例
        if (!isAi)
        {

            SetInstance();
        }
    }









    // 一级刷新
    void FixedUpdate()
    {

        if (isAi)
        {
            AiAction();
        }
        else
        {
            MouseCamera();
            JudgeNowState();
            DoingByPlayerStates(nowStates);
        }
    }

    //二级刷新
    protected virtual void Update()
    {
        if (!isAi)
        {
            //判断是否触发跳跃
            if (Input.GetKeyDown(KeyCode.Space))
            {
                nowStates.Add(PlayerState.Jump);
                Debug.Log("触发空格");

            }
        }
        else
        {
            if (isNavAiStutas)
            {

                //以下为自动寻路ai测试（鼠标点哪去哪）
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        SetNavAiPos(hit.point);
                    }
                }
            }



        }

    }


    [ContextMenu("设置此为单例")]
    void SetInstance()
    {
        Instance = this;
    }





    //通过玩家的输入，计算当前的状态
    private void JudgeNowState()
    {




        //判断是否正在移动
        hValue = Input.GetAxis("Horizontal");
        vValue = Input.GetAxis("Vertical");
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            //判断是否在移动的同事按住了shift,切换跑和走状态
            if (Input.GetKey(KeyCode.LeftShift))
            {
                nowStates.Add(PlayerState.Walk);
            }
            else
            {
                nowStates.Add(PlayerState.Run);

            }



        }


    }




    //通过计算出的状态做出行为
    private void DoingByPlayerStates(List<PlayerState> playerStates)
    {
        if (playerStates.Count != 0)
        {
            foreach (PlayerState pState in playerStates)
            {
                Invoke(pState.ToString(), 0);
                // Debug.Log(pState.ToString());
            }
            //完成所有状态行为，清空状态
            playerStates.Clear();
        }
        else
        {
            Invoke("Idel", 0);
        }
    }




    //玩家可能的状态枚举
    public enum PlayerState
    {
        Idel,
        Walk,
        Run,
        Jump
    }


    //public float cameraAddMaxY = 5f;
    //镜头函数
    protected virtual void MouseCamera()
    {
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");
        if (x != 0 || y != 0)
        {
            cameraTemp.RotateAround(transform.position, transform.up, mouseSpeed * x * Time.fixedDeltaTime);
            cameraTemp.RotateAround(transform.position, -cameraTemp.right, mouseSpeed * y * Time.fixedDeltaTime);        
        }

    }



    //可控行为函数**************************************
    protected virtual void Idel()
    {


    }
    protected virtual void Walk()
    {

    }
    protected virtual void Run()
    {

        //根据摄像机位置设置角色移动方向
        //step1:虚拟主摄像机位置与角色同一水平线，并获取这样的摄像机世界桌标
        Vector3 cameraPos = Camera.main.transform.position;
        Vector3 cameraVirtualPos = new Vector3(cameraPos.x, transform.position.y, cameraPos.z);
        //step2:算出虚拟摄像机位置朝向玩家的向量，并归一化，此为玩家向前施加力的方向向量。
        Vector3 forceForward = (transform.position - cameraVirtualPos).normalized;
        //step3:推导出向右施加力的方向向量
        Vector3 forceRight = Quaternion.AngleAxis(90, Vector3.up) * forceForward;



        //step4:看向要移动的方向，给角色施加移动的力道
        Vector3 forceTemp = forceForward * vValue * Time.fixedDeltaTime * runForce + forceRight * hValue * Time.fixedDeltaTime * runForce;

        Vector3 lookAtPos = transform.position + new Vector3(-forceTemp.z, forceTemp.y, forceTemp.x);
        AddMoveForce(forceTemp.x, forceTemp.z, lookAtPos);

    }

    //施加力并看向那个方向
    void AddMoveForce(float xValue, float zValue, Vector3 lookAtPos)
    {
        parts.LookAt(lookAtPos);
        rigidBody.velocity = new Vector3(xValue, rigidBody.velocity.y, zValue);
    }




    protected virtual void Jump()
    {

        if (ff.couldJump == true)
        {
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, JumpForce * Time.deltaTime, rigidBody.velocity.z) * JumpForce;
        }

    }

    //ai行为函数*****************************
    protected virtual void AiAction()
    {
        if (!isNavAiStutas)
        {
            aiPassTime += Time.fixedDeltaTime;
            if (aiPassTime >= aiResetCd)
            {
                Jump();




                hValue = Random.Range(-1f, 1f);


                vValue = Random.Range(-1f, 1f);



                aiResetCd = Random.Range(aiResetCdMin, aiResetCdMax);
                aiPassTime = 0;
            }

            //施加力移动
            float xValue = hValue * runForce * Time.fixedDeltaTime;
            float zValue = vValue * runForce * Time.fixedDeltaTime;
            Vector3 lookAtPos = transform.position + new Vector3(-zValue, 0, xValue);
            AddMoveForce(xValue, zValue, lookAtPos);

        }
        else
        {
            
        }

    }

    public virtual void SetNavAiPos(Vector3 worldPos)
    {
        myNavMeshAgent.SetDestination(worldPos);

    }


}
