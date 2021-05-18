using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SimpleActionbar;
using SimpleActionbar.ExampleCode;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;

namespace SimpleActionbar.ExampleCode
{

    public class ActionbarView : MonoBehaviour
    {
        private VisualElement _root;
        private Dictionary<int, VisualElement> _actionbarIndexes = new Dictionary<int, VisualElement>();
        private MmoSimpleActionbarController _mmoSimpleActionbarController;

        private void Start()
        {
            _mmoSimpleActionbarController = MmoSimpleActionbarController.Instance;
            _mmoSimpleActionbarController.UseActionButtonEvent += UseActionButtonEvent;
            _mmoSimpleActionbarController.GlobalCooldownEvent += OnGlobalCooldownInitiate;
            _mmoSimpleActionbarController.AddActionToActionButtonEvent += OnAddActionToActionButton;
            _mmoSimpleActionbarController.RemoveActionFromActionButtonEvent += OnRemoveActionFromActionButton;
            _mmoSimpleActionbarController.SetActionButtonKeybindLabelEvent += OnKeybindChanged;
            
            _root = GetComponent<UIDocument>().rootVisualElement;
            var i = 0;
            for (; i < _mmoSimpleActionbarController.ActionButtonCount; i++)
            {
                var name = $"ActionButton{i + 1}";
                var action = _root.Q<VisualElement>(name);

                //Set cooldown container to display none
                action.Q<VisualElement>("HotbarCooldownContainer").style.display = DisplayStyle.None;

                _actionbarIndexes.Add(i, action);

                action.RegisterCallback<ClickEvent>(HandleActionClicked);
            }

            StartCoroutine(RefreshActionBar());
        }

        private void OnKeybindChanged(object sender, SetActionButtonKeybindLabelArgs e)
        {
            if (_actionbarIndexes.TryGetValue(e.ActionIndex, out var element))
            {
                element.Q<Label>("hotbarKeyBindLabel").text = e.NewKeybind;
            }
            
        }

        private IEnumerator RefreshActionBar()
        {
            yield return new WaitForSeconds(0.1f);

            while (true)
            {
                foreach (var b in _mmoSimpleActionbarController?.ActionButtonIndexes.ToList())
                {
                    if (b.ActionToExecute == null)
                        continue;

                    if (!b.ActionToExecute.OnCooldown)
                        continue;


                    _actionbarIndexes.TryGetValue(b.Index, out var element);
                    if (element != null)
                    {
                        element.Q<VisualElement>("HotbarCooldownContainer").style.display = DisplayStyle.Flex;
                        element.Q<Label>("CooldownLabel").style.display = DisplayStyle.Flex;
                        element.Q<Label>("CooldownLabel").text = b.ActionToExecute.RemainingCooldown.ToString("0.0");
                    }
                }

                yield return new WaitForSeconds(0.1f);
            }
        }

        private void OnDestroy()
        {
            _mmoSimpleActionbarController.UseActionButtonEvent -= UseActionButtonEvent;
            _mmoSimpleActionbarController.GlobalCooldownEvent -= OnGlobalCooldownInitiate;
            _actionbarIndexes.Values.ToList().ForEach(v => v.UnregisterCallback<ClickEvent>(HandleActionClicked));
            _actionbarIndexes.Clear();
            StopCoroutine(RefreshActionBar());
        }

        public void OnAddActionToActionButton(object sender, AddActionToActionButtonArgs<IAction> args)
        {
            if (_actionbarIndexes.TryGetValue(args.Index, out var visualElement))
            {
                var texture = Resources.Load<Texture>(args.Action.ActionImagePath);
                visualElement.style.backgroundImage = (StyleBackground) texture;
            }
        }

        private void OnRemoveActionFromActionButton(object sender, RemoveActionFromActionButtonArgs args)
        {
            if (_actionbarIndexes.TryGetValue(args.Index, out var visualElement))
            {
                visualElement.style.backgroundImage = null;
            }
        }

        public void UseActionButtonEvent(object sender, UseActionButtonEventArgs args)
        {
            if (_actionbarIndexes.TryGetValue(args.Index, out var action))
            {
                action.experimental.animation.Start(
                        new StyleValues() {backgroundColor = Color.black, unityBackgroundImageTintColor = Color.black},
                        new StyleValues()
                            {backgroundColor = new Color(0, 0, 0, 0), unityBackgroundImageTintColor = Color.white},
                        1000)
                    .Ease(Easing.Linear);

                if (args.Duration > 0)
                    InitiateCooldown(args.Index, args.Duration, action);
            }
        }

        private void InitiateCooldown(int index, float duration, VisualElement action)
        {
            action.Q<Label>("CooldownLabel").style.display = DisplayStyle.Flex;
            CoolDownAnimation(action, duration, () => FinishCooldown(index));
        }

        public void OnGlobalCooldownInitiate(object sender, GlobalCooldownEventArgs args)
        {
            _root.Q<VisualElement>("Actionbar").Children().ToList().ForEach(v => CoolDownAnimation(v, args.Duration));
        }

        private void CoolDownAnimation(VisualElement action, float cooldown, Action callback = null)
        {
            var cooldownContainer = action.Q<VisualElement>("HotbarCooldownContainer");

            cooldownContainer.style.display = DisplayStyle.Flex;
            cooldownContainer.experimental.animation.Start(
                    new StyleValues {backgroundColor = Color.black},
                    new StyleValues {backgroundColor = Color.clear},
                    (int) (cooldown * 1000))
                .Ease(Easing.Linear)
                .OnCompleted(callback);
        }

        public void FinishCooldown(int index)
        {
            if (_actionbarIndexes.TryGetValue(index, out var action))
            {
                action.Q<Label>("CooldownLabel").style.display = DisplayStyle.None;
            }
        }

        private void HandleActionClicked(ClickEvent ev)
        {
            //New input system does not support the new UI system :(
            //atleast not Unity 2020.1.16f
            // _mmoHotbarController.InvokeActionButton(ev.target.ToString());
        }
    }
}