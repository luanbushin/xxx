using UnityEngine;

public class MonsterEntity : MonoBehaviour
{
    public Animation anim;
    private GameObject model;

    public GameObject target;
    void Start()
    {
        Debug.Log(GameMain.Instance.player);


    }

    public int state;

    public bool chackAttack()
    {
        if (Vector3.Distance(transform.position, target.transform.position) < 7)
        {
            return true;
        }
        else {
            return false;
        }
    }


    public void loadModel(string str)
    {
        model = Instantiate(MonsterManager.Instance.getWarriorMonster(str), transform.position, Quaternion.identity) as GameObject;
        model.transform.SetParent(transform);
        anim = model.GetComponent<Animation>();
        anim.CrossFade("Idle", 0.08f);
    }

    void Update()
    {

    }
}