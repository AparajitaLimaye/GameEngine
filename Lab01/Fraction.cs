using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab01
{
    public struct Fraction
    {
        private int _numerator;
        private int _denominator;

        //*** Properties
        public int Numerator { 
            get { return _numerator; }
            set { _numerator = value; }
        }
        public int Denominator { 
            get { return _denominator; }
            set { _denominator = value;}
        }

        public Fraction(int n, int d = 1)
        {
            _numerator = n;
            if( d == 0 )
            {
                d = 1;
            }
            _denominator = d;
            Simplify();
        }

        public override String ToString()
        {
            return _numerator + "/" + _denominator;
        }

        private void Simplify()
        {
            if(_denominator < 0)
            {
                _denominator *= -1;
                _numerator *= -1;
            }
            int gcd = GCD(_numerator, _denominator);
            _numerator /= gcd; //numerator = numerator/gcd
            _denominator /= gcd;
        }

        public static Fraction operator +(Fraction lhs, Fraction rhs)
        {

            return new Fraction((lhs._numerator * rhs._denominator) + (rhs._numerator * lhs._denominator), lhs._denominator * rhs._denominator);
        }
        public static Fraction operator -(Fraction lhs, Fraction rhs)
        {
            return new Fraction((lhs._numerator * rhs._denominator) - (rhs._numerator * lhs._denominator), lhs._denominator * rhs._denominator);
        }
        public static Fraction operator *(Fraction lhs, Fraction rhs)
        {
            return new Fraction(lhs._numerator*rhs._numerator, lhs._denominator*rhs._denominator);
        }
        public static Fraction operator /(Fraction lhs, Fraction rhs)
        {
            int cNum = lhs._numerator * rhs._denominator;
            int cDen = lhs._denominator * rhs._numerator;

            return new Fraction(cNum, cDen);
        }
        public static int GCD(int a, int b)
        {
            if (b == 0)
                return a;
            else
                return GCD(b, a % b);
        }

        /*
         public static Fraction Multiply(Fraction lhs, Fraction rhs)
        {
            return new Fraction(lhs._numerator*rhs._numerator, lhs._denominator*rhs._denominator);
        }
        public static Fraction Divide(Fraction lhs, Fraction rhs)
        {
            int cNum = lhs._numerator * rhs._denominator;
            int cDen = lhs._denominator * rhs._numerator;

            return new Fraction(cNum, cDen);
        }
         */
    }
}
