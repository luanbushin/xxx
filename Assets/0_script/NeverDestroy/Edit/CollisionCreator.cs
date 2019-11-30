using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CollisionCreator
{
    public static CollisionCreator Instance;

    public CollisionCreator() {
        Instance = this;
    }

    public GameObject creatCube(Transform _tf,Vector3 sizev3, Vector3 loctionv3) {
        GameObject collision;
        collision = GameObject.CreatePrimitive(PrimitiveType.Cube);
        collision.transform.localScale = sizev3;
        collision.transform.position = _tf.position + loctionv3;
        collision.transform.SetParent(_tf);
        collision.SetActive(false);
        collision.GetComponent<BoxCollider>().isTrigger = true;
        collision.AddComponent<CollisionLogic>();
        collision.GetComponent<MeshRenderer>().enabled = false;

        return collision;
    }
}
