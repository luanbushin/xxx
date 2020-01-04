using UnityEngine;
using UnityEditor;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using Game.UI.Battle;

public class BackItem :
       Game.MonoNotice,
       IDragHandler,
       IPointerDownHandler,
       IPointerUpHandler
{
    private Vector2 pos = Vector2.zero;
    private GameObject obj;
    public void OnDrag(PointerEventData eventData)
    {
        obj.GetComponent<RectTransform>().position = eventData.position - new Vector2(60,60);
        obj.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
        //throw new System.NotImplementedException();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
        obj = GameObject.Instantiate(gameObject, Vector3.zero, transform.rotation);
        obj.transform.SetParent(transform.parent);
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Destroy(obj);

        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = eventData.position;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        for (int i = 0; i < results.Count; i++) {
            if (results[i].gameObject.GetComponent<SkillCDView>()) {

            }
            //Debug.Log(results[i].gameObject);

        }


        //EventSystem.current.IsPointerOverGameObject()
        //Debug.Log("event==========" + GameObject.Find("K"));
       
        //throw new System.NotImplementedException();
    }
}