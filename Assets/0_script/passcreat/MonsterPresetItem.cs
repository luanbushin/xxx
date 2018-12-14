using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Config;
using UnityEngine.UI;
using System;
using Game.Noticfacation;

public class MonsterPresetItem : MonoBehaviour {

    public Button btn;
    public MonsterPresetDataValueData value;
    void Start () {
        btn.onClick.AddListener(onBuildClick);
    }

    public void setData(MonsterPresetDataValueData data)
    {
        value = data;
        Text text = btn.transform.Find("Text").GetComponent<Text>();
        text.text = data.NAME;
    }

    public void setTrapData(string trapname) {
        Text text = btn.transform.Find("Text").GetComponent<Text>();
        text.text = trapname;
    }

    private void onBuildClick()
    {
        if(value == null)
            Notice.CREAT_Trap.broadcast(btn.transform.Find("Text").GetComponent<Text>().text);
        else
            Notice.CREAT_MONSTER.broadcast(value.ID);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
