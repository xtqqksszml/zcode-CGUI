
/***************************************************************
 * Copyright 2016 By Zhang Minglin
 * Author: Zhang Minglin
 * Create: 2015/11/16
 * Note  : GUI管理器
***************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CGUI
{
    /// <summary>
    /// GUI管理器
    /// </summary>
    public class CGUIManager : MonoSingleton<CGUIManager>
    {
        /// <summary>
        ///   是否开启屏幕分辨率自适应
        /// </summary>
        public bool SceenResolutionAdaptive = true;

        /// <summary>
        /// Layer
        /// </summary>
        public GameObject[] Layer = new GameObject[(int)CGUI.UILayer.Max];

        /// <summary>
        /// 界面列表
        /// </summary>
        private Dictionary<System.Type, List<CGUIWindow>> windows_;

        /// <summary>
        /// 动画序列控制器
        /// </summary>
        public CGUIAnimationSequence AnimationSequence { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        private UICamera ui_camera_;

        /// <summary>
        /// 
        /// </summary>
        public CGUIManager()
        {
            windows_ = new Dictionary<System.Type, List<CGUIWindow>>();
        }

        /// <summary>
        /// 创建界面
        /// </summary>
        public T CreateWindow<T>(string name = null) where T : CGUIWindow
        {
            //已有界面直接返回
            T win = GetWindow<T>(name);
            if (win != null)
                return win;

            string window_key = typeof(T).Name;
            T window = ResourceLoad.WindowLoader.Load(window_key) as T;
            if (window == null)
                return default(T);

            GameObject obj = window.gameObject;

            //预处理
            obj.SetActive(false);

            //记录原始位置
            Vector3 position = obj.transform.localPosition;

            //设置父对象及参数
            GameObject parent = GetLayer(window.WindowType);
            obj.transform.parent = parent.transform;
            obj.gameObject.layer = parent.layer;
            obj.gameObject.transform.localScale = Vector3.one;

            //名称
            if (!string.IsNullOrEmpty(name)) window.name = name;
            //位置
            window.Position = position;

            //加入列表中
            System.Type key = window.GetType();
            if (!windows_.ContainsKey(key))
                windows_.Add(key, new List<CGUIWindow>());
            windows_[key].Add(window);

            return window;
        }

        /// <summary>
        /// 删除界面
        /// </summary>
        public void DeleteWindow<T>(string name = null) where T : CGUIWindow
        {
            DeleteWindow(GetWindow<T>(name));
        }

        /// <summary>
        /// 删除界面
        /// </summary>
        public void DeleteWindow(CGUIWindow window, bool hide_window = true)
        {
            if (window != null)
            {
                if (hide_window)
                {
                    window.Hide();
                    if (window.HidePlan != CGUI.WindowHidePlan.Delete)
                        return;
                }

                System.Type key = window.GetType();
                if (windows_.ContainsKey(key))
                    windows_[key].Remove(window);

                UnityEngine.Object.Destroy(window.gameObject);

            }
        }

        /// <summary>
        /// 删除所有界面
        /// </summary>
        public void DeleteAllWindow()
        {
            var copy = new Dictionary<System.Type, List<CGUIWindow>>(windows_);
            var itr = copy.Values.GetEnumerator();
            while (itr.MoveNext())
            {
                var copy_list = new List<CGUIWindow>(itr.Current);
                var win_itr = copy_list.GetEnumerator();
                while (win_itr.MoveNext())
                    DeleteWindow(win_itr.Current);
                win_itr.Dispose();
            }
            itr.Dispose();

            windows_.Clear();
        }

        /// <summary>
        /// 获得一个界面
        /// </summary>
        public T GetWindow<T>(string name = null) where T : CGUIWindow
        {
            List<CGUIWindow> list;
            windows_.TryGetValue(typeof(T), out list);
            if (list == null || list.Count == 0)
                return default(T);

            CGUIWindow window = null;
            if (string.IsNullOrEmpty(name))
            {
                window = list[0];
            }
            else
            {
                for (int i = 0; i < list.Count; ++i)
                {
                    if (list[i].name == name)
                    {
                        window = list[i];
                        break;
                    }
                }
            }

            return window as T;
        }

        /// <summary>
        ///   显示一个界面，没有则直接加载
        /// </summary>
        public T ShowWindow<T>(string name = null) where T : CGUIWindow
        {
            T win = GetWindow<T>(name);
            if (win == null)
                win = CreateWindow<T>(name);

            ShowWindow(win);
            return win;
        }

        /// <summary>
        ///   显示一个界面，没有则直接加载
        /// </summary>
        public void ShowWindow<T>(T window) where T : CGUIWindow
        {
            if (window != null)
                window.Show();
        }

        /// <summary>
        ///   隐藏一个界面，没有则无视
        /// </summary>
        public void HideWindow<T>(string name = null) where T : CGUIWindow
        {
            HideWindow(GetWindow<T>(name));
        }

        /// <summary>
        ///   隐藏一个界面，没有则无视
        /// </summary>
        public void HideWindow<T>(T window) where T : CGUIWindow
        {
            if (window != null)
            {
                if (window.HidePlan == CGUI.WindowHidePlan.Delete)
                    DeleteWindow(window);
                else
                    window.Hide();
            }
        }

        /// <summary>
        /// 界面弹至最前
        /// </summary>
        public void BringForward(CGUIWindow window)
        {
            if (window.transform.parent == null)
            {
                Debug.LogError("Can't find parent's UIPanel.");
                return;
            }
            UIPanel parent = window.transform.parent.GetComponentInParent<UIPanel>();
            if (parent == null)
            {
                Debug.LogError("Can't find parent's UIPanel.");
                return;
            }

            int max_depth = 0;
            UIPanel[] panels = parent.gameObject.GetComponentsInChildren<UIPanel>();
            for (int i = 0; i < panels.Length; ++i)
            {
                if (max_depth < panels[i].depth)
                    max_depth = panels[i].depth;
            }

            //设置新的depth
            window.Depth = max_depth + 1;
            //调整窗口的depth
            AdjustPanelDepth((UIPanel)window);
            //调整所有depth
            AdjustPanelDepth(parent);
        }

        /// <summary>
        /// 界面弹至最后
        /// </summary>
        public void PushBack(CGUIWindow window)
        {
            if (window.transform.parent == null)
            {
                Debug.LogError("Can't find parent's UIPanel.");
                return;
            }
            UIPanel parent = window.transform.parent.GetComponentInParent<UIPanel>();
            if (parent == null)
            {
                Debug.LogError("Can't find parent's UIPanel.");
                return;
            }

            int min_depth = 0;
            UIPanel[] panels = parent.GetComponentsInChildren<UIPanel>();
            for (int i = 0; i < panels.Length; ++i)
            {
                if (min_depth > panels[i].depth)
                    min_depth = panels[i].depth;
            }

            //设置新的depth
            window.Depth = min_depth - 1;

            //调整所有depth
            AdjustPanelDepth(parent);
        }

        /// <summary>
        /// 调整Panel's Depth
        /// </summary>
        public void AdjustPanelDepth(UIPanel parent)
        {
            //获得所有子物体
            List<UIPanel> panels = CGUI.Common.GetChildComponents<UIPanel>(parent.gameObject);
            int size = panels.Count;
            if (size > 0)
            {
                int current = parent.depth;
                panels.Sort(UIPanel.CompareFunc);
                for (int i = 0; i < size; ++i)
                {
                    UIPanel w = panels[i];
                    w.depth = ++current;
                }
            }
        }

        /// <summary>
        ///   获得窗口类型的Layer
        /// </summary>
        public GameObject GetLayer(CGUI.WindowType type)
        {
            if (type < CGUI.WindowType.Max)
                return Layer[(int)Common.GetLayerByWindowType(type)];
            return null;
        }

        /// <summary>
        /// 开关界面输入设备事件响应
        /// </summary>
        public void ToggleEvent(bool on)
        {
            if (ui_camera_)
            {
                ui_camera_.useMouse = on;
                ui_camera_.useTouch = on;
                ui_camera_.useKeyboard = on;
                ui_camera_.useController = on;
            }
        }

        /// <summary>
        ///   初始化Layer的Depth
        /// </summary>
        void InitializeWindowLayerDepth()
        {
            var count = (int)CGUI.UILayer.Max;
            for (int i = 0; i < count; ++i)
            {
                if (Layer[i] != null)
                {
                    UIPanel layer = Layer[i].GetComponent<UIPanel>();
                    if (layer != null)
                        layer.depth = CGUI.Constant.WindowLayerDepthList[i];
                }
            }
        }

        #region SceenResolutionAdaptive
        /// <summary>
        ///   屏幕分辨率自适应
        /// </summary>
        public void DoSceenResolutionAdaptive()
        {
            UIRoot root = GetComponentInParent<UIRoot>();
            if (root == null)
                return;
#if UNITY_IPHONE || UNITY_ANDROID
        root.scalingStyle = UIRoot.Scaling.ConstrainedOnMobiles;
#else
            root.scalingStyle = UIRoot.Scaling.Constrained;
#endif
        }
        #endregion

        #region FPS
        /// <summary>
        ///   
        /// </summary>
        public CGUIFPS FPSControl;

        /// <summary>
        ///   EnableLog
        /// </summary>
        public bool EnableFPS
        {
            get
            {
                if (FPSControl != null)
                    return FPSControl.gameObject.activeSelf;

                return false;
            }
            set
            {
                if (FPSControl != null)
                {
                    FPSControl.gameObject.SetActive(value);
                }
            }
        }
        #endregion

        #region Console
        /// <summary>
        ///   
        /// </summary>
        public CGUIConsole ConsoleControl;

        /// <summary>
        ///   EnableConsole
        /// </summary>
        public bool EnableConsole
        {
            get
            {
                if (ConsoleControl != null)
                    return ConsoleControl.gameObject.activeSelf;

                return false;
            }
            set
            {
                if (ConsoleControl != null)
                {
                    ConsoleControl.gameObject.SetActive(value);
                }
            }
        }
        #endregion

        #region MonoBehaviour
        /// <summary>
        /// 
        /// </summary>
        void Awake()
        {
            InitializeWindowLayerDepth();
            DoSceenResolutionAdaptive();

            ui_camera_ = GetComponentInChildren<UICamera>();
            AnimationSequence = CGUI.Common.GetOrAddComponent<CGUIAnimationSequence>(gameObject);
        }

        /// <summary>
        /// 
        /// </summary>
        void Start()
        {

        }

        /// <summary>
        ///   
        /// </summary>
        void Update()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        void OnDestroy()
        {
            windows_.Clear();
        }
        #endregion
    }
}