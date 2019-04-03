using UnityEngine;
using System.Collections;

public class EnemyAutoAi : MonoBehaviour
{
    private int state = 1;
    private Animation anim;

    public Rect roomrect;
    public Vector3 target;
    private Vector3[] targetList;
    private int curTargetIndex;

    public int TranslateSpeed = 5;

    // Use this for initialization
    void Start()
    {
        anim = gameObject.GetComponent<Animation>();
        anim.CrossFade("Idle", 0.08f);

        setTargetList();
    }

    private void setTargetList() {
        targetList = new Vector3[4];
        targetList[0] = new Vector3(roomrect.x, 1, roomrect.y);
        targetList[1] = new Vector3(roomrect.x + roomrect.width, 1, roomrect.y);
        targetList[2] = new Vector3(roomrect.x + roomrect.width, 1, roomrect.y+ roomrect.height);
        targetList[3] = new Vector3(roomrect.x, 1, roomrect.y + roomrect.height);
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

    private void walk(){
        float distance = Vector3.Distance(target, transform.position);
        if (distance < 1) {
            findtarget();
            return;
        }
        Vector3 move3 = target - this.transform.position;
        transform.Translate(move3.normalized * Time.deltaTime * TranslateSpeed);

        anim.CrossFade("RunRight", 0.08f);
    }

    //private move

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
