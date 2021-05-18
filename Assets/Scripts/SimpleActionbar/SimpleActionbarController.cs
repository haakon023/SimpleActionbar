using System;
using System.Collections.Generic;
using System.Security;
using JetBrains.Annotations;
using SimpleActionbar.ExampleCode;
using UnityEngine;

namespace SimpleActionbar
{
    /// <summary>
    /// The Generic T is to define what IActionButton should trigger whenever it was executed
    /// </summary>
    public abstract class SimpleActionbarController<T> : MonoBehaviour
    {
        public IActionButton<T>[] ActionButtonIndexes { get; protected set; }
                
        public event EventHandler<UseActionButtonEventArgs> UseActionButtonEvent;
        
        public event EventHandler<GlobalCooldownEventArgs> GlobalCooldownEvent;
        
        public event EventHandler<AddActionToActionButtonArgs<T>> AddActionToActionButtonEvent;
        
        public event EventHandler<RemoveActionFromActionButtonArgs> RemoveActionFromActionButtonEvent;

        public event EventHandler<SetActionButtonKeybindLabelArgs> SetActionButtonKeybindLabelEvent;
        
        public event EventHandler<SetActionButtonDisabledArgs> SetActionButtonDisabledEvent;
        
        public virtual void OnAddActionToActionButton(AddActionToActionButtonArgs<T> args)
        {
            var handler = AddActionToActionButtonEvent;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        public virtual void OnRemoveActionFromActionButton(RemoveActionFromActionButtonArgs args)
        {
            var handler = RemoveActionFromActionButtonEvent;
            if (handler != null)
            {
                handler(this, args);
            }
        }
        
        protected virtual void OnUseActionButton(UseActionButtonEventArgs args)
        {
            var handler = UseActionButtonEvent;
            if (handler != null)
            {
                handler(this, args);
            }
        }
        
        protected virtual void OnGlobalCooldown(GlobalCooldownEventArgs args)
        {
            var handler = GlobalCooldownEvent;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        public virtual void OnSetActionButtonKeyBindLabel(SetActionButtonKeybindLabelArgs args)
        {
            var handler = SetActionButtonKeybindLabelEvent;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        public virtual void OnSetActionButtonDisabled(SetActionButtonDisabledArgs args)
        {
            var handler = SetActionButtonDisabledEvent;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        
        [SerializeField]
        protected bool UseGlobalCooldown;
        
        [SerializeField]
        protected float CurrentGlobalCooldown;

        [SerializeField]
        private float _globalCooldownLength;

        protected float GlobalCooldownLength
        {
            get => _globalCooldownLength;
            set => _globalCooldownLength = value;
        }

        /// <summary>
        /// Evaluates if an actionbutton can be invoked, example if actionbutton has an action to execute
        /// </summary>
        /// <param name="actionbutton"></param>
        /// <returns></returns>
        protected virtual bool EvaluateCanInvokeActionbarButton(IActionButton<T> actionbutton)
        {
            if (UseGlobalCooldown)
            {
                if (Time.time < CurrentGlobalCooldown)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Perfect for invoking that a button has been pressed from UI element.
        /// </summary>
        public abstract void InvokeActionButton(int actionIndex);
        
        /// <summary>
        /// Method used to create an action bar based on implementation. Use external file, or class to define actionbar buttons/indexes.
        /// </summary>
        public abstract void InitializeActionbar();
    }
}