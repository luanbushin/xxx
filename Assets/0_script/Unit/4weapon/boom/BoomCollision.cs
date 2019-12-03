using UnityEngine;
using Game.Global;
using Game.Noticfacation;

public class BoomCollision : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "boom")
        {
            other.gameObject.GetComponent<Boom>().detonateBoom();
        }
        else if (other.gameObject.tag == "Player")
        {
            //other.gameObject.GetComponent<plyaer>().OverPanel.SetActive(true);
            //Destroy(other.gameObject);
            Notice.WeaponCollision.broadcast("", other.gameObject);
        }
        else if (other.gameObject.tag == "pohuai")
        {
            if(other.gameObject.GetComponent<EnemyAI>())
                Destroy(other.gameObject.GetComponent<EnemyAI>().enemyview);
            Destroy(other.gameObject);
        }
        else if (other.gameObject.tag == "diaoluo") {
            Vector3 v3 = other.gameObject.transform.position;
            Assets.instance.loadPrefab(
            "prop/cube",
            (GameObject object_, string name) =>
            {
                object_.transform.position = v3;
            });
            Destroy(other.gameObject);
        }
    }
    void Start()
    {
       Invoke("finish", .5f);
    }

    private void finish() {
        Destroy(this.gameObject);
    }

}