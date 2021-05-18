using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using UnityEngine;

namespace SimpleActionbar.ExampleCode
{
    public class PlayerDataController : MonoBehaviour
    {
        private void Start()
        {
            var data = new PlayerActionbarData()
            {
                ActionbarData = new ActionButtonData[] {
                    new ActionButtonData()
                    {
                        ActionBarIndex = 0,
                        Action = new SpellAction()
                        {
                            ActionName = $"{nameof(SpellAction)}",
                            ActionImagePath = "Fireball",
                            Spell = "Fireball (Rank 1)"
                        }
                    },
                    new ActionButtonData()
                    {
                        ActionBarIndex = 1,
                        Action = new SpellAction()
                        {
                            ActionName = "Comet",
                            ActionImagePath = "Comet",
                            Spell = "Comet"
                        }
                    },
                    new ActionButtonData()
                    {
                        ActionBarIndex = 2,
                        Action = new EquipAction()
                        {
                            ActionName = $" {nameof(EquipAction)}",
                            ActionImagePath = "Comet",
                            EquipmentName = "Sword"
                        }
                    }
                }
            };

            var json = JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto
            });

            // var writer = new StreamWriter(Application.dataPath + "/Scripts/SimpleActionBar/ExampleCode/ActionbarData.json");
            // writer.Write(json);
            // writer.Close();

            var actions = JsonConvert.DeserializeObject<PlayerActionbarData>(File.ReadAllText(Application.dataPath + "/Scripts/SimpleActionBar/ExampleCode/ActionbarData.json"), new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });

            var controller = MmoSimpleActionbarController.Instance;
            
            foreach (var action in actions.ActionbarData)
            {
                controller.OnAddActionToActionButton(new AddActionToActionButtonArgs<IAction>
                {
                    Action = action.Action,
                    Index = action.ActionBarIndex
                });
                controller.OnSetActionButtonKeyBindLabel(new SetActionButtonKeybindLabelArgs
                {
                    NewKeybind = action.KeyBind,
                    ActionIndex = action.ActionBarIndex
                });
            }
        }
    }
}