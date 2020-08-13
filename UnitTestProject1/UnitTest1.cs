using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK_Riemann_Mating;

namespace UnitTests
{
    [TestClass]
    public class Test_BigDouble
    {
        [TestMethod]
        public void TestInit()
        {
            Console.WriteLine(-55);
            Console.WriteLine("\t" + new BigDouble(-55));
            Console.WriteLine("\t" + new BigDouble(-55).ToInt());
            Console.WriteLine(-5.5);
            Console.WriteLine("\t" + new BigDouble(-5.5));
            Console.WriteLine("\t" + new BigDouble(-5.5).ToDouble());
            Console.WriteLine(-5);
            Console.WriteLine("\t" + new BigDouble(-5));
            Console.WriteLine("\t" + new BigDouble(-5).ToInt());
            Console.WriteLine(0);
            Console.WriteLine("\t" + new BigDouble(0));
            Console.WriteLine("\t" + new BigDouble(0).ToInt());
            Console.WriteLine(5);
            Console.WriteLine("\t" + new BigDouble(5));
            Console.WriteLine("\t" + new BigDouble(5).ToInt());
            Console.WriteLine(5.5);
            Console.WriteLine("\t" + new BigDouble(5.5));
            Console.WriteLine("\t" + new BigDouble(5.5).ToDouble());
            Console.WriteLine(55);
            Console.WriteLine("\t" + new BigDouble(55));
            Console.WriteLine("\t" + new BigDouble(55).ToInt());
        }

        [TestMethod]
        public void TestAdd()
        {/**/
            Console.WriteLine("114 + 613");
            Console.WriteLine("\t" + (114 + 613));
            Console.WriteLine("114 + e 613");
            Console.WriteLine("\t" + (114 + new BigDouble(613)));
            Console.WriteLine("e 114 + 613");
            Console.WriteLine("\t" + (new BigDouble(114) + 613));
            Console.WriteLine("e 114 + e 613");
            Console.WriteLine("\t" + (new BigDouble(114) + new BigDouble(613)));

            Console.WriteLine();
            Console.WriteLine("114 + 613");
            Console.WriteLine("\t" + (new BigDouble(114) + new BigDouble(613)));
            Console.WriteLine("-114 + -613");
            Console.WriteLine("\t" + (new BigDouble(-114) + new BigDouble(-613)));
            Console.WriteLine("114 + -613");
            Console.WriteLine("\t" + (new BigDouble(114) + new BigDouble(-613)));
            Console.WriteLine("-114 + 613");
            Console.WriteLine("\t" + (new BigDouble(-114) + new BigDouble(613)));

            Console.WriteLine();

            Console.WriteLine("11.4 + 61.3");
            Console.WriteLine("\t" + (new BigDouble(11.4) + new BigDouble(61.3)));
            Console.WriteLine("-11.4 + -61.3");
            Console.WriteLine("\t" + (new BigDouble(-11.4) + new BigDouble(-61.3)));
            Console.WriteLine("11.4 + -61.3");
            Console.WriteLine("\t" + (new BigDouble(11.4) + new BigDouble(-61.3)));
            Console.WriteLine("-11.4 + 61.3");
            Console.WriteLine("\t" + (new BigDouble(-11.4) + new BigDouble(61.3)));

            Console.WriteLine();

            Console.WriteLine("11.4 + 6.13");
            Console.WriteLine("\t" + (new BigDouble(11.4) + new BigDouble(6.13)));
            Console.WriteLine("-11.4 + -6.13");
            Console.WriteLine("\t" + (new BigDouble(-11.4) + new BigDouble(-6.13)));
            Console.WriteLine("11.4 + -6.13");
            Console.WriteLine("\t" + (new BigDouble(11.4) + new BigDouble(-6.13)));
            Console.WriteLine("-11.4 + 6.13");
            Console.WriteLine("\t" + (new BigDouble(-11.4) + new BigDouble(6.13)));

            Console.WriteLine();

            Console.WriteLine("400000019 + 19");
            Console.WriteLine("\t" + (new BigDouble(400000019) + new BigDouble(19)));
            Console.WriteLine("-400000019 + -19");
            Console.WriteLine("\t" + (new BigDouble(-400000019) + new BigDouble(-19)));
            Console.WriteLine("400000019 + -19");
            Console.WriteLine("\t" + (new BigDouble(400000019) + new BigDouble(-19)));
            Console.WriteLine("-400000019 + 19");
            Console.WriteLine("\t" + (new BigDouble(-400000019) + new BigDouble(19)));

            Console.WriteLine();

            Random r = new Random();
            double d1;
            double d2;

            for (int i = 0; i < 100; i++)
            {
                d1 = (r.NextDouble() - .5) * Math.Pow(10, 100 * r.NextDouble());
                d2 = (r.NextDouble() - .5) * Math.Pow(10, 100 * r.NextDouble());

                Console.WriteLine(d1 + " + " + d2);
                Console.WriteLine("\tAnswer: " + (d1 + d2));
                Console.WriteLine("\tAnswer: " + (new BigDouble(d1) + new BigDouble(d2)).ToDouble());
                Console.WriteLine("\tExp: " + (new BigDouble(d1) + new BigDouble(d2)));
                Console.WriteLine();
            }
        }

