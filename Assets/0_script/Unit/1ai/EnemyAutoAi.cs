using UnityEngine;
using System.Collections;

public class EnemyAutoAi : MonoBehaviour
{
    private int state = -1;
    private Animation anim;

    public Rect roomrect;
    public Vector3 target;
    private Vector3[] targetList;
    private int curTargetIndex;

    private GameObject model;

    private bool arrivetarget;

    public int TranslateSpeed = 3;

    // Use this for initialization
    void Start()
    {
        loadModel();



    }

    private void loadModel() {
        model = Instantiate(MonsterManager.Instance.getWarriorMonster(),transform.position, Quaternion.identity) as GameObject;
        model.transform.SetParent(transform);
        anim = model.GetComponent<Animation>();
        anim.CrossFade("Idle", 0.08f);
        setTargetList();

    }

    private void setTargetList() {
        targetList = new Vector3[4];
        targetList[0] = new Vector3(roomrect.x, 1, roomrect.y);
        targetList[1] = new Vector3(roomrect.x + roomrect.width-1, 1, roomrect.y);
        targetList[2] = new Vector3(roomrect.x + roomrect.width-1, 1, roomrect.y + roomrect.height-1);
        targetList[3] = new Vector3(roomrect.x, 1, roomrect.y + roomrect.height-1);
        curTargetIndex = 0;

        state = 0;
    }

    private void findtarget() {
        state = 1;
        target = targetList[curTargetIndex];
        curTargetIndex++;
        if (curTargetIndex == targetList.Length)
            curTargetIndex = 0;
    }

    private void walk() {
        float distance = Vector3.Distance(target, transform.position);
        //Debug.Log(distance);
        if (arrivetarget) {
            transform.position = target;
            findtarget();
            arrivetarget = false;
            return;
        }
        if (distance < 0.1)
        {
            arrivetarget = true;
            return;
        }
        else {
            arrivetarget = false;
        }
        Vector3 move3 = target - this.transform.position;
  

        moveDirection(move3.normalized);
    }

    private void moveDirection(Vector3 v3){
        model.transform.LookAt(new Vector3(v3.x+transform.position.x,v3.y + transform.position.y, v3.z+transform.position.z));
        transform.Translate(v3 * Time.deltaTime * TranslateSpeed);
        anim.CrossFade("RunRight", 0.08f);
    }

    // Update is called once per frame
    void Update()
    {
        if (state == 0)
        {
            findtarget();
        }
        else {
            walk();
        }

    }
}
