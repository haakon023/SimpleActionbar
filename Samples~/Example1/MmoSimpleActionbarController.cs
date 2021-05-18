using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace SimpleActionbar.ExampleCode
{
    public class MmoSimpleActionbarController : SimpleActionbarController<IAction>
    {
        private static MmoSimpleActionbarController _instance;
        private IActionButtonMapper<string> _mapper;
        
        [SerializeField]
        private int _actionButtonCount;

        public static MmoSimpleActionbarController Instance
        {
            get { return _instance; }
        }

        public int ActionButtonCount
        {
            get => _actionButtonCount;
            set => _actionButtonCount = value;
        }


        protected override void OnUseActionButton(UseActionButtonEventArgs args)
        {
            var action = ActionButtonIndexes[args.Index];
            
            //duration 0 to indicate that a button has been pressed but has no cooldown, and as such will not display any cooldown text
            //Only cooldown will be global cooldown if it is set to true
            base.OnUseActionButton(args);
            
            if (EvaluateCanInvokeActionbarButton(action))
            {
                if (!action.Execute())
                    return;

                base.OnUseActionButton(args);
                    
                if (UseGlobalCooldown)
                {
                    OnGlobalCooldown(new GlobalCooldownEventArgs()
                    {
                        Duration = GlobalCooldownLength
                    });
                    
                    CurrentGlobalCooldown = Time.time + GlobalCooldownLength;
                }
            }
        }

        protected override bool EvaluateCanInvokeActionbarButton(IActionButton<IAction> actionbutton)
        {
            if (actionbutton == null)
                return false;

            if (Time.time < CurrentGlobalCooldown)
                return false;
            
            if (actionbutton.ActionToExecute?.RemainingCooldown > 0)
                return false;

            return true;
        }

        public override void OnRemoveActionFromActionButton(RemoveActionFromActionButtonArgs args)
        {
            var action = ActionButtonIndexes[args.Index];
            action.ActionToExecute = null;
            base.OnRemoveActionFromActionButton(new RemoveActionFromActionButtonArgs()
            {
                Index = action.Index,
            });
        }
        
        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this);
            }
            else
            {
                _instance = this;
            }
            InitializeActionbar();
            _mapper = new ActionButtonMapper();
            PlayerInputManager.ActionBarEvent += OnActionButtonPressedMapper;
        }
        
        public override void OnAddActionToActionButton(AddActionToActionButtonArgs<IAction> args)
        {
            var action = ActionButtonIndexes[args.Index];
            action.ActionToExecute = args.Action;
            base.OnAddActionToActionButton(args);
        }

        private void OnActionButtonPressedMapper(string input)
        {
            if (!_mapper.Output(input, out var index))
            {
                Debug.LogWarning("Index did not exist in mapping");    
            }
            OnUseActionButton(new UseActionButtonEventArgs
            {
                Index = index
            });
        }

        public override void InvokeActionButton(int actionButton)
        {
            OnUseActionButton(new UseActionButtonEventArgs
            {
                Index = actionButton
            });
        }

        public override void InitializeActionbar()
        {
            ActionButtonIndexes = new ActionButton [ActionButtonCount];
            for (var i = 0; i < ActionButtonIndexes.Length; i++)
            {
                ActionButtonIndexes[i] = new ActionButton
                {
                    Index = i,
                    ActionToExecute = null,
                    Disabled = false,
                    CooldownLabel = "5",
                    ButtonKeyBindingLabel = $"{i}"
                };
            }
        }
    }
}