        [TestMethod]
        public void TestSubtract()
        {/**/
            Console.WriteLine("114 - e 613");
            Console.WriteLine("\t" + (114 - new BigDouble(613)));
            Console.WriteLine("e 114 - 613");
            Console.WriteLine("\t" + (new BigDouble(114) - 613));
            Console.WriteLine("e 114 - e 613");
            Console.WriteLine("\t" + (new BigDouble(114) - new BigDouble(613)));

            Console.WriteLine();
            Console.WriteLine("114 - 613");
            Console.WriteLine("\t" + (new BigDouble(114) - new BigDouble(613)));
            Console.WriteLine("-114 - -613");
            Console.WriteLine("\t" + (new BigDouble(-114) - new BigDouble(-613)));
            Console.WriteLine("114 - -613");
            Console.WriteLine("\t" + (new BigDouble(114) - new BigDouble(-613)));
            Console.WriteLine("-114 - 613");
            Console.WriteLine("\t" + (new BigDouble(-114) - new BigDouble(613)));

            Console.WriteLine();

            Console.WriteLine("11.4 - 61.3");
            Console.WriteLine("\t" + (new BigDouble(11.4) - new BigDouble(61.3)));
            Console.WriteLine("-11.4 - -61.3");
            Console.WriteLine("\t" + (new BigDouble(-11.4) - new BigDouble(-61.3)));
            Console.WriteLine("11.4 - -61.3");
            Console.WriteLine("\t" + (new BigDouble(11.4) - new BigDouble(-61.3)));
            Console.WriteLine("-11.4 - 61.3");
            Console.WriteLine("\t" + (new BigDouble(-11.4) - new BigDouble(61.3)));

            Console.WriteLine();

            Console.WriteLine("11.4 - 6.13");
            Console.WriteLine("\t" + (new BigDouble(11.4) - new BigDouble(6.13)));
            Console.WriteLine("-11.4 - -6.13");
            Console.WriteLine("\t" + (new BigDouble(-11.4) - new BigDouble(-6.13)));
            Console.WriteLine("11.4 - -6.13");
            Console.WriteLine("\t" + (new BigDouble(11.4) - new BigDouble(-6.13)));
            Console.WriteLine("-11.4 - 6.13");
            Console.WriteLine("\t" + (new BigDouble(-11.4) - new BigDouble(6.13)));

            Console.WriteLine();

            Console.WriteLine("400000019 - 19");
            Console.WriteLine("\t" + (new BigDouble(400000019) - new BigDouble(19)));
            Console.WriteLine("-400000019 - -19");
            Console.WriteLine("\t" + (new BigDouble(-400000019) - new BigDouble(-19)));
            Console.WriteLine("400000019 - -19");
            Console.WriteLine("\t" + (new BigDouble(400000019) - new BigDouble(-19)));
            Console.WriteLine("-400000019 - 19");
            Console.WriteLine("\t" + (new BigDouble(-400000019) - new BigDouble(19)));

            Console.WriteLine();

            Random r = new Random();
            double d1;
            double d2;

            for (int i = 0; i < 100; i++)
            {
                d1 = (r.NextDouble() - .5) * Math.Pow(10, 100 * r.NextDouble());
                d2 = (r.NextDouble() - .5) * Math.Pow(10, 100 * r.NextDouble());

                Console.WriteLine(d1 + " - " + d2);
                Console.WriteLine("\tAnswer: " + (d1 - d2));
                Console.WriteLine("\tAnswer: " + (new BigDouble(d1) - new BigDouble(d2)).ToDouble());
                Console.WriteLine("\tExp: " + (new BigDouble(d1) - new BigDouble(d2)));
                Console.WriteLine();
            }
        }

