/***************************************************************
 * Copyright 2016 By Zhang Minglin
 * Author: Zhang Minglin
 * Create: 2016/11/14
 * Note  : 用于显示隐藏AnimationWindow的窗口
***************************************************************/
using UnityEngine;
using System.Collections;
using CGUI;

public class SecondAnimationWindow : CGUIWindow
{
    #region CGUI
    /// <summary>
    /// 界面类型
    /// 派生的界面，有需要可以重写这个属性
    /// </summary>
    public override CGUI.WindowType WindowType
    {
        get { return CGUI.WindowType.Normal; }
    }

    /// <summary>
    /// 界面关闭策略
    /// 派生的界面，有需要可以重写这个属性
    /// </summary>
    public override CGUI.WindowHidePlan HidePlan
    {
        get { return CGUI.WindowHidePlan.Delete; }
    }
    #endregion

    #region Animation
    /// <summary>
    /// 
    /// </summary>
    private TweenPosition cmpt_;

    /// <summary>
    /// 
    /// </summary>
    void BuildAnimation()
    {
        cmpt_ = ((GameObject)this["Background"]).GetComponent<TweenPosition>();
        BindShowAnimation(ShowAnimation);
        BindHideAnimation(HideAnimation);
    }

    /// <summary>
    /// 
    /// </summary>
    IEnumerator ShowAnimation()
    {
        cmpt_.PlayForward();
        while (cmpt_.enabled)
            yield return 1;

        yield return 0;
    }

    /// <summary>
    /// 
    /// </summary>
    IEnumerator HideAnimation()
    {
        cmpt_.PlayReverse();
        while (cmpt_.enabled)
            yield return 1;

        yield return 0;
    }
    #endregion

    /// <summary>
    /// 
    /// </summary>
    void OnClick_Toggle(GameObject go)
    {
        PlayHideAnimation();
        CGUIManager.Instance.CreateWindow<FirstAnimationWindow>().PlayShowAnimation();
    }

    void Awake()
    {
        UIEventListener.Get(((GameObject)this["Background/Toggle"])).onClick += OnClick_Toggle;

        BuildAnimation();
    }

}
