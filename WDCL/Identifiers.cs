using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WDCL.AST;

namespace WDCL
{
    public class Identifiers
    {
        private static Identifiers singleton;

        private Dictionary<string, KeyValuePair<DataType,object>> identifiers;
        private Dictionary<string, KeyValuePair<DataType, object>> solvedIdentifiers;

        private Identifiers()
        {
            identifiers = new Dictionary<string, KeyValuePair<DataType, object>>();
            solvedIdentifiers = new Dictionary<string, KeyValuePair<DataType, object>>();
        }

        public static Identifiers Instance
        {
            get
            {
                if (singleton == null)
                {
                    singleton = new Identifiers();
                }
                return singleton;
            }
        }
        
        public void addID(string idName, object id, DataType t)
        {
            identifiers.Add(idName, new KeyValuePair<DataType, object>(t,id));
        }
        
        public DataType getTypeID(string id)
        {
            if (solvedIdentifiers.ContainsKey(id))
                return solvedIdentifiers[id].Key;

            return identifiers[id].Key;
        }

        public object getID(string id)
        {
            if (solvedIdentifiers.ContainsKey(id))
                return solvedIdentifiers[id].Value;

            return identifiers[id].Value;
        }

        public void setID(string id, object value)
        {
            var previousType = identifiers[id].Key;

            identifiers[id] = new KeyValuePair<DataType, object>(previousType, value);
        }

        //public void resolveID(string id, object value, DataType t)
        //{
        //    var previousType = identifiers[id].Key;

        //    if (previousType != DataType.Cond && previousType != DataType.Expr)
        //    {
        //        throw new Exception("Resolve is allowed only for complex drivers and subconditions");
        //    }

        //    if (previousType == DataType.Cond && t != DataType.Bool)
        //    {
        //        throw new Exception("Cannot resolve a subcondition in a non-boolean value");
        //    }

        //    if ((previousType == DataType.Expr) && 
        //        ((t == DataType.Bool) || (t == DataType.Expr) || (t == DataType.Cond)))
        //    {
        //        throw new Exception("Cannot resolve a complex driver in a bool value, Expressions or Conditions");
        //    }

        //    //identifiers[id] = new KeyValuePair<DataType, object>(t, value);
        //    solvedIdentifiers.Add(id, new KeyValuePair<DataType, object>(t, value));
        //}

        public void resolveID(string id)
        {
            var idType = identifiers[id].Key;

            if (idType != DataType.Cond && idType != DataType.Expr)
            {
                throw new Exception("Resolve is allowed only for complex drivers and subconditions");
            }

            //Eval e = new Eval();

            if (idType == DataType.Cond)
            {
                string subcond = (string)getID(id);
                EvalCondition e = new EvalCondition(subcond);
                if(!e.parse())
                {
                    throw new WDCLParseException(e.getSyntaxErrors(), id);
                }
                
                solvedIdentifiers.Add(id, new KeyValuePair<DataType, object>(DataType.Bool, e.getResult()));
            }
            else
            {
                string complexDriver = (string)getID(id);
                EvalExpression e = new EvalExpression(complexDriver);
                if(!e.parse())
                {
                    throw new WDCLParseException(e.getSyntaxErrors(), id);
                }

                var expression = e.getResult();

                solvedIdentifiers.Add(id, new KeyValuePair<DataType, object> (expression.Type, expression.Value));
            }
        }

        public void resetEvaluation()
        {
            solvedIdentifiers = new Dictionary<string, KeyValuePair<DataType, object>>();
        }

        //private bool checkType(object value, DataType t)
        //{
        //    switch (t)
        //    {
        //        case DataType.Int: return (value is int); 
        //        case DataType.Double: return (value is double);
        //        case DataType.Date: return (value is DateTime);
        //        case DataType.String: return (value is string);
        //        case DataType.Bool: return (value is bool);
        //        case DataType.Expr: return (value is string);
        //        case DataType.Cond: return (value is string);
        //        default: return false;
        //    }
        //}

        public static bool isCondition(string id)
        {
            var type = singleton.getTypeID(id);

            if (type == DataType.Bool || type == DataType.Cond)
                return true;

            return false;
        }

        public static bool isDriver(string id)
        {
            return !isCondition(id);
        }
    }
}
