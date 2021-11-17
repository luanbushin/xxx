using UnityEngine;
using System.Collections;
using Game;
using Game.Noticfacation;
using Game.Config;

public class PropManager : MonoNotice
{
    public static PropManager Instance;
    public GameObject propObj;
    // Use this for initialization

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {

        addListener(Notice.CreatProp, (Vector3 v3,string s) =>
        {
            GameObject obj = GameObject.Instantiate(propObj, v3, propObj.transform.rotation);
            obj.AddComponent<PropObject>().initPropData("");
            //obj.transform.SetParent(gameo.transform);
            //obj.GetComponent<EnemyAI>().initMonsterValue(MonsterPresetData.get(1));
            //enemyObject[obj.transform.position] = obj;
        });
        addListener(Notice.GetProp, (string s,GameObject g,GameObject t) =>
        {
            Destroy(g);
        });
    }

    public void initProp() {

        Debug.Log(PropData.get(1).image);
    }


    public GameObject getPropModel(string str)
    {
        //return (GameObject)Resources.Load("enemy/"+str);.
        return (GameObject)Resources.Load(str);
    }


    // Update is called once per frame
    void Update()
    {
    }
}
