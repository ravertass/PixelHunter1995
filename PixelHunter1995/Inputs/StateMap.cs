using System.Collections.Generic;
using System.Linq;

namespace PixelHunter1995.Inputs
{
    class StateMap<T>
    {

        /// <summary>
        /// A dictionary where T maps to a corresponding state.
        /// This is used to compare current state to the prior state, allowing to know if the state changed or not.
        /// 
        /// It has been decided that the dictionary should not store keys not currently pressed,
        /// to avoid it increasing in size each time a new key is pressed until it potentially is as 'large' as
        /// the set of possible keys.
        /// As a result, if the key is not in the dictionary, its state is SignalState.Up.
        /// </summary>
        private readonly Dictionary<T, SignalState> state = new Dictionary<T, SignalState>();

        public StateMap()
        {
        }

        protected void Update(IEnumerable<T> pressedKeys)
        {
            var releasedKeys = state.Keys.Except(pressedKeys);

            //Keys currently pressed
            this.Update(pressedKeys, SignalState.Down);

            // Keys not currently pressed, but were stored as something other than ProperKeyState.Up
            this.Update(releasedKeys, SignalState.Up);
        }

        private void Update(IEnumerable<T> list, bool isUp)
        {
            // ToList ensures it is a copy, as `state` is mutated during the iteration and `list` might depend on it.
            // Specifically, `state.Keys.Except(pressedKeys)` returns an IEnumerable that does.
            foreach (var key in list.ToList())
            {
                bool success = state.TryGetValue(key, out var priorState);
                if (!success)
                {
                    priorState = SignalState.Up; // If not in dict, the key is considered to be Up.
                }

                var currentState = new SignalState(isUp, priorState.IsUp != isUp);
                if (currentState.Equals(SignalState.Up))
                {
                    state.Remove(key);
                    //Console.WriteLine("key: " + key + " - state: " + currentState + " - Removed from dict!");
                }
                else
                {
                    state[key] = currentState;
                    //Console.WriteLine("key: " + key + " - state: " + currentState);
                }
            }
        }
        private void Update(IEnumerable<T> list, SignalState state)
        {
            Update(list, state.IsUp);
        }


        public SignalState GetState(T key)
        {
            bool success = state.TryGetValue(key, out var keyState);
            if (success)
            {
                return keyState;
            }
            return SignalState.Up;
        }

        public bool isPressed(T key)
        {
            return GetState(key).IsEdgeDown;
        }
        public bool isDown(T key)
        {
            return GetState(key).IsDown;
        }
        public bool isStrictlyDown(T key)
        {
            return GetState(key).IsHeldDown;
        }
        public bool IsReleased(T key)
        {
            return GetState(key).IsEdgeUp;
        }
        public bool isUp(T key)
        {
            return GetState(key).IsUp;
        }
        public bool isStrictlyUp(T key)
        {
            return GetState(key).IsHeldUp;
        }

    }
}
