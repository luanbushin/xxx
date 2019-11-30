using Game;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Game.Noticfacation;
using Game.Config;
using UnityEngine.UI;

public class plyaer : MonoNotice
{
    public int TranslateSpeed = 4;
    public int BaseSpeed = 5;
    public int liftHP = 3;
    // Use this for initialization
    public GameObject CreateObject;
    public int playerRotion = 0;

    public GameObject shootObject;
    public int speeduptime;

    public GameObject fanwei;

    public int curhp = 100;
    public int maxhp = 100;
    public Text hptxt;
    public Image hpbar;



    public Animation anim;                //Character Animation


    public int curState = 0;

    
    private Dictionary<string, Vector3> forceVector = new Dictionary<string, Vector3>();


    void Start()
    {

        //Debug.Log(Game.Shared.HostPlayer.data.attact);
        anim = transform.Find("430").gameObject.GetComponent<Animation>();

        anim.CrossFade("run", 0.08f);

        //gameObject.SetActive(false);

    }

    public void initHp(int num) {
        curhp += num;
        hptxt.text = curhp + "/" + maxhp;
        hpbar.GetComponent<RectTransform>().sizeDelta = new Vector2(356* curhp / maxhp, 14);
    }

    public void pushForceVector(string id,Vector3 v3) {
        forceVector[id] = v3;
    }
    public void removeForceVector(string id)
    {
        forceVector.Remove(id);
    }


    public void setPlaterState(int state,float time,string str) {
        curState = state;

        Invoke("backToState", time);
    }

    public void backToState() {
        curState = 0;
        anim.CrossFade("idle", 0.08f);
    }

    //public void boomState() {
    //    if (target) {
    //        if (Mathf.Abs(target.transform.position.x - transform.position.x) < 10 && Mathf.Abs(target.transform.position.z - transform.position.z) < 10)
    //        {
    //            RaycastHit hit;

    //            Physics.Raycast(transform.position + new Vector3(0, .3f, 0), target.transform.position - (transform.position + new Vector3(0, .3f, 0)), out hit, 500f, Physics.AllLayers);
    //            Debug.DrawLine(transform.position + new Vector3(0, .3f, 0), target.transform.position, Color.red);
    //            if (!hit.collider)
    //            {
    //                target.GetComponent<MeshRenderer>().enabled = false;
    //            }
    //            else
    //            {

    //                if (hit.collider.gameObject.tag == "boom")
    //                {
    //                    target.GetComponent<MeshRenderer>().enabled = true;
    //                    Debug.Log(hit.collider.gameObject.tag);
    //                }
    //                else
    //                    target.GetComponent<MeshRenderer>().enabled = false;
    //            }

    //        }
    //        else
    //        {
    //            target.GetComponent<MeshRenderer>().enabled = false;
    //        }
    //    }
    //}



            // Update is called once per frame
    void Update () {
        bool w = false;
        bool s = false;
        bool a = false;
        bool d = false;
        //如果你按下了W键
        if (Input.GetKey(KeyCode.W))
        {
            //transform.Translate(Vector3.forward * Time.deltaTime * TranslateSpeed);
            //transform.Find("430").transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            w = true;
        }
        else if (Input.GetKey(KeyCode.S))
        {
           // transform.Translate(Vector3.forward * Time.deltaTime * (-TranslateSpeed));
           // transform.Find("430").transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            s = true;
        }
        //如果你按下了A键
        if (Input.GetKey(KeyCode.A))
        {
         //   transform.Translate(Vector3.right * Time.deltaTime * (-TranslateSpeed));
            //transform.Find("430").transform.rotation = Quaternion.Euler(new Vector3(0, 270, 0));
            a = true;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            //transform.Translate(Vector3.right * Time.deltaTime * TranslateSpeed);
            //transform.Find("430").transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
            d = true;
        }
        TranslateSpeed = BaseSpeed;
        if (speeduptime > 0 && (w||s||a||d))
        {
            speeduptime--;
            TranslateSpeed = BaseSpeed +3;
            GetComponent<GhostShadow>().enabled = true;
        }
        else {
            GetComponent<GhostShadow>().enabled = false;
        }


        float num = Mathf.Cos(Mathf.PI / 4);


        
        
        foreach (Vector3 v3 in forceVector.Values)
        { 
           GetComponent<Rigidbody>().AddForce(v3); 
        }


        if (curState == 0) {

            if (w)
            {
                if (a)
                {
                    //GetComponent<Rigidbody>().velocity = Vector3.forward * Time.deltaTime * TranslateSpeed * num;
                    transform.Translate(Vector3.forward * Time.deltaTime * TranslateSpeed * num);
                    playerRotion = 315;
                    //transform.Find("430").transform.rotation = Quaternion.Euler(new Vector3(0, 315, 0));
                }
                else if (d)
                {
                    transform.Translate(Vector3.forward * Time.deltaTime * TranslateSpeed * num);
                    playerRotion = 45;
                    //transform.Find("430").transform.rotation = Quaternion.Euler(new Vector3(0, 45, 0));
                }
                else
                {
                    transform.Translate(Vector3.forward * Time.deltaTime * TranslateSpeed);
                    playerRotion = 0;
                    // transform.Find("430").transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                }
            }
            else if (s)
            {
                if (a)
                {
                    transform.Translate(Vector3.forward * Time.deltaTime * (-TranslateSpeed) * num);
                    playerRotion = 225;
                    //transform.Find("430").transform.rotation = Quaternion.Euler(new Vector3(0, 225, 0));
                }
                else if (d)
                {
                    transform.Translate(Vector3.forward * Time.deltaTime * (-TranslateSpeed) * num);
                    playerRotion = 135;
                    //transform.Find("430").transform.rotation = Quaternion.Euler(new Vector3(0, 135, 0));
                }
                else
                {
                    transform.Translate(Vector3.forward * Time.deltaTime * (-TranslateSpeed));
                    playerRotion = 180;
                    //transform.Find("430").transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
                }
            }

            if (a)
            {
                if (w || s)
                {
                    transform.Translate(Vector3.right * Time.deltaTime * (-TranslateSpeed) * num);
                }
                else
                {
                    playerRotion = 270;
                    //transform.Find("430").transform.rotation = Quaternion.Euler(new Vector3(0, 270, 0));
                    transform.Translate(Vector3.right * Time.deltaTime * (-TranslateSpeed));
                }
            }
            else if (d)
            {
                if (w || s)
                {
                    transform.Translate(Vector3.right * Time.deltaTime * TranslateSpeed * num);
                }
                else
                {
                    playerRotion = 90;
                    //transform.Find("430").transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
                    transform.Translate(Vector3.right * Time.deltaTime * TranslateSpeed);
                }
            }


            if (a || d || w || s)
            {
                transform.Find("430").transform.rotation = Quaternion.Euler(new Vector3(0, playerRotion, 0));
                anim.CrossFade("run", 0.08f);
            }
            else
            {
                anim.CrossFade("idle", 0.08f);
            }
        }




        if (transform.position.y < -10)
        {
            Notice.TrapCollision.broadcast("");
        }
    }
}
