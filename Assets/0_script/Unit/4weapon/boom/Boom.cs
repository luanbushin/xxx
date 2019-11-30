using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom : MonoBehaviour {

    // Use this for initialization

    public GameObject leftcollsion;
    public float difference = 0;


    void Start() {
        Invoke("detonateBoom",3f);
    }

    private void Update()
    {
        //Debug.DrawLine(transform.position + new Vector3(0, .2f, 0), transform.position + new Vector3(10, .2f, 0), Color.red);
    }

    public void detonateBoom() {
        //  GameObject obj = GameObject.Instantiate(leftcollsion,gameObject.transform.position, gameObject.transform.rotation);
        Instantiate(leftcollsion, transform.position + new Vector3(0, difference, 0), Quaternion.identity);

        StartCoroutine(CreateExplosions(Vector3.forward));
        StartCoroutine(CreateExplosions(Vector3.right));
        StartCoroutine(CreateExplosions(Vector3.back));
        StartCoroutine(CreateExplosions(Vector3.left));

        // Debug.DrawLine(gameObject.transform.position, new Vector3(gameObject.transform.position.x-5, gameObject.transform.position.y, gameObject.transform.position.z), Color.red);
     


        Destroy(this.gameObject);
    }



    private IEnumerator CreateExplosions(Vector3 direction)
    {
        for (int i = 1; i < 3; i++)
        {
            RaycastHit hit;
            
            Physics.Raycast(transform.position + new Vector3(0, .3f, 0), direction, out hit, i,1<<0);



            if (!hit.collider)
            {
                Instantiate(leftcollsion, transform.position + (i * direction) + new Vector3(0, difference, 0), leftcollsion.transform.rotation);
            }
            else
            {
                if (hit.collider.gameObject.tag == "Player")
                {
                    Instantiate(leftcollsion, transform.position + (i * direction) + new Vector3(0, difference, 0), leftcollsion.transform.rotation);
                }
                else if (hit.collider.gameObject.tag == "boom")
                {
                    Instantiate(leftcollsion, transform.position + (i * direction) + new Vector3(0, difference, 0), leftcollsion.transform.rotation);
                }
                else if (hit.collider.gameObject.tag == "pohuai")
                {
                    Instantiate(leftcollsion, transform.position + (i * direction) + new Vector3(0, difference, 0), leftcollsion.transform.rotation);
                    Destroy(hit.collider.gameObject);
                }
                else if (hit.collider.gameObject.GetComponent<MonsterEntity>())
                {
                    Instantiate(leftcollsion, transform.position + (i * direction) + new Vector3(0, difference, 0), leftcollsion.transform.rotation);
                    Destroy(hit.collider.gameObject);
                }
                else if (hit.collider.gameObject.tag == "diaoluo")
                {
                    Instantiate(leftcollsion, transform.position + (i * direction) + new Vector3(0, difference, 0), leftcollsion.transform.rotation);
                }
                break;
            }
        }
        yield return new WaitForSeconds(.05f);
    }
    
}
