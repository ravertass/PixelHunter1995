using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PixelHunter1995
{
    class Input
    {
        
        private Game game;

        private Dictionary<Keys, KeyState> naiveState = new Dictionary<Keys, KeyState>();
        private Dictionary<Keys, ProperKeyState> properState = new Dictionary<Keys, ProperKeyState>();

        public Input(Game game)
        {
            this.game = game;
        }

        //? Perhaps make this track input as a queue, to persist chronological order.
        //? Can even allow for pressing, releasing, and pressing again, inbetween an object checking, and still being caught.
        //? Can either work by the class being updated per-frame/per-timer/event-based, or queues are ignored entirely and instead make asking for an input check current state and compare to last time.
        //?
        //? If using queues, they can either be cleared periodically (ie. per-frame), or emptied as stuff 'consume' an input.
        //? Most likely a combination of both would be used, to ensure old garbage doesn't pile up, but also that things can claim "Only I should see this".
        //? But it could also be essentialyl a memory-leak, and stuff has to keep track of their pointer of where they are in the queue.
        //? The latter is obviously problematic, but combinations of tactics might make something leaning towards that side actually work well.
        //? This could for example allow for histories of the players actions, which in turn allows for replays or telemetry.
        //?
        //? When it comes to multiple objects caring about same input, where one is intended to block the other from acting on it (ie. a text-field with focus), a good system has to be made. If it is more event-based, where thye either listen for a press, or implement a callback that is called every frame to handleInput(), then the order would clearly matter.
        //? For such a system to allow something to consume an event, it would need to pass along a bool that it sets to false if it doesnt want later ones to handle the input, and then return it so prior ones can see that something later in the chain didnt want it to handle it.
        //? Then each would need to see the given bool, and the returned bool (`(in && out) == true`) to know if anything wnats to consume it, and thus can handle it safely. Such a system would obviously in no way have priorities or similar, so if 2 wants to consume the same, they will fight undefinedly.
        //? Instead, it might be better to have a pre-event that can tell this class that it consumed an input, to remove it from the latter event. Again, no priorities or such, but a far more straightforward implementation.
        //?
        //? As for giving stuff priority or other conflict-resolutions, I doubt anything in the scope of the game will require such a thing.
        //? The only things I can imagine would need to consume input at all is the mentioned text-field (if some puzzle wants player to input a text), or hotkeys (like how fullscreens alt+enter should not let menu's enter still work).
        //? As in, stuff that might tbh make more sense to simply solve by letting them be aware of eachother, and see if someone else has focus or otherwise seems more relevant. Or have inherent ordering.
        //? As for mouse-clicks of overlapping elements, that can be solved by everything from simply checking the z-indices, to more complicated ways.
        //?
        //TODO Currently, I am mostly leaning towards a simplle per-frame update input.
        //TODO This would happen before everything else updates, so it can and should poll input in its normal update method.
        //TODO Nothing is allowed to truly 'consume' an input, and such cases is instead solved by giving them too much awareness of eachother, because the issue is so niche.
        //TODO Hoykeys might be the only exception, instead being handled separately directly after inputs updated, and actually ocnsuming the inputs they used in some way.
        //TODO Perhaps by removing them from this class, or probably by somehow adding a flag for it that a hoykey used it.
        //TODO with the latter tactic, things can still work alongside hotkeys if they truly want, which might be useful for example for modifier keys that also affect sprinting. Or whatever.
        //TODO Could also set its own flag instead, so when checking an input one might also check specifically that hotkeys that shouldn't occur at same time didn't. Should be more benign and avoid bugs when keys are re-bound.

        
        /// <summary>
        /// Called to update the state of inputs. Perhaps even more often than per-frame? Event-based?
        /// </summary>
        public void Update()
        {
            // The game requires focus to handle input.
            if (game.IsActive)
            {
                var keyboardState = Keyboard.GetState();
                var mouseState = Mouse.GetState();

                // Keys currently pressed
                var pressedKeys = keyboardState.GetPressedKeys();
                foreach (Keys key in pressedKeys)
                {
                    KeyState priorState;
                    bool success = naiveState.TryGetValue(key, out priorState);
                    if (!success)
                    {
                        priorState = KeyState.Up;
                    }

                    if (priorState == KeyState.Up)
                    {
                        // TODO Edge-down
                        properState.Add(key, ProperKeyState.EdgeDown);
                    }
                    else
                    {
                        // TODO Held-down
                        properState.Add(key, ProperKeyState.Down);
                    }
                    // TODO Down
                    naiveState.Add(key, KeyState.Down);
                }

                // Keys not currently pressed
                var releasedKeys = naiveState.Keys.Except(pressedKeys);
                foreach (Keys key in releasedKeys)
                {
                    KeyState priorState;
                    bool success = naiveState.TryGetValue(key, out priorState);
                    if (!success)
                    {
                        //! Should not be possible (releasedKeys comes from a Set-operation on the dict's key-set)!
                        priorState = KeyState.Up;
                    }

                    if (priorState == KeyState.Down)
                    {
                        // TODO Edge-Up
                        properState.Add(key, ProperKeyState.EdgeUp);
                    }
                    else
                    {
                        // TODO Held-up
                        properState.Add(key, ProperKeyState.Up);
                    }
                    // TODO Up
                    naiveState.Add(key, KeyState.Up);
                }
            }
        }

        public ProperKeyState GetKeyState(Keys key)
        {
            ProperKeyState state;
            bool success = properState.TryGetValue(key, out state);
            if (success)
            {
                return state;
            }
            return ProperKeyState.Up;
        }

        /// <summary>
        /// Returns true if the keys has the given state.
        /// Do note that this method considers edge-states to be a subset of their base-states,
        /// meaning if a key is EdgeDown, then `IsKeyState(key, Down)` will also return true.
        /// Do however also note this means that if the key is Down, then `IsKeyState(key, EdgeDown)` returns false.
        /// 
        /// Essentially, if a key is Down in any way, then the key is Down.
        /// But it is only EdgeDown if the key is EdgeDown.
        /// 
        /// This is to allow for easily checking if a key is down/up without handling edges explicitly,
        /// but to also not break excepted behavior if changed to explicitly require/check for an edge.
        /// 
        /// If you want a more strict check, use `IsStrictlyKeyState` instead.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public bool IsKeyState(Keys key, ProperKeyState state)
        {
            ProperKeyState currentState;
            bool success = properState.TryGetValue(key, out currentState);
            if (!success)
            {
                currentState = ProperKeyState.Up;
            }

            // Ignores the third edge-bit in currentState when comparing the states.
            // So ie. EdgeDown == Down, while the vice versa is not true: Down != EdgeDown
            return ((currentState & ~ProperKeyState.Edge) == state);
        }

        /// <summary>
        /// Returns true if the keys has the given state.
        /// Do note that this method performs a strict equals operation, so ie `Down == EdgeDown` returns false.
        /// 
        /// If you want a less strict (and more usable) check, use `IsKeyState` instead.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public bool IsStrictlyKeyState(Keys key, ProperKeyState state)
        {
            ProperKeyState currentState;
            bool success = properState.TryGetValue(key, out currentState);
            if (!success)
            {
                currentState = ProperKeyState.Up;
            }

            return currentState == state;
        }

        public bool isKeyPressed(Keys key)
        {
            return IsKeyState(key, ProperKeyState.EdgeDown);
        }
        public bool isKeyDown(Keys key)
        {
            return IsKeyState(key, ProperKeyState.Down);
        }
        public bool isStrictlyKeyDown(Keys key)
        {
            return IsStrictlyKeyState(key, ProperKeyState.Down);
        }
        public bool IsKeyReleased(Keys key)
        {
            return IsKeyState(key, ProperKeyState.EdgeUp);
        }
        public bool isKeyUp(Keys key)
        {
            return IsKeyState(key, ProperKeyState.Up);
        }
        public bool isStrictlyKeyUp(Keys key)
        {
            return IsStrictlyKeyState(key, ProperKeyState.Up);
        }

    }

    /// <summary>
    /// The proper keystate, as monogame does not give any information on whether the key was released or pressed.
    /// The values are selected such that binary operations can be used to see whether the key is up/down,
    /// regardless of whether it is also an Edge* state.
    /// In short, edge-states are like their base-state, except with the third bit set as well (`state+4` in decimal)
    /// 
    /// It is possible to simple state that it is an edge without specifying direction,
    /// to make certain operations easier, so be careful with that.
    /// </summary>
    public enum ProperKeyState
    {
        Up      = 1<<0, // 1
        Down    = 1<<1, // 2
        Edge    = 1<<2, // 4
        EdgeUp  = Edge+Up, // 5
        EdgeDown= Edge+Down // 6
    }
}
