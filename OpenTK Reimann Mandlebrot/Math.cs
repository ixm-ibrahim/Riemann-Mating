using System;
using System.Linq;
using System.Collections.Generic;
using OpenTK;
using System.Numerics;

namespace OpenTK_Reimann_Mating
{
	public struct Vector3D
	{
		public double X;
		public double Y;
		public double Z;

		public Vector3D(double x, double y, double z)
		{
			X = x;
			Y = y;
			Z = z;
		}
		public Vector3D(OpenTK.Vector3 v)
		{
			X = v.X;
			Y = v.Y;
			Z = v.Z;
		}

		public static Vector3D Zero
		{
			get { return new Vector3D(0, 0, 0); }
		}

		public static Vector3D Right
		{
			get { return new Vector3D(1, 0, 0); }
		}
		public static Vector3D Left
		{
			get { return -Right; }
		}
		public static Vector3D Forward
		{
			get { return new Vector3D(0, 1, 0); }
		}
		public static Vector3D Backward
		{
			get { return -Forward; }
		}
		public static Vector3D Up
		{
			get { return new Vector3D(0, 0, 1); }
		}
		public static Vector3D Down
		{
			get { return -Up; }
		}

		public double Magnitude()
		{
			return Math.Sqrt(X * X + Y * Y + Z * Z);
		}
		public static double Magnitude(Vector3D v)
		{
			return Math.Sqrt(v.X * v.X + v.Y * v.Y + v.Z * v.Z);
		}

		public Vector3D Normalize()
		{
			return this / Magnitude();
		}
		public static Vector3D Normalize(Vector3D v)
		{
			return v / Magnitude(v);
		}

		public double Angle(Vector3D v)
		{
			return Math.Acos(DotProduct(v) / (Magnitude() * Magnitude(v)));
		}

		public override string ToString()
		{
			return (System.String.Format("({0}, {1}, {2})", X, Y, Z));
		}

		public override bool Equals(object obj)
		{
			return this == (Vector3D)obj;
		}

		public override int GetHashCode()
		{
			return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
		}

		public static bool operator ==(Vector3D a, Vector3D b)
		{
			return a.X.Equals(b.X) && a.Y.Equals(b.Y) && a.Z.Equals(b.Z);
		}

		public static bool operator !=(Vector3D a, Vector3D b)
		{
			return !(a == b);
		}

