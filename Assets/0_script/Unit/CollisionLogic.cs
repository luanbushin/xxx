

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
        if (other.gameObject.tag == "Player")
            Notice.MonsterATK.broadcast("", other.gameObject, this.transform.parent.gameObject);
        else if (other.gameObject.GetComponent<MonsterEntity>()) {
            Notice.MonsterBeATK.broadcast("", other.gameObject);
        }
    }
}
