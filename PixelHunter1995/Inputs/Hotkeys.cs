using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace PixelHunter1995.Inputs
{
    using AllKeys = Either<Keys, MouseKeys>;
    //using KeyDisjunction = Either<AllKeys, HashSet<AllKeys>>;
    using KeyDisjunction = Dictionary<Either<Keys, MouseKeys>, SignalState>;
    using KeyConjunction = HashSet<Dictionary<Either<Keys, MouseKeys>, SignalState>>;

    class Hotkeys : StateMap<Actions>
    {

        private readonly Dictionary<Actions, KeyConjunction> binds = new Dictionary<Actions, KeyConjunction>();

        public Hotkeys()
        {
            // TODO Currently defined here, should probably be part of a config file...
            
            //! No inherent support for chords
            binds[Actions.Exit] = new KeyConjunction { new KeyDisjunction { [Keys.Escape] = SignalState.EdgeDown } }; // Feels like while the ability to specify edge is nice, the action should have an edge instead
            binds[Actions.Pause] = new KeyConjunction { new KeyDisjunction { [Keys.Escape] = SignalState.EdgeDown } };
            binds[Actions.ToggleFullscreen] = new KeyConjunction { new KeyDisjunction { [Keys.LeftAlt] = SignalState.Down, [Keys.RightAlt] = SignalState.Down }, new KeyDisjunction { [Keys.Enter] = SignalState.EdgeDown } };
            binds[Actions.Accept] = new KeyConjunction { new KeyDisjunction { [Keys.LeftAlt] = SignalState.Up }, new KeyDisjunction { [Keys.RightAlt] = SignalState.Up }, new KeyDisjunction { [Keys.Enter] = SignalState.EdgeDown } }; //? keybinds can be explicitly made to avoid collisions... Should not be done in the binding imo. Even if the ability to is nice...
            binds[Actions.MouseLeft] = new KeyConjunction { new KeyDisjunction { [MouseKeys.LeftButton] = SignalState.Down } };
        }

        public void Update(Input input)
        {
            var activeActions = new HashSet<Actions>();

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


                    // TODO perform action
                    //? most likely done through Action behaviour defined elsewhere somewhere,
                    //? and retrieved with the enum (or enum is changed to be an interface for them,
                    //? where they add themselves to binds or similar), before told to run.
                }
            }

            this.Update(activeActions);
        }
    }

    // TODO Make its own class.
    // TODO Rename stuff (please give inputs! pun intended)
    // TODO Ideally, anything could be bound, and all 'inputs' were 'actions' instead of 'keys'
    // TODO Although mouse-position might be tricky to do so for :/
    // TODO And for writing/text-input it obviosuly wont work well.
    enum Actions
    {
        // Global
        Exit,
        ToggleFullscreen,
        QuickSave,
        QuickLoad,
        // Menu
        Back, // can be held?
        Resume, // is this equivalent to Action.Back when at the root?
        Accept,
        Decline,
        MenuNext, // can be held? // up/down?
        MenuPrior, // can be held? 
        MenuRight, // can be held // right/left? Like when controlling volume in options with arrowkeys
        MenuLeft, // can be held
        // Playing
        Pause,
        MouseLeft, // nothing says it has to be a mouse-button
        MouseRight,
        InventoryScrollUp,
        InventoryScrollDown,
        MonkeyIslandGive,// dummy actions for debugging purposes
        MonkeyIslandPickUp,
        MonkeyIslandUse,
        MonkeyIslandOpen,
        MonkeyIslandLookAt,
        MonkeyIslandPush,
        MonkeyIslandClose,
        MonkeyIslandTalkTo,
        MonkeyIslandPull,
        MoveUp, // Should it be possible to control without a mouse? Would this move the pointer then, or the character?
        MoveDown,
        MoveLeft,
        MoveRight,
        // Cutscene (dialog, credits, actual cutscenes, etc.) Most of those are actually same key/action, but writing down everything so I know what to think about.
        Skip, // skip to next part player can do anything
        JumpAhead, // next dialog please!
        Complete, // skip animation (ie. writing-animation, while remaining on same dialog to read it)!
        FastForward, // Hold to remove (most) delays
        HistoryBack, // Missed something?
        HistoryForward // same as jump ahead? Should not go further than where history started, if held
    }

}
