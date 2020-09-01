﻿using Model;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Hotfix
{
    public class Joystack : MonoBehaviour
    {
        public static Joystack instance;
        //最大拖动距离
        public float maxDistance;
        //激活移动的最低距离
        public float activeDistance;
        public Vector3 origin;
        public float distance;
        //标准化移动的距离
        public Vector2 normalDistance;
        public bool drag;
        public float angle;
        //标准操作
        public bool standard;

        public GameObject joystack;
        public GameObject joystackBG;
        void Awake()
        {
            instance = this;
        }
        void Start()
        {
        }
        void Update()
        {
            //根据触摸位置算出来的拖动距离
            distance = Vector3.Distance(joystack.transform.localPosition, origin);
            //距离大于最大拖动距离
            if (distance >= maxDistance)
            {
                //求圆上的一点 (目标点-原点) * 半径 / 原点到目标点的距离
                joystack.transform.localPosition = origin + (joystack.transform.localPosition - origin) * maxDistance / distance;
            }
            //距离大于激活移动的最低距离
            if (distance >= activeDistance)
            {
                normalDistance = (joystack.transform.localPosition - origin).normalized;
            }
            else
            {
                normalDistance = Vector2.zero;
            }
            angle = Mathf.Atan2(normalDistance.x, normalDistance.y) * Mathf.Rad2Deg;
        }
        public void InitJoystack(float maxDistance, float activeDistance, GameObject joystack, GameObject joystackBG)
        {
            this.maxDistance = maxDistance;
            this.activeDistance = activeDistance;
            this.joystack = joystack;
            this.joystackBG = joystackBG;
            //设置原点
            origin = joystack.transform.localPosition;
            Drag.GetDrag(gameObject).down = OnDown;
            Drag.GetDrag(gameObject).up = OnUp;
            Drag.GetDrag(gameObject).beginDrag = OnBeginDrag;
            Drag.GetDrag(gameObject).drag = OnDrag;
            Drag.GetDrag(gameObject).endDrag = OnEndDrag;
            joystack.SetActive(false);
            joystackBG.SetActive(false);
        }
        public void OnDown(PointerEventData eventData)
        {
            if (Input.touchCount == 1)
            {
                standard = true;
            }
            if (Input.touchCount == 2)
            {
                //找到最左边的手指
                int left;
                float point1 = Input.GetTouch(0).position.x;
                float point2 = Input.GetTouch(1).position.x;
                if (point1 < point2)
                {
                    left = 0;
                }
                else
                {
                    left = 1;
                }
                //如果最左边的手指等于摇杆触发位置的话就是常规操作 否则是非常规操作
                if (Input.GetTouch(left).position == eventData.position)
                {
                    standard = true;
                }
            }
            RectTransform rect = (RectTransform)joystackBG.transform;
            rect.localPosition = GameUtil.ScreenToUgui((RectTransform)transform, eventData.position);
            joystack.SetActive(true);
            joystackBG.SetActive(true);
        }
        public void OnUp(PointerEventData eventData)
        {
            standard = false;
            joystack.SetActive(false);
            joystackBG.SetActive(false);
        }
        public void OnBeginDrag()
        {
        }
        public void OnDrag(Vector2 delta)
        {
            if (!drag)
            {
                drag = true;
            }
            joystack.transform.localPosition += new Vector3(delta.x, delta.y, 0);
        }
        public void OnEndDrag()
        {
            drag = false;
            joystack.transform.localPosition = origin;
        }
    }
}