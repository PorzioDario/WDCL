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

        private Identifiers()
        {
            identifiers = new Dictionary<string, KeyValuePair<DataType, object>>();
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
            return identifiers[id].Key;
        }

        public object getID(string id)
        {
            return identifiers[id].Value;
        }

        public void setID(string id, object value)
        {
            var previousType = identifiers[id].Key;

            identifiers[id] = new KeyValuePair<DataType, object>(previousType, value);
        }

        public void resolveID(string id, object value, DataType t)
        {
            var previousType = identifiers[id].Key;

            if (previousType != DataType.Cond && previousType != DataType.Expr)
            {
                throw new Exception("Resolve is allowed only for complex drivers and subconditions");
            }

            if (previousType == DataType.Cond && t != DataType.Bool)
            {
                throw new Exception("Cannot resolve a subcondition in a non-boolean value");
            }

            if ((previousType == DataType.Expr) && 
                ((t == DataType.Bool) || (t == DataType.Expr) || (t == DataType.Cond)))
            {
                throw new Exception("Cannot resolve a complex driver in a bool value, Expressions or Conditions");
            }

            identifiers[id] = new KeyValuePair<DataType, object>(t, value);
        }
    }
}
