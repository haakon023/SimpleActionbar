using System;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

namespace SimpleActionbar.ExampleCode
{
    public class MmoSimpleActionbarController : SimpleActionbarController<IAction>
    {
        private static MmoSimpleActionbarController _instance;
        private IActionButtonMapper<string> _mapper;

        public event Action OnActionbarInitialized;
        
        
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

            if (!EvaluateCanInvokeActionbarButton(action)) return;
            
            if (!action.Execute()) return;
                
            args.Duration = action.ActionToExecute.ActionCooldown;
            base.OnUseActionButton(args);

            if (!UseGlobalCooldown) return;
            
            OnGlobalCooldown(new GlobalCooldownEventArgs()
            {
                Duration = GlobalCooldownLength
            });
            CurrentGlobalCooldown = Time.time + GlobalCooldownLength;
        }

        protected override bool EvaluateCanInvokeActionbarButton(IActionButton<IAction> actionbutton)
        {
            if (actionbutton == null)
                return false;

            if (Time.time < CurrentGlobalCooldown && !actionbutton.ActionToExecute.IgnoreGlobalCooldown)
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
                    ButtonKeyBindingLabel = $"{i+1}"
                };
            }

            OnActionbarInitialized?.Invoke();

            OnAddActionToActionButton( new AddActionToActionButtonArgs<IAction>
            {
                Index = 0,
                Action = new SpellAction
                {
                    Spell = "Fireball",
                    ActionCooldown = 5,
                    ActionDescription = "A fireball",
                    ActionName = "test",
                    ActionImage = Resources.Load<Sprite>($"SpellIcons/Fireball")
                }
            });
            OnAddActionToActionButton( new AddActionToActionButtonArgs<IAction>
            {
                Index = 1,
                Action = new SpellAction
                {
                    Spell = "Rend",
                    ActionCooldown = 2,
                    ActionDescription = "Rend",
                    ActionName = "test",
                    ActionImage = Resources.Load<Sprite>($"SpellIcons/Rend")
                }
            });             OnAddActionToActionButton( new AddActionToActionButtonArgs<IAction>
            {
                Index = 2,
                Action = new SpellAction
                {
                    Spell = "Regrowth",
                    ActionCooldown = 1.5f,
                    ActionDescription = "A fireball",
                    ActionName = "test",
                    ActionImage = Resources.Load<Sprite>($"SpellIcons/Regrowth")
                }
            });
            OnAddActionToActionButton( new AddActionToActionButtonArgs<IAction>
            {
                Index = 3,
                Action = new SpellAction
                {
                    Spell = "Flamestrike",
                    ActionCooldown = 15,
                    ActionDescription = "Rend",
                    ActionName = "test",
                    ActionImage = Resources.Load<Sprite>($"SpellIcons/Flamestrike")
                }
            }); 
            OnAddActionToActionButton( new AddActionToActionButtonArgs<IAction>
            {
                Index = 4,
                Action = new SpellAction
                {
                    Spell = "Inferno",
                    ActionCooldown = 8,
                    ActionDescription = "A fireball",
                    ActionName = "test",
                    ActionImage = Resources.Load<Sprite>($"SpellIcons/Inferno")
                }
            });
            OnAddActionToActionButton( new AddActionToActionButtonArgs<IAction>
            {
                Index = 5,
                Action = new SpellAction
                {
                    Spell = "Cone Of Cold",
                    ActionCooldown = 2,
                    ActionDescription = "Rend",
                    ActionName = "test",
                    ActionImage = Resources.Load<Sprite>($"SpellIcons/ConeOfCold")
                }
            });
            OnAddActionToActionButton( new AddActionToActionButtonArgs<IAction>
            {
                Index = 6,
                Action = new SpellAction
                {
                    Spell = "Speed",
                    ActionCooldown = 30,
                    ActionDescription = "A fireball",
                    ActionName = "test",
                    IgnoreGlobalCooldown = true,
                    ActionImage = Resources.Load<Sprite>($"SpellIcons/Speed")
                }
            });
            OnAddActionToActionButton( new AddActionToActionButtonArgs<IAction>
            {
                Index = 7,
                Action = new SpellAction
                {
                    Spell = "SummonWraith",
                    ActionCooldown = 120,
                    ActionDescription = "Rend",
                    ActionName = "test",
                    ActionImage = Resources.Load<Sprite>($"SpellIcons/SummonWraith")
                }
            });
            OnAddActionToActionButton( new AddActionToActionButtonArgs<IAction>
            {
                Index = 8,
                Action = new SpellAction
                {
                    Spell = "Charge",
                    ActionCooldown = 12,
                    ActionDescription = "A fireball",
                    ActionName = "test",
                    ActionImage = Resources.Load<Sprite>($"SpellIcons/Charge")
                }
            });
            OnAddActionToActionButton( new AddActionToActionButtonArgs<IAction>
            {
                Index = 9,
                Action = new SpellAction
                {
                    Spell = "ChainLightning",
                    ActionCooldown = 7,
                    ActionDescription = "Rend",
                    ActionName = "test",
                    ActionImage = Resources.Load<Sprite>($"SpellIcons/ChainLightning")
                }
            }); 
        }
        
        private void Start() => InitializeActionbar();
    }
}