/***************************************************************
 * Copyright 2016 By Zhang Minglin
 * Author: Zhang Minglin
 * Create: 2016/11/14
 * Note  : Demo - 聊天窗口
***************************************************************/
using UnityEngine;
using System.Collections;
using CGUI;

public class ChatWindow : CGUIWindow
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

    /// <summary>
    /// 信息数量
    /// </summary>
    private int message_count_;

    /// <summary>
    /// 信息统计
    /// </summary>
    private UILabel information_;

    /// <summary>
    /// 
    /// </summary>
    public void OnChatSend()
    {
        ChatInput chat_input = ((GameObject)this["Background/Chat Input"]).GetComponent<ChatInput>();
        chat_input.OnSubmit();

        AddMessageCount();
    }

    /// <summary>
    /// 
    /// </summary>
    void AddMessageCount()
    {
        message_count_++;
        if (information_ != null)
            information_.text = "You sent " + message_count_ + " messages.";
    }

    /// <summary>
    /// 
    /// </summary>
    void OnClick_Send(GameObject go)
    {
        OnChatSend();
    }

    void Awake()
    {
        UIEventListener.Get(((GameObject)this["Background/Send"])).onClick += OnClick_Send;

        information_ = (UILabel)this["Background/Information"];
    }

    void Start()
    {

    }
}
