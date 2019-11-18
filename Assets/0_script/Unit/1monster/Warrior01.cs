using UnityEngine;
using System.Collections;

public class Warrior01 : MonsterEntity
{

    // Use this for initialization
    void Start()
    {
        int random = UnityEngine.Random.Range(1, 3);
        loadModel("Warrior_0" + random);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