		public static Vector3D operator +(Vector3D a, Vector3D b)
		{
			return new Vector3D(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
		}
		public static Vector3D operator +(Vector3D a, Quaternion b)
		{
			return new Vector3D(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
		}

		public static Vector3D operator -(Vector3D v)
		{
			return v * -1;
		}
		public static Vector3D operator -(Vector3D a, Vector3D b)
		{
			return a + -b;
		}

		public static Vector3D operator *(Vector3D v, double d)
		{
			return new Vector3D(v.X * d, v.Y * d, v.Z * d);
		}
		public static Vector3D operator *(double d, Vector3D v)
		{
			return v * d;
		}
		public static Vector3D operator *(Quaternion q, Vector3D v)
		{
			double xx = q.X * q.X;
			double yy = q.Y * q.Y;
			double zz = q.Z * q.Z;
			double xy = q.X * q.Y;
			double xz = q.X * q.Z;
			double yz = q.Y * q.Z;
			double wx = q.W * q.X;
			double wy = q.W * q.Y;
			double wz = q.W * q.Z;

			Vector3D result;

			result.X = v.X * (1 - 2 * (yy + zz)) + v.Y * 2 * (xy - wz) + v.Z * 2 * (xz + wy);
			result.Y = v.X * 2 * (xy + wz) + v.Y * (1 - 2 * (xx + zz)) + v.Z * 2 * (yz - wx);
			result.Z = v.X * 2 * (xz - wy) + v.Y * 2 * (yz + wx) + v.Z * (1 - 2 * (xx + yy));

			return result;
		}
		public static Vector3D operator *(Vector3D v, Quaternion q)
		{
			double xx = q.X * q.X;
			double yy = q.Y * q.Y;
			double zz = q.Z * q.Z;
			double xy = q.X * q.Y;
			double xz = q.X * q.Z;
			double yz = q.Y * q.Z;
			double wx = q.W * q.X;
			double wy = q.W * q.Y;
			double wz = q.W * q.Z;

			Vector3D result;

			result.X = v.X * (1 - 2 * (yy + zz)) + v.Y * 2 * (xy - wz) + v.Z * 2 * (xz + wy);
			result.Y = v.X * 2 * (xy + wz) + v.Y * (1 - 2 * (xx + zz)) + v.Z * 2 * (yz - wx);
			result.Z = v.X * 2 * (xz - wy) + v.Y * 2 * (yz + wx) + v.Z * (1 - 2 * (xx + yy));

			return result;
		}

		public static Vector3D operator /(Vector3D v, double d)
		{
			return v * (1 / d);
		}

		public double DotProduct(Vector3D v)
		{
			return (X * v.X) + (Y * v.Y) + (Z * v.Z);
		}
		public static double DotProduct(Vector3D a, Vector3D b)
		{
			return a.DotProduct(b);
		}

		public Vector3D CrossProduct(Vector3D v)
		{
			return new Vector3D(Y * v.Z - Z * v.Y, Z * v.X - X * v.Z, X * v.Y - Y * v.X);
		}
		public static Vector3D CrossProduct(Vector3D a, Vector3D b)
		{
			return a.CrossProduct(b);
		}

		public static double DistanceBetween(Vector3D a, Vector3D b)
		{
			return (a - b).Magnitude();
		}
	}

	public struct Complex
	{
		public double R;
		public double I;

		public Complex(double real, double imaginary)
		{
			R = real;
			I = imaginary;
		}
		public Complex(Quaternion q)
		{
			R = q.W;
			I = q.X;
		}

		public static Complex NaN
		{
			get { return new Complex(Double.NaN, Double.NaN); }
		}

		public static Complex Zero
		{
			get { return new Complex(0, 0); }
		}

		public static Complex i
		{
			get { return new Complex(0, 1); }
		}

		public double Real
		{
			get { return R; }

			set { R = 1; }
		}

		public double Imaginary
		{
			get { return I; }

			set { I = 1; }
		}

		public double Radius
		{
			get { return Abs(new Complex(R, I)); }
		}

		public double RadiusSquared
		{
			get { return R*R + I*I; }
		}

		public double Angle
		{
			get { return Arg(new Complex(R, I)); }
		}

		public override string ToString()
		{
			return (System.String.Format("({0}, {1}i)", R, I));
		}

		public override bool Equals(object obj)
		{
			return this == (Complex)obj;
		}

		public override int GetHashCode()
		{
			return R.GetHashCode() ^ I.GetHashCode();
		}

		public static bool operator ==(Complex z, int d)
		{
			return z.R.Equals(d);
		}
		public static bool operator ==(int d, Complex z)
		{
			return z.R.Equals(d);
		}
		public static bool operator ==(Complex z, double d)
		{
			return z.R.Equals(d);
		}
		public static bool operator ==(double d, Complex z)
		{
			return z.R.Equals(d);
		}
		public static bool operator ==(Complex z, Complex d)
		{
			return z.R.Equals(d.R) && z.I.Equals(d.I);
		}

		public static bool operator !=(Complex z, int d)
		{
			return !z.R.Equals(d);
		}
		public static bool operator !=(int d, Complex z)
		{
			return !z.R.Equals(d);
		}
		public static bool operator !=(Complex z, double d)
		{
			return !z.R.Equals(d);
		}
		public static bool operator !=(double d, Complex z)
		{
			return !z.R.Equals(d);
		}
		public static bool operator !=(Complex z, Complex d)
		{
			return !(z == d);
		}

		public static Complex operator +(Complex z, int d)
		{
			return z + System.Convert.ToDouble(d);
		}
		public static Complex operator +(int d, Complex z)
		{
			return z + System.Convert.ToDouble(d);
		}
		public static Complex operator +(Complex z, double d)
		{
			return d + z;
		}
		public static Complex operator +(double d, Complex z)
		{
			return new Complex(z.R + d, z.I);
		}
		public static Complex operator +(Complex z, Complex d)
		{
			return new Complex(z.R + d.R, z.I + d.I);
		}

		public static Complex operator -(Complex z)
		{
			return z * -1;
		}
		public static Complex operator -(Complex z, int d)
		{
			return new Complex(z.R - d, z.I);
		}
		public static Complex operator -(int d, Complex z)
		{
			return new Complex(d - z.R, -z.I);
		}
		public static Complex operator -(Complex z, double d)
		{
			return new Complex(z.R - d, z.I);
		}
		public static Complex operator -(double d, Complex z)
		{
			return new Complex(d - z.R, -z.I);
		}
		public static Complex operator -(Complex z, Complex d)
		{
			return new Complex(z.R - d.R, z.I - d.I);
		}

		public static Complex operator *(Complex z, int d)
		{
			return z * System.Convert.ToDouble(d);
		}
		public static Complex operator *(int d, Complex z)
		{
			return z * System.Convert.ToDouble(d);
		}
		public static Complex operator *(Complex z, double d)
		{
			return new Complex(z.R * d, z.I * d);
		}
		public static Complex operator *(double d, Complex z)
		{
			return new Complex(z.R * d, z.I * d);
		}
		public static Complex operator *(Complex z, Complex d)
		{
			return new Complex(z.R * d.R - z.I * d.I, z.R * d.I + d.R * z.I);
		}

		public static Complex operator /(Complex z, int d)
		{
			return new Complex(z.R / d, z.I / d);
		}
		public static Complex operator /(int d, Complex z)
		{
			double x = z.R * z.R + z.I * z.I;

			return new Complex(d * z.R / x, -d * z.I / x);
		}
		public static Complex operator /(Complex z, double d)
		{
			return new Complex(z.R / d, z.I / d);
		}
		public static Complex operator /(double d, Complex z)
		{
			double x = z.R * z.R + z.I * z.I;

			return new Complex(d * z.R / x, -d * z.I / x);
		}
		public static Complex operator /(Complex z, Complex d)
		{
			double x = d.R * d.R + d.I * d.I;
			
			if (Double.IsInfinity(d.R))
				return Zero;
			if (x == 0)
				return new Complex(double.PositiveInfinity, 0);

			return new Complex((z.R * d.R + z.I * d.I) / x, (d.R * z.I - z.R * d.I) / x);
		}

		public static Complex operator ^(Complex z, int p)
		{
			Complex nz = z;

			if (p.Equals(0))
				return new Complex(1, 0);
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

			if (Complex.IsZero(nz))
				return Complex.Zero;

			return 1 / nz;
		}
		public static Complex operator ^(int d, Complex z)
		{
			double r = Math.Pow(d, z.R),
				   theta = z.I * Math.Log(d);

			return Double.IsNaN(theta) || Double.IsNaN(r) ? z : new Complex(r * Math.Cos(theta), r * Math.Sin(theta));
		}
		public static Complex operator ^(Complex z, double p)
		{
			if (p.Equals(0))
				return new Complex(1, 0);
			if (p.Equals(1))
				return z;
			if (p > 0 && p.Equals((int)p))
			{
				Complex nz = z;

				for (int i = 0; i < p - 1; i++)
					nz = nz * z;

				return nz;
			}
			if (p < 0 && p.Equals((int)p))
			{
				Complex nz = z;

				for (int i = 0; i < -p - 1; i++)
					nz = nz * z;

				if (Complex.IsZero(nz))
					return Complex.Zero;

				return 1 / nz;
			}

			double r = Math.Pow(Abs(z), p),
				   theta = Arg(z);

			return Double.IsNaN(theta) || Double.IsNaN(r) ? z : new Complex(r * Math.Cos(theta * p), r * Math.Sin(theta * p));
		}
		public static Complex operator ^(double d, Complex z)
		{
			double r = Math.Pow(d, z.R),
				   theta = z.I * Math.Log(d);

			return Double.IsNaN(theta) || Double.IsNaN(r) ? z : new Complex(r * Math.Cos(theta), r * Math.Sin(theta));
		}
		public static Complex operator ^(Complex z, Complex p)
		{
			return ((z.R * z.R + z.I * z.I) ^ (p / 2)) * Exp((new Complex(0, 1)) * p * Arg(z));
		}

		public Complex Pow(int d)
		{
			return this ^ d;
		}
		public Complex Pow(double d)
		{
			return this ^ d;
		}
		public Complex Pow(Complex c)
		{
			return this ^ c;
		}

		public static bool IsZero(Complex z)
		{
			return z.R.Equals(0) && z.I.Equals(0);
		}

		public static bool IsNaN(Complex z)
		{
			return Double.IsNaN(z.R) || Double.IsNaN(z.I);
		}

		public static bool IsInfinity(Complex z)
		{
			return Double.IsInfinity(z.R) || Double.IsInfinity(z.I);
		}

		public static bool IsPositiveInfinity(Complex z)
		{
			return Double.IsPositiveInfinity(z.R) || Double.IsPositiveInfinity(z.I);
		}

		public static bool IsNegativeInfinity(Complex z)
		{
			return Double.IsNegativeInfinity(z.R) || Double.IsNegativeInfinity(z.I);
		}

		public static Complex Inverse(Complex z)
		{
			double x = z.R * z.R + z.I * z.I;

			return new Complex(z.R / x, -z.I / x);
		}

		public static Complex Conjugate(Complex z)
		{
			return new Complex(z.R, -z.I);
		}

		public static int Sign(Complex z)
		{
			return z.R.Equals(0) ? Math.Sign(z.I) : Math.Sign(z.R);
		}

		public static double Magnitude(Complex z)
		{
			return z.Radius;
		}

		public static Complex Proj(Complex z)
		{
			if (!IsNaN(z) && !IsInfinity(z))
				return z;

			return new Complex(double.PositiveInfinity, 0);
		}

		public static double Arg(Complex z)
		{
			double arg = 0;

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

		public static double Abs(Complex z)
		{
			/*
			if (Double.IsPositiveInfinity(z.R))
				z.R = 10e99;
			else if (Double.IsNegativeInfinity(z.R))
				z.R = -10e99;
			if (Double.IsPositiveInfinity(z.I))
				z.I = 10e99;
			else if (Double.IsNegativeInfinity(z.I))
				z.I = -10e99;
				*/
			return Math.Sqrt(z.R * z.R + z.I * z.I);
		}

		public static Complex Exp(Complex z)
		{
			double r = Math.Exp(z.R);

			if (z.R.Equals(0))
				return new Complex(Math.Cos(z.I), Math.Sin(z.I));
			if (z.I.Equals(0))
				return new Complex(r, 0);

			return IsNaN(z) ? z : new Complex(r * Math.Cos(z.I), r * Math.Sin(z.I));
		}

		public static Complex Ln(Complex z)
		{
			if (z.R.Equals(0))
				return new Complex(Math.Log(z.I), Math.PI / 2);
			if (z.I.Equals(0))
				return new Complex(Math.Log(z.R), 0);

			return new Complex(.5 * Math.Log(z.R * z.R + z.I * z.I), Math.Atan2(z.I, z.R));
		}

		public static Complex Log10(Complex z)
		{
			double ln10 = Math.Log(10);

			return new Complex(0.5 * Math.Log(z.R * z.R + z.I * z.I) / ln10, Arg(z) / ln10);
		}

		public static Complex Log(double b, Complex a)
		{
			return Ln(a) / Math.Log(b);
		}
		public static Complex Log(Complex b, Complex a)
		{
			return Ln(a) / Ln(b);
		}

		public static Complex Sqrt(Complex z)
		{/*
			if (IsNaN(z) || IsInfinity(z))
				return new Complex(double.PositiveInfinity, 0);
			*/
			double r = Abs(z);

			//return new Complex(.5 * Math.Sqrt(2 * (r + z.R)), .5 * Sign(new Complex(z.I, -r)) * Math.Sqrt(2 * (r - z.R)));
			return .5 * Math.Sqrt(2) * new Complex(Math.Sqrt(r + z.R), Math.Sign(z.I) * Math.Sqrt(r - z.R));
		}

		public static Complex Sin(Complex z)
		{
			if (z.R.Equals(0))
				return new Complex(0, Math.Sinh(z.I));
			if (z.I.Equals(0))
				return new Complex(Math.Sin(z.R), 0);

			return new Complex(Math.Sin(z.R % (2 * Math.PI)) * Math.Cosh(z.I), Math.Cos(z.R % (2 * Math.PI)) * Math.Sinh(z.I));
		}

		public static Complex Cos(Complex z)
		{
			if (z.R.Equals(0))
				return new Complex(Math.Cosh(z.I), 0);
			if (z.I.Equals(0))
				return new Complex(Math.Cos(z.R), 0);

			return new Complex(Math.Cos(z.R % (2 * Math.PI)) * Math.Cosh(z.I), -Math.Sin(z.R % (2 * Math.PI)) * Math.Sinh(z.I));
		}

		public static Complex Tan(Complex z)
		{
			if (z.R.Equals(0))
				return new Complex(0, Math.Tanh(z.I));
			if (z.I.Equals(0))
				return new Complex(Math.Tan(z.R % (2 * Math.PI)), 0);

			double cosr = Math.Cos(z.R % (2 * Math.PI));
			double sinhi = Math.Sinh(z.I % (2 * Math.PI));

			double denom = cosr * cosr + sinhi * sinhi;

			return new Complex(Math.Sin(z.R % (2 * Math.PI)) * cosr / denom, sinhi * Math.Cosh(z.I % (2 * Math.PI)) / denom);
		}

		public static Complex Sinh(Complex z)
		{
			if (z.R.Equals(0))
				return new Complex(0, Math.Sin(z.I % (2 * Math.PI)));
			if (z.I.Equals(0))
				return new Complex(Math.Sinh(z.R), 0);

			return new Complex(Math.Sinh(z.R) * Math.Cos(z.I % (2 * Math.PI)), Math.Cosh(z.R) * Math.Sin(z.I % (2 * Math.PI)));
		}

		public static Complex Cosh(Complex z)
		{
			if (z.R.Equals(0))
				return new Complex(Math.Cos(z.I % (2 * Math.PI)), 0);
			if (z.I.Equals(0))
				return new Complex(Math.Cosh(z.R), 0);

			return new Complex(Math.Cosh(z.R) * Math.Cos(z.I % (2 * Math.PI)), Math.Sinh(z.R) * Math.Sin(z.I % (2 * Math.PI)));
		}

		public static Complex Tanh(Complex z)
		{
			if (z.R.Equals(0))
				return new Complex(z.R, Math.Tan(z.I % (2 * Math.PI)));
			if (z.I.Equals(0))
				return new Complex(Math.Tanh(z.R), z.I);

			double sinhr = Math.Sinh(z.R);
			double cosi = Math.Cos(z.I % (2 * Math.PI));
			double denom = sinhr * sinhr + cosi * cosi;

			return Sinh(z) / Cosh(z);
		}

		public static Complex Asin(Complex z)
		{
			if (z.R.Equals(0))
				return new Complex(0, Math.Log(z.I + Math.Sqrt(z.I * z.I + 1)));
			if (z.I.Equals(0))
			{
				if (z.R >= -1 && z.R <= 1)
					return new Complex(Math.Asin(z.R), 0);

				return new Complex(Double.NaN, 0);
			}

			double ss = z.R * z.R + z.I * z.I + 1;
			double ssp2r = Math.Sqrt(ss + 2 * z.R);
			double ssm2r = Math.Sqrt(ss - 2 * z.R);
			double sum = .5 * (ssp2r + ssm2r);

			return new Complex(Math.Asin(.5 * (ssp2r - ssm2r)), Complex.Sign(new Complex(z.I, -z.R)) * Math.Log(sum + Math.Sqrt(sum * sum - 1)));
		}

		public static Complex Acos(Complex z)
		{
			//	    	if (z.R.Equals(0)) // Doesn't work
			//				return new Complex(Math.PI / 2, Math.Sign(z.I) * Math.Log(Math.Sqrt(z.I*z.I + 1) + Math.Sqrt(z.I*z.I)));
			if (z.I.Equals(0))
			{
				if (z.R >= -1 && z.R <= 1)
					return new Complex(Math.Acos(z.R), 0);

				return new Complex(Double.NaN, 0);
			}

			double ss = z.R * z.R + z.I * z.I + 1;
			double ssp2r = Math.Sqrt(ss + 2 * z.R);
			double ssm2r = Math.Sqrt(ss - 2 * z.R);
			double sum = .5 * (ssp2r + ssm2r);

			return new Complex(Math.Acos(ssp2r / 2 - ssm2r / 2), -Complex.Sign(new Complex(z.I, -z.R)) * Math.Log(sum + Math.Sqrt(sum * sum - 1)));
		}

		public static Complex Atan(Complex z)
		{
			if (z.I.Equals(0))
				return new Complex(Math.Atan(z.R), z.I);

			double opi = 1 + z.I;
			double omi = 1 - z.I;
			double rr = z.R * z.R;

			return new Complex(.5 * (Math.Atan2(z.R, omi) - Math.Atan2(-z.R, opi)), .25 * Math.Log((rr + opi * opi) / (rr + omi * omi)));
		}

		public static Complex Asinh(Complex z)
		{
			return Ln(z + Sqrt(z * z + 1));
		}

		public static Complex Acosh(Complex z)
		{
			return Ln(z + Sqrt(z * z - 1));
		}

		public static Complex Atanh(Complex z)
		{
			return .5 * Ln((1 + z) / (1 - z));
		}
	}

	public struct BigComplex
	{
		public BigInteger R;
		public BigInteger I;

		public BigComplex(BigInteger real, BigInteger imaginary)
		{
			R = real;
			I = imaginary;
		}

		public BigComplex(Complex z)
		{
			R = (int) z.R;
			I = (int) z.I;
		}

		public static BigComplex Zero
		{
			get { return new BigComplex(0, 0); }
		}

		public static BigComplex i
		{
			get { return new BigComplex(0, 1); }
		}

		public BigInteger Real
		{
			get { return R; }

			set { R = 1; }
		}

		public BigInteger Imaginary
		{
			get { return I; }

			set { I = 1; }
		}

		public BigInteger Radius
		{
			get { return Sqrt(R * R + I * I); }
		}

		public BigInteger RadiusSquared
		{
			get { return R * R + I * I; }
		}

		public double Angle
		{
			get { return Arg(new BigComplex(R, I)); }
		}

		public Complex toComplex()
		{
			return new Complex((double)R, (double)I);
		}

		public override string ToString()
		{
			return (System.String.Format("({0}, {1}i)", R, I));
		}

		public override bool Equals(object obj)
		{
			return this == (BigComplex)obj;
		}

		public override int GetHashCode()
		{
			return R.GetHashCode() ^ I.GetHashCode();
		}

		public static bool operator ==(BigComplex z, int d)
		{
			return z.R.Equals(d);
		}
		public static bool operator ==(int d, BigComplex z)
		{
			return z.R.Equals(d);
		}
		public static bool operator ==(BigComplex z, double d)
		{
			return z.R.Equals(d);
		}
		public static bool operator ==(double d, BigComplex z)
		{
			return z.R.Equals(d);
		}
		public static bool operator ==(BigComplex z, BigComplex d)
		{
			return z.R.Equals(d.R) && z.I.Equals(d.I);
		}

		public static bool operator !=(BigComplex z, int d)
		{
			return !z.R.Equals(d);
		}
		public static bool operator !=(int d, BigComplex z)
		{
			return !z.R.Equals(d);
		}
		public static bool operator !=(BigComplex z, double d)
		{
			return !z.R.Equals(d);
		}
		public static bool operator !=(double d, BigComplex z)
		{
			return !z.R.Equals(d);
		}
		public static bool operator !=(BigComplex z, BigComplex d)
		{
			return !(z == d);
		}

		public static BigComplex operator +(BigComplex z, int d)
		{
			return z + d;
		}
		public static BigComplex operator +(int d, BigComplex z)
		{
			return z + d;
		}
		public static BigComplex operator +(BigComplex z, BigInteger d)
		{
			return d + z;
		}
		public static BigComplex operator +(BigInteger d, BigComplex z)
		{
			return new BigComplex(z.R + d, z.I);
		}
		public static BigComplex operator +(BigComplex z, BigComplex d)
		{
			return new BigComplex(z.R + d.R, z.I + d.I);
		}

		public static BigComplex operator -(BigComplex z)
		{
			return z * -1;
		}
		public static BigComplex operator -(BigComplex z, int d)
		{
			return new BigComplex(z.R - d, z.I);
		}
		public static BigComplex operator -(int d, BigComplex z)
		{
			return new BigComplex(d - z.R, -z.I);
		}
		public static BigComplex operator -(BigComplex z, BigInteger d)
		{
			return new BigComplex(z.R - d, z.I);
		}
		public static BigComplex operator -(BigInteger d, BigComplex z)
		{
			return new BigComplex(d - z.R, -z.I);
		}
		public static BigComplex operator -(BigComplex z, BigComplex d)
		{
			return new BigComplex(z.R - d.R, z.I - d.I);
		}

		public static BigComplex operator *(BigComplex z, int d)
		{
			return new BigComplex(z.R * d, z.I * d);
		}
		public static BigComplex operator *(int d, BigComplex z)
		{
			return z * d;
		}
		public static BigComplex operator *(BigComplex z, BigInteger d)
		{
			return new BigComplex(z.R * d, z.I * d);
		}
		public static BigComplex operator *(BigInteger d, BigComplex z)
		{
			return new BigComplex(z.R * d, z.I * d);
		}
		public static BigComplex operator *(BigComplex z, BigComplex d)
		{
			return new BigComplex(z.R * d.R - z.I * d.I, z.R * d.I + d.R * z.I);
		}

		public static BigComplex operator /(BigComplex z, int d)
		{
			return new BigComplex(z.R / d, z.I / d);
		}
		public static BigComplex operator /(int d, BigComplex z)
		{
			BigInteger x = z.R * z.R + z.I * z.I;

			return new BigComplex(d * z.R / x, -d * z.I / x);
		}
		public static BigComplex operator /(BigComplex z, BigInteger d)
		{
			return new BigComplex(z.R / d, z.I / d);
		}
		public static BigComplex operator /(BigInteger d, BigComplex z)
		{
			BigInteger x = z.R * z.R + z.I * z.I;

			return new BigComplex(d * z.R / x, -d * z.I / x);
		}
		public static BigComplex operator /(BigComplex z, BigComplex d)
		{
			BigInteger r = d.R * d.R + d.I * d.I;

			if (r == 0)
				return BigComplex.Zero;

			return new BigComplex((z.R * d.R + z.I * d.I) / r, (d.R * z.I - z.R * d.I) / r);
		}

		public static BigComplex operator ^(BigComplex z, int p)
		{
			BigComplex nz = z;

			if (p.Equals(0))
				return new BigComplex(1, 0);
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

			if (BigComplex.IsZero(nz))
				return BigComplex.Zero;

			return 1 / nz;
		}

		public BigComplex Pow(int d)
		{
			return this ^ d;
		}

		public static bool IsZero(BigComplex z)
		{
			return z.R.Equals(0) && z.I.Equals(0);
		}

		public static BigComplex Inverse(BigComplex z)
		{
			BigInteger x = z.R * z.R + z.I * z.I;

			return new BigComplex(z.R / x, -z.I / x);
		}

		public static BigComplex Conjugate(BigComplex z)
		{
			return new BigComplex(z.R, -z.I);
		}

		public static int Sign(BigComplex z)
		{
			return z.R.Equals(0) ? z.I.Sign : z.R.Sign;
		}

		public static BigInteger Magnitude(BigComplex z)
		{
			return z.Radius;
		}

		public static double Arg(BigComplex z)
		{
			double arg = 0;

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
			else if (z == new BigComplex(1, 1))
				arg = Math.PI / 4;
			else
			{
				for (int i = 0; i < 1000 && (Double.IsInfinity((double)z.R) || Double.IsInfinity((double)z.I)); i++)
					z /= 10;

				arg = Math.Atan2((double) z.I, (double) z.R);
			}

			return arg;
		}

		// https://stackoverflow.com/a/58697726/13394053
		static BigInteger Sqrt(BigInteger number)
		{
			if (number < 9)
			{
				if (number == 0)
					return 0;
				if (number < 4)
					return 1;
				else
					return 2;
			}

			BigInteger n = 0, p = 0;
			var high = number >> 1;
			var low = BigInteger.Zero;

			while (high > low + 1)
			{
				n = (high + low) >> 1;
				p = n * n;
				if (number < p)
				{
					high = n;
				}
				else if (number > p)
				{
					low = n;
				}
				else
				{
					break;
				}
			}

			return number == p ? n : low;
		}

		public static BigComplex Sqrt(BigComplex c)
		{
			BigInteger r = c.Radius;

			return (BigInteger) (.5 * Math.Sqrt(2)) * new BigComplex(Sqrt(r + c.R), c.I.Sign * Sqrt(r - c.R));
		}
	}

	public class ComplexFix
	{
		public Complex c = Complex.Zero;
		public BigComplex bigC = BigComplex.Zero;
		public bool big = false;

		public ComplexFix(Complex c, BigComplex bigC, bool big = false)
		{
			this.c = c;
			this.bigC = bigC;
			this.big = big;
		}

		public static ComplexFix Zero => new ComplexFix(Complex.Zero, BigComplex.Zero, false);

		public static ComplexFix operator -(ComplexFix z)
		{
			return new ComplexFix(-z.c, -z.bigC, z.big);
		}
	}

	public struct Quaternion
	{
		public double W;
		public double X;
		public double Y;
		public double Z;

		public Quaternion(double w, double x, double y, double z)
		{
			W = w;
			X = x;
			Y = y;
			Z = z;
		}
		public Quaternion(double w, Vector3D v)
		{
			W = w;
			X = v.X;
			Y = v.Y;
			Z = v.Z;
		}
		public Quaternion(Complex c, Complex d)
		{
			W = c.R;
			X = c.I;
			Y = d.R;
			Z = d.I;
		}
		public Quaternion(Complex c)
		{
			W = c.R;
			X = c.I;
			Y = 0;
			Z = 0;
		}

		public static Quaternion FromAxisAngle(double angle, Vector3D axis)
		{
			return new Quaternion(Math.Cos(angle / 2), axis * Math.Sin(angle / 2));
		}

		public static Quaternion From2Vectors(Vector3D u, Vector3D v)
		{
			//			double m = Math.Sqrt(2 + 2 * Vector3D.DotProduct(u, v));
			//			
			//			return new Quaternion(.5 * m, Vector3D.CrossProduct(u, v) / m);


			Quaternion q = new Quaternion(0, Vector3D.CrossProduct(u, v));

			if (Equals(q.ImaginaryComponent().Magnitude(), 1))
				q.W = 1 + Vector3D.DotProduct(u, v);
			else
				q.W = Math.Sqrt(Math.Pow(u.Magnitude(), 2) * Math.Pow(v.Magnitude(), 2)) + Vector3D.DotProduct(u, v);

			q.Normalize();

			return q;
		}

		public static Quaternion Identity
		{
			get { return new Quaternion(1, 0, 0, 0); }
		}

		public double Magnitude()
		{
			return Math.Sqrt(W * W + X * X + Y * Y + Z * Z);
		}
		public static double Magnitude(Quaternion q)
		{
			return Math.Sqrt(q.W * q.W + q.X * q.X + q.Y * q.Y + q.Z * q.Z);
		}

		public Quaternion Conjugate()
		{
			return new Quaternion(W, -X, -Y, -Z);
		}
		public static Quaternion Conjugate(Quaternion q)
		{
			return new Quaternion(q.W, -q.X, -q.Y, -q.Z);
		}

		public Quaternion Inverse()
		{
			return this / (W * W + X * X + Y * Y + Z * Z);
		}
		public static Quaternion Inverse(Quaternion q)
		{
			return q / (q.W * q.W + q.X * q.X + q.Y * q.Y + q.Z * q.Z);
		}

		public Quaternion Normalize()
		{
			return this / Magnitude();
		}
		public static Quaternion Normalize(Quaternion q)
		{
			return q / Magnitude(q);
		}

		public Vector3D ImaginaryComponent()
		{
			return new Vector3D(X, Y, Z);
		}
		public static Vector3D ImaginaryComponent(Quaternion q)
		{
			return new Vector3D(q.X, q.Y, q.Z);
		}

		public double ImaginaryMagnitude()
		{
			return ImaginaryComponent().Magnitude();
		}
		public static double ImaginaryMagnitude(Quaternion q)
		{
			return q.ImaginaryComponent().Magnitude();
		}

		public Vector3D ImaginaryNormalize()
		{
			return ImaginaryComponent().Normalize();
		}
		public static Vector3D ImaginaryNormalize(Quaternion q)
		{
			return q.ImaginaryComponent().Normalize();
		}

		public Vector3D Axis()
		{
			return ImaginaryComponent().Normalize();
		}
		public static Vector3D Axis(Quaternion q)
		{
			return q.ImaginaryComponent().Normalize();
		}

		public double Angle()
		{
			return 2 * Math.Acos(Normalize().W);
		}
		public static double Angle(Quaternion q)
		{
			return 2 * Math.Acos(q.Normalize().W);
		}

		public void ToEuler(ref double yaw, ref double pitch, ref double roll)
		{
			double ysqr = Y * Y;

			// pitch (x-axis rotation)
			double t0 = 2 * (W * X + Y * Z);
			double t1 = 1 - 2 * (X * X + ysqr);
			roll = Math.Atan2(t0, t1);

			// roll (y-axis rotation)
			t0 = 2 * (W * Y - Z * X);
			t0 = t0 > 1 ? 1 : t0;
			t0 = t0 < -1 ? -1 : t0;
			pitch = Math.Asin(t0);

			// yaw (z-axis rotation)
			t0 = 2 * (W * Z + X * Y);
			t1 = 1 - 2 * (ysqr + Z * Z);
			yaw = Math.Atan2(t0, t1);
		}
		public static void ToEuler(Quaternion q, ref double yaw, ref double pitch, ref double roll)
		{
			double ysqr = q.Y * q.Y;

			// pitch (x-axis rotation)
			double t0 = 2 * (q.W * q.X + q.Y * q.Z);
			double t1 = 1 - 2 * (q.X * q.X + ysqr);
			roll = Math.Atan2(t0, t1);

			// roll (y-axis rotation)
			t0 = 2 * (q.W * q.Y - q.Z * q.X);
			t0 = t0 > 1 ? 1 : t0;
			t0 = t0 < -1 ? -1 : t0;
			pitch = Math.Asin(t0);

			// yaw (z-axis rotation)
			t0 = 2 * (q.W * q.Z + q.X * q.Y);
			t1 = 1 - 2 * (ysqr + q.Z * q.Z);
			yaw = Math.Atan2(t0, t1);
		}

		public override string ToString()
		{
			return (System.String.Format("({0}, {1}i, {2}j, {3}k)", W, X, Y, Z));
		}

		public override bool Equals(object obj)
		{
			return this == (Quaternion)obj;
		}

		public override int GetHashCode()
		{
			return W.GetHashCode() ^ X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
		}

		public static bool operator ==(Quaternion a, Quaternion b)
		{
			return a.W.Equals(b.W) && a.X.Equals(b.X) && a.Y.Equals(b.Y) && a.Z.Equals(b.Z);
		}

		public static bool operator !=(Quaternion a, Quaternion b)
		{
			return !(a == b);
		}

		public static Quaternion operator +(Quaternion a, int b)
		{
			return b + a;
		}
		public static Quaternion operator +(int a, Quaternion b)
		{
			return new Quaternion(a + b.W, b.X, b.Y, b.Z);
		}
		public static Quaternion operator +(Quaternion a, double b)
		{
			return b + a;
		}
		public static Quaternion operator +(double a, Quaternion b)
		{
			return new Quaternion(a + b.W, b.X, b.Y, b.Z);
		}
		public static Quaternion operator +(Quaternion a, Complex b)
		{
			return b + a;
		}
		public static Quaternion operator +(Complex a, Quaternion b)
		{
			return new Quaternion(a.R + b.W, a.I + b.X, b.Y, b.Z);
		}
		public static Quaternion operator +(Quaternion a, Vector3D b)
		{
			return new Quaternion(a.W, a.X + b.X, a.Y + b.Y, a.Z + b.Z);
		}
		public static Quaternion operator +(Vector3D a, Quaternion b)
		{
			return b + a;
		}
		public static Quaternion operator +(Quaternion a, Quaternion b)
		{
			return new Quaternion(a.W + b.W, a.X + b.X, a.Y + b.Y, a.Z + b.Z);
		}

		public static Quaternion operator -(Quaternion q)
		{
			return q * -1;
		}
		public static Quaternion operator -(Quaternion a, int b)
		{
			return new Quaternion(a.W - b, a.X, a.Y, a.Z);
		}
		public static Quaternion operator -(int a, Quaternion b)
		{
			return new Quaternion(a - b.W, -b.X, -b.Y, -b.Z);
		}
		public static Quaternion operator -(Quaternion a, double b)
		{
			return new Quaternion(a.W - b, a.X, a.Y, a.Z);
		}
		public static Quaternion operator -(double a, Quaternion b)
		{
			return new Quaternion(a - b.W, -b.X, -b.Y, -b.Z);
		}
		public static Quaternion operator -(Quaternion a, Complex b)
		{
			return new Quaternion(a.W - b.R, a.X - b.I, a.Y, a.Z);
		}
		public static Quaternion operator -(Complex a, Quaternion b)
		{
			return new Quaternion(a.R - b.W, a.I - b.X, -b.Y, -b.Z);
		}
		public static Quaternion operator -(Quaternion a, Vector3D b)
		{
			return new Quaternion(a.W, a.X - b.X, a.Y - b.Y, a.Z - b.Z);
		}
		public static Quaternion operator -(Vector3D a, Quaternion b)
		{
			return new Quaternion(-b.W, a.X - b.X, a.Y - b.Y, a.Z - b.Z);
		}
		public static Quaternion operator -(Quaternion a, Quaternion b)
		{
			return new Quaternion(a.W - b.W, a.X - b.X, a.Y - b.Y, a.Z - b.Z);
		}

		public static Quaternion operator *(Quaternion q, int d)
		{
			return new Quaternion(q.W * d, q.X * d, q.Y * d, q.Z * d);
		}
		public static Quaternion operator *(int d, Quaternion q)
		{
			return q * d;
		}
		public static Quaternion operator *(Quaternion q, double d)
		{
			return new Quaternion(q.W * d, q.X * d, q.Y * d, q.Z * d);
		}
		public static Quaternion operator *(double d, Quaternion q)
		{
			return q * d;
		}
		public static Quaternion operator *(Quaternion a, Quaternion b)
		{
			return new Quaternion(a.W * b.W - a.X * b.X - a.Y * b.Y - a.Z * b.Z,
								  a.W * b.X + a.X * b.W + a.Y * b.Z - a.Z * b.Y,
								  a.W * b.Y - a.X * b.Z + a.Y * b.W + a.Z * b.X,
								  a.W * b.Z + a.X * b.Y - a.Y * b.X + a.Z * b.W);
		}

		public static Quaternion operator /(Quaternion q, int d)
		{
			return new Quaternion(q.W / d, q.X / d, q.Y / d, q.Z / d);
		}
		public static Quaternion operator /(int d, Quaternion q)
		{
			return d * q.Inverse();
		}
		public static Quaternion operator /(Quaternion q, double d)
		{
			return new Quaternion(q.W / d, q.X / d, q.Y / d, q.Z / d);
		}
		public static Quaternion operator /(double d, Quaternion q)
		{
			return d * q.Inverse();
		}
		public static Quaternion operator /(Quaternion a, Quaternion b)
		{
			return new Quaternion(a.W * b.W + a.X * b.X + a.Y * b.Y + a.Z * b.Z,
								  -a.W * b.X + a.X * b.W - a.Y * b.Z + a.Z * b.Y,
								  -a.W * b.Y + a.X * b.Z + a.Y * b.W - a.Z * b.X,
								  -a.W * b.Z - a.X * b.Y + a.Y * b.X + a.Z * b.W)
						/ (b.W * b.W + b.X * b.X + b.Y * b.Y + b.Z * b.Z);
		}

		public static Quaternion operator ^(Quaternion q, int power)
		{
			return Exp((Ln(q) * power));
		}
		public static Quaternion operator ^(int n, Quaternion power)
		{
			return Exp((Math.Log(n) * power));
		}
		public static Quaternion operator ^(Quaternion q, double power)
		{
			return Exp((Ln(q) * power));
		}
		public static Quaternion operator ^(double d, Quaternion power)
		{
			return Exp((Math.Log(d) * power));
		}
		public static Quaternion operator ^(Quaternion q, Quaternion power)
		{
			return Exp((Ln(q) * power));
		}

		public Quaternion Pow(int d)
		{
			return this ^ d;
		}
		public Quaternion Pow(double d)
		{
			return this ^ d;
		}
		public Quaternion Pow(Quaternion c)
		{
			return this ^ c;
		}

		public static bool IsZero(Quaternion q)
		{
			return q.W.Equals(0) && q.X.Equals(0) && q.Y.Equals(0) && q.Z.Equals(0);
		}

		public static bool IsNaN(Quaternion q)
		{
			return Double.IsNaN(q.W) || Double.IsNaN(q.X) || Double.IsNaN(q.Y) || Double.IsNaN(q.Z);
		}

		public static bool IsInfinity(Quaternion q)
		{
			return Double.IsInfinity(q.W) || Double.IsInfinity(q.X) || Double.IsInfinity(q.Y) || Double.IsInfinity(q.Z);
		}

		public static bool IsPositiveInfinity(Quaternion q)
		{
			return Double.IsPositiveInfinity(q.W) || Double.IsPositiveInfinity(q.X) || Double.IsPositiveInfinity(q.Y) || Double.IsPositiveInfinity(q.Z);
		}

		public static bool IsNegativeInfinity(Quaternion q)
		{
			return Double.IsPositiveInfinity(q.W) || Double.IsPositiveInfinity(q.X) || Double.IsPositiveInfinity(q.Y) || Double.IsPositiveInfinity(q.Z);
		}

		public static double Abs(Quaternion q)
		{
			return Math.Sqrt(q.W * q.W + q.X * q.X + q.Y * q.Y + q.Z * q.Z);
		}

		public static Quaternion Exp(Quaternion q)
		{
			double r = Math.Sqrt(q.X * q.X + q.Y * q.Y + q.Z * q.Z);
			double et = Math.Exp(q.W);
			double s = r.Equals(0) ? 0 : et * Math.Sin(r) / r;

			q.W = et * Math.Cos(r);
			q.X *= s;
			q.Y *= s;
			q.Z *= s;

			return q;
		}

		public static Quaternion Ln(Quaternion q)
		{
			double r = Math.Sqrt(q.X * q.X + q.Y * q.Y + q.Z * q.Z);
			double t = r.Equals(0) ? 0 : Math.Atan2(r, q.W) / r;

			q.W = 0.5 * Math.Log(q.W * q.W + q.X * q.X + q.Y * q.Y + q.Z * q.Z);
			q.X *= t;
			q.Y *= t;
			q.Z *= t;

			return q;
		}

		public static Quaternion Log(Quaternion q, double d)
		{
			return Ln(q) / Math.Log(d);
		}
		public static Quaternion Log(Quaternion q, Quaternion d)
		{
			return Ln(q) / Ln(d);
		}

		public static Quaternion Sqrt(Quaternion q)
		{
			double m = 0;
			double absIm = Math.Sqrt(q.X * q.X + q.Y * q.Y + q.Z * q.Z);
			Complex z = Complex.Sqrt(new Complex(q.W, absIm));

			if (absIm.Equals(0))
				m = z.I;
			else
				m = z.I / absIm;

			return new Quaternion(z.R, m * q.X, m * q.Y, m * q.Z);
		}

		public static Quaternion Sin(Quaternion q)
		{
			double r = q.ImaginaryComponent().Magnitude();

			return new Quaternion(Math.Sin(q.W) * Math.Cosh(r), Math.Cos(q.W) * Math.Sinh(r) * (q.ImaginaryComponent() / r));
		}

		public static Quaternion Cos(Quaternion q)
		{
			double r = q.ImaginaryComponent().Magnitude();

			return new Quaternion(Math.Cos(q.W) * Math.Cosh(r), -Math.Sin(q.W) * Math.Sinh(r) * (q.ImaginaryComponent() / r));
		}

		public static Quaternion Tan(Quaternion q)
		{
			double absIm = q.ImaginaryComponent().Magnitude();
			Complex z = Complex.Tan(new Complex(q.W, absIm));

			return new Quaternion(z.R, (absIm.Equals(0) ? z.I : z.I / absIm) * q.ImaginaryComponent());
		}

		public static Quaternion Sinh(Quaternion q)
		{
			double absIm = q.ImaginaryComponent().Magnitude();
			Complex z = Complex.Sinh(new Complex(q.W, absIm));

			return new Quaternion(z.R, (absIm.Equals(0) ? z.I : z.I / absIm) * q.ImaginaryComponent());
		}

		public static Quaternion Cosh(Quaternion q)
		{
			double absIm = q.ImaginaryComponent().Magnitude();
			Complex z = Complex.Cosh(new Complex(q.W, absIm));

			return new Quaternion(z.R, (absIm.Equals(0) ? z.I : z.I / absIm) * q.ImaginaryComponent());
		}

		public static Quaternion Tanh(Quaternion q)
		{
			double absIm = q.ImaginaryComponent().Magnitude();
			Complex z = Complex.Tanh(new Complex(q.W, absIm));

			return new Quaternion(z.R, (absIm.Equals(0) ? z.I : z.I / absIm) * q.ImaginaryComponent());
		}

		public static Quaternion Asin(Quaternion q)
		{
			double absIm = q.ImaginaryComponent().Magnitude();
			Complex z = Complex.Asin(new Complex(q.W, absIm));

			return new Quaternion(z.R, (absIm.Equals(0) ? z.I : z.I / absIm) * q.ImaginaryComponent());
		}

		public static Quaternion Acos(Quaternion q)
		{
			double absIm = q.ImaginaryComponent().Magnitude();
			Complex z = Complex.Acos(new Complex(q.W, absIm));

			return new Quaternion(z.R, (absIm.Equals(0) ? z.I : z.I / absIm) * q.ImaginaryComponent());
		}

		public static Quaternion Atan(Quaternion q)
		{
			double absIm = q.ImaginaryComponent().Magnitude();
			Complex z = Complex.Atan(new Complex(q.W, absIm));

			return new Quaternion(z.R, (absIm.Equals(0) ? z.I : z.I / absIm) * q.ImaginaryComponent());
		}

		public static Quaternion Asinh(Quaternion q)
		{
			double absIm = q.ImaginaryComponent().Magnitude();
			Complex z = Complex.Asinh(new Complex(q.W, absIm));

			return new Quaternion(z.R, (absIm.Equals(0) ? z.I : z.I / absIm) * q.ImaginaryComponent());
		}

		public static Quaternion Acosh(Quaternion q)
		{
			double absIm = q.ImaginaryComponent().Magnitude();
			Complex z = Complex.Acosh(new Complex(q.W, absIm));

			return new Quaternion(z.R, (absIm.Equals(0) ? z.I : z.I / absIm) * q.ImaginaryComponent());
		}

		public static Quaternion Atanh(Quaternion q)
		{
			double absIm = q.ImaginaryComponent().Magnitude();
			Complex z = Complex.Atanh(new Complex(q.W, absIm));

			return new Quaternion(z.R, (absIm.Equals(0) ? z.I : z.I / absIm) * q.ImaginaryComponent());
		}
	}
}