using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelHunter1995
{
    public struct Either<L, R>
    {
        public bool IsLeft { get; }

        public L left { get; }
        public R right { get; }

        public Either(L left)
        {
            this.IsLeft = true;
            this.left = left;
            this.right = default(R);
        }
        public Either(R right)
        {
            this.IsLeft = false;
            this.left = default(L);
            this.right = right;
        }
    }
    //public class Either2<L, R>
    //{
    //    public bool IsLeft { get; }

    //    private readonly L left;
    //    private readonly R right;

    //    public Either2(L left)
    //    {
    //        this.IsLeft = true;
    //        this.left = left;
    //    }
    //    public Either2(R right)
    //    {
    //        this.IsLeft = false;
    //        this.right = right;
    //    }

    //    public override bool Equals(object obj)
    //    {
    //        //return obj is Either2<L, R> either &&
    //        //       IsLeft == either.IsLeft &&
    //        //       EqualityComparer<L>.Default.Equals(left, either.left) &&
    //        //       EqualityComparer<R>.Default.Equals(right, either.right);
    //        return obj is Either2<L, R> either &&
    //                (this.IsLeft == either.IsLeft && this.IsLeft ?
    //                EqualityComparer<L>.Default.Equals(this.left, either.left) :
    //                EqualityComparer<R>.Default.Equals(this.right, either.right));
    //    }

    //    public override int GetHashCode()
    //    {
    //        //var hashCode = -935455819;
    //        //hashCode = hashCode * -1521134295 + IsLeft.GetHashCode();
    //        //hashCode = hashCode * -1521134295 + EqualityComparer<L>.Default.GetHashCode(left);
    //        //hashCode = hashCode * -1521134295 + EqualityComparer<R>.Default.GetHashCode(right);
    //        //return hashCode;
    //        return this.IsLeft ? this.left.GetHashCode() : this.right.GetHashCode();
    //    }
    //}
}
