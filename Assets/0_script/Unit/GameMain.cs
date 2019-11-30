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

    void Awake()
    {
        Application.targetFrameRate = 30;//此处限定60帧
        Instance = this;


        CollisionCreator creator = new CollisionCreator();
    } 
    void Start () {
        overPanel.SetActive(false);


        initBaseLogic();

        gamestrat();
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
            gameObject.GetComponent<MonsterManager>().initMonster(xml.LoadEnemyXml(patrolObject));
        });
        addListener(Notice.WeaponCollision, (string s) =>
        {
            overPanel.SetActive(true);
            player.SetActive(false);
            //Destroy(player);
        });

        addListener(Notice.TrapCollision, (string s) =>
        {
            overPanel.SetActive(true);
            Destroy(player);
        });

        addListener(Notice.GAME_PLAYER_ATIVE, () =>
        {
            Debug.Log("=====");
            player.SetActive(true);
        });

    }

    public XMLTest xml = new XMLTest();
    private void gamestrat() {
         
        //gameObject.SetActive(false);
        curPassInfo = xml.loadPassInfo();

        string fullPath = Application.dataPath + "/Resources/map/Prefabs" + "/";

        //DirectoryInfo direction = new DirectoryInfo(fullPath);
       // FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);

        gameObject.GetComponent<MapManager>().initmap(xml.LoadXml());
        
        gameObject.GetComponent<MonsterManager>().initMonsterPatrol(patrolObject);
        gameObject.GetComponent<PassMananger>().initBox(xml.LoadBoxXml());
        gameObject.GetComponent<PassMananger>().initPassInfo(curPassInfo);
        gameObject.GetComponent<TrapManager>().inittrap(curPassInfo.trapList);


        Invoke("activePlayer", 3f);
    }

    private void activePlayer() {
        player.SetActive(true);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
