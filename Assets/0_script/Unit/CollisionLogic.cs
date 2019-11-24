

using Game.Noticfacation;
using UnityEngine;

public class CollisionLogic : MonoBehaviour
{
    void Start()
    {
    }

    public void init(string id) {
        //Debug.Log(id);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.tag == "Player")
            Notice.MonsterATK.broadcast("", other.gameObject);
    }
}
