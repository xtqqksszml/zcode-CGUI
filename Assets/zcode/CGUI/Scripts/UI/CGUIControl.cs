/***************************************************************
 * Copyright 2016 By Zhang Minglin
 * Author: Zhang Minglin
 * Create: 2015/11/16
 * Note  : CGUI控件基类
***************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CGUI
{
    /// <summary>
    /// CGUI控件基类
    /// </summary>
    public class CGUIControl
    {
        /// <summary>
        /// 无效的控件类
        /// </summary>
        public static CGUIControl Invalid = new CGUIControl();

        /// <summary>
        /// 绑定的对象
        /// </summary>
        private GameObject game_object_;

        /// <summary>
        /// 
        /// </summary>
        private CGUIControl()
        {
            game_object_ = null;
        }

        /// <summary>
        /// 
        /// </summary>
        public CGUIControl(GameObject obj)
        {
            game_object_ = obj;
        }

        /// <summary>
        /// 父对象
        /// </summary>
        public CGUIControl Parent
        {
            get
            {
                if (game_object_ != null && game_object_.transform != null && game_object_.transform.parent != null)
                {
                    return new CGUIControl(game_object_.transform.parent.gameObject);
                }

                return null;
            }
        }

        /// <summary>
        /// 获得子对象
        /// </summary>
        public CGUIControl this[string name]
        {
            get
            {
                try
                {
                    if (game_object_ != null)
                        return new CGUIControl(game_object_.transform.FindChild(name).gameObject);
                }
                catch (System.Exception)
                {
                    Debug.Log("Cann't find GuiControl, Name is " + name);
                }

#if UNITY_EDITOR
                return null;
#else
            return Invalid;
#endif
            }
        }

        #region NGUI
        /// <summary>
        ///   获得UI组件
        /// </summary>
        private static T GetUIComponent<T>(CGUIControl c) where T : MonoBehaviour
        {
            return (c != null && c.game_object_ != null) ? c.game_object_.GetComponent<T>() : null;
        }

        /// <summary>
        /// 
        /// </summary>
        public static explicit operator GameObject(CGUIControl c)
        {
            return (c != null && c.game_object_ != null) ? c.game_object_ : null;
        }
        /// <summary>
        /// 
        /// </summary>
        public static explicit operator UIWidget(CGUIControl c)
        {
            return GetUIComponent<UIWidget>(c);
        }

        /// <summary>
        /// 
        /// </summary>
        public static explicit operator UIPanel(CGUIControl c)
        {
            return GetUIComponent<UIPanel>(c);
        }

        /// <summary>
        /// 
        /// </summary>
        public static explicit operator UISprite(CGUIControl c)
        {
            return GetUIComponent<UISprite>(c);
        }

        /// <summary>
        /// 
        /// </summary>
        public static explicit operator UI2DSprite(CGUIControl c)
        {
            return GetUIComponent<UI2DSprite>(c);
        }

        /// <summary>
        /// 
        /// </summary>
        public static explicit operator UITexture(CGUIControl c)
        {
            return GetUIComponent<UITexture>(c);
        }

        /// <summary>
        /// 
        /// </summary>
        public static explicit operator UILabel(CGUIControl c)
        {
            return GetUIComponent<UILabel>(c);
        }

        /// <summary>
        /// 
        /// </summary>
        public static explicit operator UIButton(CGUIControl c)
        {
            return GetUIComponent<UIButton>(c);
        }

        /// <summary>
        /// 
        /// </summary>
        public static explicit operator UISlider(CGUIControl c)
        {
            return GetUIComponent<UISlider>(c);
        }
        /// <summary>
        /// 
        /// </summary>
        public static explicit operator UIToggle(CGUIControl c)
        {
            return GetUIComponent<UIToggle>(c);
        }
        /// <summary>
        /// 
        /// </summary>
        public static explicit operator UIInput(CGUIControl c)
        {
            return GetUIComponent<UIInput>(c);
        }
        #endregion
    }
}

