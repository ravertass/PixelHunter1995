using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PixelHunter1995.Inputs
{
    using AllKeys = Either<Keys, MouseKeys>;

    class Input : StateMap<AllKeys>
    {

        public Hotkeys Hotkeys { get; }

        public int X { get; private set; }
        public int Y { get; private set; }
        public Point Position { get => new Point(this.X, this.Y); }

        public int ScrollWheelValue { get; private set; }
        public int HorizontalScrollWheelValue { get; private set; }

        public Input()
        {
            this.Hotkeys = new Hotkeys();
        }

        //? Perhaps make this track input as a queue, to persist chronological order.
        //? Can even allow for pressing, releasing, and pressing again, inbetween an object checking, and still being caught.
        //? Can either work by the class being updated per-frame/per-timer/event-based, or queues are ignored entirely and instead make asking for an input check current state and compare to last time.
        //! Won't work (well), as monogame just supplies "these keys are currently pressed",
        //! and no way to poll them better than per-frame. As input is handled per-frame anyway, a queue is useless.
        //?
        //? To compensate, they can either be cleared periodically (ie. every x frames), and otherwise emptied as stuff 'consume' an input.
        //? Stuff would have to keep track of their pointer of where they are in the queue, and when something wants to consume, stuff gets complicated.
        //? If consuming (by emptying the queue) isn't possilbe, this could for example allow for histories of the players actions, which in turn allows for replays or telemetry.
        //! Overly complicated, particulary for our use-case. The only reason I could ever see this used, is for said replay and telemetry.
        //! But that can easily be done in other ways too, and I don't really see a charm in a input-by-input replay of a point-and-click game anyway...
        //?
        //? When it comes to multiple objects caring about same input, where one is intended to block the other from acting on it (ie. a text-field with focus), a good system has to be made.
        //? If it is more event-based, where they either listen for a press, or implement a callback that is called every frame to handleInput(), then the order would clearly matter.
        //? One option is having a stack of event-listeners. When something should steal input (ie. textbox gets focus), it is added to the stack, where only first element is triggered (can be a container of several things).
        //? When it loses focus, it is simply popped from the stack. While if it should bleed inputs it doesn't care about down the stack, it can do so explicitly in some way.
        //? This makes the priority-order more explicit, for better or worse.
        //? Another way to make a system allowing something to consume an event, is to have a list of thingsh andling input, where a bool is passed along that is set to false if later ones shouldn't handle the input, and then return it so prior ones can see that something later in the chain didnt want it to handle it.
        //? Essentially, each thing says what it should consume, pass the event (and consumed stuff) along to the next, receives stuff consumed by later things, and then actually handles input where `(in && out) == true` is used to check if something was consumed.
        //? Such a system would obviously in no way have priorities or similar, so if 2 wants to consume the same, they will fight undefinedly.
        //? A simpler way is to simply have two events, where stuff that want to consume happens in the pre- one, and other stuff in the post- one,
        //? Again no real priorities in this system (or rather, there is implicitly a single level of priority), but far more straightforward, and should suffice for something like hotkeys consuming events before low-prio ones.
        //! Most options are overly complicated, but something like passing input through the states (like update) should keep stuff sepparate enough to avoid most collisions (clicking in pausemenu vs Playing),
        //! and hotkeys (like fullscreen) could be some hackon system that is checked before anything else and actually consumes.
        //?
        //? As for giving stuff priority or other conflict-resolutions, I doubt anything in the scope of the game will require such a thing.
        //? The only things I can imagine would need to consume input at all is the mentioned text-field (if some puzzle wants player to input a text), or hotkeys (like how fullscreens alt+enter should not let menu's enter still work).
        //? As in, stuff that might tbh make more sense to simply solve by letting them be aware of eachother, and see if someone else has focus or otherwise seems more relevant. Or have inherent ordering.
        //? The text-field example might even be better solved through adding a specific State for it.
        //? As for mouse-clicks of overlapping elements, that can be solved by everything from simply checking the z-indices, to more complicated ways, to ignoring issues.
        //?
        //? Gamepad support? Should be easy to add, but what use is it? At best I suppose it can be allowed to control the pointer?
        //? As for the of the buttons, it would simply need to be bound to actions.
        //?
        //TODO Currently, I am mostly leaning towards a simple per-frame update of input.
        //TODO This would happen before everything else `Update`s, so they can and should poll input in their normal update method.
        //TODO Nothing is allowed to truly 'consume' an input, and such cases is instead solved by giving them too much awareness of eachother, because the issue is so niche.
        //TODO Hoykeys might be the only exception, instead being handled separately directly after inputs updated before anything else, and actually ocnsuming the inputs they used in some way.
        //TODO Perhaps by removing them from the state dictionary, or perhaps by somehow adding a flag for it that a hoykey used it (allows things to still use it if they know they really want to).
        //TODO Could also set its own flag instead, so when checking an input one might also check specifically that hotkeys that shouldn't occur at same time didn't.
        //TODO Should be more benign and avoid bugs when keys are re-bound, but also more work as each hotkey has to be considered when coding every input-usage (luckily I don't expect many hotkeys, so still an option).


        /// <summary>
        /// Called to update the state of inputs. Perhaps even more often than per-frame? Event-based?
        /// </summary>
        public void Update(Game game, GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();
            var mouseState = Mouse.GetState();

            // Handle mouse
            var pressedMouse = new Dictionary<MouseKeys, ButtonState>()
            {
                [MouseKeys.LeftButton] = mouseState.LeftButton,
                [MouseKeys.RightButton] = mouseState.RightButton,
                [MouseKeys.MiddleButton] = mouseState.MiddleButton,
                [MouseKeys.XButton1] = mouseState.XButton1,
                [MouseKeys.XButton2] = mouseState.XButton2,
            }.Where(kv => kv.Value == ButtonState.Pressed).Select(kv => (AllKeys) kv.Key);

            // Merge keyboard and mouse
            var pressedKeys = pressedMouse.Concat(keyboardState.GetPressedKeys().Select(k => (AllKeys) k));
           
            this.HandleKeys(pressedKeys);

            // fix fullscreen (moved to static class, in case other things need to do it. also removes the need for a Screen reference.)
            //double ratioWidth = GlobalSettings.WINDOW_WIDTH / (double)screen.Width;
            //double ratioHeight = GlobalSettings.WINDOW_HEIGHT / (double)screen.Height;
            //this.X = (int)(mouseState.X * ratioWidth);
            //this.Y = (int)(mouseState.Y * ratioHeight);

            this.X = FullscreenFix.GetFixedX(mouseState.X);
            this.Y = FullscreenFix.GetFixedX(mouseState.Y);

            this.ScrollWheelValue = mouseState.ScrollWheelValue;
            this.HorizontalScrollWheelValue = mouseState.HorizontalScrollWheelValue;

            this.Hotkeys.HandleInput(gameTime, this);
        }

    }

    ///// <summary>
    ///// The proper keystate, as monogame does not give any information on whether the key was released or pressed.
    ///// Rather than an enum like one would expect, this is a class, meaning each ProperKeyState is an instance.
    ///// There are static references for each permutation to instances describing them, to allow easier use.
    ///// 
    ///// The advantage with doing it this way however, is that Properties can be used to nicely get a bool for various
    ///// properties of the keystate.
    ///// So `input.GetKeyState(key) == ProperKeyState.EdgeDown` becomes `input.GetKeyState(key).IsEdgeDown` instead.
    ///// As a result however, it makes much less sense to enable 'Is' syntax (ie `input.IsKeyState(key, state)`),
    ///// although nothing stops one from implementing that as options/aliases of the former.
    ///// 
    ///// The disadvantage could have been that one has to be careful about making sure to use the static references,
    ///// as otherwise `==` or `.Equals` would compare reference and not value.
    ///// This has however been rendered moot by overriding the relevant methods,
    ///// as well as by turning it into a struct (making it a value-type).
    ///// </summary>
    //class ProperKeyState : SignalState
    //{
    //    public ProperKeyState(bool isUp, bool isEdge) : base(isUp, isEdge) { }
    //    public ProperKeyState(SignalState blah) : base(blah.IsUp, blah.IsEdge) { }

    //    public static ProperKeyState Up = new ProperKeyState(SignalState.Up);
    //    public static ProperKeyState Down = new ProperKeyState(SignalState.Down);
    //    public static ProperKeyState EdgeUp = new ProperKeyState(SignalState.EdgeUp);
    //    public static ProperKeyState EdgeDown = new ProperKeyState(SignalState.EdgeDown);

    //    //public static implicit operator ProperKeyState(SignalState blah) => (ProperKeyState)blah;
    //}
    //struct ProperKeyState // doesn't work, cant see a way to implicitly convert on the lhs of `.` (for properties and fields)
    //{
    //    private SignalState wrapped;

    //    public ProperKeyState(SignalState wrapped)
    //    {
    //        this.wrapped = wrapped;
    //    }
    //    public ProperKeyState(bool isUp, bool isEdge)
    //    {
    //        this.wrapped = new SignalState(isUp, isEdge);
    //    }

    //    public static ProperKeyState Up = SignalState.Up;
    //    public static ProperKeyState Down = SignalState.Down;
    //    public static ProperKeyState EdgeUp = SignalState.EdgeUp;
    //    public static ProperKeyState EdgeDown = SignalState.EdgeDown;

    //    //public override string ToString()
    //    //{
    //    //    return "ProperKeyState." + (this.IsEdge ? "Edge" : "") + (this.IsUp ? "Up" : "Down");
    //    //}
    //    public static implicit operator ProperKeyState(SignalState blah) => new ProperKeyState(blah);
    //    public static implicit operator SignalState(ProperKeyState blah) => blah.wrapped;
    //}

    /// <summary>
    /// An enum for the various keys on a mouse,
    /// as well as `None` for when one wants to make it explicitly that it relates to none.
    /// </summary>
    enum MouseKeys
    {
        None,
        LeftButton,
        RightButton,
        MiddleButton,
        XButton1,
        XButton2
    }
}
