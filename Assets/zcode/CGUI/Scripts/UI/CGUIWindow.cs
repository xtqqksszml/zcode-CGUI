/***************************************************************
 * Copyright 2016 By Zhang Minglin
 * Author: Zhang Minglin
 * Create: 2015/11/16
 * Note  : GUI窗口基类
***************************************************************/
using UnityEngine;
using System.Collections;

namespace CGUI
{
    /// <summary>
    /// 
    /// </summary>
    [RequireComponent(typeof(UIPanel))]
    public class CGUIWindow : MonoBehaviour
    {
        /// <summary>
        /// 显示
        /// </summary>
        public bool Visible { get; private set; }

        /// <summary>
        ///   位置
        /// </summary>
        public Vector3 Position
        {
            get
            {
                if (HidePlan == CGUI.WindowHidePlan.OutSide)
                {
                    if (Visible)
                    {
                        return new Vector3(transform.localPosition.x
                        , transform.localPosition.y
                        , transform.localPosition.z);
                    }
                    else
                    {
                        return new Vector3(transform.localPosition.x + CGUI.Constant.WINDOW_OUTSIDE_OFFSET
                        , transform.localPosition.y
                        , transform.localPosition.z);
                    }
                }

                return transform.localPosition;
            }
            set
            {
                if (HidePlan == CGUI.WindowHidePlan.OutSide)
                {
                    if (Visible)
                    {
                        transform.localPosition = value;
                    }
                    else
                    {
                        transform.localPosition = new Vector3(value.x + CGUI.Constant.WINDOW_OUTSIDE_OFFSET
                                                            , value.y
                                                            , value.z);
                    }
                }
                else
                {
                    transform.localPosition = value;
                }
            }
        }

        /// <summary>
        /// 界面类型
        /// 派生的界面，有需要可以重写这个属性
        /// </summary>
        public virtual CGUI.WindowType WindowType
        {
            get { return CGUI.WindowType.Normal; }
        }

        /// <summary>
        /// 界面关闭策略
        /// 派生的界面，有需要可以重写这个属性
        /// </summary>
        public virtual CGUI.WindowHidePlan HidePlan
        {
            get { return CGUI.WindowHidePlan.Delete; }
        }

        /// <summary>
        /// 获得子控件
        /// </summary>
        public CGUIControl this[string name]
        {
            get
            {
                try
                {
                    Transform child = transform.FindChild(name);
                    if (child != null)
                        return new CGUIControl(child.gameObject);
                }
                catch (System.Exception)
                {
                    Debug.Log("Cann't find GuiControl, Name is " + name);
                }

#if UNITY_EDITOR
                return null;
#else
            return CGUIControl.Invalid;
#endif
            }
        }

        /// <summary>
        ///   
        /// </summary>
        protected CGUIWindow()
        {
            //窗口默认为关闭
            Visible = false;
        }

        #region Layer
        /// <summary>
        /// 弹至最上层
        /// </summary>
        public void BringForward()
        {
            CGUIManager.Instance.BringForward(this);
        }

        /// <summary>
        /// 弹至最下层
        /// </summary>
        public void PushBack()
        {
            CGUIManager.Instance.PushBack(this);
        }
        #endregion

        #region Show/Hide
        /// <summary>
        ///   显示
        /// <param name="is_animation">是否播放动画</param>
        /// <param name="is_queued">是否插入动画序列器播放</param>
        /// <param name="callback">回调</param>
        /// </summary>
        public void Show(bool is_animation = false, bool is_queued = false, System.Action callback = null)
        {
            if (Visible)
                return;
            if(is_animation)
            {
                if(is_queued)
                    CGUIManager.Instance.AnimationSequence.PlayQueued(_PlayShowAnimation, callback);
                else
                {
                    CGUIManager.Instance.AnimationSequence.Play(_PlayShowAnimation, callback);
                }
            }
            else
            {
                _Show();
            }
        }

