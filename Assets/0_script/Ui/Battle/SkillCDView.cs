using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Game.UI.Extension;
using Game.Noticfacation;
using UnityEngine.EventSystems;

namespace Game.UI.Battle
{
    public class SkillCDView : MonoBehaviour, IPointerDownHandler
    {
        public KeyCode slot;
        public CircleImage mask;
        public Text count;
        private RectTransform rt;
        private Vector2 touch_start;

        public float maxCd = 2;

        private float time = 0;

        private bool _is_show = true;

        void Start()
        {
            setShow(false);
            rt = GetComponent<RectTransform>();
        }
        
        void setShow(bool is_show)
        {
            if (_is_show == is_show) return;
            _is_show = is_show;
            mask.gameObject.SetActive(is_show);
            count.gameObject.SetActive(is_show);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (time <= 0)
            {
                Notice.Click_Use_Skill.broadcast(slot.ToString());
                time = maxCd;
            }
        }

        void Update()
        {
            // var host = Character.host;
            //if (host == null) return;
            //var sk = host.cSkillData.findInUIShow(slot);
            //if (sk == null) return;
            if (Input.GetKeyDown(slot)) {
                if (time <= 0)
                {
                    Notice.Click_Use_Skill.broadcast(slot.ToString());
                    time = maxCd;
                }
            }

            if (time > 0) {
                time -= 0.03f;
                bool is_show = time > 0f;
                setShow(is_show);
                if (is_show)
                {
                    mask.setPercent(time / maxCd);
                    if (time < 1) {
                        float num = time * 10;
                        count.text = "0."+((int)num).ToString();
                    }
                    else
                        count.text = ((int)time).ToString();
                }
            }
        }
    }
}
