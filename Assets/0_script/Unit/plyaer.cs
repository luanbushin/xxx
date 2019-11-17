using Game;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Game.Noticfacation;
using Game.Config;

public class plyaer : MonoNotice
{
    public int TranslateSpeed = 5;
    public int BaseSpeed = 5;
    public int liftHP = 3;
    // Use this for initialization
    public GameObject CreateObject;
    private int playerRotion = 0;

    public GameObject shootObject;
    public int speeduptime;

    public GameObject fanwei;



    private Animation anim;                //Character Animation

    
    private Dictionary<string, Vector3> forceVector = new Dictionary<string, Vector3>();


    void Start()
    {

        //Debug.Log(Game.Shared.HostPlayer.data.attact);
        anim = transform.Find("430").gameObject.GetComponent<Animation>();

        anim.CrossFade("run", 0.08f);

        //gameObject.SetActive(false);




        addListener(Notice.Click_Use_Skill, (id) =>
        {
            if (id == "Space")
            {
                GameObject.Instantiate(CreateObject, new Vector3(Mathf.Round(gameObject.transform.position.x), gameObject.transform.position.y - 0.3f, Mathf.Round(gameObject.transform.position.z)), gameObject.transform.rotation);
            }
            if (id == "L")
            {
                GameObject obj = GameObject.Instantiate(shootObject, gameObject.transform.position, transform.Find("430").transform.rotation);
                anim.CrossFade("skill_q", 0.08f);
                obj.GetComponent<Rigidbody>().AddForce(obj.transform.forward * 1000);
            }
            if (id == "J")
            {
                speeduptime = 100;
            }
            if (id == "I")
            {
                GameObject obj = GameObject.Instantiate(fanwei, gameObject.transform.position, transform.rotation);
                obj.transform.Translate(new Vector3(0, -0.8f, 0));
            }
            if (id == "K")
            {
                float num = Mathf.Cos(Mathf.PI / 4);
                switch (playerRotion + "")
                {
                    case "0":
                        StartCoroutine(CreateExplosions(Vector3.forward));
                        break;
                    case "45":
                        StartCoroutine(CreateExplosions((Vector3.forward + Vector3.right) * num));
                        break;
                    case "90":
                        StartCoroutine(CreateExplosions(Vector3.right));
                        break;
                    case "135":
                        StartCoroutine(CreateExplosions((Vector3.back + Vector3.right) * num));
                        break;
                    case "180":
                        StartCoroutine(CreateExplosions(Vector3.back));
                        break;
                    case "225":
                        StartCoroutine(CreateExplosions((Vector3.back + Vector3.left) * num));
                        break;
                    case "270":
                        StartCoroutine(CreateExplosions(Vector3.left));
                        break;
                    case "315":
                        StartCoroutine(CreateExplosions((Vector3.forward + Vector3.left) * num));
                        break;
                }
            }
        });

    }

    public void pushForceVector(string id,Vector3 v3) {
        forceVector[id] = v3;
    }
    public void removeForceVector(string id)
    {
        forceVector.Remove(id);
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


    private IEnumerator CreateExplosions(Vector3 direction)
    {
        for (int i = 1; i < 4; i++)
        {
            RaycastHit hit;

            Physics.Raycast(transform.position + new Vector3(0.0f, -0.1f, 0.0f), direction, out hit, 1, 1 << 0);

            if (!hit.collider)
            {
                transform.Translate(direction);
            }
            else
            {
                break;
            }
        }

        yield return new WaitForSeconds(.05f);
    }
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
        else if (s) {
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

        if (a) {
            if (w || s) {
                transform.Translate(Vector3.right * Time.deltaTime * (-TranslateSpeed)* num);
            }
            else {
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
        
        foreach (Vector3 v3 in forceVector.Values)
        { 
           GetComponent<Rigidbody>().AddForce(v3); 
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



        if (transform.position.y < -10)
        {
            Notice.TrapCollision.broadcast("");
        }
    }
}
