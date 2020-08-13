namespace PixelHunter1995.Utilities
{
    public struct Either<L, R>
    {
        public bool IsLeft { get; }
        
        public L left { get; }
        public R right { get; }

        public Either(bool isLeft, L left, R right)
        {
            this.IsLeft = isLeft;
            this.left = left;
            this.right = right;
        }

        //! only works (properly) if L != R (not same type or subtype)
        public Either(L left) : this(true, left, default(R)) { }
        public Either(R right) : this(false, default(L), right) { }

        public override string ToString()
        {
            return "Either< " + this.left.ToString() + ", " +  this.right.ToString() + " >";
        }
        public string ToShortString()
        {
            return this.IsLeft ? this.left.ToString() : this.right.ToString();
        }

        // Overloads the operator for implicit type-casting
        //  (or rather, creates it in this case so one exists at all).
        // So say you have a method `void Foo(Either<int,string> bar)`,
        // These lines makes the line `Foo("foobar");` both valid,
        // and equivalent to `Foo(new Either<int,string>("foobar"));`.
        // That is to say, no need to explicitly create an Either instance,
        // as a form of sugary syntax.
        //! only works (properly) if L != R (not same type or subtype)
        public static implicit operator Either<L, R>(L left) => new Either<L, R>(left);
        public static implicit operator Either<L, R>(R right) => new Either<L, R>(right);
    }
}
