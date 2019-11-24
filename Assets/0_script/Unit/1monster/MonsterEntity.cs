using UnityEngine;

public class MonsterEntity : MonoBehaviour
{
    public Animation anim;
    public GameObject model;

    public string animAction = "Idle";

    public GameObject target;
    void Start()
    {
        Debug.Log(GameMain.Instance.player);


    }

    public void backatk()
    {
        state = 0;
        anim.CrossFade(animAction, 0.08f);
    }


    public int state;

    public bool chackAttack()
    {
        if (Vector3.Distance(transform.position, target.transform.position) < 1.3f)
        {
            return true;
        }
        else {
            return false;
        }
    }

    public bool chackAttackRange() {
        if (Vector3.Distance(transform.position, target.transform.position) < 7)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public void loadModel(string str)
    {
        model = Instantiate(MonsterManager.Instance.getWarriorMonster(str), transform.position, Quaternion.identity) as GameObject;
        model.transform.SetParent(transform);
        anim = model.GetComponent<Animation>();
        //anim.CrossFade("Idle", 0.08f);
    }

    void Update()
    {

    }
}