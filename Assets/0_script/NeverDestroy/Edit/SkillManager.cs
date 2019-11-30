using UnityEngine;
using System.Collections;
using Game;
using Game.Noticfacation;

public class SkillManager : MonoNotice
{
    public static SkillManager Instance;

    public plyaer player;

    private void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start()
    {
        player = GameMain.Instance.player.GetComponent<plyaer>();
        addListener(Notice.Click_Use_Skill, (id) =>
        {
            if (id == "Space")
            {
                //Debug.Log(1111111);
                //useSkillByData();
                GameObject obj = GameObject.Instantiate(player.shootObject, player.transform.position, player.transform.Find("430").transform.rotation);
                obj.GetComponent<ShootCollisoon>().selfObj = player.gameObject;
                player.anim.CrossFade("atk", 0.08f);
                obj.GetComponent<Rigidbody>().AddForce(obj.transform.forward * 1000);
                player.setPlaterState(1, 0.5f, "idle");
            }
            if (id == "J")
            {
                //speeduptime = 100;
                float num = Mathf.Cos(Mathf.PI / 4);
                switch (player.playerRotion + "")
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
            if (id == "L")
            {

            }
            if (id == "I")
            {
                GameObject obj = GameObject.Instantiate(player.fanwei, player.transform.position, transform.rotation);
                obj.transform.Translate(new Vector3(0, -0.8f, 0));
            }
            if (id == "K")
            {
      
            }
        });
    }

    public void useSkillByData(SkillData data) {
        switch (data.id) {
            case 10001:
                Vector3 v3 = new Vector3(Mathf.Round(gameObject.transform.position.x), gameObject.transform.position.y - 0.3f, Mathf.Round(gameObject.transform.position.z));
                skill10001(v3);
                break;
            case 10002:

                break;
            case 10003:

                break;
            case 10004:

                break;
            case 10005:

                break;
            case 10006:

                break;
            case 10007:

                break;
            case 10008:

                break;
        }
    }

    public void skill10001(Vector3 v3) {
        //transform.rotation
        //new Vector3(Mathf.Round(gameObject.transform.position.x), gameObject.transform.position.y - 0.3f, Mathf.Round(gameObject.transform.position.z))
        GameObject.Instantiate(player.CreateObject,v3, new Quaternion(0,0,0,1));
    }



    private IEnumerator CreateExplosions(Vector3 direction)
    {
        for (int i = 1; i < 4; i++)
        {
            RaycastHit hit;

            Physics.Raycast(player.transform.position + new Vector3(0.0f, -0.1f, 0.0f), direction, out hit, 1, 1 << 0);

            if (!hit.collider)
            {
                player.transform.Translate(direction);
            }
            else
            {
                break;
            }
        }

        yield return new WaitForSeconds(.05f);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
