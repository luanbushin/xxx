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
        DataAll.addStart(MonsterPresetData.start);

        DataAll.start(() =>
        {
            StartCoroutine(loadCompleteInfo());
        });

        addListener(Notice.EnemyCreat, (string s,GameObject gameo) =>
        {
            GameObject obj = GameObject.Instantiate(enemyGameObject, gameo.transform.position, gameObject.transform.rotation);
            obj.transform.SetParent(gameo.transform);
            obj.GetComponent<EnemyAI>().initMonsterValue(MonsterPresetData.get(1));
            enemyObject[obj.transform.position] = obj;
        });

        addListener(Notice.MonsterATK, (string s, GameObject g) =>
        {
            GameMain.Instance.overPanel.SetActive(true);
            GameMain.Instance.player.SetActive(false);
        });

        addListener(Notice.MonsterBeATK, (string s, GameObject g) =>
        {
            Destroy(g.GetComponent<EnemyAI>().enemyview);
            Destroy(g);

            if (g.transform.parent.gameObject != enemyparent) {
                TrapManager.Instance.deleteMonster(g.transform.parent.gameObject);
            }
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



    private IEnumerator loadCompleteInfo()
    {
        //var data = MonsterPresetData.get(1);
        Notice.EnemyComplete.broadcast();
        yield return 1;
    }

    public void initMonster(Dictionary<Vector3, int> enemlist) {
  
        foreach (Vector3 list in enemlist.Keys)
        {
            GameObject obj = GameObject.Instantiate(enemyBox, list, gameObject.transform.rotation);
            switch (enemlist[list]) {
                case 1:
                    obj.AddComponent<PigMonster>();
                    break;
            }
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