        [TestMethod]
        public void TestMultiply()
        {/**/
            Console.WriteLine("114 * 613");
            Console.WriteLine("\t" + (114 * 613));
            Console.WriteLine("114 * e 613");
            Console.WriteLine("\t" + (114 * new BigDouble(613)));
            Console.WriteLine("e 114 * 613");
            Console.WriteLine("\t" + (new BigDouble(114) * 613));
            Console.WriteLine("e 114 * e 613");
            Console.WriteLine("\t" + (new BigDouble(114) * new BigDouble(613)));

            Console.WriteLine();
            Console.WriteLine("114 * 613");
            Console.WriteLine("\t" + (new BigDouble(114) * new BigDouble(613)));
            Console.WriteLine("-114 * -613");
            Console.WriteLine("\t" + (new BigDouble(-114) * new BigDouble(-613)));
            Console.WriteLine("114 * -613");
            Console.WriteLine("\t" + (new BigDouble(114) * new BigDouble(-613)));
            Console.WriteLine("-114 * 613");
            Console.WriteLine("\t" + (new BigDouble(-114) * new BigDouble(613)));

            Console.WriteLine();

            Console.WriteLine("11.4 * 61.3");
            Console.WriteLine("\t" + (new BigDouble(11.4) * new BigDouble(61.3)));
            Console.WriteLine("-11.4 * -61.3");
            Console.WriteLine("\t" + (new BigDouble(-11.4) * new BigDouble(-61.3)));
            Console.WriteLine("11.4 * -61.3");
            Console.WriteLine("\t" + (new BigDouble(11.4) * new BigDouble(-61.3)));
            Console.WriteLine("-11.4 * 61.3");
            Console.WriteLine("\t" + (new BigDouble(-11.4) * new BigDouble(61.3)));

            Console.WriteLine();

            Console.WriteLine("11.4 * 6.13");
            Console.WriteLine("\t" + (new BigDouble(11.4) * new BigDouble(6.13)));
            Console.WriteLine("-11.4 * -6.13");
            Console.WriteLine("\t" + (new BigDouble(-11.4) * new BigDouble(-6.13)));
            Console.WriteLine("11.4 * -6.13");
            Console.WriteLine("\t" + (new BigDouble(11.4) * new BigDouble(-6.13)));
            Console.WriteLine("-11.4 * 6.13");
            Console.WriteLine("\t" + (new BigDouble(-11.4) * new BigDouble(6.13)));

            Console.WriteLine();

            Console.WriteLine("400000019 * 19");
            Console.WriteLine("\t" + (new BigDouble(400000019) * new BigDouble(19)));
            Console.WriteLine("-400000019 * -19");
            Console.WriteLine("\t" + (new BigDouble(-400000019) * new BigDouble(-19)));
            Console.WriteLine("400000019 * -19");
            Console.WriteLine("\t" + (new BigDouble(400000019) * new BigDouble(-19)));
            Console.WriteLine("-400000019 * 19");
            Console.WriteLine("\t" + (new BigDouble(-400000019) * new BigDouble(19)));

            Console.WriteLine();

            Random r = new Random();
            double d1;
            double d2;

            for (int i = 0; i < 100; i++)
            {
                d1 = (r.NextDouble() - .5) * Math.Pow(10, 100 * r.NextDouble());
                d2 = (r.NextDouble() - .5) * Math.Pow(10, 100 * r.NextDouble());

                Console.WriteLine(d1 + " * e " + d2);
                Console.WriteLine("\tAnswer: " + (d1 * d2));
                Console.WriteLine("\tAnswer: " + (d1 * new BigDouble(d2)).ToDouble());
                Console.WriteLine("\tExp: " + (d1 * new BigDouble(d2)));
                Console.WriteLine();


                d1 = (r.NextDouble() - .5) * Math.Pow(10, 100 * r.NextDouble());
                d2 = (r.NextDouble() - .5) * Math.Pow(10, 100 * r.NextDouble());

                Console.WriteLine("e " + d1 + " * " + d2);
                Console.WriteLine("\tAnswer: " + (d1 * d2));
                Console.WriteLine("\tAnswer: " + (new BigDouble(d1) * d2).ToDouble());
                Console.WriteLine("\tExp: " + (new BigDouble(d1) * d2));
                Console.WriteLine();


                d1 = (r.NextDouble() - .5) * Math.Pow(10, 100 * r.NextDouble());
                d2 = (r.NextDouble() - .5) * Math.Pow(10, 100 * r.NextDouble());

                Console.WriteLine("e " + d1 + " * e " + d2);
                Console.WriteLine("\tAnswer: " + (d1 * d2));
                Console.WriteLine("\tAnswer: " + (new BigDouble(d1) * new BigDouble(d2)).ToDouble());
                Console.WriteLine("\tExp: " + (new BigDouble(d1) * new BigDouble(d2)));
                Console.WriteLine();
            }
            /**/
        }

