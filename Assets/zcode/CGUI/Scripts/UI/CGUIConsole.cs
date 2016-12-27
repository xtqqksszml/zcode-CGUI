/***************************************************************
 * Copyright 2016 By Zhang Minglin
 * Author: Zhang Minglin
 * Create: 2016/11/16
 * Note  :  控制台系统
            支持日志输出（Debuger）
            支持命令响应
***************************************************************/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class CGUIConsole : MonoBehaviour
{
    public class LogHandler : ILogHandler
    {
        /// <summary>
        /// 默认的日志处理
        /// </summary>
        public ILogHandler DefaultLogHandler;

        /// <summary>
        /// 缓存日志
        /// </summary>
        public List<string> LogBuffer = new List<string>();

        // Summary:
        //     A variant of ILogHandler.LogFormat that logs an exception message.
        //
        // Parameters:
        //   exception:
        //     Runtime Exception.
        //
        //   context:
        //     Object to which the message applies.
        public void LogException(Exception exception, UnityEngine.Object context)
        {
            string message = exception.Message;
            _Log(LogType.Exception, message);
        }

        //
        // Summary:
        //     Logs a formatted message.
        //
        // Parameters:
        //   logType:
        //     The type of the log message.
        //
        //   context:
        //     Object to which the message applies.
        //
        //   format:
        //     A composite format string.
        //
        //   args:
        //     Format arguments.
        public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
        {
            string message = string.Format(format, args);
            _Log(logType, message);
        }

        /// <summary>
        ///   
        /// </summary>
        void _Log(LogType logType, string message)
        {
            string str = _GetLogColor(logType);
            str += message.ToString();
            LogBuffer.Add(str);
        }

        /// <summary>
        /// 
        /// </summary>
        string _GetLogColor(LogType logType)
        {
            string str = "";
            if (logType == LogType.Log)
                str = "[ffffff]";
            else if (logType == LogType.Warning)
                str = "[ffff00]";
            else if (logType == LogType.Error)
                str = "[ff0000]";
            else if (logType == LogType.Assert)
                str = "[ffffff]";
            else if (logType == LogType.Exception)
                str = "[ff0000]";

            return str;
        }
    }

    /// <summary>
    ///   Content
    /// </summary>
    public GameObject Content;

    /// <summary>
    ///   Command
    /// </summary>
    public GameObject Command;

    /// <summary>
    /// 
    /// </summary>
    public UITextList LogList;

    /// <summary>
    /// 
    /// </summary>
    public UILabel CommandText;

    /// <summary>
    ///   命令行消息
    /// </summary>
    public System.Action<string> CommandSendEvent;

    /// <summary>
    /// Debug日志处理器
    /// </summary>
    private LogHandler log_handler_;

    /// <summary>
    /// 
    /// </summary>
    public void SetCommandTooltip(string tip)
    {
        CommandText.text = tip;
    }

    /// <summary>
    ///   Send Command
    /// </summary>
    public void SendCommand(GameObject go)
    {
        var input = go.GetComponent<UIInput>();
        string command = input.value;

        Debug.Log(command, null);

        if (CommandSendEvent != null)
            CommandSendEvent(command);

        input.value = "";
    }

    /// <summary>
    ///   开启关闭日志显示
    /// </summary>
    public void SwitchLog()
    {
        Content.SetActive(!Content.activeSelf);
    }

    /// <summary>
    ///   开启日志显示
    /// </summary>
    public void ShowLog()
    {
        Content.SetActive(true);
    }

    /// <summary>
    ///   关闭日志显示
    /// </summary>
    public void HideLog()
    {
        Content.SetActive(false);
    }

    /// <summary>
    /// 打印日志信息至控制台
    /// </summary>
    private void PrintLogToConsole()
    {
        if (log_handler_ != null && log_handler_.LogBuffer.Count > 0)
        {
            for (int i = 0; i < log_handler_.LogBuffer.Count; ++i)
            {
                LogList.Add(log_handler_.LogBuffer[i]);
            }
            log_handler_.LogBuffer.Clear();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void Awake()
    {
        HideLog();
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
    void OnEnable()
    {
        if (log_handler_ == null)
            log_handler_ = new LogHandler();
        log_handler_.DefaultLogHandler = Debug.logger.logHandler;
        Debug.logger.logHandler = log_handler_;
    }

    /// <summary>
    /// 
    /// </summary>
    void OnDisable()
    {
        Debug.logger.logHandler = log_handler_.DefaultLogHandler;
        log_handler_ = null;
    }
    
    /// <summary>
    /// 
    /// </summary>
    void Update()
    {
        PrintLogToConsole();
    }
}
