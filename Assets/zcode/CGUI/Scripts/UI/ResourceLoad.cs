/***************************************************************
 * Copyright 2016 By Zhang Minglin
 * Author: Zhang Minglin
 * Create: 2016/11/14
 * Note  : 资源加载
***************************************************************/
using UnityEngine;
using System.Collections;

namespace CGUI
{
    /// <summary>
    /// 界面加载接口
    /// </summary>
    public interface IWindowLoader
    {
        /// <summary>
        ///   载入
        /// </summary>
        CGUIWindow Load(string name);
    }

    /// <summary>
    /// 默认加载类
    /// </summary>
    public class DefaultWindowLoader : IWindowLoader
    {
        /// <summary>
        /// 界面Profile所在的加载路径
        /// </summary>
        public const string WINDOW_PREFAB_DEFAULT_LOAD_PATH = "CGUI/";

        /// <summary>
        ///   载入
        /// </summary>
        public CGUIWindow Load(string name)
        {
            //载入与类同名的Prefab
            GameObject prefab = Resources.Load<GameObject>(WINDOW_PREFAB_DEFAULT_LOAD_PATH + name);
            if (prefab == null)
            {
                Debug.LogError("Can't find window's prefab, window type is " + name);
                return default(CGUIWindow);
            }

            //创建实例
            GameObject obj = UnityEngine.Object.Instantiate(prefab) as GameObject;
            if (obj == null)
            {
                Debug.LogError("Can't instantiate window's prefab, window type is " + name);
                return default(CGUIWindow);
            }
            CGUIWindow window = obj.GetComponent<CGUIWindow>();
            if (window == null)
            {
                Debug.LogError("Don't get GameObject's component, component type is " + name);
                return default(CGUIWindow);
            }

            return window;
        }
    }

    /// <summary>
    /// 资源加载器
    /// </summary>
    public static class ResourceLoad
    {
        /// <summary>
        /// 界面加载器
        /// </summary>
        public static IWindowLoader WindowLoader = new DefaultWindowLoader();
    }
}