        [TestMethod]
        public void TestDivide()
        {/**/
            Console.WriteLine("114 / 613");
            Console.WriteLine("\t" + (114.0 / 613.0));
            Console.WriteLine("114 / e 613");
            Console.WriteLine("\t" + (114 / new BigDouble(613)));
            Console.WriteLine("e 114 / 613");
            Console.WriteLine("\t" + (new BigDouble(114) / 613));
            Console.WriteLine("e 114 / e 613");
            Console.WriteLine("\t" + (new BigDouble(114) / new BigDouble(613)));

            Console.WriteLine();
            Console.WriteLine("114 / 613");
            Console.WriteLine("\t" + (new BigDouble(114) / new BigDouble(613)));
            Console.WriteLine("-114 / -613");
            Console.WriteLine("\t" + (new BigDouble(-114) / new BigDouble(-613)));
            Console.WriteLine("114 / -613");
            Console.WriteLine("\t" + (new BigDouble(114) / new BigDouble(-613)));
            Console.WriteLine("-114 / 613");
            Console.WriteLine("\t" + (new BigDouble(-114) / new BigDouble(613)));

            Console.WriteLine();

            Console.WriteLine("11.4 / 61.3");
            Console.WriteLine("\t" + (new BigDouble(11.4) / new BigDouble(61.3)));
            Console.WriteLine("-11.4 / -61.3");
            Console.WriteLine("\t" + (new BigDouble(-11.4) / new BigDouble(-61.3)));
            Console.WriteLine("11.4 / -61.3");
            Console.WriteLine("\t" + (new BigDouble(11.4) / new BigDouble(-61.3)));
            Console.WriteLine("-11.4 / 61.3");
            Console.WriteLine("\t" + (new BigDouble(-11.4) / new BigDouble(61.3)));

            Console.WriteLine();

            Console.WriteLine("11.4 / 6.13");
            Console.WriteLine("\t" + (new BigDouble(11.4) / new BigDouble(6.13)));
            Console.WriteLine("-11.4 / -6.13");
            Console.WriteLine("\t" + (new BigDouble(-11.4) / new BigDouble(-6.13)));
            Console.WriteLine("11.4 / -6.13");
            Console.WriteLine("\t" + (new BigDouble(11.4) / new BigDouble(-6.13)));
            Console.WriteLine("-11.4 / 6.13");
            Console.WriteLine("\t" + (new BigDouble(-11.4) / new BigDouble(6.13)));

            Console.WriteLine();

            Console.WriteLine("400000019 / 19");
            Console.WriteLine("\t" + (new BigDouble(400000019) / new BigDouble(19)));
            Console.WriteLine("-400000019 / -19");
            Console.WriteLine("\t" + (new BigDouble(-400000019) / new BigDouble(-19)));
            Console.WriteLine("400000019 / -19");
            Console.WriteLine("\t" + (new BigDouble(400000019) / new BigDouble(-19)));
            Console.WriteLine("-400000019 / 19");
            Console.WriteLine("\t" + (new BigDouble(-400000019) / new BigDouble(19)));

            Console.WriteLine();

            Random r = new Random();
            double d1;
            double d2;

            for (int i = 0; i < 100; i++)
            {
                d1 = (r.NextDouble() - .5) * Math.Pow(10, 100 * r.NextDouble());
                d2 = (r.NextDouble() - .5) * Math.Pow(10, 100 * r.NextDouble());

                Console.WriteLine(d1 + " / " + d2);
                Console.WriteLine("\tAnswer: " + (d1 / d2));
                Console.WriteLine("\tAnswer: " + (new BigDouble(d1) / new BigDouble(d2)).ToDouble());
                Console.WriteLine("\tExp: " + (new BigDouble(d1) / new BigDouble(d2)));
                Console.WriteLine();
            }
            /**/
        }

