using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WDCL.AST
{
    public class ExpressionNodeEval : INodeEval
    {
        public DataType Type;

        public object Value;

        #region Operator Overloading

        #region Arithmetic Operators
        public static ExpressionNodeEval operator +(ExpressionNodeEval l, ExpressionNodeEval r)
        {
            /*  controllo di coerenza tipi:
                    | +      | Int          | Double | Date   | String | Bool | 
                    |--------|--------------|--------|--------|--------|------| 
                    | Int    | Int          | Double | NA     | String | NA   | 
                    | Double | Double       | Double | NA     | String | NA   | 
                    | Date   | Date(giorni) | NA     | NA     | String | NA   | 
                    | String | String       | String | String | String | NA   | 
                    | Bool   | NA           | NA     | NA     | NA     | NA   | 
             */

            switch (l.Type)
            {
                case DataType.Int:
                    switch (r.Type)
                    {
                        case DataType.Int:
                            return new ExpressionNodeEval() { Type = DataType.Int, Value = (int)l.Value + (int)r.Value };
                        case DataType.Double:
                            return new ExpressionNodeEval() { Type = DataType.Double, Value = (int)l.Value + (double)r.Value };
                        case DataType.Date:
                            throw new ArgumentException("Mismatched data types in Addition: Int+Date");
                        case DataType.String:
                            return new ExpressionNodeEval() { Type = DataType.String, Value = (int)l.Value + (string)r.Value };
                        default: //boolean 
                            throw new ArgumentException("Mismatched data types in Addition: Boolean not allowed");
                    }
                    
                case DataType.Double:
                    switch (r.Type)
                    {
                        case DataType.Int:
                            return new ExpressionNodeEval() { Type = DataType.Double, Value = (double)l.Value + (int)r.Value };
                        case DataType.Double:
                            return new ExpressionNodeEval() { Type = DataType.Double, Value = (double)l.Value + (double)r.Value };
                        case DataType.Date:
                            throw new ArgumentException("Mismatched data types in Addition: Double+Date");
                        case DataType.String:
                            return new ExpressionNodeEval() { Type = DataType.String, Value = (double)l.Value + (string)r.Value };
                        default: //boolean 
                            throw new ArgumentException("Mismatched data types in Addition: Boolean not allowed");
                    }
                    
                case DataType.Date:
                    switch (r.Type)
                    {
                        case DataType.Int:
                            return new ExpressionNodeEval() { Type = DataType.Date, Value = ((DateTime)l.Value).AddDays((int)r.Value) };
                        case DataType.Double:
                            throw new ArgumentException("Mismatched data types in Addition: Date+Double");
                        case DataType.Date:
                            throw new ArgumentException("Mismatched data types in Addition: Date+Date");
                        case DataType.String:
                            return new ExpressionNodeEval() { Type = DataType.String, Value = (DateTime)l.Value + (string)r.Value };
                        default: //boolean 
                            throw new ArgumentException("Mismatched data types in Addition: Boolean not allowed");
                    }
                    
                case DataType.String:
                    switch (r.Type)
                    {
                        case DataType.Int:
                            return new ExpressionNodeEval() { Type = DataType.String, Value = (string)l.Value + (int)r.Value };
                        case DataType.Double:
                            return new ExpressionNodeEval() { Type = DataType.String, Value = (string)l.Value + (double)r.Value };
                        case DataType.Date:
                            return new ExpressionNodeEval() { Type = DataType.String, Value = (string)l.Value + (DateTime)r.Value };
                        case DataType.String:
                            return new ExpressionNodeEval() { Type = DataType.String, Value = (string)l.Value + (string)r.Value };
                        default: //boolean 
                            throw new ArgumentException("Mismatched data types in Addition: Boolean not allowed");
                    }
                    
                default: //boolean 
                    throw new ArgumentException("Mismatched data types in Addition: Boolean not allowed");
            }

            //if ((l.Type == DataType.String) || (r.Type == DataType.String))
            //{
            //    //converto entrambi gli operatori in stringa e concateno
            //    return new ExpressionNodeEval() { Type=DataType.String, Value = (string)l.Value + (string)r.Value };
            //}

            ////somma algebrica
            //if ((l.Type == DataType.Double) && (r.Type == DataType.Double))
            //{
            //    //sommo algebricamente i valori convertiti in double
            //    return new ExpressionNodeEval() { Type = DataType.Double, Value = (double)l.Value + (double)r.Value };
            //}

            //throw new ArgumentException("Mismatched data types in Addition");
        }

        public static ExpressionNodeEval operator -(ExpressionNodeEval l, ExpressionNodeEval r)
        {
            /* controllo di coerenza tipi:
                | -      | Int          | Double | Date | String | Bool | 
                |--------|--------------|--------|------|--------|------| 
                | Int    | Int          | Double | NA   | NA     | NA   | 
                | Double | Double       | Double | NA   | NA     | NA   | 
                | Date   | Date(giorni) | NA     | NA   | NA     | NA   | 
                | String | NA           | NA     | NA   | NA     | NA   | 
                | Bool   | NA           | NA     | NA   | NA     | NA   | 
             */
            switch (l.Type)
            {
                case DataType.Int:
                    switch (r.Type)
                    {
                        case DataType.Int:
                            return new ExpressionNodeEval() { Type = DataType.Int, Value = (int)l.Value - (int)r.Value };
                        case DataType.Double:
                            return new ExpressionNodeEval() { Type = DataType.Double, Value = (int)l.Value - (double)r.Value };
                        default: 
                            throw new ArgumentException("Mismatched data types in Substraction");
                    }

                case DataType.Double:
                    switch (r.Type)
                    {
                        case DataType.Int:
                            return new ExpressionNodeEval() { Type = DataType.Double, Value = (double)l.Value - (int)r.Value };
                        case DataType.Double:
                            return new ExpressionNodeEval() { Type = DataType.Double, Value = (double)l.Value - (double)r.Value };
                        default:
                            throw new ArgumentException("Mismatched data types in Substraction");
                    }

                case DataType.Date:
                    switch (r.Type)
                    {
                        case DataType.Int:
                            return new ExpressionNodeEval() { Type = DataType.Date, Value = ((DateTime)l.Value).AddDays((-1)*(int)r.Value) };
                        default:
                            throw new ArgumentException("Mismatched data types in Substraction");
                    }
                default:  //string or boolean
                    throw new ArgumentException("Mismatched data types in Substraction");
            }
        }

        public static ExpressionNodeEval operator -(ExpressionNodeEval l)
        {
            if (l.Type == DataType.Double)
                return new ExpressionNodeEval() { Type = DataType.Double, Value = -(double)l.Value};

            if (l.Type == DataType.Int)
                return new ExpressionNodeEval() { Type = DataType.Int, Value = -(int)l.Value };

            throw new ArgumentException("Mismatched data types in Unary Minus");
        }

        public static ExpressionNodeEval operator *(ExpressionNodeEval l, ExpressionNodeEval r)
        {
            /* controllo di coerenza tipi:
                | *      | Int    | Double | Date | String | Bool | 
                |--------|--------|--------|------|--------|------| 
                | Int    | Int    | Double | NA   | NA     | NA   | 
                | Double | Double | Double | NA   | NA     | NA   | 
                | Date   | NA     | NA     | NA   | NA     | NA   | 
                | String | NA     | NA     | NA   | NA     | NA   | 
                | Bool   | NA     | NA     | NA   | NA     | NA   | 
            */

            switch (l.Type)
            {
                case DataType.Int:
                    switch (r.Type)
                    {
                        case DataType.Int:
                            return new ExpressionNodeEval() { Type = DataType.Int, Value = (int)l.Value * (int)r.Value };
                        case DataType.Double:
                            return new ExpressionNodeEval() { Type = DataType.Double, Value = (int)l.Value * (double)r.Value };
                        default:
                            throw new ArgumentException("Mismatched data types in Multiplication");
                    }

                case DataType.Double:
                    switch (r.Type)
                    {
                        case DataType.Int:
                            return new ExpressionNodeEval() { Type = DataType.Double, Value = (double)l.Value * (int)r.Value };
                        case DataType.Double:
                            return new ExpressionNodeEval() { Type = DataType.Double, Value = (double)l.Value * (double)r.Value };
                        default:
                            throw new ArgumentException("Mismatched data types in Multiplication");
                    }
                default:  //date or string or boolean
                    throw new ArgumentException("Mismatched data types in Multiplication");
            }

            //if ((l.Type == DataType.Double) && (r.Type == DataType.Double))
            //    return new ExpressionNodeEval() { Type = DataType.Double, Value = (double)l.Value * (double)r.Value };

            //throw new ArgumentException("Mismatched data types in Multiplication");
        }

        public static ExpressionNodeEval operator /(ExpressionNodeEval l, ExpressionNodeEval r)
        {
            /* controllo di coerenza tipi:
                | /      | Int    | Double | Date | String | Bool | 
                |--------|--------|--------|------|--------|------| 
                | Int    | Double | Double | NA   | NA     | NA   | 
                | Double | Double | Double | NA   | NA     | NA   | 
                | Date   | NA     | NA     | NA   | NA     | NA   | 
                | String | NA     | NA     | NA   | NA     | NA   | 
                | Bool   | NA     | NA     | NA   | NA     | NA   | 
            */

            switch (l.Type)
            {
                case DataType.Int:
                    switch (r.Type)
                    {
                        case DataType.Int:
                            if (((int)r.Value) == 0)
                                throw new DivideByZeroException("Division by zero");
                            return new ExpressionNodeEval() { Type = DataType.Double, Value = (int)l.Value / (int)r.Value };
                        case DataType.Double:
                            if (((double)r.Value) == 0)
                                throw new DivideByZeroException("Division by zero");
                            return new ExpressionNodeEval() { Type = DataType.Double, Value = (int)l.Value / (double)r.Value };
                        default:
                            throw new ArgumentException("Mismatched data types in Division");
                    }

                case DataType.Double:
                    switch (r.Type)
                    {
                        case DataType.Int:
                            if (((int)r.Value) == 0)
                                throw new DivideByZeroException("Division by zero");
                            return new ExpressionNodeEval() { Type = DataType.Double, Value = (double)l.Value / (int)r.Value };
                        case DataType.Double:
                            if (((double)r.Value) == 0)
                                throw new DivideByZeroException("Division by zero");
                            return new ExpressionNodeEval() { Type = DataType.Double, Value = (double)l.Value / (double)r.Value };
                        default:
                            throw new ArgumentException("Mismatched data types in Division");
                    }
                default:  //date or string or boolean
                    throw new ArgumentException("Mismatched data types in Division");
            }

            //if ((l.Type == DataType.Double) && (r.Type == DataType.Double))
            //{
            //    if (((double)r.Value) == 0)
            //        throw new DivideByZeroException("Division by zero");

            //    return new ExpressionNodeEval() { Type = DataType.Double, Value = (double)l.Value / (double)r.Value };
            //}

            //throw new ArgumentException("Mismatched data types in Division");
        }

        public static ExpressionNodeEval operator %(ExpressionNodeEval l, ExpressionNodeEval r)
        {
            /*
                | %      | Int | Double | Date | String | Bool | 
                |--------|-----|--------|------|--------|------| 
                | Int    | Int | NA     | NA   | NA     | NA   | 
                | Double | NA  | NA     | NA   | NA     | NA   | 
                | Date   | NA  | NA     | NA   | NA     | NA   | 
                | String | NA  | NA     | NA   | NA     | NA   | 
                | Bool   | NA  | NA     | NA   | NA     | NA   | 
            */

            if ((l.Type == DataType.Int) && (r.Type == DataType.Int))
            {
                if (((int)r.Value) == 0)
                    throw new DivideByZeroException("Division by zero");

                return new ExpressionNodeEval() { Type = DataType.Int, Value = (int)l.Value % (int)r.Value };
            }

            throw new ArgumentException("Mismatched data types in Module Operation");
        }

        public static ExpressionNodeEval operator ^(ExpressionNodeEval l, ExpressionNodeEval r)
        {
            /* controllo di coerenza tipi:
                | ^      | Int    | Double | Date | String | Bool | 
                |--------|--------|--------|------|--------|------| 
                | Int    | Int    | Double | NA   | NA     | NA   | 
                | Double | Double | Double | NA   | NA     | NA   | 
                | Date   | NA     | NA     | NA   | NA     | NA   | 
                | String | NA     | NA     | NA   | NA     | NA   | 
                | Bool   | NA     | NA     | NA   | NA     | NA   |
            */

            switch (l.Type)
            {
                case DataType.Int:
                    switch (r.Type)
                    {
                        case DataType.Int:
                            return new ExpressionNodeEval() { Type = DataType.Int, Value = Math.Pow((int)l.Value, (int)r.Value) };
                        case DataType.Double:
                            return new ExpressionNodeEval() { Type = DataType.Double, Value = Math.Pow((int)l.Value, (double)r.Value) };
                        default:
                            throw new ArgumentException("Mismatched data types in Power Operation");
                    }

                case DataType.Double:
                    switch (r.Type)
                    {
                        case DataType.Int:
                            return new ExpressionNodeEval() { Type = DataType.Double, Value = Math.Pow((double)l.Value, (int)r.Value) };
                        case DataType.Double:
                            return new ExpressionNodeEval() { Type = DataType.Double, Value = Math.Pow((double)l.Value, (double)r.Value) };
                        default:
                            throw new ArgumentException("Mismatched data types in Power Operation");
                    }
                default:  //date or string or boolean
                    throw new ArgumentException("Mismatched data types in Power Operation");
            }

            //if ((l.Type == DataType.Double) && (r.Type == DataType.Double))
            //    return new ExpressionNodeEval() { Type = DataType.Double, Value = Math.Pow((double)l.Value,(double)r.Value) };

            //throw new ArgumentException("Mismatched data types in Power Operation");
        }

        #endregion

        #region Comparison Operators
        public static bool operator ==(ExpressionNodeEval l, ExpressionNodeEval r)
        {
            /* operator logic:
                | =      | Int     | Double | Date  | String  | Bool  | 
                |--------|---------|--------|-------|---------|-------| 
                | Int    | ==      | ==     | FALSE | (int)== | FALSE | 
                | Double | ==      | ==     | FALSE | FALSE   | FALSE | 
                | Date   | FALSE   | FALSE  | ==    | FALSE   | FALSE | 
                | String | (int)== | FALSE  | FALSE | .Equals | FALSE | 
                | Bool   | FALSE   | FALSE  | FALSE | FALSE   | ==    | 
             */

            switch (l.Type)
            {
                case DataType.Int:
                    switch (r.Type)
                    {
                        case DataType.Int:
                            return (int)l.Value == (int)r.Value;
                        case DataType.Double:
                            return (int)l.Value == (double)r.Value;
                        case DataType.Date:
                            throw new ArgumentException("Mismatched data types in Comparison");
                        case DataType.String:
                            try
                            {
                                var intS = int.Parse((string)r.Value);
                                return (int)l.Value == intS;
                            }
                            catch
                            {
                                return false;
                            }
                        case DataType.Bool:
                            throw new ArgumentException("Mismatched data types in Comparison");
                        default: throw new ArgumentException("Mismatched data types in Comparison: Type Unknown");
                    }

                case DataType.Double:
                    switch (r.Type)
                    {
                        case DataType.Int:
                            return (double)l.Value == (int)r.Value;
                        case DataType.Double:
                            return (double)l.Value == (double)r.Value;
                        case DataType.Date:
                            throw new ArgumentException("Mismatched data types in Comparison");
                        case DataType.String:
                            throw new ArgumentException("Mismatched data types in Comparison");
                        case DataType.Bool:
                            throw new ArgumentException("Mismatched data types in Comparison");
                        default: throw new ArgumentException("Mismatched data types in Comparison: Type Unknown");
                    }

                case DataType.Date:
                    switch (r.Type)
                    {
                        case DataType.Int:
                            throw new ArgumentException("Mismatched data types in Comparison");
                        case DataType.Double:
                            throw new ArgumentException("Mismatched data types in Comparison");
                        case DataType.Date:
                            return (DateTime)l.Value == (DateTime)r.Value;
                        case DataType.String:
                            throw new ArgumentException("Mismatched data types in Comparison");
                        case DataType.Bool:
                            throw new ArgumentException("Mismatched data types in Comparison");
                        default: 
                            throw new ArgumentException("Mismatched data types in Comparison: Type Unknown");
                    }

                case DataType.String:
                    switch (r.Type)
                    {
                        case DataType.Int:
                            try
                            {
                                var intS = int.Parse((string)l.Value);
                                return (int)r.Value == intS;
                            }
                            catch
                            {
                                return false;
                            }
                        case DataType.Double:
                            throw new ArgumentException("Mismatched data types in Comparison");
                        case DataType.Date:
                            throw new ArgumentException("Mismatched data types in Comparison");
                        case DataType.String:
                            return ((string)l.Value).Equals((string)r.Value);
                        case DataType.Bool:
                            throw new ArgumentException("Mismatched data types in Comparison");
                        default:  
                            throw new ArgumentException("Mismatched data types in Comparison: Type Unknown");
                    }

                case DataType.Bool:
                    throw new ArgumentException("Mismatched data types in Comparison");

                default: throw new ArgumentException("Mismatched data types in Comparison: Type Unknown");
            }

            //if (l.Type != r.Type) return false;

            //switch (l.Type)
            //{
            //    case DataType.Date: return ((DateTime)l.Value).Equals((DateTime)r.Value);
            //    case DataType.String: return ((string)l.Value).Equals((string)r.Value);
            //    case DataType.Double: return (double)l.Value == (double)r.Value;
            //    default:    throw new Exception("Invalid Data Type");
            //}
        }

        public static bool operator !=(ExpressionNodeEval l, ExpressionNodeEval r)
        {
            return !(l == r);
        }

        public static bool operator <(ExpressionNodeEval l, ExpressionNodeEval r)
        {
            /*
                | <      | Int | Double | Date | String | Bool | 
                |--------|-----|--------|------|--------|------| 
                | Int    | <   | <      | NA   | NA     | NA   | 
                | Double | <   | <      | NA   | NA     | NA   | 
                | Date   | NA  | NA     | <    | NA     | NA   | 
                | String | NA  | NA     | NA   | NA     | NA   | 
                | Bool   | NA  | NA     | NA   | NA     | NA   | 
            */

            switch (l.Type)
            {
                case DataType.Int:
                    switch (r.Type)
                    {
                        case DataType.Int:
                            return (int)l.Value < (int)r.Value;
                        case DataType.Double:
                            return (int)l.Value < (double)r.Value;
                        case DataType.Date:
                            throw new ArgumentException("Mismatched data types in Comparison");
                        case DataType.String:
                            throw new ArgumentException("Mismatched data types in Comparison");
                        case DataType.Bool:
                            throw new ArgumentException("Mismatched data types in Comparison");
                        default: throw new ArgumentException("Mismatched data types in Comparison: Type Unknown");
                    }

                case DataType.Double:
                    switch (r.Type)
                    {
                        case DataType.Int:
                            return (double)l.Value < (int)r.Value;
                        case DataType.Double:
                            return (double)l.Value < (double)r.Value;
                        case DataType.Date:
                            throw new ArgumentException("Mismatched data types in Comparison");
                        case DataType.String:
                            throw new ArgumentException("Mismatched data types in Comparison");
                        case DataType.Bool:
                            throw new ArgumentException("Mismatched data types in Comparison");
                        default: throw new ArgumentException("Mismatched data types in Comparison: Type Unknown");
                    }

                case DataType.Date:
                    switch (r.Type)
                    {
                        case DataType.Int:
                            throw new ArgumentException("Mismatched data types in Comparison");
                        case DataType.Double:
                            throw new ArgumentException("Mismatched data types in Comparison");
                        case DataType.Date:
                            return (DateTime)l.Value < (DateTime)r.Value;
                        case DataType.String:
                            throw new ArgumentException("Mismatched data types in Comparison");
                        case DataType.Bool:
                            throw new ArgumentException("Mismatched data types in Comparison");
                        default:
                            throw new ArgumentException("Mismatched data types in Comparison: Type Unknown");
                    }

                case DataType.String:
                    throw new ArgumentException("Mismatched data types in Comparison");

                case DataType.Bool:
                    throw new ArgumentException("Mismatched data types in Comparison");

                default: throw new ArgumentException("Mismatched data types in Comparison: Type Unknown");
            }

            //if (l.Type != r.Type) return false;

            //switch (l.Type)
            //{
            //    case DataType.Date: return ((DateTime)l.Value) < ((DateTime)r.Value);
            //    case DataType.String: return string.Compare((string)l.Value,(string)r.Value)<0;
            //    case DataType.Double: return (double)l.Value < (double)r.Value;
            //    default: throw new Exception("Invalid Data Type");
            //}
        }

        public static bool operator >(ExpressionNodeEval l, ExpressionNodeEval r)
        {
            /*
                | >      | Int | Double | Date | String | Bool | 
                |--------|-----|--------|------|--------|------| 
                | Int    | >   | >      | NA   | NA     | NA   | 
                | Double | >   | >      | NA   | NA     | NA   | 
                | Date   | NA  | NA     | >    | NA     | NA   | 
                | String | NA  | NA     | NA   | NA     | NA   | 
                | Bool   | NA  | NA     | NA   | NA     | NA   | 
             */


            switch (l.Type)
            {
                case DataType.Int:
                    switch (r.Type)
                    {
                        case DataType.Int:
                            return (int)l.Value > (int)r.Value;
                        case DataType.Double:
                            return (int)l.Value > (double)r.Value;
                        case DataType.Date:
                            throw new ArgumentException("Mismatched data types in Comparison");
                        case DataType.String:
                            throw new ArgumentException("Mismatched data types in Comparison");
                        case DataType.Bool:
                            throw new ArgumentException("Mismatched data types in Comparison");
                        default: throw new ArgumentException("Mismatched data types in Comparison: Type Unknown");
                    }

                case DataType.Double:
                    switch (r.Type)
                    {
                        case DataType.Int:
                            return (double)l.Value > (int)r.Value;
                        case DataType.Double:
                            return (double)l.Value > (double)r.Value;
                        case DataType.Date:
                            throw new ArgumentException("Mismatched data types in Comparison");
                        case DataType.String:
                            throw new ArgumentException("Mismatched data types in Comparison");
                        case DataType.Bool:
                            throw new ArgumentException("Mismatched data types in Comparison");
                        default: throw new ArgumentException("Mismatched data types in Comparison: Type Unknown");
                    }

                case DataType.Date:
                    switch (r.Type)
                    {
                        case DataType.Int:
                            throw new ArgumentException("Mismatched data types in Comparison");
                        case DataType.Double:
                            throw new ArgumentException("Mismatched data types in Comparison");
                        case DataType.Date:
                            return (DateTime)l.Value > (DateTime)r.Value;
                        case DataType.String:
                            throw new ArgumentException("Mismatched data types in Comparison");
                        case DataType.Bool:
                            throw new ArgumentException("Mismatched data types in Comparison");
                        default:
                            throw new ArgumentException("Mismatched data types in Comparison: Type Unknown");
                    }

                case DataType.String:
                    throw new ArgumentException("Mismatched data types in Comparison");

                case DataType.Bool:
                    throw new ArgumentException("Mismatched data types in Comparison");

                default: throw new ArgumentException("Mismatched data types in Comparison: Type Unknown");
            }

            //if (l.Type != r.Type) return false;

            //switch (l.Type)
            //{
            //    case DataType.Date: return ((DateTime)l.Value) > ((DateTime)r.Value);
            //    case DataType.String: return string.Compare((string)l.Value, (string)r.Value) > 0;
            //    case DataType.Double: return (double)l.Value > (double)r.Value;
            //    default: throw new Exception("Invalid Data Type");
            //}
        }

        public static bool operator <=(ExpressionNodeEval l, ExpressionNodeEval r)
        {
            return !(l > r);
        }

        public static bool operator >=(ExpressionNodeEval l, ExpressionNodeEval r)
        {
            return !(l < r);
        }
        #endregion

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
            return Type + " " + Value;
        }

        #endregion
    }
}
