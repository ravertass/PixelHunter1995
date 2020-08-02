using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace PixelHunter1995.Inputs
{
    using AllKeys = Either<Keys, MouseKeys>;
    //using KeyDisjunction = Either<AllKeys, HashSet<AllKeys>>;
    using KeyDisjunction = Dictionary<Either<Keys, MouseKeys>, SignalState>;
    using KeyConjunction = HashSet<Dictionary<Either<Keys, MouseKeys>, SignalState>>;

    class Actions : StateMap<Action>
    {

        private readonly Dictionary<Action, KeyConjunction> binds = new Dictionary<Action, KeyConjunction>();

        // public Actions()
        // {
            // //! No inherent support for chords
            // this.binds[Action.Exit] = new KeyConjunction { new KeyDisjunction { [Keys.Escape] = SignalState.EdgeDown } }; // Feels like while the ability to specify edge is nice, the action should have an edge instead
            // this.binds[Action.Pause] = new KeyConjunction { new KeyDisjunction { [Keys.Escape] = SignalState.EdgeDown } };
            // this.binds[Action.ToggleFullscreen] = new KeyConjunction { new KeyDisjunction { [Keys.LeftAlt] = SignalState.Down, [Keys.RightAlt] = SignalState.Down }, new KeyDisjunction { [Keys.Enter] = SignalState.EdgeDown } };
            // this.binds[Action.Accept] = new KeyConjunction { new KeyDisjunction { [Keys.LeftAlt] = SignalState.Up }, new KeyDisjunction { [Keys.RightAlt] = SignalState.Up }, new KeyDisjunction { [Keys.Enter] = SignalState.EdgeDown } }; //? keybinds can be explicitly made to avoid collisions... Should not be done in the binding imo. Even if the ability to is nice...
            // this.binds[Action.MouseLeft] = new KeyConjunction { new KeyDisjunction { [MouseKeys.LeftButton] = SignalState.Down } };
        // }
        
        public Actions(Dictionary<Action, KeyConjunction> binds)
        {
            this.binds = binds;
        }

        public void Update(Input input)
        {
            var activeActions = new HashSet<Action>();

            foreach (var item in binds)
            {
                var action = item.Key;
                KeyConjunction bind = item.Value;

                bool active = true;
                foreach (KeyDisjunction disjunction in bind)
                {
                    bool any = false;
                    foreach (var item2 in disjunction)
                    {
                        AllKeys key = item2.Key;
                        SignalState desiredState = item2.Value;
                        SignalState state = input.GetState(key);
                        // Current state's edge only matters if an edge is desired.
                        if (desiredState.IsDown == state.IsDown && (desiredState.IsHeld || state.IsEdge))
                        {
                            any = true;
                            break; // No need to test every conditional in an or-statement
                        }
                    }
                    if (any == false) {
                        active = false;
                        break; // A part of the keybind was not pressed.
                    }
                }

                if (active)
                {
                    activeActions.Add(action);
                }
            }

            this.Update(activeActions);
        }
    }

}
