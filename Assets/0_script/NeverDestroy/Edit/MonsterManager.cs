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
        GameObject enemy = Instantiate(MonsterManager.Instance.getWarriorMonster(), new Vector3(rect.x + rect.width / 2, 1.0f, rect.y + rect.height / 2), Quaternion.identity) as GameObject;
        enemy.AddComponent<EnemyAutoAi>() ;
    }

    public GameObject getWarriorMonster()
    {
        int random = UnityEngine.Random.Range(1,3);
        return (GameObject)Resources.Load("enemy/Warrior_0" + random);
    }

    private IEnumerator loadCompleteInfo()
    {
        var data = MonsterPresetData.get(1);
        Notice.EnemyComplete.broadcast();
        yield return 1;
    }

    public void initMonster(Dictionary<Vector3, int> enemlist) {
  
        foreach (Vector3 list in enemlist.Keys)
        {
            GameObject obj = GameObject.Instantiate(enemyGameObject, list, gameObject.transform.rotation);
            obj.GetComponent<EnemyAI>().initMonsterValue(MonsterPresetData.get(enemlist[list]));

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