        [TestMethod]
        public void TestPower()
        {/**/
            Console.WriteLine("14 ^ 61");
            Console.WriteLine("\t" + Math.Pow(14, 61));
            Console.WriteLine("14 ^ -61");
            Console.WriteLine("\t" + Math.Pow(14, -61));
            Console.WriteLine("e 14 ^ 61");
            Console.WriteLine("\t" + (new BigDouble(14) ^ 61));
            Console.WriteLine("e 14 ^ -61");
            Console.WriteLine("\t" + (new BigDouble(14) ^ -61));
            Console.WriteLine("e -14 ^ 61");
            Console.WriteLine("\t" + (new BigDouble(-14) ^ 61));
            Console.WriteLine("e -14 ^ -61");
            Console.WriteLine("\t" + (new BigDouble(-14) ^ -61));

            Console.WriteLine();

            Console.WriteLine("1.4 ^ 61");
            Console.WriteLine("\t" + Math.Pow(1.4, 61));
            Console.WriteLine("1.4 ^ -61");
            Console.WriteLine("\t" + Math.Pow(1.4, -61));
            Console.WriteLine("e 1.4 ^ 61");
            Console.WriteLine("\t" + (new BigDouble(1.4) ^ 61));
            Console.WriteLine("e 1.4 ^ -61");
            Console.WriteLine("\t" + (new BigDouble(1.4) ^ -61));
            Console.WriteLine("e -1.4 ^ 61");
            Console.WriteLine("\t" + (new BigDouble(-1.4) ^ 61));
            Console.WriteLine("e -1.4 ^ -61");
            Console.WriteLine("\t" + (new BigDouble(-1.4) ^ -61));

            Console.WriteLine();

            Random r = new Random();
            int n;
            double d;

            for (int i = 0; i < 100; i++)
            {
                n = r.Next(-200, 200);
                d = (r.NextDouble() - .5) * 1000;

                Console.WriteLine(d + " ^ " + n);
                Console.WriteLine("\tAnswer: " + (Math.Pow(d, n)));
                Console.WriteLine("\tAnswer: " + (new BigDouble(d) ^ n).ToDouble());
                Console.WriteLine("\tExp: " + (new BigDouble(d) ^ n));
                Console.WriteLine();
            }
            /**/
        }

        [TestMethod]
        public void TestSqrt()
        {/**/
            Random r = new Random();
            double d;

            for (int i = 0; i < 100; i++)
            {
                d = r.NextDouble() * Math.Pow(10, 100 * r.NextDouble());

                Console.WriteLine("sqrt " + d);
                Console.WriteLine("\tAnswer: " + Math.Sqrt(d));
                Console.WriteLine("\tAnswer: " + BigDouble.Sqrt(new BigDouble(d)).ToDouble());
                Console.WriteLine("\tExp: " + BigDouble.Sqrt(new BigDouble(d)));
                Console.WriteLine();
            }
            /**/
        }
    }

    [TestClass]
    public class Test_BigComplex
    {
        [TestMethod]
        public void TestInit()
        {
            Console.WriteLine("0 + 0i");
            Console.WriteLine("\t" + new BigComplex(0, 0));
            Console.WriteLine("1 + 0i");
            Console.WriteLine("\t" + new BigComplex(1, 0));
            Console.WriteLine("0 + 1i");
            Console.WriteLine("\t" + new BigComplex(0, 1));
            Console.WriteLine("-1 + -1i");
            Console.WriteLine("\t" + new BigComplex(-1, -1));
            Console.WriteLine("1e99 + -1e99i");
            Console.WriteLine("\t" + new BigComplex(1e99, -1e99));
            Console.WriteLine("e 1e99 + e -1e99i");
            Console.WriteLine("\t" + new BigComplex(new BigDouble(1e99), new BigDouble(-1e99)));
        }

        [TestMethod]
        public void TestAdd()
        {/**/
            Console.WriteLine("(314 + 159265i) + (3 + 56979i)");
            Console.WriteLine("\t" + (new Complex(314, 159265) + new Complex(3, 56979)));
            Console.WriteLine("e (314 + 159265i) + e (3 + 56979i)");
            Console.WriteLine("\t" + (new BigComplex(314, 159265) + new BigComplex(3, 56979)));
            Console.WriteLine("(314 + 159265i) + -(3 + 56979i)");
            Console.WriteLine("\t" + (new Complex(314, 159265) + -new Complex(3, 56979)));
            Console.WriteLine("e (314 + 159265i) + e -(3 + 56979i)");
            Console.WriteLine("\t" + (new BigComplex(314, 159265) + -new BigComplex(3, 56979)));

            Console.WriteLine();

            Console.WriteLine("(3.14 + .159265i) + (3 + 5697.9i)");
            Console.WriteLine("\t" + (new Complex(3.14, .159265) + new Complex(3, 5697.9)));
            Console.WriteLine("e (3.14 + .159265i) + e (3 + 5697.9i)");
            Console.WriteLine("\t" + (new BigComplex(3.14, .159265) + new BigComplex(3, 5697.9)));
            Console.WriteLine("(3.14 + .159265i) + -(3 + 5697.9i)");
            Console.WriteLine("\t" + (new Complex(3.14, .159265) + -new Complex(3, 5697.9)));
            Console.WriteLine("e (3.14 + .159265i) + e -(3 + 5697.9i)");
            Console.WriteLine("\t" + (new BigComplex(3.14, .159265) + -new BigComplex(3, 5697.9)));

            Console.WriteLine();

            Random r = new Random();
            double d1;
            double d2;
            double d3;
            double d4;

            for (int i = 0; i < 100; i++)
            {
                d1 = (r.NextDouble() - .5) * Math.Pow(10, 10 * r.NextDouble() - 5);
                d2 = (r.NextDouble() - .5) * Math.Pow(10, 10 * r.NextDouble() - 5);
                d3 = (r.NextDouble() - .5) * Math.Pow(10, 10 * r.NextDouble() - 5);
                d4 = (r.NextDouble() - .5) * Math.Pow(10, 10 * r.NextDouble() - 5);

                Console.WriteLine("(" + d1 + " + " + d2 + "i) + (" + d3 + " + " + d4 + "i)");
                Console.WriteLine("\tAnswer: " + (new Complex(d1, d2) + new Complex(d3, d4)));
                Console.WriteLine("\tAnswer: " + (new BigComplex(d1, d2) + new BigComplex(d3, d4)).ToComplex());
                Console.WriteLine("\tExp: " + (new BigComplex(d1, d2) + new BigComplex(d3, d4)));
                Console.WriteLine();
            }
        }