        /// <summary>
        ///   隐藏
        /// <param name="is_animation">是否播放动画</param>
        /// <param name="is_queued">是否插入动画序列器播放</param>
        /// <param name="callback">回调</param>
        /// </summary>
        public void Hide(bool is_animation = false, bool is_queued = false, System.Action callback = null)
        {
            if (!Visible)
                return;
            if (is_animation)
            {
                if (is_queued)
                    CGUIManager.Instance.AnimationSequence.PlayQueued(_PlayHideAnimation, callback);
                else
                {
                    CGUIManager.Instance.AnimationSequence.Play(_PlayHideAnimation, callback);
                }
            }
            else
            {
                _Hide();
            }

        }

        /// <summary>
        /// 显示
        /// </summary>
        void _Show()
        {
            if (Visible)
                return;

            Visible = true;

            if (!gameObject.activeSelf)
                gameObject.SetActive(true);

            if (HidePlan == CGUI.WindowHidePlan.OutSide)
            {
                transform.localPosition = new Vector2(transform.localPosition.x - CGUI.Constant.WINDOW_OUTSIDE_OFFSET,
                                                      transform.localPosition.y);
            }
            else if (HidePlan == CGUI.WindowHidePlan.Scale0)
            {
                transform.localScale = Vector3.one;
            }

            BringForward();

            OnShow();
        }

        /// <summary>
        /// 隐藏
        /// </summary>
        void _Hide()
        {
            if (!Visible)
                return;

            Visible = false;

            if (HidePlan == CGUI.WindowHidePlan.Hide)
            {
                gameObject.SetActive(false);
            }
            else if (HidePlan == CGUI.WindowHidePlan.Delete)
            {
                CGUIManager.Instance.DeleteWindow(this, false);
            }
            else if (HidePlan == CGUI.WindowHidePlan.OutSide)
            {
                transform.localPosition = new Vector2(transform.localPosition.x + CGUI.Constant.WINDOW_OUTSIDE_OFFSET,
                                                      transform.localPosition.y);
            }
            else if (HidePlan == CGUI.WindowHidePlan.Scale0)
            {
                transform.localScale = Vector3.zero;
            }

            OnHide();
        }

        /// <summary>
        /// 显示
        /// </summary>
        protected virtual void OnShow() { }

        /// <summary>
        /// 隐藏
        /// </summary>
        protected virtual void OnHide() { }
        #endregion

        #region Animation
        /// <summary>
        ///   
        /// </summary>
        IEnumerator _PlayShowAnimation()
        {
            _Show();
            yield return ShowAnimation();
        }

        /// <summary>
        ///   
        /// </summary>
        IEnumerator _PlayHideAnimation()
        {
            yield return HideAnimation();
            _Hide();
        }

        /// <summary>
        /// 显示动画控制, 派生界面重写此方法编写动画逻辑
        /// </summary>
        protected virtual IEnumerator ShowAnimation()
        {
            yield return 0;
        }

        /// <summary>
        /// 隐藏动画控制, 派生界面重写此方法编写动画逻辑
        /// </summary>
        protected virtual IEnumerator HideAnimation()
        {
            yield return 0;
        }

        #endregion

        #region NGUI
        /// <summary>
        /// Depth
        /// </summary>
        public int Depth
        {
            get
            {
                UIPanel p = (UIPanel)this;
                if (p != null)
                    return p.depth;
                return 0;
            }
            set
            {
                UIPanel p = (UIPanel)this;
                if (p != null)
                    p.depth = value;
            }
        }

        /// <summary>
        ///   UIRoot
        /// </summary>
        public UIRoot GetUIRoot()
        {
            return NGUITools.FindInParents<UIRoot>(gameObject);
        }

        /// <summary>
        /// 
        /// </summary>
        public static explicit operator UIWidget(CGUIWindow w)
        {
            return _GetComponent<UIWidget>(w);
        }

        /// <summary>
        /// 
        /// </summary>
        public static explicit operator UIPanel(CGUIWindow w)
        {
            return _GetComponent<UIPanel>(w);
        }

        /// <summary>
        /// 获得指定Component
        /// </summary>
        private static T _GetComponent<T>(CGUIWindow w) where T : Component
        {
            return w != null ? w.GetComponent<T>() : null;
        }
        #endregion
    }
}