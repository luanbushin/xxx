using UnityEngine;
using System.Collections;

public class EnemyAutoAi : MonoBehaviour
{
    private int state;
    private Animation anim;

    // Use this for initialization
    void Start()
    {
        anim = gameObject.GetComponent<Animation>();
        anim.CrossFade("Idle", 0.08f);
        state = 0;
    }

    private void findtarget() {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
