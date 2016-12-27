/***************************************************************
 * Copyright 2016 By Zhang Minglin
 * Author: Zhang Minglin
 * Create: 2016/11/16
 * Note  : 例子 - Console使用方式
***************************************************************/
using UnityEngine;
using System.Collections;
using CGUI;

public class Example2 : MonoBehaviour {

    /// <summary>
    ///   
    /// </summary>
    public void InitializeConsole()
    {
        Debug.logger.logEnabled = true;
        CGUI.CGUIManager.Instance.EnableConsole = true;
        CGUI.CGUIManager.Instance.ConsoleControl.ShowLog();
        CGUI.CGUIManager.Instance.ConsoleControl.CommandSendEvent += OnCommandSendCallback;
    }

    /// <summary>
    ///   处理命令行消息
    /// </summary>
    void OnCommandSendCallback(string cmd)
    {
        Debug.Log(cmd + " - commmand sent succeed!");
    }

	/// <summary>
    ///   MonoBehaviour.Start
    /// </summary>
	void Start () 
	{
	    //初始化控制台
        InitializeConsole();
	}
	
	/// <summary>
    ///   MonoBehaviour.Update
    /// </summary>
	void Update () 
	{
	
	}

    /// <summary>
    /// 
    /// </summary>
    void OnGUI()
    {
        if (GUI.Button(new Rect(0f, 0f, 210f, 20f), "Toggle Console's Log/ CGUI's Log"))
        {
            CGUIManager.Instance.EnableConsole = !CGUIManager.Instance.EnableConsole;
        }

        if(GUI.Button(new Rect(0f, 40f, 160f, 20f), "Print Log"))
        {
            Debug.Log("This is a log.");
        }
        if (GUI.Button(new Rect(0f, 60f, 160f, 20f), "Print Warning Log"))
        {
            Debug.LogWarning("This is a warning log.");
        }
        if (GUI.Button(new Rect(0f, 80f, 160f, 20f), "Print Error Log"))
        {
            Debug.LogError("This is a error log.");
        }
    }
}
