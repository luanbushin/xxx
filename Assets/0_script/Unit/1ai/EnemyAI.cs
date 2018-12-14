using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Game.Config;
using System;

public class EnemyAI : Boom {
    public GameObject AIVeiw;

    public GameObject target;
    private NavMeshAgent agent;
    public GameObject enemyview;

    public MonsterPresetDataValueData value;

    private bool isdestination = false;

    private List<Vector3> patrolPositionList;
    private List<PatrolData> patrolList;

    private int curPatrolIndex = -1;

    // Use this for initialization
    void Start() {
        difference = 0.5f;
        target = GameObject.Find("player");
        agent = GetComponent<NavMeshAgent>();       //获取自动寻径组件

        if (value != null)
            agent.speed = value.move;
    }

    public void initMonsterValue(MonsterPresetDataValueData v) {
        value = v;
        if (agent)
            agent.speed = v.move;

        creatView(v.viewrange,v.viewagent);
    }


    private void creatView(float r,float a) {
        enemyview = GameObject.Instantiate(AIVeiw, new Vector3(Mathf.Round(gameObject.transform.position.x), gameObject.transform.position.y + 0.5f, Mathf.Round(gameObject.transform.position.z)), gameObject.transform.rotation);
        enemyview.GetComponent<yuan>().Radius = r;
        enemyview.GetComponent<yuan>().angleDegree = a;
        enemyview.GetComponent<yuan>().Initialization();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(agent);
            Debug.Log("========"+transform.position);
            detonateBoom();
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(agent);
            detonateBoom();
        }
    }



    public void initPatrol(Dictionary<Vector3, PatrolData> list)
    {
        patrolPositionList = new List<Vector3>();
        patrolList = new List<PatrolData>();

        foreach (Vector3 v3 in list.Keys)
        {
            patrolPositionList.Add(v3);
            patrolList.Add(list[v3]);
        }

        curPatrolIndex = 0;
    }
    public bool checkTargetDirForMe()
    {
        Vector3 b = target.transform.position;
        Vector3 a = transform.position;

        b.x -= a.x;
        b.z -= a.z;

        float angle = 0;
        float deltaAngle = 0;
        if (b.x == 0 && b.z == 0)
        {
            angle = 0;
        }
        else if (b.x > 0 && b.z > 0)
        {
            deltaAngle = 0;
        }
        else if (b.x > 0 && b.z == 0)
        {
            angle = 90;
        }
        else if (b.x > 0 && b.z < 0)
        {
            deltaAngle = 180;
        }
        else if (b.x == 0 && b.z < 0)
        {
            angle =  180;
        }
        else if (b.x < 0 && b.z < 0)
        {
            deltaAngle = -180;
        }
        else if (b.x < 0 && b.z == 0)
        {
            angle =  - 90;
        }
        else if (b.x < 0 && b.z > 0)
        {
            deltaAngle = 0;
        }

        angle = Mathf.Atan(b.x / b.z) * Mathf.Rad2Deg + deltaAngle;
  
        //Debug.Log(angle + "==========" + Math.Abs(transform.eulerAngles.y - angle));
        if (Math.Abs(transform.eulerAngles.y - angle) < value.viewagent / 2) {
            //Debug.Log("在视角范围！！");
            return true;
        }
        return false;
    }


    private bool isFindActTarget() {
        if (Vector3.Distance(transform.position, target.transform.position) < value.viewrange) {

            if (checkTargetDirForMe()) {
                return true;
            }
            //Debug.Log("在攻击范围！！");
        }
        return false ;
    }

    private bool iswalk = false;
    private int jiangge;

    private PatrolData curPatrolData;
    private int patrolIndex;
    private bool resetAgent;
    private int curloop;

    private void updataPatrol() {
        if (patrolPositionList == null||patrolPositionList.Count == 0)
            return;

        if (curPatrolData != null) {
            if (resetAgent == false) {
                if (transform.eulerAngles.y - curPatrolData.agentmin > 180)
                {
                    transform.Rotate(new Vector3(0, 3, 0));
                }
                else
                {
                    transform.Rotate(new Vector3(0, -3, 0));
                }
                if (Math.Abs(transform.eulerAngles.y - curPatrolData.agentmin) < 10) {
                    resetAgent = true;
                }
                return;
            }
            else{
                if (curloop > curPatrolData.loop)
                {

                }
                else {
                    if (curloop % 2 == 0)
                    {
                        transform.Rotate(new Vector3(0, (curPatrolData.agentmax - curPatrolData.agentmin)/ curPatrolData.stayTime, 0));
                        if (Math.Abs(transform.eulerAngles.y - curPatrolData.agentmax) < Math.Abs((curPatrolData.agentmax - curPatrolData.agentmin) / curPatrolData.stayTime))
                        {
                            curloop++;
                        }
                    }
                    else {
                        transform.Rotate(new Vector3(0, (curPatrolData.agentmin - curPatrolData.agentmax) / curPatrolData.stayTime, 0));
                        if (Math.Abs(transform.eulerAngles.y - curPatrolData.agentmin) < Math.Abs((curPatrolData.agentmax - curPatrolData.agentmin) / curPatrolData.stayTime))
                        {
                            curloop++;
                        }
                    }
                }
            }

            patrolIndex ++;
            if (patrolIndex > curPatrolData.stayTime* curPatrolData.loop) {
                agent.enabled = true;
                iswalk = false;
                curPatrolIndex++;
                if (curPatrolIndex >= patrolPositionList.Count)
                {
                    curPatrolIndex = 0;
                }
                curPatrolData = null;
            }
            return;
        }

        if (curPatrolIndex >=0 ) {
            if (iswalk == false) {
                if (Vector3.Distance(patrolPositionList[curPatrolIndex], transform.position) < 1) {
                    if (patrolList[curPatrolIndex] == null)
                    {
                        iswalk = false;
                        curPatrolIndex++;
                        if (curPatrolIndex >= patrolPositionList.Count)
                        {
                            curPatrolIndex = 0;
                        }
                    }
                    else
                    {
                        curPatrolData = patrolList[curPatrolIndex];
                        curloop = patrolIndex = 0;
                        agent.enabled = false;
                        resetAgent = false;
                    }
                    return;
                }
                else {
                    agent.SetDestination(patrolPositionList[curPatrolIndex]);
                    iswalk = true;
                    jiangge = 0;
                }
            }
            if (jiangge < 20) {
                jiangge++;
                return;
            }
            if (Math.Abs(agent.remainingDistance) < 0.02&&agent.enabled&&agent.pathStatus == NavMeshPathStatus.PathComplete) {
                if (patrolList[curPatrolIndex] == null)
                {
                    iswalk = false;
                    curPatrolIndex++;
                    if (curPatrolIndex >= patrolPositionList.Count) {
                        curPatrolIndex = 0;
                    }
                }
                else {
                    curPatrolData = patrolList[curPatrolIndex];
                    curloop = patrolIndex = 0;
                    agent.enabled = false;
                    resetAgent = false;
                    //iswalk = false;
                    //curPatrolIndex++;
                    //if (curPatrolIndex >= patrolPositionList.Count)
                    //{
                    //    curPatrolIndex = 0;
                    //}
                }
            }
        }
    }

    private long mFrameCount = 0;
    private long mLastFrameTime = 0;
    static long mLastFps = 0;
    private void UpdateTick()
    {
        if (true)
        {
            mFrameCount++;
            long nCurTime = TickToMilliSec(System.DateTime.Now.Ticks);
            if (mLastFrameTime == 0)
            {
                mLastFrameTime = TickToMilliSec(System.DateTime.Now.Ticks);
            }

            if ((nCurTime - mLastFrameTime) >= 1000)
            {
                long fps = (long)(mFrameCount * 1.0f / ((nCurTime - mLastFrameTime) / 1000.0f));

                mLastFps = fps;

                mFrameCount = 0;

                mLastFrameTime = nCurTime;
            }
           // Debug.Log(mLastFps);
        }
    }
    public static long TickToMilliSec(long tick)
    {
        return tick / (10 * 1000);
    }

    // Update is called once per frame
    void Update () {

        if (isFindActTarget())
        {
            isdestination = true;
            agent.enabled = true;
            agent.SetDestination(target.transform.position);
        }
        else {
            updataPatrol();
        }
     

        //UpdateTick();
        //updataPatrol();
        if (enemyview)
        {
            enemyview.transform.rotation = this.transform.rotation;
            enemyview.transform.Rotate(0, -45, 0);
            enemyview.transform.position = this.transform.position;
        }
       // if (target == null)
        return;
       

        if (target.transform.position.x - transform.position.x < -6 || target.transform.position.x - transform.position.x >6)
        {
            return;
        }
        else if (target.transform.position.z - transform.position.z < -6 || target.transform.position.z - transform.position.z >6)
        {
            return;
        }



        if (agent.remainingDistance <0.1)
        {
          //  AnimationToldle();           //调用下面那个静止不动效果的方法
            if (isdestination)
            {
                //stopCount++;
               // if (stopCount > 200)
                //{
                   // stopCount = 0;
                    isdestination = false;
                //}
            }

        }
        else
        {
           // AnimationTowalk();          //开始走动方法
        }
    }
}
