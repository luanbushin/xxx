using UnityEngine;
using System.Collections;
using Game.Noticfacation;

public class PropObject : MonoBehaviour
{
    public GameObject model;
    // Use this for initialization
    void Start()
    {

    }

    public void initPropData(string str) {
        Debug.Log(transform);
        model = Instantiate(PropManager.Instance.getPropModel("map/Prefabs/Props_C22_6"), transform.position, Quaternion.identity) as GameObject;
        model.transform.SetParent(transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
            Notice.GetProp.broadcast("", transform.gameObject,other.gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
