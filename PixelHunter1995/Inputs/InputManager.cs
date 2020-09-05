using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using PixelHunter1995.Utilities;

namespace PixelHunter1995.Inputs
{
    using AllKeys = Either<Keys, MouseKeys>;
    using KeyDisjunction = List<Dictionary<Either<Keys, MouseKeys>, SignalState>>;
    using KeyConjunction = Dictionary<Either<Keys, MouseKeys>, SignalState>;

    /// <summary>
    /// The class used to get the state of `InputCommand`s,
    /// wraps a `StateMap` for the underlying state management,
    /// and an `InputReader` to read inputdevices for checking the bindings.
    /// </summary>
    class InputManager
    {
        
        public StateMap<InputCommand> Statemap { get; }
        public InputReader Input { get; }
        
        public int MouseX { get => this.Input.MouseX; }
        public int MouseY { get => this.Input.MouseY; }
        public int MouseSceneX { get => this.Input.MouseSceneX; }
        public int MouseSceneY { get => this.Input.MouseSceneY; }
        public int ScrollWheelValue { get => this.Input.ScrollWheelValue; }
        public int ScrollWheelDelta { get => this.Input.ScrollWheelDelta; }
        public int ScrollWheelTicksDelta { get => this.Input.ScrollWheelTicksDelta; }
        
        private readonly Dictionary<InputCommand, KeyDisjunction> bindings =
                new Dictionary<InputCommand, KeyDisjunction>();

        public InputManager(Dictionary<InputCommand, KeyDisjunction> bindings)
                : this(bindings, new InputReader()) { }
        public InputManager(Dictionary<InputCommand, KeyDisjunction> bindings, InputReader input)
        {
            this.bindings = bindings;
            this.Input = input;
            this.Statemap = new StateMap<InputCommand>();
        }

        public void Update()
        {
            this.Input.Update();
            
            var activeCommands = new HashSet<InputCommand>();
            foreach (var item in bindings)
            {
                var action = item.Key;
                KeyDisjunction bind = item.Value;

                bool any = false;
                foreach (KeyConjunction disjunction in bind)
                {
                    bool all = true;
                    foreach (var item2 in disjunction)
                    {
                        AllKeys key = item2.Key;
                        SignalState desiredState = item2.Value;
                        SignalState state = this.Input.GetKeyState(key);
                        // Current state's edge only matters if an edge is desired.
                        if (!desiredState.NonStrictEquals(state))
                        {
                            all = false;
                            break; // A part of the keybind was not pressed.
                        }
                    }
                    if (all) {
                        any = true;
                        break; // No need to test every conditional in an or-statement
                    }
                }

                if (any)
                {
                    activeCommands.Add(action);
                }
            }

            this.Update(activeCommands);
        }

        private void Update(HashSet<InputCommand> activeCommands)
        {
            this.Statemap.Update(activeCommands);
        }

        public SignalState GetState(InputCommand cmd)
        {
            return this.Statemap.GetState(cmd);
        }

        /// <summary>
        /// A method that returned an object with extra-data was requested,
        /// for some reason.
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public EventData GetEvent(InputCommand cmd)
        {
            var state = this.GetState(cmd);
            return new EventData(state, this.MouseX, this.MouseY, this.ScrollWheelValue);
        }
        /// <summary>
        /// A method that returned an object with extra-data was requested,
        /// for some reason.
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public EventData GetEvent(AllKeys key)
        {
            var state = this.Input.GetKeyState(key);
            return new EventData(state, this.MouseX, this.MouseY, this.ScrollWheelValue);
        }
        
        /// <summary>
        /// A method specifically to connect LeftButton to mouse-pos was requested,
        /// for some reason.
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public EventData GetClick(InputCommand cmd)
        {
            return this.GetEvent(MouseKeys.LeftButton);
        }
    }
    
    class EventData
    {
        public SignalState State { get; }
        public int MouseX { get; }
        public int MouseY { get; }
        public int ScrollWheelValue { get; }
        
        public EventData(SignalState state, int mouseX, int mouseY, int scroll)
        {
            this.State = state;
            this.MouseX = mouseX;
            this.MouseY = mouseY;
            this.ScrollWheelValue = scroll;
        }
    }
}
