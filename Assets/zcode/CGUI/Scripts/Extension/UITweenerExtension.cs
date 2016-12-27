/***************************************************************
 * Copyright 2016 By Zhang Minglin
 * Author: Zhang Minglin
 * Create: 2016/11/03
 * Note  : UITweener扩展函数
***************************************************************/
using UnityEngine;
using System.Collections;

namespace CGUI
{
    public static class UITweenerExtension
    {
        /// <summary>
        /// Replay the tween forward.
        /// </summary>
        static public void ReplayForward(this UITweener self)
        {
            self.tweenFactor = 0f;
            self.Sample(self.tweenFactor, false);
            self.PlayForward();
        }

        /// <summary>
        /// Replay the tween Reverse.
        /// </summary>
        static public void ReplayReverse(this UITweener self)
        {
            self.tweenFactor = 1f;
            self.Sample(self.tweenFactor, false);
            self.PlayReverse();
        }
    }
}
