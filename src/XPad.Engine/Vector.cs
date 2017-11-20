using System;

namespace XPad.Engine
{
    /// <summary>
    /// Simple value type to represent a 2D vector.
    /// This struct is immutable and features equality checks using operators
    /// <code>==</code> and <code>!=</code>.
    /// </summary>
    public struct Vector : IEquatable<Vector>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="Vector"/>.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordingate.</param>
        public Vector(double x, double y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Gets the X coordinate.
        /// </summary>
        public double X { get; }

        /// <summary>
        /// Gets the Y coordinate.
        /// </summary>
        public double Y { get; }

        /// <summary>
        /// Gets the vector length or magnitude.
        /// </summary>
        public double Length => Math.Sqrt(X * X + Y * Y);

        /// <summary>
        /// Returns a normalized Vector that points in the same direction as this vector.
        /// A normalized vector has length 1.
        /// </summary>
        /// <returns>A new vector.</returns>
        public Vector Normalize()
        {
            var im = 1.0 / Length;
            return new Vector(X * im, Y * im);
        }

        /// <summary>
        /// Calculates the distance between two vectors.
        /// </summary>
        /// <param name="a">First vector.</param>
        /// <param name="b">Second vector.</param>
        /// <returns>The distance between a and b.</returns>
        public static double Distance(Vector a, Vector b)
        {
            var dx = a.X - b.X;
            var dy = a.Y - b.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        public override bool Equals(object obj)
        {
            return obj != null
                && Equals((Vector)obj);
        }

        public bool Equals(Vector other)
        {
            return X == other.X && Y == other.Y;
        }

        public override int GetHashCode()
        {
            var hashCode = -621239040;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            return hashCode;
        }

        public override string ToString() => $"({X}, {Y})";

        /// <summary>
        /// Subtracts two vectors.
        /// </summary>
        /// <param name="a">First vector.</param>
        /// <param name="b">Vector to subtract from a.</param>
        /// <returns>The difference vector from a to b.</returns>
        public static Vector operator -(Vector a, Vector b)
        {
            return new Vector(a.X - b.X, a.Y - b.Y);
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="a">First vector.</param>
        /// <param name="b">Second vector.</param>
        /// <returns>The result vector.</returns>
        public static Vector operator +(Vector a, Vector b)
        {
            return new Vector(a.X + b.X, a.Y + b.Y);
        }

        /// <summary>
        /// Multiplies a vector by a scalar, growing or shrinking the vector.
        /// </summary>
        /// <param name="v">The vector.</param>
        /// <param name="scalar">The scalar.</param>
        /// <returns>The result vector.</returns>
        public static Vector operator *(Vector v, double scalar)
        {
            return new Vector(v.X * scalar, v.Y * scalar);
        }

        /// <summary>
        /// Compares two vectors for equality.
        /// </summary>
        /// <param name="a">First vector.</param>
        /// <param name="b">Second vector.</param>
        /// <returns><code>true</code> if the two vectors are equal.</returns>
        public static bool operator ==(Vector a, Vector b)
        {
            return a.Equals(b);
        }

        /// <summary>
        /// Compares two vectors for inequality.
        /// </summary>
        /// <param name="a">First vector.</param>
        /// <param name="b">Second vector.</param>
        /// <returns><code>true</code> if the two vectors are not equal.</returns>
        public static bool operator !=(Vector a, Vector b)
        {
            return a.Equals(b) == false;
        }
    }
}
