/***************************************************************
 * Copyright 2016 By Zhang Minglin
 * Author: Zhang Minglin
 * Create: 2016/12/27
 * Note  : 例子 - 窗口动画
***************************************************************/
using UnityEngine;
using System.Collections;

public class Example1 : MonoBehaviour
{

	/// <summary>
    ///   MonoBehaviour.Start
    /// </summary>
	void Start () 
	{
        var win = CGUI.CGUIManager.Instance.CreateWindow<FirstAnimationWindow>();
        win.PlayShowAnimation();
	}
	
	/// <summary>
    ///   MonoBehaviour.Update
    /// </summary>
	void Update () 
	{
	
	}
}
