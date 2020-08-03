using System.Collections.Generic;
using System.Linq;

namespace PixelHunter1995.Inputs
{
    class StateMap<T>
    {

        /// <summary>
        /// A dictionary where T maps to a corresponding state.
        /// This is used to compare current state to the prior state,
        /// allowing to know if the state changed or not.
        /// 
        /// If an item is not in the dictionary, its state is `SignalState.Up`.
        /// </summary>
        private readonly Dictionary<T, SignalState> state = new Dictionary<T, SignalState>();

        public StateMap()
        {
        }

        protected void Update(IEnumerable<T> activeItems)
        {
            var releasedItems = state.Keys.Except(activeItems);

            // Active items are considered Down (or EdgeDown),
            // as ones in the dictionary are defined as Up.
            this.Update(activeItems, SignalState.Down);

            // Items that were stored as something other than ProperKeyState.Up,
            // but that are not currently active. Should be EdgeUp or Up.
            this.Update(releasedItems, SignalState.Up);
        }

        private void Update(IEnumerable<T> list, bool isUp)
        {
            // ToList ensures it is a copy, as `state` is mutated during the iteration and `list` might depend on it.
            // Specifically, `state.Keys.Except(activeItems)` returns an IEnumerable that does, and has to be copied.
            foreach (var item in list.ToList())
            {
                bool success = state.TryGetValue(item, out var priorState);
                if (!success)
                {
                    // If not in dict, the item is considered to be Up.
                    priorState = SignalState.Up;
                }

                var currentState = new SignalState(isUp, priorState.IsUp != isUp);
                if (currentState.Equals(SignalState.Up))
                {
                    state.Remove(item);
                    //Console.WriteLine("item: " + item + " - state: " + currentState + " - Removed from dict!");
                }
                else
                {
                    state[item] = currentState;
                    //Console.WriteLine("item: " + item + " - state: " + currentState);
                }
            }
        }
        private void Update(IEnumerable<T> list, SignalState state)
        {
            Update(list, state.IsUp);
        }


        public SignalState GetState(T item)
        {
            bool success = state.TryGetValue(item, out var itemState);
            if (success)
            {
                return itemState;
            }
            return SignalState.Up;
        }

        public bool isPressed(T item)
        {
            return GetState(item).IsEdgeDown;
        }
        public bool isDown(T item)
        {
            return GetState(item).IsDown;
        }
        public bool isStrictlyDown(T item)
        {
            return GetState(item).IsHeldDown;
        }
        public bool IsReleased(T item)
        {
            return GetState(item).IsEdgeUp;
        }
        public bool isUp(T item)
        {
            return GetState(item).IsUp;
        }
        public bool isStrictlyUp(T item)
        {
            return GetState(item).IsHeldUp;
        }

    }
}
