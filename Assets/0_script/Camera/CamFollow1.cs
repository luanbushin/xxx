using System.Collections;
using System.Collections.Generic;
using UnityEngine;

  public class CamFollow1 : MonoBehaviour
  {
  
      private Vector3 offset;
      public Transform player;
  
      void Start()
      {
          offset = player.position - transform.position;
     }
 
     void Update()
     {
        if(player)
         transform.position = Vector3.Lerp(transform.position, player.position - offset,Time.deltaTime*5);
     }
 }