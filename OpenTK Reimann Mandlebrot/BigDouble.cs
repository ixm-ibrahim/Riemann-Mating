using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTK_Riemann_Mating
{
	public class BigDouble
	{
		double digits;
		int exponent;

		public BigDouble(double n)
		{
			var d = FromDouble(n);

			digits = d.Digits;
			exponent = d.exponent;
		}
		public BigDouble(double n, int e)
		{
			digits = n;
			exponent = e;

			Simplify();
		}


		public double Digits => digits;
		public int Exponent => exponent;


		public static BigDouble Zero => new BigDouble(0, 0);
		public static BigDouble One => new BigDouble(1, 0);
		public static BigDouble NaN => new BigDouble(double.NaN, 0);
		public static BigDouble PositiveInfinity => new BigDouble(double.PositiveInfinity, 0);
		public static BigDouble NegativeInfinity => new BigDouble(double.NegativeInfinity, 0);


		public override string ToString()
		{
			return (Exponent > 308) ? Digits + " x10^ " + Exponent : ToDouble().ToString();
		}
		public override bool Equals(object obj)
		{
			return this == (BigDouble)obj;
		}
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}


		public int ToInt()
		{
			return (int)(digits * Math.Pow(10, exponent));
		}
		public double ToDouble()
		{
			return digits * Math.Pow(10, exponent);
		}
		public BigDouble FromInt(int n)
		{
			if (n == 0)
				return BigDouble.Zero;

			int e = GetExponent(n);

			return new BigDouble(n / Math.Pow(10, e), e);
		}
		public BigDouble FromDouble(double n)
		{
			if (n == 0)
				return BigDouble.Zero;
			if (n == Double.NaN)
				return BigDouble.NaN;
			if (n == Double.PositiveInfinity)
				return BigDouble.PositiveInfinity;
			if (n == Double.NegativeInfinity)
				return BigDouble.NegativeInfinity;

			int e = GetExponent(n);

			return new BigDouble(n / Math.Pow(10, e), e);
		}


		public static explicit operator float(BigDouble a)
		{
			return (float)a.ToDouble();
		}
		public static explicit operator double(BigDouble a)
		{
			return a.ToDouble();
		}


		public static bool operator ==(BigDouble a, double b)
		{
			return a.ToDouble() == b;
		}
		public static bool operator ==(double a, BigDouble b)
		{
			return b.ToDouble() == a;
		}
		public static bool operator ==(BigDouble a, BigDouble b)
		{
			return a.Digits == b.Digits && a.Exponent == b.Exponent;
		}

		public static bool operator !=(BigDouble a, double b)
		{
			return !(a == b);
		}
		public static bool operator !=(double a, BigDouble b)
		{
			return !(a == b);
		}
		public static bool operator !=(BigDouble a, BigDouble b)
		{
			return !(a == b);
		}

		public static bool operator <(BigDouble a, double b)
		{
			return a.ToDouble() < b;
		}
		public static bool operator <(double a, BigDouble b)
		{
			return a < b.ToDouble();
		}
		public static bool operator <(BigDouble a, BigDouble b)
		{
			return a.Exponent < b.Exponent || (a.Exponent == b.Exponent && a.Digits < b.Digits);
		}

		public static bool operator >(BigDouble a, double b)
		{
			return a.ToDouble() > b;
		}
		public static bool operator >(double a, BigDouble b)
		{
			return a > b.ToDouble();
		}
		public static bool operator >(BigDouble a, BigDouble b)
		{
			return a.Exponent > b.Exponent || (a.Exponent == b.Exponent && a.Digits > b.Digits);
		}


		public static BigDouble operator +(BigDouble a, double b)
		{
			return a + new BigDouble(b);
		}
		public static BigDouble operator +(double a, BigDouble b)
		{
			return b + new BigDouble(a);
		}
		public static BigDouble operator +(BigDouble a, BigDouble b)
		{
			double test = a.ToDouble() + b.ToDouble();

			if (!Double.IsNaN(test) && !Double.IsInfinity(test))
				return new BigDouble(test).Simplify();

			int diff = a.Exponent - b.Exponent;
			
			if (diff < -20)
				return b;
			if (diff > 20)
				return a;

			if (diff >= 0)
				return new BigDouble((a.digits * Math.Pow(10, diff)) + b.digits, b.exponent).Simplify();

			return new BigDouble(a.digits + (b.digits * Math.Pow(10, -diff)), a.exponent).Simplify();
		}

		public static BigDouble operator -(BigDouble a)
		{
			return new BigDouble(-a.Digits, a.Exponent);
		}
		public static BigDouble operator -(BigDouble a, double b)
		{
			return a + new BigDouble(-b);
		}
		public static BigDouble operator -(double a, BigDouble b)
		{
			return -b + new BigDouble(a);
		}
		public static BigDouble operator -(BigDouble a, BigDouble b)
		{
			return a + -b;
		}

		public static BigDouble operator *(BigDouble a, double b)
		{
			return new BigDouble(a.Digits * b, a.Exponent).Simplify();
		}
		public static BigDouble operator *(double a, BigDouble b)
		{
			return new BigDouble(a * b.Digits, b.Exponent).Simplify();
		}
		public static BigDouble operator *(BigDouble a, BigDouble b)
		{
			return new BigDouble(a.Digits * b.Digits, a.Exponent + b.Exponent).Simplify();
		}

		public static BigDouble operator /(BigDouble a, double b)
		{
			return new BigDouble(a.Digits / b, a.Exponent).Simplify();
		}
		public static BigDouble operator /(double a, BigDouble b)
		{
			return new BigDouble(a / b.Digits, -b.Exponent).Simplify();
		}
		public static BigDouble operator /(BigDouble a, BigDouble b)
		{
			return new BigDouble(a.Digits / b.Digits, a.Exponent - b.Exponent).Simplify();
		}

		public static BigDouble operator ^(BigDouble a, int b)
		{
			return new BigDouble(Math.Pow(a.Digits, b), a.Exponent * b).Simplify();
		}
		//@TODO: Fix potential overflow here, with the second pow method
        public static BigDouble operator ^(double a, BigDouble b)
        {
            return new BigDouble(Math.Pow(a, b.Digits) * Math.Pow(a, Math.Pow(10, b.Exponent))).Simplify();
        }
		//@TODO: Figure out how to take the power of a decimal (need to always have an integer exponent for standard scientific notation)
        /*
        public static BigDouble operator ^(BigDouble a, BigDouble b)
        {
            return (a ^ b.Digits) * (a ^ (int) Math.Pow(10, b.Exponent)).Simplify();
        }*/

		static int GetExponent(double n)
		{
			if (n == 0)
				return 0;
			else if (Math.Abs(n) < 1)
				return GetLeadingZeroNum(n);

			return GetDigitNum(n) - 1;
		}
		static int GetDigitNum(double n)
		{
			if (n == 0)
				return 1;

			return (int)Math.Floor(Math.Log10(Math.Abs(n)) + 1);
		}
		static int GetLeadingZeroNum(double n)
		{
			if (n == 0 || Math.Abs(n) > 1)
				return 0;

			int count = 0;

			while (Math.Abs(n) < 1)
			{
				n *= 10;
				count++;
			}

			return count;
		}
		public BigDouble Simplify()
		{
			if (Digits == 0)
				exponent = 0;
			else if (Math.Abs(Digits) < 1)
			{
				int n = GetLeadingZeroNum(Digits);

				digits *= Math.Pow(10, n);
				exponent -= n;
			}
			else
			{
				int n = GetDigitNum(Digits);

				if (n > 1)
				{
					digits /= Math.Pow(10, n - 1);
					exponent += n - 1;
				}
			}

			return this;
		}


		public static bool IsZero(BigDouble d)
		{
			return d.Digits == 0;
		}

		public static bool IsNaN(BigDouble d)
		{
			return Double.IsNaN(d.Digits);
		}

		public static bool IsInfinity(BigDouble d)
		{
			return IsPositiveInfinity(d) || IsNegativeInfinity(d);
		}

		public static bool IsPositiveInfinity(BigDouble d)
		{
			return Double.IsPositiveInfinity(d.Digits);
		}

		public static bool IsNegativeInfinity(BigDouble d)
		{
			return Double.IsNegativeInfinity(d.Digits);
		}


		public int Sign()
		{
			if (Digits > 0)
				return 1;
			else if (Digits < 0)
				return -1;

			return 0;
		}

		public static BigDouble Abs(BigDouble d)
		{
			return new BigDouble(Math.Abs(d.Digits), d.Exponent);
		}

		public static BigDouble Invert(BigDouble d)
		{
			return 1 / d;
		}

		public static BigDouble Sqrt(BigDouble d)
		{
			if (Math.Abs(d.Exponent) % 2 == 1)
				return new BigDouble(Math.Sqrt(d.Digits * 10), (d.Exponent - 1) / 2);

			return new BigDouble(Math.Sqrt(d.Digits), d.Exponent / 2);
		}
	}

	public struct BigComplex
	{
		public BigDouble R;
		public BigDouble I;

		public BigComplex(double real, double imaginary)
		{
			R = new BigDouble(real);
			I = new BigDouble(imaginary);
		}
		public BigComplex(BigDouble real, BigDouble imaginary)
		{
			R = real;
			I = imaginary;
		}
		public BigComplex(Complex z)
		{
			R = new BigDouble(z.R);
			I = new BigDouble(z.I);
		}


		public static BigComplex Zero
		{
			get { return new BigComplex(BigDouble.Zero, BigDouble.Zero); }
		}
		public static BigComplex One
		{
			get { return new BigComplex(BigDouble.One, BigDouble.Zero); }
		}
		public static BigComplex i
		{
			get { return new BigComplex(BigDouble.Zero, BigDouble.One); }
		}
		public static BigComplex NaN => new BigComplex(double.NaN, 0);
		public static BigComplex PositiveInfinity => new BigComplex(double.PositiveInfinity, 0);
		public static BigComplex NegativeInfinity => new BigComplex(double.NegativeInfinity, 0);


		public BigDouble Real
		{
			get { return R; }

			set { R = value; }
		}
		public BigDouble Imaginary
		{
			get { return I; }

			set { I = value; }
		}
		public BigDouble Radius
		{
			get { return BigDouble.Sqrt(R * R + I * I); }
		}
		public BigDouble RadiusSquared
		{
			get { return R * R + I * I; }
		}
		public double Angle
		{
			get { return Arg(new BigComplex(R, I)); }
		}


		public Complex ToComplex()
		{
			return new Complex(R.ToDouble(), I.ToDouble());
		}


		public override string ToString()
		{
			return String.Format("({0}, {1}i)", R, I);
		}
		public override bool Equals(object obj)
		{
			return this == (BigComplex)obj;
		}
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}


		public static bool operator ==(BigComplex z, double d)
		{
			return z.R == d && z.I == BigDouble.Zero;
		}
		public static bool operator ==(double d, BigComplex z)
		{
			return z.R == d && z.I == BigDouble.Zero;
		}
		public static bool operator ==(BigComplex z, BigComplex d)
		{
			return z.R == d.R && z.I == d.I;
		}

		public static bool operator !=(BigComplex z, double d)
		{
			return !(z.R == d);
		}
		public static bool operator !=(double d, BigComplex z)
		{
			return !(z.R == d);
		}
		public static bool operator !=(BigComplex z, BigComplex d)
		{
			return !(z == d);
		}

		public static BigComplex operator +(BigComplex z, double d)
		{
			return new BigComplex(z.R + d, z.I);
		}
		public static BigComplex operator +(double d, BigComplex z)
		{
			return new BigComplex(z.R + d, z.I);
		}
		public static BigComplex operator +(BigComplex z, BigDouble d)
		{
			return new BigComplex(z.R + d, z.I);
		}
		public static BigComplex operator +(BigDouble d, BigComplex z)
		{
			return new BigComplex(z.R + d, z.I);
		}
		public static BigComplex operator +(BigComplex z, BigComplex d)
		{
			return new BigComplex(z.R + d.R, z.I + d.I);
		}

		public static BigComplex operator -(BigComplex z)
		{
			return new BigComplex(-z.R, -z.I);
		}
		public static BigComplex operator -(BigComplex z, double d)
		{
			return new BigComplex(z.R - d, z.I);
		}
		public static BigComplex operator -(double d, BigComplex z)
		{
			return new BigComplex(d - z.R, -z.I);
		}
		public static BigComplex operator -(BigComplex z, BigDouble d)
		{
			return new BigComplex(z.R - d, z.I);
		}
		public static BigComplex operator -(BigDouble d, BigComplex z)
		{
			return new BigComplex(d - z.R, -z.I);
		}
		public static BigComplex operator -(BigComplex z, BigComplex d)
		{
			return new BigComplex(z.R - d.R, z.I - d.I);
		}

		public static BigComplex operator *(BigComplex z, double d)
		{
			return new BigComplex(z.R * d, z.I * d);
		}
		public static BigComplex operator *(double d, BigComplex z)
		{
			return z * d;
		}
		public static BigComplex operator *(BigComplex z, BigDouble d)
		{
			return new BigComplex(z.R * d, z.I * d);
		}
		public static BigComplex operator *(BigDouble d, BigComplex z)
		{
			return z * d;
		}
		public static BigComplex operator *(BigComplex z, BigComplex d)
		{
			return new BigComplex((z.R * d.R) - (z.I * d.I), (z.R * d.I) + (d.R * z.I));
		}

		public static BigComplex operator /(BigComplex z, double d)
		{
			return new BigComplex(z.R / d, z.I / d);
		}
		public static BigComplex operator /(double d, BigComplex z)
		{
			BigDouble x = (z.R * z.R) + (z.I * z.I);

			return new BigComplex(d * z.R / x, -d * z.I / x);
		}
		public static BigComplex operator /(BigComplex z, BigDouble d)
		{
			return new BigComplex(z.R / d, z.I / d);
		}
		public static BigComplex operator /(BigDouble d, BigComplex z)
		{
			BigDouble x = z.R * z.R + z.I * z.I;

			return new BigComplex(d * z.R / x, -d * z.I / x);
		}
		public static BigComplex operator /(BigComplex z, BigComplex d)
		{
			BigDouble r = d.R * d.R + d.I * d.I;

			if (r == 0)
				return Zero;

			return new BigComplex(((z.R * d.R) + (z.I * d.I)) / r, ((d.R * z.I) - (z.R * d.I)) / r);
		}

		public static BigComplex operator ^(BigComplex z, int p)
		{
			BigComplex nz = z;

			if (p.Equals(0))
				return One;
			if (p.Equals(1))
				return z;
			if (p > 0)
			{
				for (int i = 0; i < p - 1; i++)
					nz = nz * z;

				return nz;
			}

			// p < 0
			for (int i = 0; i < -p - 1; i++)
				nz = nz * z;

			if (IsZero(nz))
				return Zero;

			return 1 / nz;
		}


		public static bool IsZero(BigComplex z)
		{
			return z.R == 0 && z.I == 0;
		}
		public static bool IsNaN(BigComplex z)
		{
			return BigDouble.IsNaN(z.R) || BigDouble.IsNaN(z.I);
		}
		public static bool IsInfinity(BigComplex z)
		{
			return BigDouble.IsInfinity(z.R) || BigDouble.IsInfinity(z.I);
		}
		public static bool IsPositiveInfinity(BigComplex z)
		{
			return BigDouble.IsPositiveInfinity(z.R) || BigDouble.IsPositiveInfinity(z.I);
		}
		public static bool IsNegativeInfinity(BigComplex z)
		{
			return BigDouble.IsNegativeInfinity(z.R) || BigDouble.IsNegativeInfinity(z.I);
		}


		public BigComplex Pow(int d)
		{
			return this ^ d;
		}

		public static BigComplex Inverse(BigComplex z)
		{
			return 1 / z;
		}

		public static BigComplex Normalize(BigComplex z)
		{
			return z / z.Radius;
		}

		public static BigComplex Conjugate(BigComplex z)
		{
			return new BigComplex(z.R, -z.I);
		}

		public static int Sign(BigComplex z)
		{
			return z.R == 0 ? z.I.Sign() : z.R.Sign();
		}

		public static BigDouble Magnitude(BigComplex z)
		{
			return z.Radius;
		}

		public static double Arg(BigComplex bz)
		{
			double arg = 0;

			var z = Normalize(bz).ToComplex();

			/* 
 			 * 	Atan2 (four-quadrant arctangent):
	    	 * 		
	    	 * 		double atan = Math.Atan(z.I/z.R);
	    	 * 		
	    	 * 		if (z.R > 0)
	    	 * 			arg = atan;
	    	 * 		if (z.R < 0 && z.I >= 0)
	    	 * 			arg = atan + Math.PI;
	    	 * 		if (z.R < 0 && z.I < 0)
	    	 * 			arg = atan - Math.PI;
	    	 * 		if (z.R.Equals(0) && z.I > 0)
	    	 * 			arg = Math.PI / 2;
	    	 * 		if (z.R.Equals(0) && z.I < 0)
	    	 * 			arg = -Math.PI / 2;
	    	 * 		if (z == new Complex (0,0))
	    	 * 			arg = Double.NaN;
	    	 * 
	    	 */

			if (z.R.Equals(0))
			{
				if (z.I < 0 || z.I.Equals(-1))
					arg = -Math.PI / 2;
				else if (z.I.Equals(0))
					return Double.NaN;
				else if (z.I > 0 || z.I.Equals(1))
					arg = Math.PI / 2;
			}
			else if (z.I.Equals(0))
			{
				if (z.R.Equals(1))
					arg = 0;
				else if (z.R.Equals(-1))
					arg = Math.PI;
			}
			else if (z == new Complex(1, 1))
				arg = Math.PI / 4;
			else
				arg = Math.Atan2(z.I, z.R);

			return arg;
		}

		public static BigComplex Sqrt(BigComplex c)
		{
			BigDouble r = c.Radius;
			double test = c.ToComplex().Radius - c.R.ToDouble();

			if (test == 0)
				return new BigComplex(Complex.Sqrt(c.ToComplex()));

			return .5 * Math.Sqrt(2) * new BigComplex(BigDouble.Sqrt(r + c.R), c.I.Sign() * BigDouble.Sqrt(r - c.R));


			//return .5 * Math.Sqrt(2) * new BigComplex(BigDouble.Sqrt(r + c.R), (test == 0) ? BigDouble.Zero : c.I.Sign() * BigDouble.Sqrt(r - c.R));
		}

		public static BigComplex Proj(BigComplex z)
		{
			if (!IsNaN(z) && !IsInfinity(z))
				return z;

			return PositiveInfinity;
		}
	}
}
