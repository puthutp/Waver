using System;

namespace waver
{
    /// <summary>
    /// Complex number.
    /// </summary>
    struct IdolComplexNumber
    {
        public double Re;
        public double Im;

        public IdolComplexNumber(double re)
        {
            this.Re = re;
            this.Im = 0;
        }

        public IdolComplexNumber(double re, double im)
        {
            this.Re = re;
            this.Im = im;
        }

        public static IdolComplexNumber operator *(IdolComplexNumber n1, IdolComplexNumber n2)
        {
            return new IdolComplexNumber(n1.Re * n2.Re - n1.Im * n2.Im,
                n1.Im * n2.Re + n1.Re * n2.Im);
        }

        public static IdolComplexNumber operator +(IdolComplexNumber n1, IdolComplexNumber n2)
        {
            return new IdolComplexNumber(n1.Re + n2.Re, n1.Im + n2.Im);
        }

        public static IdolComplexNumber operator -(IdolComplexNumber n1, IdolComplexNumber n2)
        {
            return new IdolComplexNumber(n1.Re - n2.Re, n1.Im - n2.Im);
        }

        public static IdolComplexNumber operator -(IdolComplexNumber n)
        {
            return new IdolComplexNumber(-n.Re, -n.Im);
        }

        public static implicit operator IdolComplexNumber(double n)
        {
            return new IdolComplexNumber(n, 0);
        }

        public IdolComplexNumber PoweredE()
        {
            double e = Math.Exp(Re);
            return new IdolComplexNumber(e * Math.Cos(Im), e * Math.Sin(Im));
        }

        public double Power2()
        {
            return Re * Re - Im * Im;
        }

        public double AbsPower2()
        {
            return Re * Re + Im * Im;
        }

        public override string ToString()
        {
            return String.Format("{0}+i*{1}", Re, Im);
        }
    }
}