        [TestMethod]
        public void TestSubtract()
        {/**/
            Console.WriteLine("(314 + 159265i) - (3 + 56979i)");
            Console.WriteLine("\t" + (new Complex(314, 159265) - new Complex(3, 56979)));
            Console.WriteLine("e (314 + 159265i) - e (3 + 56979i)");
            Console.WriteLine("\t" + (new BigComplex(314, 159265) - new BigComplex(3, 56979)));
            Console.WriteLine("(314 + 159265i) - -(3 + 56979i)");
            Console.WriteLine("\t" + (new Complex(314, 159265) - -new Complex(3, 56979)));
            Console.WriteLine("e (314 + 159265i) - e -(3 + 56979i)");
            Console.WriteLine("\t" + (new BigComplex(314, 159265) - -new BigComplex(3, 56979)));

            Console.WriteLine();

            Console.WriteLine("(3.14 + .159265i) - (3 + 5697.9i)");
            Console.WriteLine("\t" + (new Complex(3.14, .159265) - new Complex(3, 5697.9)));
            Console.WriteLine("e (3.14 + .159265i) - e (3 + 5697.9i)");
            Console.WriteLine("\t" + (new BigComplex(3.14, .159265) - new BigComplex(3, 5697.9)));
            Console.WriteLine("(3.14 + .159265i) - -(3 + 5697.9i)");
            Console.WriteLine("\t" + (new Complex(3.14, .159265) - -new Complex(3, 5697.9)));
            Console.WriteLine("e (3.14 + .159265i) - e -(3 + 5697.9i)");
            Console.WriteLine("\t" + (new BigComplex(3.14, .159265) - -new BigComplex(3, 5697.9)));

            Console.WriteLine();

            Random r = new Random();
            double d1;
            double d2;
            double d3;
            double d4;

            for (int i = 0; i < 100; i++)
            {
                d1 = (r.NextDouble() - .5) * Math.Pow(10, 10 * r.NextDouble() - 5);
                d2 = (r.NextDouble() - .5) * Math.Pow(10, 10 * r.NextDouble() - 5);
                d3 = (r.NextDouble() - .5) * Math.Pow(10, 10 * r.NextDouble() - 5);
                d4 = (r.NextDouble() - .5) * Math.Pow(10, 10 * r.NextDouble() - 5);

                Console.WriteLine("(" + d1 + " + " + d2 + "i) - (" + d3 + " + " + d4 + "i)");
                Console.WriteLine("\tAnswer: " + (new Complex(d1, d2) - new Complex(d3, d4)));
                Console.WriteLine("\tAnswer: " + (new BigComplex(d1, d2) - new BigComplex(d3, d4)).ToComplex());
                Console.WriteLine("\tExp: " + (new BigComplex(d1, d2) - new BigComplex(d3, d4)));
                Console.WriteLine();
            }
        }

