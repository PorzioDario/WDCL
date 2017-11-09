using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WDCL.AST
{
    public class ExpressionNodeTranslate : INodeTranslate
    {
        public DataType Type;

        //public object Value;

        public string Code;
        /*
        #region Operator Overloading
        public static ExpressionNodeTranslate operator +(ExpressionNodeTranslate l, ExpressionNodeTranslate r)
        {
            //controllo di coerenza tipi:
            //numerici con numerici
            //stringhe con tutto
            //date con date o con numero no!!
            if((l.Type == DataType.String) || (r.Type == DataType.String))
            {
                //converto entrambi gli operatori in stringa e concateno
                return new ExpressionNodeTranslate() { Type=DataType.String, Value = (string)l.Value + (string)r.Value };
            }

            //somma algebrica
            if ((l.Type == DataType.Double) && (r.Type == DataType.Double))
            {
                //sommo algebricamente i valori convertiti in double
                return new ExpressionNodeTranslate() { Type = DataType.Double, Value = (double)l.Value + (double)r.Value };
            }

            throw new ArgumentException("Mismatched data types in Addition");
        }

        public static ExpressionNodeTranslate operator -(ExpressionNodeTranslate l, ExpressionNodeTranslate r)
        {
            if ((l.Type == DataType.Double) && (r.Type == DataType.Double))
            {
                //sottraggo algebricamente i valori convertiti in double
                return new ExpressionNodeTranslate() { Type = DataType.Double, Value = (double)l.Value - (double)r.Value };
            }

            throw new ArgumentException("Mismatched data types in Substraction");
        }

        public static ExpressionNodeTranslate operator -(ExpressionNodeTranslate l)
        {
            if (l.Type == DataType.Double)
                return new ExpressionNodeTranslate() { Type = DataType.Double, Value = -(double)l.Value};

            throw new ArgumentException("Mismatched data types in Unary Minus");
        }

        public static ExpressionNodeTranslate operator *(ExpressionNodeTranslate l, ExpressionNodeTranslate r)
        {
            if ((l.Type == DataType.Double) && (r.Type == DataType.Double))
                return new ExpressionNodeTranslate() { Type = DataType.Double, Value = (double)l.Value * (double)r.Value };

            throw new ArgumentException("Mismatched data types in Multiplication");
        }

        public static ExpressionNodeTranslate operator /(ExpressionNodeTranslate l, ExpressionNodeTranslate r)
        {
            if ((l.Type == DataType.Double) && (r.Type == DataType.Double))
            {
                if (((double)r.Value) == 0)
                    throw new DivideByZeroException("Division by zero");

                return new ExpressionNodeTranslate() { Type = DataType.Double, Value = (double)l.Value / (double)r.Value };
            }

            throw new ArgumentException("Mismatched data types in Division");
        }

        public static ExpressionNodeTranslate operator %(ExpressionNodeTranslate l, ExpressionNodeTranslate r)
        {
            if ((l.Type == DataType.Double) && (r.Type == DataType.Double))
            {
                if (((double)r.Value) == 0)
                    throw new DivideByZeroException("Division by zero");

                return new ExpressionNodeTranslate() { Type = DataType.Double, Value = (double)l.Value % (double)r.Value };
            }

            throw new ArgumentException("Mismatched data types in Module Operation");
        }

        public static ExpressionNodeTranslate operator ^(ExpressionNodeTranslate l, ExpressionNodeTranslate r)
        {
            if ((l.Type == DataType.Double) && (r.Type == DataType.Double))
                return new ExpressionNodeTranslate() { Type = DataType.Double, Value = Math.Pow((double)l.Value,(double)r.Value) };

            throw new ArgumentException("Mismatched data types in Power Operation");
        }

        public static bool operator ==(ExpressionNodeTranslate l, ExpressionNodeTranslate r)
        {
            if (l.Type != r.Type) return false;

            switch (l.Type)
            {
                case DataType.Date: return ((DateTime)l.Value).Equals((DateTime)r.Value);
                case DataType.String: return ((string)l.Value).Equals((string)r.Value);
                case DataType.Double: return (double)l.Value == (double)r.Value;
                default:    throw new Exception("Invalid Data Type");
            }
        }

        public static bool operator !=(ExpressionNodeTranslate l, ExpressionNodeTranslate r)
        {
            return !(l == r);
        }

        public static bool operator <(ExpressionNodeTranslate l, ExpressionNodeTranslate r)
        {
            if (l.Type != r.Type) return false;

            switch (l.Type)
            {
                case DataType.Date: return ((DateTime)l.Value) < ((DateTime)r.Value);
                case DataType.String: return string.Compare((string)l.Value,(string)r.Value)<0;
                case DataType.Double: return (double)l.Value < (double)r.Value;
                default: throw new Exception("Invalid Data Type");
            }
        }

        public static bool operator >(ExpressionNodeTranslate l, ExpressionNodeTranslate r)
        {
            if (l.Type != r.Type) return false;

            switch (l.Type)
            {
                case DataType.Date: return ((DateTime)l.Value) > ((DateTime)r.Value);
                case DataType.String: return string.Compare((string)l.Value, (string)r.Value) > 0;
                case DataType.Double: return (double)l.Value > (double)r.Value;
                default: throw new Exception("Invalid Data Type");
            }
        }

        public static bool operator <=(ExpressionNodeTranslate l, ExpressionNodeTranslate r)
        {
            return !(l > r);
        }

        public static bool operator >=(ExpressionNodeTranslate l, ExpressionNodeTranslate r)
        {
            return !(l < r);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return Type + " " + Code;
        }

        #endregion

    */
    }
}
