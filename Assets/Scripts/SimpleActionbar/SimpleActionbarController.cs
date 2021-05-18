using System;
using System.Collections.Generic;
using System.Security;
using JetBrains.Annotations;
using SimpleActionbar.ExampleCode;
using UnityEngine;

namespace SimpleActionbar
{
    /// <summary>
    /// This solution is based on the new input system of unity
    /// ActionButton names has been defined in InputActionAsset
    /// The Generic T is to define what IActionButton should trigger whenever it was executed
    /// </summary>
    public abstract class SimpleActionbarController<T> : MonoBehaviour
    {
        public IActionButton<T>[] ActionButtonIndexes { get; protected set; }
                
        public event EventHandler<ActionButtonEventArgs> UseActionButtonEvent;
        
        public event EventHandler<GlobalCooldownEventArgs> GlobalCooldownEvent;
        
        public event EventHandler<AddActionToActionButtonArgs<T>> AddActionToActionButtonEvent;
        
        public event EventHandler<RemoveActionFromActionButtonArgs<T>> RemoveActionFromActionButtonEvent;

        public event EventHandler<SetActionButtonKeybindLabelEventArgs> SetActionButtonKeybindLabelEvent;
        
        public event EventHandler<SetActionButtonDisabledEvent> SetActionButtonDisabledEvent;
        
        protected virtual void OnAddActionToActionButton(AddActionToActionButtonArgs<T> args)
        {
            var handler = AddActionToActionButtonEvent;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        protected virtual void OnRemoveActionFromActionButton(RemoveActionFromActionButtonArgs<T> args)
        {
            var handler = RemoveActionFromActionButtonEvent;
            if (handler != null)
            {
                handler(this, args);
            }
        }
        
        protected virtual void OnUseActionButton(ActionButtonEventArgs args)
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

        public virtual void OnSetActionButtonKeyBindLabel(SetActionButtonKeybindLabelEventArgs args)
        {
            var handler = SetActionButtonKeybindLabelEvent;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        public virtual void OnSetActionButtonDisabled(SetActionButtonDisabledEvent args)
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
        /// Listens for an event with whichever ActionButton that has been pressed from an external Input manager.
        /// </summary>
        /// <param name="actionIndex"></param>
        protected virtual void OnActionButtonPressed(int actionIndex)
        {
            //evaluate if you can click a action button
            //can be a cooldown, a global cooldown
            //or simply that the object is already selected
            if (EvaluateCanInvokeActionbarButton(null))
            {
                OnUseActionButton(new ActionButtonEventArgs(){ Index = actionIndex, Duration = 2 });
            }
        }

        /// <summary>
        /// Perfect for invoking that a button has been pressed from UI element.
        /// </summary>
        public abstract void InvokeActionButton(int actionIndex);

        /// <summary>
        /// Adds a generic action T to the actionbar, ActionButton defined by <paramref name="actionIndex"/>.
        /// </summary>
        /// <param name="actionToAdd"></param>
        /// <param name="actionIndex"></param>
        public abstract void AddActionToActionBarButton(T actionToAdd, int actionIndex);
        
        /// <summary>
        /// Removes generic Action T from the Action bar, ActionButton defined by <paramref name="actionIndex"/>.
        /// </summary>
        /// <param name="actionToRemove"></param>
        /// <param name="actionIndex"></param>
        public abstract void RemoveActionFromActionbarButton(int actionIndex);
        
        /// <summary>
        /// Method used to create an action bar based on implementation. Use external file, or class to define actionbar buttons/indexes.
        /// </summary>
        public abstract void InitializeActionbar();
    }
}