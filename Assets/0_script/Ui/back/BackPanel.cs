using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BackPanel :
       Game.MonoNotice
{ 

    // Use this for initialization
    public List<Image> ItemList;


    void Start () {
        Transform[] father = GetComponentsInChildren<Transform>();

        ItemList = new List<Image>();
        foreach (var child in father)
        {
            if (child.GetComponent<Image>()) {
                if (child.name != "back") {
                    Debug.Log(child.name);
                    child.gameObject.AddComponent<BackItem>();
                    ItemList.Add(child.GetComponent<Image>());
                }
            }
          
        }
        Debug.Log(ItemList.Count);

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
