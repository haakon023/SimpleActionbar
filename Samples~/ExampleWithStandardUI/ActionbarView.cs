using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace SimpleActionbar.ExampleCode
{

    public class ActionbarView : MonoBehaviour
    {
        private Dictionary<int, ActionbarButtonView> _actionbarButtons = new Dictionary<int, ActionbarButtonView>();
        private MmoSimpleActionbarController _actionbarController;

        private void Start()
        {
            //initialize UI
            _actionbarController = MmoSimpleActionbarController.Instance;

            _actionbarController.AddActionToActionButtonEvent += ActionbarControllerOnAddActionToActionButtonEvent;
            _actionbarController.SetActionButtonDisabledEvent += ActionbarControllerOnSetActionButtonDisabledEvent;
            _actionbarController.SetActionButtonKeybindLabelEvent += ActionbarControllerOnSetActionButtonKeybindLabelEvent;
            _actionbarController.RemoveActionFromActionButtonEvent += ActionbarControllerOnRemoveActionFromActionButtonEvent;
            _actionbarController.GlobalCooldownEvent += ActionbarControllerOnGlobalCooldownEvent;
            _actionbarController.UseActionButtonEvent += ActionbarControllerOnUseActionButtonEvent;
            _actionbarController.OnActionbarInitialized += OnActionbarInitialized;
        }

        private void OnDestroy()
        {
            _actionbarController.AddActionToActionButtonEvent -= ActionbarControllerOnAddActionToActionButtonEvent;
            _actionbarController.SetActionButtonDisabledEvent -= ActionbarControllerOnSetActionButtonDisabledEvent;
            _actionbarController.SetActionButtonKeybindLabelEvent -= ActionbarControllerOnSetActionButtonKeybindLabelEvent;
            _actionbarController.RemoveActionFromActionButtonEvent -= ActionbarControllerOnRemoveActionFromActionButtonEvent;
            _actionbarController.GlobalCooldownEvent -= ActionbarControllerOnGlobalCooldownEvent;
            _actionbarController.UseActionButtonEvent -= ActionbarControllerOnUseActionButtonEvent;
            _actionbarController.OnActionbarInitialized -= OnActionbarInitialized;
        }

        private void OnActionbarInitialized()
        {
            var count = _actionbarController.ActionButtonCount;
            for (var i = 0; i < count; i++)
            {
                // lambda Closure issue
                var copyIndex  = i;
                var buttonView = transform.Find($"ActionButton{i + 1}");
                var component = buttonView.GetComponent<ActionbarButtonView>();
                component.ButtonModel = _actionbarController.ActionButtonIndexes[i];
                component.GetComponent<Button>().onClick.AddListener(() => _actionbarController.InvokeActionButton(copyIndex));
                _actionbarButtons.Add(i, component);
            }
        }

        private void ActionbarControllerOnRemoveActionFromActionButtonEvent(object sender, RemoveActionFromActionButtonArgs e)
        {
            _actionbarButtons[e.Index].Clear();
        }

        private void ActionbarControllerOnSetActionButtonKeybindLabelEvent(object sender, SetActionButtonKeybindLabelArgs e)
        {
            _actionbarButtons[e.ActionIndex].SetKeybindTextLabel = e.NewKeybind;
        }

        private void ActionbarControllerOnSetActionButtonDisabledEvent(object sender, SetActionButtonDisabledArgs e)
        {
            if (_actionbarButtons.TryGetValue(e.ActionIndex, out var value))
            {
                value.ButtonModel.Disabled = e.Disabled;
            }
        }

        private void ActionbarControllerOnGlobalCooldownEvent(object sender, GlobalCooldownEventArgs e)
        {
            foreach (var button in _actionbarButtons)
            {
                if(button.Value.ButtonModel.ActionToExecute.IgnoreGlobalCooldown)
                    continue;
                StartCoroutine(button.Value.DoCooldownIteration(e.Duration));
            }
        }

        private void ActionbarControllerOnUseActionButtonEvent(object sender, UseActionButtonEventArgs e)
        {
            if (_actionbarButtons.TryGetValue(e.Index, out var value))
            {
                StartCoroutine(value.DoCooldownIteration(e.Duration));
            }
        }

        private void ActionbarControllerOnAddActionToActionButtonEvent(object sender, AddActionToActionButtonArgs<IAction> e)
        {
            if(_actionbarButtons.TryGetValue(e.Index, out var value))
            {
                value.AddAction();
            }
        }
    }
}