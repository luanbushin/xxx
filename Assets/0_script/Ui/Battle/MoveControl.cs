using Game;
using Game.Noticfacation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets._0_script.Ui.Battle
{
    public class MoveControl :
       MonoNotice,
       IDragHandler,
       IPointerDownHandler,
       IPointerUpHandler
    {
        public RectTransform DetectArea;
        public RectTransform Center;
        public RectTransform JoyStick;
        public RectTransform Direction;

        public float Radius;

        private bool active = false;
        private Vector2 pos = Vector2.zero;
        private Vector3 ang = Vector3.zero;

        private void act(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                DetectArea,
                eventData.position,
                eventData.pressEventCamera,
                out pos);
            pos -= Center.anchoredPosition;

            var normalized = pos.normalized;

            if (pos.magnitude > Radius)
                pos = normalized * Radius;
            JoyStick.anchoredPosition = pos + Center.anchoredPosition;
            ang.z = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg;

            pos = normalized;
        }

        public void OnDrag(PointerEventData eventData)
        {
            Direction.gameObject.SetActive(true);
            act(eventData);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                DetectArea,
                eventData.position,
                eventData.pressEventCamera,
                out pos);
            Center.anchoredPosition = pos;

            act(eventData);
            active = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            JoyStick.anchoredPosition = Center.anchoredPosition;
            active = false;
            Direction.gameObject.SetActive(false);
        }

        void Start()
        {
            JoyStick.anchoredPosition = Center.anchoredPosition;
            Direction.gameObject.SetActive(false);
        }

        void Update()
        {
            if (!active) return;
            var ang = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg;
            Direction.eulerAngles = new Vector3(0f, 0f, ang);

            //Debug.Log(Direction.eulerAngles);
            Notice.CTRL_MOVE.broadcast(pos.x, pos.y, Mathf.Atan2(pos.y, -pos.x) * Mathf.Rad2Deg);
            //Debug.Log(pos.x+"======="+pos.y);
        }
    }
}
