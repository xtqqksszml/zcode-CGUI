/***************************************************************
 * Copyright 2016 By Zhang Minglin
 * Author: Zhang Minglin
 * Create: 2016/11/14
 * Note  : FPS显示
***************************************************************/
using UnityEngine;
using System.Collections;

namespace CGUI
{
    /// <summary>
    /// FPS显示控件
    /// </summary>
    public class CGUIFPS : MonoBehaviour
    {
        /// <summary>
        ///   显示FPS的文本框
        /// </summary>
        public UILabel Label;

        /// <summary>
        ///   
        /// </summary>
        private long frame_count_;

        /// <summary>
        ///   
        /// </summary>
        private long last_frame_time;

        /// <summary>
        ///   
        /// </summary>
        static long last_fps_;

        /// <summary>
        ///   
        /// </summary>
        private void UpdateTick()
        {
            if (true)
            {
                frame_count_++;
                long curMillSec = TickToMilliSec(System.DateTime.Now.Ticks);
                if (last_frame_time == 0)
                {
                    last_frame_time = TickToMilliSec(System.DateTime.Now.Ticks);
                }

                if ((curMillSec - last_frame_time) >= 1000)
                {
                    long fps = (long)(frame_count_ * 1.0f / ((curMillSec - last_frame_time) / 1000.0f));

                    last_fps_ = fps;

                    frame_count_ = 0;

                    last_frame_time = curMillSec;
                }
            }

            Label.text = "FPS:" + last_fps_;
        }
        public static long TickToMilliSec(long tick)
        {
            return tick / (10 * 1000);
        }

        #region MonoBehaviour
        /// <summary>
        ///   MonoBehaviour.Update
        /// </summary>
        void Update()
        {
            UpdateTick();
        }
        #endregion
    }
}

