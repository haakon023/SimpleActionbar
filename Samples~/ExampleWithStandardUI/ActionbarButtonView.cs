using System;
using System.Collections;
using SimpleActionbar.ExampleCode;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// ReSharper disable once CheckNamespace
namespace SimpleActionbar.ExampleCode
{
    public class ActionbarButtonView : MonoBehaviour
    {
        private IActionButton<IAction> _buttonModel;
        private Image _actionImage;
        private Image _cooldownPanel;
        private TMP_Text _cooldownLabel;
        private TMP_Text _keybindText;
        public string SetKeybindTextLabel
        {
            set
            {
                _keybindText.text = value;
            }
        }

        public IActionButton<IAction> ButtonModel
        {
            get => _buttonModel;
            set
            {
                _buttonModel = value;
                Setup();
            }
            
        }

        private void Setup()
        {
            _cooldownLabel = transform.GetChild(3).GetComponent<TMP_Text>();
            _cooldownLabel.text = "";

            _cooldownPanel = transform.GetChild(1).GetComponent<Image>();
            
            if (_keybindText == null)
            {
                _keybindText = transform.GetChild(2).GetComponent<TMP_Text>();
            }
            _keybindText.text = ButtonModel.ButtonKeyBindingLabel;
        }

        public void AddAction()
        {
            if (_buttonModel.ActionToExecute == null) return;
            
            _actionImage = transform.GetChild(0).GetComponent<Image>();
            _actionImage.sprite = _buttonModel.ActionToExecute.ActionImage;
        }

        public void Clear()
        {
            _actionImage = null;
            _cooldownLabel.text = "";
        }

        public IEnumerator DoCooldownIteration(float duration)
        {
            var durationTotalTime = duration;

            if (_cooldownPanel.fillAmount > 0)
                yield break;
            
            _cooldownPanel.fillAmount = 1; 
            while (durationTotalTime > 0)
            {
                durationTotalTime -= Time.deltaTime;
                _cooldownPanel.fillAmount -= 1.0f / duration * Time.deltaTime;
                _cooldownLabel.text = durationTotalTime.ToString("0.0");
                yield return null;
            }

            _cooldownPanel.fillAmount = 0;
            _cooldownLabel.text = "";
        }
    }
}