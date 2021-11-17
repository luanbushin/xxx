using Game.Config;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Game;
using Game.Noticfacation;


public class MonsterManager : MonoNotice
{
    public static MonsterManager Instance;

    //public static MonsterManager current { get; private set; }
    // Use this for initialization

    public GameObject enemyparent;
    public GameObject enemyGameObject;
    public GameObject enemyBox;
    private Dictionary<Vector3, GameObject> enemyObject = new Dictionary<Vector3, GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    void Start () {

  

        addListener(Notice.EnemyCreat, (string s,GameObject gameo) =>
        {
            GameObject obj = GameObject.Instantiate(enemyGameObject, gameo.transform.position, gameObject.transform.rotation);
            obj.transform.SetParent(gameo.transform);
            obj.GetComponent<EnemyAI>().initMonsterValue(MonsterPresetData.get(1));
            enemyObject[obj.transform.position] = obj;
        });

        addListener(Notice.MonsterATK, (string s, GameObject g, GameObject monster) =>
        {
            int dps = monster.GetComponent<MonsterEntity>().monsterValue.dps;
            //Debug.Log(monster);
            GameMain.Instance.player.GetComponent<plyaer>().initHp(-dps);
            //GameMain.Instance.overPanel.SetActive(true);
            //GameMain.Instance.player.SetActive(false);
        });

        addListener(Notice.MonsterBeATK, (string s, GameObject g) =>
        {
            g.GetComponent<MonsterEntity>().curHp -= 1;
            if (g.GetComponent<MonsterEntity>().curHp <= 0) {
                Destroy(g);

                Notice.CreatProp.broadcast(g.transform.position,"");
            }
            //Debug.Log(g);
            //Destroy(g.GetComponent<EnemyAI>().enemyview);
            //Destroy(g);

            //if (g.transform.parent.gameObject != enemyparent) {
            //    TrapManager.Instance.deleteMonster(g.transform.parent.gameObject);
            //}
        });
    }

    public void creatEnemy(Rect rect) {
        GameObject box = Instantiate(enemyBox, new Vector3(rect.x + rect.width / 2, 1.0f, rect.y + rect.height / 2), Quaternion.identity) as GameObject;
        box.AddComponent<EnemyAutoAi>().roomrect = rect;
    }

    public GameObject getWarriorMonster(string str)
    {
        //return (GameObject)Resources.Load("enemy/"+str);.
        return (GameObject)Resources.Load(str);
    }





    public void initMonster(Dictionary<Vector3, int> enemlist) {

        foreach (Vector3 list in enemlist.Keys)
        {
            GameObject obj = GameObject.Instantiate(enemyBox, list, gameObject.transform.rotation);

            //(enemlist[list]
            int num = UnityEngine.Random.Range(1, 3);

            int[] listrandom = new int[] { 1, 2, 16};
            switch (listrandom[num]) {
                case 1:
                    //小猪
                    obj.AddComponent<PigMonster>();
                    break;
                case 2:
                    //大猪
                    obj.AddComponent<Pig2>();
                    break;
                case 3:
                    //牛头
                    obj.AddComponent<CattleMan>();
                    break;
                case 4:
                    //龙
                    obj.AddComponent<Dragon>();
                    break;
                case 5:
                    //小龙
                    obj.AddComponent<Dragon01>();
                    break;
                case 6:
                    //大螃蟹
                    obj.AddComponent<CrabBig>();
                    break;
                case 7:
                    //小螃蟹
                    obj.AddComponent<CrabLil>();
                    break;
                case 8:
                    //幽灵
                    obj.AddComponent<Ghost>();
                    break;
                case 9:
                    //小蜘蛛
                    obj.AddComponent<LitSpider>();
                    break;
                case 10:
                    //蛇
                    obj.AddComponent<SnakeMan>();
                    break;
                case 11:
                    //蜘蛛人
                    obj.AddComponent<CattleMan>();
                    break;
                case 12:
                    //战士
                    obj.AddComponent<Warrior01>();
                    break;
                case 13:
                    //小野人
                    obj.AddComponent<WildMan1>();
                    break;
                case 14:
                    //大野人
                    obj.AddComponent<WildMan2>();
                    break;
                case 15:
                    //狼人
                    obj.AddComponent<WolfMan>();
                    break;
                case 16:
                    //弓箭手
                    obj.AddComponent<Archer01>();
                    break;
            }
            obj.GetComponent<MonsterEntity>().curHp = MonsterPresetData.get(enemlist[list]).hp;
            obj.GetComponent<MonsterEntity>().monsterValue = MonsterPresetData.get(enemlist[list]);
            //Debug.Log(enemlist[list]);
            //int random = UnityEngine.Random.Range(1, 1);
            //if (random == 1)
            //{
            //    obj.AddComponent<Archer01>();
            //}
            //else { 
            //    obj.AddComponent<Warrior01>();
            //}
            // obj.GetComponent<EnemyAI>().initMonsterValue(MonsterPresetData.get(enemlist[list]));

            enemyObject[obj.transform.position] = obj;
            obj.transform.SetParent(enemyparent.transform);
        }
    }

    public void initMonsterPatrol(Dictionary<Vector3, Dictionary<Vector3, PatrolData>> patrolObject) {
        foreach(Vector3 v3 in patrolObject.Keys)
        {
            enemyObject[v3].GetComponent<EnemyAI>().initPatrol(patrolObject[v3]);



        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
