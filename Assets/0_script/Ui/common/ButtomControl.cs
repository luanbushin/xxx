using Game.Noticfacation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtomControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
        this.GetComponent<Button>().onClick.AddListener(onClick);

    }

    private void onClick(){
        Notice.Click_Use_Skill.broadcast(KeyCode.E.ToString());
    }

    // Update is called once per frame
    void Update () {
		
	}
}
