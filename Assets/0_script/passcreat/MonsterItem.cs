using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Game.Config;
using UnityEngine;
using UnityEngine.UI;
using Game.Noticfacation;

public class MonsterItem : MonoBehaviour {

    // Use this for initialization
    public MonsterData data;
    public Button btn;
    private MonsterPresetDataValueData value;
    private Vector3 v3;
    void Start () {
        btn.onClick.AddListener(onClick);

    }

    public void setData(MonsterPresetDataValueData data,Vector3 v3)
    {
        value = data;
        Text text = btn.transform.Find("Text").GetComponent<Text>();
        text.text = data.NAME + "   " + v3.ToString();
        this.v3 = v3;
    }

    private void onClick()
    {
        Notice.CLICK_MONSTER.broadcast(v3);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
