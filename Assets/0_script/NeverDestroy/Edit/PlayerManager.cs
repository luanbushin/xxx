using UnityEngine;
using System.Collections;
using Game;
using Game.Noticfacation;
using System;

public class PlayerManager : MonoNotice
{
    public static PlayerManager Instance;

    public GameObject player;
    public GameObject backPanel;

    public int TranslateSpeed = 4;
    // Use this for initialization
    void Start()
    {
        Instance = this;
        backPanel.SetActive(false);
        addListener(Notice.CTRL_MOVE, (float dx, float dz, float angle) =>
        {
            onControlMove(dx, dz, angle);
        });
    }

    private void onControlMove(float dx, float dz, float angle)
    {
        angle += 270;

        if (player.GetComponent<plyaer>().curState == 0)
        {
            player.GetComponent<plyaer>().selfrotation = angle;
            player.transform.Find("430").transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
            player.transform.Translate(new Vector3((float)Math.Sin(angle * Math.PI / 180), 0, (float)Math.Cos(angle * Math.PI / 180)) * Time.deltaTime * TranslateSpeed);
            player.GetComponent<plyaer>().anim.CrossFade("run", 0.08f);
            player.GetComponent<plyaer>().ismove = true;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
