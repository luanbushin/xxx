using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassMananger : MonoBehaviour {
    public static PassMananger Instance;


    private Dictionary<Vector3, GameObject> boxObject = new Dictionary<Vector3, GameObject>();
    public GameObject boxparent;
    public GameObject boxGameObject;

    public GameObject savePoint;

    public PassInfo passInfo;
    private void Awake()
    {
        Instance = this;
    }

    void Start() {
        
    }

    public void initPassInfo(PassInfo info){
        int random = Random.Range(0, info.bornList.Count);
        gameObject.GetComponent<GameMain>().player.transform.position = info.bornList[random];
    }
    public void initBox(Dictionary<Vector3, int> boxlist)
    {
        foreach (Vector3 list in boxlist.Keys)
        {
            GameObject obj = GameObject.Instantiate(boxGameObject, list, gameObject.transform.rotation);
            boxObject[obj.transform.position] = obj;
            obj.transform.SetParent(boxparent.transform);
        }
    }

	
	// Update is called once per frame
	void Update () {
		
	}
}
