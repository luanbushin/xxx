using UnityEngine;
using System.Collections;
using Game.Global;
using Game.Noticfacation;
using System.Collections.Generic;

public class TrapCollisoon : MonoBehaviour
{
    public int id;
    public int type;
    public string use;

    public List<Vector3> list;

    // Use this for initialization
    void Start()
    {

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "pohuai")
        {
            string str = id + "=" + type + "=";
            if (type == 1)
            {
                if (use == "clear")
                {
                    str += "open";
                }
            }
            Notice.TrapCtrl_Vector.broadcast(str, list);
        }
        else if (other.gameObject.tag == "Player") {
            string str = id + "=" + type + "=";
            if (type == 4)
            {
                str += "remove";
                Notice.TrapCtrl.broadcast(str);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.tag);
        if (type == 4) {
            if (list == null) {
                list = new List<Vector3>();
            }
            list.Add(other.gameObject.transform.position);
        }

        //Debug.Log(use);
        if (other.gameObject.tag == "Player")
        {
            //other.gameObject.GetComponent<plyaer>().OverPanel.SetActive(true);
            //Destroy(other.gameObject);
            string str = id + "=" + type + "=";
            if (type == 1)
            {
                if (use == "trigger")
                {
                    str += "trigger";
                }
                Notice.TrapCtrl.broadcast(str);
            }
            else if (type == 4)
            {
                str += "add";
                Notice.TrapCtrl.broadcast(str);
                if (use == "atk1")
                    Destroy(this.gameObject);
            }
            else if (type == 5)
            {
                str += transform.position;
                Notice.TrapCtrl.broadcast(str);
            }
            else if (type == 6)
            {
                if (use == "trigger")
                {
                    str += "trigger";
                }
                else if (use == "atk")
                {
                    str += "atk";
                }
                Notice.TrapCtrl.broadcast(str);
            }
            else if (type == 7) {
                if (use == "trigger")
                {
                    str += "trigger";
                }
                else if (use == "atk")
                {
                    str += "atk";
                }
                Notice.TrapCtrl.broadcast(str);
            }
        }
        else if (other.gameObject.tag == "pohuai")
        {
            string str = id + "=" + type + "=";
            if (type == 1)
            {
                if (use == "clear")
                {
                    str += "close";
                }
                Notice.TrapCtrl.broadcast(str);
            }


        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
