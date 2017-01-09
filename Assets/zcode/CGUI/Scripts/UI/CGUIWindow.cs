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
        /// 显示动画
        /// </summary>
        private System.Func<IEnumerator> show_animation_ = null;

        /// <summary>
        /// 隐藏动画
        /// </summary>
        private System.Func<IEnumerator> hide_animation_ = null;

        /// <summary>
        ///   
        /// </summary>
        protected CGUIWindow()
        { }

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

        /// <summary>
        /// 显示
        /// </summary>
        public void Show()
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
        public void Hide()
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

        #region Animation
        /// <summary>
        /// 显示界面并播放显示动画
        /// </summary>
        public void PlayShowAnimation(System.Action callback = null)
        {
            CGUIManager.Instance.AnimationSequence.Play(_PlayShowAnimation, callback);
        }

        /// <summary>
        /// 加入动画序列，等待显示界面并播放显示动画
        /// </summary>
        public void PlayQueuedShowAnimation(System.Action callback = null)
        {
            CGUIManager.Instance.AnimationSequence.PlayQueued(_PlayShowAnimation, callback);
        }
        IEnumerator _PlayShowAnimation()
        {
            Show();

            if (show_animation_ != null)
                yield return show_animation_();

            yield return 0;
        }

        /// <summary>
        /// 隐藏界面并播放隐藏动画
        /// </summary>
        public void PlayHideAnimation(System.Action callback = null)
        {
            CGUIManager.Instance.AnimationSequence.Play(_PlayHideAnimation, callback);
        }

        /// <summary>
        /// 加入动画序列，等待隐藏界面并播放隐藏动画
        /// </summary>
        public void PlayQueuedHideAnimation(System.Action callback = null)
        {
            CGUIManager.Instance.AnimationSequence.PlayQueued(_PlayHideAnimation, callback);
        }
        IEnumerator _PlayHideAnimation()
        {
            if (hide_animation_ != null)
                yield return hide_animation_();

            Hide();

            yield return 0;
        }

        /// <summary>
        /// 绑定显示动画
        /// </summary>
        public void BindShowAnimation(System.Func<IEnumerator> animation)
        {
            show_animation_ = animation;
        }

        /// <summary>
        /// 取绑显示动画
        /// </summary>
        public void UnbindShowAnimation()
        {
            show_animation_ = null;
        }

        /// <summary>
        /// 绑定隐藏动画
        /// </summary>
        public void BindHideAnimation(System.Func<IEnumerator> animation)
        {
            hide_animation_ = animation;
        }

        /// <summary>
        /// 取绑隐藏动画
        /// </summary>
        public void UnbindHideAnimation()
        {
            hide_animation_ = null;
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

        #region MonoBehaviour
        /// <summary>
        /// 
        /// </summary>
        void Awake()
        {
            //窗口默认为关闭
            Visible = false;
        }
        #endregion
    }
}