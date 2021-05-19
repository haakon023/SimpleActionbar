
# SimpleActionbar
Simple actionbar is a quick and easy way of setting up logic for your actionbar. You have to create 
your own implementation of SimpleActionbarController, and create your own view.

To view how this can be implemented, take a look at the Example code given. *Warning* The example code has dependencies, not having these installed with result in errors. Either install these dependencies, or remove the example code.


- UI Toolkit
- Newtonsoft.JSON
- UI Builder
## Install

To install the Simple Actionbar:
1. Open Unity
2. Open up Unity's Package Manager, under -> Window -> Package Manager
3. Click the `+` symbol, in top left corner and press "Add from git URL"
4. Paste in `https://github.com/haakon023/SimpleActionbar` 
5. Press `Add`
6. Optionally, you can add in my sample code, which has the dependencies described above.
 <p align="center">
 <img src="https://user-images.githubusercontent.com/20074902/118721036-a289a900-b82a-11eb-9f8e-19c5146e960a.png">
 </p>


## UML Class Diagram
<p align="center">
<img src="https://user-images.githubusercontent.com/20074902/118694399-53cd1680-b80c-11eb-830d-d44cb4fc8ebb.png">
</p>


## Getting started
As you can see in the above chart, `SimpleActionbarController<T>` is a generic class that has mutiple properties, events and methods that can be overriden, and some that has to be implemented.

To start using this, you create a new class that implements `SimpleActionbarController<T>`, for example `MyActionBarController : SimpleActionbarController<IAction>`
where `IAction` is a interface that describes the action that will be executed when a button is pressed.

From the `UML Diagram` section, you can see that `SimpleActionbarController` also is depending on an `IActionButton<T>` which is the container, that is placed on each button index.
That meaning that we also must create a class `MyActionButton : IActionButton<IAction>`
where `IAction` is the same interface that is being used by our new controller.

To let our controller know that an button has been pressed, it is a good practice to have a `InputManager` that can fire off events with either a string or integer that our controller can use as a mapping to each button.

An example implementation of the SimpleActionbar could look something like this:

<p align="center">
<img src="https://user-images.githubusercontent.com/20074902/118721764-8e927700-b82b-11eb-9881-ac1ab19511b4.png">
</p>

## Documentation

### Properties and fields

`public IActionButton<T>[] ActionButtonIndexes { get; protected set; }`

Is an Array of IActionButton, that also has to be implemented, given in the Example code aswell. This takes a `<T>` Which should be the `Action` that you want the button to Execute when it is invoked.

Next fields are protected since no other classes should need to access these, but implementing classes can override this.

`protected bool UseGlobalCooldown;`

Wether to use global cooldown or not, global cooldown is usually invoked after an action has been fired, making the user have to wait a certain amount of time before
a new action can be invoked.

`protected float GlobalCooldownLength`

The time a user have to wait untill next action can be invoked

`protected float CurrentGlobalCooldown;`

Used as a timer to calculate the remaining time an user needs to wait for global cooldown to be finished
In theory this can be set as either private or public, so that UI elements can use this to do animation calculations and such

### Methods

`protected virtual bool EvaluateCanInvokeActionbarButton(IActionButton<T> actionbutton)`

Evaluates if a ActionButton can be invoked, can check if button is disabled, has global cooldown, if the button has an action that can be triggered at all

`public abstract void InvokeActionButton(int actionIndex);` 

Can be invoked from clicking on the UI elements button

`public abstract void InitializeActionbar();`

Used for any initialization of the actionbar, populating the Array `ActionButtonIndexes`, reading from a players JSON file that contains details about said players actionbar, his keybindings, actions that is placed on the actionbar.

### Events

All events are using an `EventHandler<Args>` where `args` is a class that has properties about the event that is being triggered


`public event EventHandler<UseActionButtonEventArgs> UseActionButtonEvent;`

Can be subscribed by UI elements to know whenever a ActionButton has been invoked

The implementation is given below, should be called whenever a ActionButton is invoked where `T` is the action that is being added
```
public virtual void OnAddActionToActionButton(AddActionToActionButtonArgs<T> args)
{
    var handler = AddActionToActionButtonEvent;
    if (handler != null)
    {
        handler(this, args);
    }
}
``` 

The above code is also the way all the other events are implemented, and can be overriden by your own implementation to do more checks if a button can be invoked.
The other events are as following

`GlobalCooldownEvent;`

Lets subscribers of this event know that a global cooldown with a `float Duration` of time must pass before any new action can be invoked

`AddActionToActionButtonEvent;`

Lets subscribers of this event know that an action `T` has been added to an `IActionButton<T>`

`RemoveActionFromActionButtonEvent;` 

Lets subscribers know that an action `T` has been removed from an `IActionButton<T>`

`SetActionButtonKeybindLabelEvent;`

Lets subscribers know that an `IActionButton<T>` has changed its keybind

`SetActionButtonDisabledEvent;`

Lets subscribers know that an `IActionButton<T>` has been disabled/undisabled
