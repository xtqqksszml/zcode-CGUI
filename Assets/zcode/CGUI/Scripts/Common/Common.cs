/***************************************************************
 * Copyright 2016 By Zhang Minglin
 * Author: Zhang Minglin
 * Create: 2016/11/14
 * Note  : 
***************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CGUI
{
    /// <summary>
    /// UILayer
    /// </summary>
    public enum UILayer
    {
        Game,               // Lowest layer, used for UI used in the game scene.
        NormalWindow,       // 普通窗口
        TopWindow,          // 置顶窗口
        ModalWindow,        // 模态窗口
        Tooltip,            // 提示信息UI

        Max
    }

    /// <summary>
    /// Hide's plan
    /// </summary>
    public enum WindowHidePlan
    {
        Hide,           // 隐藏
        Delete,         // 删除
        OutSide,        // 移出边界
        Scale0,         // 缩小为0
    }

    /// <summary>
    /// GUI窗口类型
    /// </summary>
    public enum WindowType
    {
        Normal,         // 普通窗口
        Top,            // 置顶窗口
        Modal,          // 模态窗口

        Max
    }

    /// <summary>
    /// 常量定义
    /// </summary>
    public static class Constant
    {
        /// <summary>
        /// Outside偏移量
        /// </summary>
        public const int WINDOW_OUTSIDE_OFFSET = 10000;

        /// <summary>
        ///   不同窗口Depth
        /// </summary>
        public static readonly int[] WindowLayerDepthList = 
        {
            -1000,
            1,
            1000,
            2000,
            3000,
        };
    }

    /// <summary>
    /// 公共函数定义
    /// </summary>
    public static class Common
    {
        /// <summary>
        /// 获得指定窗口的UILayer
        /// </summary>
        public static UILayer GetLayerByWindowType(WindowType type)
        {
            if (type == WindowType.Normal)
                return UILayer.NormalWindow;
            else if (type == WindowType.Top)
                return UILayer.TopWindow;
            else if (type == WindowType.Modal)
                return UILayer.ModalWindow;

            return UILayer.Max;
        }

        /// <summary>
        ///   获得一个对象的指定子组件
        /// </summary>
        public static List<T> GetChildComponents<T>(GameObject go)
            where T : Component
        {
            List<T> result = new List<T>();

            if (go != null)
            {
                T[] objs = go.GetComponentsInChildren<T>();
                result.AddRange(objs);

                //剔除掉父对象中的组件
                T parent = go.GetComponent<T>();
                if (parent != null)
                    result.Remove(parent);
            }

            return result;
        }

        /// <summary>
        /// 获得或者创建一个对象的组件
        /// </summary>
        public static T GetOrAddComponent<T>(GameObject go)
            where T : Component
        {
            if (go == null)
                return null;

            T com = go.GetComponent<T>();
            return com != null ? com : go.AddComponent<T>();
        }
    }
}