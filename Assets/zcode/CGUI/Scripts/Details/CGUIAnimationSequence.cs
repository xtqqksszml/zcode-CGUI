/***************************************************************
 * Copyright 2016 By Zhang Minglin
 * Author: Zhang Minglin
 * Create: 2016/11/02
 * Note  : CGUI系统动画序列
***************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CGUI
{
    public class CGUIAnimationSequence : MonoBehaviour
    {
        class Element
        {
            public System.Func<IEnumerator> AnimationProcess;
            public System.Action Callback;
        }

        /// <summary>
        /// 
        /// </summary>
        private bool is_play_queued_;

        /// <summary>
        /// 
        /// </summary>
        private Queue<Element> sequence_;

        /// <summary>
        /// 
        /// </summary>
        protected CGUIAnimationSequence()
        { }

        /// <summary>
        /// 
        /// </summary>
        public void Play(System.Func<IEnumerator> animation, System.Action callback = null)
        {
            if (animation != null)
            {
                Element e = new Element() { AnimationProcess = animation, Callback = callback };
                StartCoroutine(_PlayAnimation(e));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void PlayQueued(System.Func<IEnumerator> animation, System.Action callback = null)
        {
            if (animation != null)
            {
                Element e = new Element() { AnimationProcess = animation, Callback = callback };
                sequence_.Enqueue(e);
            }

            if (!is_play_queued_)
                StartCoroutine(_PlayAnimationQueued());
        }

        /// <summary>
        /// 
        /// </summary>
        IEnumerator _PlayAnimation(Element elem)
        {
            CGUIManager.Instance.ToggleEvent(false);
            yield return elem.AnimationProcess();
            CGUIManager.Instance.ToggleEvent(true);
            if (elem.Callback != null)
                elem.Callback();
            yield return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        IEnumerator _PlayAnimationQueued()
        {
            CGUIManager.Instance.ToggleEvent(false);
            is_play_queued_ = true;
            while (sequence_.Count > 0)
            {
                var elem = sequence_.Dequeue();
                yield return elem.AnimationProcess();
                if (elem.Callback != null)
                    elem.Callback();
            }
            is_play_queued_ = false;
            CGUIManager.Instance.ToggleEvent(true);
            yield return 0;
        }

        #region MonoBehaviour
        /// <summary>
        /// 
        /// </summary>
        void Awake()
        {
            is_play_queued_ = false;
            sequence_ = new Queue<Element>();
        }
        #endregion
    }
}