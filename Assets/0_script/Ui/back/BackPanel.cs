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
                if (child.name == "closebtn") {
                    child.gameObject.GetComponent<Button>().onClick.AddListener(onClose);
                }
                else if (child.name != "back"&& child.name != "buttonnme") {
                    //Debug.Log(child.transform.Find("RawImage").GetComponent<RawImage>().texture);
                    child.gameObject.AddComponent<BackItem>();

                    //Debug.Log(child.gameObject.GetComponent<RawImage>().texture);
                    child.transform.Find("RawImage").GetComponent<RawImage>().texture = (Texture2D)Resources.Load("Icon/prop/none");
                    ItemList.Add(child.GetComponent<Image>());
                }
            }
          
        }
        //Debug.Log(ItemList.Count);

    }

    void onClose() {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
