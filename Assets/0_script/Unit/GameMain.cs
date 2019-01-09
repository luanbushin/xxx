using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Noticfacation;
using Game;

public class GameMain : MonoNotice{
    public static GameMain Instance;

    public GameObject player;
    public GameObject overPanel;
    private Dictionary<Vector3, Dictionary<Vector3, PatrolData>> patrolObject = new Dictionary<Vector3, Dictionary<Vector3, PatrolData>>();



    private PassInfo curPassInfo;

    void Awake()
    {
        Application.targetFrameRate = 30;//此处限定60帧
        Instance = this;
    } 
    void Start () {
        overPanel.SetActive(false);


        initBaseLogic();

        gamestrat();
    }

    public void initPlayerPostion(Vector3 v3) {
        player.transform.position = v3;
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
            Destroy(player);
        });

        addListener(Notice.TrapCollision, (string s) =>
        {
            overPanel.SetActive(true);
            Destroy(player);
        });


    }

    public XMLTest xml = new XMLTest();
    private void gamestrat() {
         
        //gameObject.SetActive(false);
        curPassInfo = xml.loadPassInfo();



        gameObject.GetComponent<MapManager>().initmap(xml.LoadXml());
      
        gameObject.GetComponent<MonsterManager>().initMonsterPatrol(patrolObject);
       ;
        gameObject.GetComponent<PassMananger>().initBox(xml.LoadBoxXml());
        gameObject.GetComponent<PassMananger>().initPassInfo(curPassInfo);
        gameObject.GetComponent<TrapManager>().inittrap(curPassInfo.trapList);

        player.SetActive(true);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
