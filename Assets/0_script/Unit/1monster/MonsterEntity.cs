using UnityEngine;
using UnityEditor;

public class MonsterEntity : MonoBehaviour
{
    private Animation anim;
    private GameObject model;
    void Start()
    {




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