        [TestMethod]
        public void TestMultiply()
        {/**/
            Console.WriteLine("(314 + 159265i) * (3 + 56979i)");
            Console.WriteLine("\t" + (new Complex(314, 159265) * new Complex(3, 56979)));
            Console.WriteLine("e (314 + 159265i) * e (3 + 56979i)");
            Console.WriteLine("\t" + (new BigComplex(314, 159265) * new BigComplex(3, 56979)));
            Console.WriteLine("(314 + 159265i) * -(3 + 56979i)");
            Console.WriteLine("\t" + (new Complex(314, 159265) * -new Complex(3, 56979)));
            Console.WriteLine("e (314 + 159265i) * e -(3 + 56979i)");
            Console.WriteLine("\t" + (new BigComplex(314, 159265) * -new BigComplex(3, 56979)));

            Console.WriteLine();

            Console.WriteLine("(3.14 + .159265i) * (3 + 5697.9i)");
            Console.WriteLine("\t" + (new Complex(3.14, .159265) * new Complex(3, 5697.9)));
            Console.WriteLine("e (3.14 + .159265i) * e (3 + 5697.9i)");
            Console.WriteLine("\t" + (new BigComplex(3.14, .159265) * new BigComplex(3, 5697.9)));
            Console.WriteLine("(3.14 + .159265i) * -(3 + 5697.9i)");
            Console.WriteLine("\t" + (new Complex(3.14, .159265) * -new Complex(3, 5697.9)));
            Console.WriteLine("e (3.14 + .159265i) * e -(3 + 5697.9i)");
            Console.WriteLine("\t" + (new BigComplex(3.14, .159265) * -new BigComplex(3, 5697.9)));

            Console.WriteLine();

            Random r = new Random();
            double d1;
            double d2;
            double d3;
            double d4;

            for (int i = 0; i < 100; i++)
            {
                d1 = (r.NextDouble() - .5) * Math.Pow(10, 10 * r.NextDouble() - 5);
                d2 = (r.NextDouble() - .5) * Math.Pow(10, 10 * r.NextDouble() - 5);
                d3 = (r.NextDouble() - .5) * Math.Pow(10, 10 * r.NextDouble() - 5);
                d4 = (r.NextDouble() - .5) * Math.Pow(10, 10 * r.NextDouble() - 5);

                Console.WriteLine("(" + d1 + " + " + d2 + "i) * (" + d3 + " + " + d4 + "i)");
                Console.WriteLine("\tAnswer: " + (new Complex(d1, d2) * new Complex(d3, d4)));
                Console.WriteLine("\tAnswer: " + (new BigComplex(d1, d2) * new BigComplex(d3, d4)).ToComplex());
                Console.WriteLine("\tExp: " + (new BigComplex(d1, d2) * new BigComplex(d3, d4)));
                Console.WriteLine();
            }
            /**/
        }

        [TestMethod]
        public void TestDivide()
        {/**/
            Console.WriteLine("(314 + 159265i) / (3 + 56979i)");
            Console.WriteLine("\t" + (new Complex(314, 159265) / new Complex(3, 56979)));
            Console.WriteLine("e (314 + 159265i) / e (3 + 56979i)");
            Console.WriteLine("\t" + (new BigComplex(314, 159265) / new BigComplex(3, 56979)));
            Console.WriteLine("(314 + 159265i) / -(3 + 56979i)");
            Console.WriteLine("\t" + (new Complex(314, 159265) / -new Complex(3, 56979)));
            Console.WriteLine("e (314 + 159265i) / e -(3 + 56979i)");
            Console.WriteLine("\t" + (new BigComplex(314, 159265) / -new BigComplex(3, 56979)));

            Console.WriteLine();

            Console.WriteLine("(3.14 + .159265i) / (3 + 5697.9i)");
            Console.WriteLine("\t" + (new Complex(3.14, .159265) / new Complex(3, 5697.9)));
            Console.WriteLine("e (3.14 + .159265i) / e (3 + 5697.9i)");
            Console.WriteLine("\t" + (new BigComplex(3.14, .159265) / new BigComplex(3, 5697.9)));
            Console.WriteLine("(3.14 + .159265i) / -(3 + 5697.9i)");
            Console.WriteLine("\t" + (new Complex(3.14, .159265) / -new Complex(3, 5697.9)));
            Console.WriteLine("e (3.14 + .159265i) / e -(3 + 5697.9i)");
            Console.WriteLine("\t" + (new BigComplex(3.14, .159265) / -new BigComplex(3, 5697.9)));

            Console.WriteLine();

            Random r = new Random();
            double d1;
            double d2;
            double d3;
            double d4;

            for (int i = 0; i < 100; i++)
            {
                d1 = (r.NextDouble() - .5) * Math.Pow(10, 10 * r.NextDouble() - 5);
                d2 = (r.NextDouble() - .5) * Math.Pow(10, 10 * r.NextDouble() - 5);
                d3 = (r.NextDouble() - .5) * Math.Pow(10, 10 * r.NextDouble() - 5);
                d4 = (r.NextDouble() - .5) * Math.Pow(10, 10 * r.NextDouble() - 5);

                Console.WriteLine("(" + d1 + " + " + d2 + "i) / (" + d3 + " + " + d4 + "i)");
                Console.WriteLine("\tAnswer: " + (new Complex(d1, d2) / new Complex(d3, d4)));
                Console.WriteLine("\tAnswer: " + (new BigComplex(d1, d2) / new BigComplex(d3, d4)).ToComplex());
                Console.WriteLine("\tExp: " + (new BigComplex(d1, d2) / new BigComplex(d3, d4)));
                Console.WriteLine();
            }
            /**/
        }

