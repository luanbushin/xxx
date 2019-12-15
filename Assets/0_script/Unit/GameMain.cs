using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Noticfacation;
using Game;
using UnityEngine.UI;

using System.IO;

public class GameMain : MonoNotice{
    public static GameMain Instance;

    public GameObject player;
    public GameObject overPanel;
    private Dictionary<Vector3, Dictionary<Vector3, PatrolData>> patrolObject = new Dictionary<Vector3, Dictionary<Vector3, PatrolData>>();

    public Text debugTxt;

    private PassInfo curPassInfo;

    private Dictionary<Vector3, int> enemyList = null;

    void Awake()
    {
        Application.targetFrameRate = 30;//此处限定60帧
        Instance = this;


        CollisionCreator creator = new CollisionCreator();
    } 
    void Start () {
        overPanel.SetActive(false);


        initBaseLogic();


        StartCoroutine(gamestrat());
        //gamestrat();
    }

    public void initPlayerPostion(Vector3 v3) {
        //player.transform.position = new Vector3(10,10,5) ;
    }

    public void initPlayerForce(Vector3 v3) {
        player.GetComponent<Rigidbody>().AddForce(v3);
    }

    private void initBaseLogic() {
        addListener(Notice.EnemyComplete,() =>
        {
            if (enemyList == null) { 
                enemyList = new Dictionary<Vector3, int>();
                StartCoroutine(xml.LoadEnemyXml(enemyList, patrolObject));
            } else {
                gameObject.GetComponent<MonsterManager>().initMonster(enemyList);
            }
        });
        addListener(Notice.WeaponCollision, (string s,GameObject monster) =>
        {
            int dps = monster.GetComponent<MonsterEntity>().monsterValue.dps;
            //Debug.Log(monster);
            GameMain.Instance.player.GetComponent<plyaer>().initHp(-dps);
            //overPanel.SetActive(true);
            //player.SetActive(false);
            //Destroy(player);
        });

        addListener(Notice.TrapCollision, (string s) =>
        {
            overPanel.SetActive(true);
            Destroy(player);
        });

        addListener(Notice.GAME_PLAYER_ATIVE, () =>
        {
            player.SetActive(true);
        });

        addListener(Notice.MAP_COMPLETE_COMPLETE, () =>
        {
            gameObject.GetComponent<MapManager>().initmap(mapIndexObj);
        });
    }

    public XMLTest xml = new XMLTest();
    private Dictionary<Vector3, int> mapIndexObj;
    private IEnumerator gamestrat() {
         
        //gameObject.SetActive(false);
        //curPassInfo = xml.loadPassInfo();

        string fullPath = Application.dataPath + "/Resources/map/Prefabs" + "/";

        //DirectoryInfo direction = new DirectoryInfo(fullPath);
        // FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
        debugTxt.text = fullPath;
        mapIndexObj = new Dictionary<Vector3, int>();
        StartCoroutine(xml.LoadXmland(mapIndexObj,debugTxt));
        yield return 5;



        //gameObject.GetComponent<MonsterManager>().initMonsterPatrol(patrolObject);
        //gameObject.GetComponent<PassMananger>().initBox(xml.LoadBoxXml());
        //gameObject.GetComponent<PassMananger>().initPassInfo(curPassInfo);
        //gameObject.GetComponent<TrapManager>().inittrap(curPassInfo.trapList);


        Invoke("activePlayer", 3f);
    }

    private void activePlayer() {
        player.SetActive(true);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
