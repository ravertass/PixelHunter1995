using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using PixelHunter1995.Utilities;

namespace PixelHunter1995.Inputs
{
    using AllKeys = Either<Keys, MouseKeys>;
    using KeyDisjunction = List<Dictionary<Either<Keys, MouseKeys>, SignalState>>;
    using KeyConjunction = Dictionary<Either<Keys, MouseKeys>, SignalState>;

    class Actions : StateMap<Action>
    {

        private readonly Dictionary<Action, KeyDisjunction> binds = new Dictionary<Action, KeyDisjunction>();
        
        public Actions(Dictionary<Action, KeyDisjunction> binds)
        {
            this.binds = binds;
        }

        public void Update(Input input)
        {
            var activeActions = new HashSet<Action>();

            foreach (var item in binds)
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
                        SignalState state = input.GetState(key);
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
                    activeActions.Add(action);
                }
            }

            this.Update(activeActions);
        }
    }

}
