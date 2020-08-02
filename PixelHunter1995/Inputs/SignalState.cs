using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelHunter1995.Inputs
{

    // TODO consider class vs struct. Overridden methods makes class work in dict anyway... while struct is value-type.
    // TODO As Input creates an "alias" for it thorugh inheritance, it needs to be class for now.
    /// <summary>
    /// The state of a signal-graph, intended to use for keypressed as monogame does not give any information on
    /// whether the key was released or pressed.
    /// Rather than an enum like one would expect, this is a class, meaning each ProperKeyState is an instance.
    /// There are static references for each permutation to instances describing them, to allow easier use.
    /// 
    /// The advantage with doing it this way however, is that class-properties can be used to nicely get a bool
    /// for various properties of the keystate.
    /// So `this.GetKeyState(key) == SignalState.EdgeDown` can be written as `this.GetKeyState(key).IsEdgeDown`.
    /// As a result however, it makes much less sense to enable 'Is' syntax (ie `input.IsKeyState(key, state)`),
    /// although nothing stops one from implementing that as options/aliases of the former.
    /// 
    /// The disadvantage could have been that one has to be careful about making sure to use the static references,
    /// as otherwise `==` or `.Equals` would compare reference and not value.
    /// This has however been rendered moot by overriding the relevant methods,
    /// and could also have been solved by turning it into a struct (making it a value-type).
    /// </summary>
    class SignalState
    {
        public bool IsUp { get; }
        public bool IsEdge { get; }
        public bool IsDown { get => !this.IsUp; }
        public bool IsHeld { get => !this.IsEdge; }

        public bool IsEdgeUp { get => this.IsUp && this.IsEdge; }
        public bool IsEdgeDown { get => this.IsDown && this.IsEdge; }

        public bool IsHeldUp { get => this.IsUp && !this.IsEdge; }
        public bool IsHeldDown { get => this.IsDown && !this.IsEdge; }

        // alias
        public bool IsReleased { get => this.IsEdgeUp; }
        public bool IsPressed { get => this.IsEdgeDown; }

        public SignalState(bool isUp, bool isEdge)
        {
            this.IsUp = isUp;
            this.IsEdge = isEdge;
        }

        public static SignalState Up = new SignalState(true, false);
        public static SignalState Down = new SignalState(false, false);
        public static SignalState EdgeUp = new SignalState(true, true);
        public static SignalState EdgeDown = new SignalState(false, true);

        public override string ToString()
        {
            return "" + this.GetType().Name + "." + (this.IsEdge ? "Edge" : "") + (this.IsUp ? "Up" : "Down");
        }

        public override bool Equals(object obj)
        {
            return obj is SignalState state &&
                   IsUp == state.IsUp &&
                   IsEdge == state.IsEdge;
        }

        public override int GetHashCode()
        {
            var hashCode = 1899926460;
            hashCode = hashCode * -1521134295 + IsUp.GetHashCode();
            hashCode = hashCode * -1521134295 + IsEdge.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(SignalState c1, SignalState c2)
        {
            return c1.Equals(c2);
        }

        public static bool operator !=(SignalState c1, SignalState c2)
        {
            return !c1.Equals(c2);
        }
    }
}