        [TestMethod]
        public void TestPower()
        {/**/
            Console.WriteLine("(314 + 159265i) * 3");
            Console.WriteLine("\t" + (new Complex(314, 159265) ^ 3));
            Console.WriteLine("e (314 + 159265i) * 3");
            Console.WriteLine("\t" + (new BigComplex(314, 159265) ^ 3));
            Console.WriteLine("(314 + 159265i) * -3");
            Console.WriteLine("\t" + (new Complex(314, 159265) ^ -3));
            Console.WriteLine("e (314 + 159265i) * -3");
            Console.WriteLine("\t" + (new BigComplex(314, 159265) ^ -3));

            Console.WriteLine();

            Console.WriteLine("(3.14 + .159265i) ^ 3");
            Console.WriteLine("\t" + (new Complex(3.14, .159265) ^ 3));
            Console.WriteLine("e (3.14 + .159265i) ^ 3");
            Console.WriteLine("\t" + (new BigComplex(3.14, .159265) ^ 3));
            Console.WriteLine("(3.14 + .159265i) ^ -3");
            Console.WriteLine("\t" + (new Complex(3.14, .159265) ^ -3));
            Console.WriteLine("e (3.14 + .159265i) ^ -3");
            Console.WriteLine("\t" + (new BigComplex(3.14, .159265) ^ -3));

            Console.WriteLine();

            Random r = new Random();
            int n;
            double d1;
            double d2;

            for (int i = 0; i < 100; i++)
            {
                n = r.Next(-400, 400);
                d1 = (r.NextDouble() - .5) * Math.Pow(10, 10 * r.NextDouble() - 5);
                d2 = (r.NextDouble() - .5) * Math.Pow(10, 10 * r.NextDouble() - 5);

                Console.WriteLine("(" + d1 + " + " + d2 + "i) ^ " + n);
                Console.WriteLine("\tAnswer: " + (new Complex(d1, d2) ^ n));
                Console.WriteLine("\tAnswer: " + (new BigComplex(d1, d2) ^ n).ToComplex());
                Console.WriteLine("\tExp: " + (new BigComplex(d1, d2) ^ n));
                Console.WriteLine();
            }
            /**/
        }

        [TestMethod]
        public void TestSqrt()
        {/**/
            Random r = new Random();
            double d1;
            double d2;

            for (int i = 0; i < 100; i++)
            {
                d1 = r.NextDouble() * Math.Pow(10, 100 * r.NextDouble());
                d2 = r.NextDouble() * Math.Pow(10, 100 * r.NextDouble());

                Console.WriteLine("sqrt " + "(" + d1 + " + " + d2 + "i)");
                Console.WriteLine("\tAnswer: " + Complex.Sqrt(new Complex(d1, d2)));
                Console.WriteLine("\tAnswer: " + BigComplex.Sqrt(new BigComplex(d1, d2)).ToComplex());
                Console.WriteLine("\tExp: " + BigComplex.Sqrt(new BigComplex(d1, d2)));
                Console.WriteLine();
            }
            /**/
        }
    }

    [TestClass]
    public class Test_Custom
    {
        [TestMethod]
        public void Julia()
        {
            //var p = new BigComplex(-.835046398, -.231926809);
            //var q = new BigComplex(.284884537, -.011121822);
            var p = new BigComplex(-.835, -.2321);
            var q = new BigComplex(.285, .01);

            var zp = BigComplex.Zero;
            var zq = BigComplex.Zero;

            int break_i = 35;

            for (int i = 0; i < 50; i++)
            {
                if (i == break_i)
                {
                    int a = 0;
                }

                zp = (zp ^ 2);
                zq = (zq ^ 2);

                if (i >= break_i)
                {
                    Console.WriteLine(i);
                    Console.WriteLine("\tzp: " + zp);
                    Console.WriteLine("\tzq: " + zq);
                }

                zp += p;
                zq += q;

                if (i >= break_i)
                {
                    Console.WriteLine();
                    Console.WriteLine("\tzp: " + zp);
                    Console.WriteLine("\tzq: " + zq);
                    Console.WriteLine();
                }
            }
        }

        [TestMethod]
        public void Precision()
        {
            var a = new BigDouble(1.7, -13);
            var b = new BigDouble(2.6, -26);

            Console.WriteLine(a + b);
            Console.WriteLine(a - b);
            Console.WriteLine(a*a);
            Console.WriteLine(b*b);
            Console.WriteLine(a*a + b*b);
            Console.WriteLine(a*a - b*b);
            Console.WriteLine();
            Console.WriteLine(BigDouble.Sqrt(a * a + b * b));
            Console.WriteLine(BigDouble.Sqrt(a * a + b * b) - a);
        }
    }
}
