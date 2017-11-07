using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;
using WDCL.AST;
using Antlr4.Runtime;

namespace WDCL
{
    public class ConditionEval : WDCLBaseVisitor<INode>
    {
        public override INode VisitParse([NotNull] WDCLParser.ParseContext context)
        {
            return this.Visit(context.C);
        }

        public override INode VisitTrue([NotNull] WDCLParser.TrueContext context)
        {
            return new Condition() { Value = true };
        }

        public override INode VisitFalse([NotNull] WDCLParser.FalseContext context)
        {
            return new Condition() { Value = false };
        }

        public override INode VisitParenCond([NotNull] WDCLParser.ParenCondContext context)
        {
            return this.Visit(context.c);
        }

        public override INode VisitNotCond([NotNull] WDCLParser.NotCondContext context)
        {
            var innerCond = (Condition)this.Visit(context.c);

            innerCond.Value = !innerCond.Value;

            return innerCond;
        }

        public override INode VisitBoolCond([NotNull] WDCLParser.BoolCondContext context)
        {
            var left = (Condition)Visit(context.lC);
            var right = (Condition)Visit(context.rC);
            var op = context.op;

            bool result;
            switch (op.Type)
            {
                case WDCLParser.AND: result = left.Value && right.Value; break;
                case WDCLParser.OR:  result = left.Value || right.Value; break;
                default:    throw new ArgumentException("Unknown operator " + op.Text);
            }

            return new Condition() { Value = result };
        }

        public override INode VisitComparisonCond([NotNull] WDCLParser.ComparisonCondContext context)
        {
            var left = (Expression)Visit(context.lE);
            var right = (Expression)Visit(context.rE);
            
            var op = context.op;
            switch (op.Type)
            {
                case WDCLParser.EQ:     return new Condition() { Value = left == right };
                case WDCLParser.NOTEQ:  return new Condition() { Value = left != right };
                case WDCLParser.LTEQ:   return new Condition() { Value = left <= right };
                case WDCLParser.LT:     return new Condition() { Value = left < right };
                case WDCLParser.GTEQ:   return new Condition() { Value = left >= right };
                case WDCLParser.GT:     return new Condition() { Value = left > right };
                default:    throw new ArgumentException("Unknown operator " + op.Text);
            }
        }

        public override INode VisitAtomExp([NotNull] WDCLParser.AtomExpContext context)
        {
            var type = context.atom.Type;
            
            switch (type)
            {
                case WDCLParser.INT: 
                case WDCLParser.FLOAT: return new Expression() { Type = DataType.Double, Value = double.Parse(context.GetText()) };
                case WDCLParser.STRING: return new Expression() { Type = DataType.String, Value = context.GetText() };
                case WDCLParser.DATE:
                    var year = int.Parse(context.atom.Text.Substring(0, 4));
                    var month = int.Parse(context.atom.Text.Substring(5,2));
                    var day = int.Parse(context.atom.Text.Substring(8,2));
                    return new Expression() {
                    Type = DataType.Date,
                    Value = new DateTime(year,month,day)
                    };
                default: throw new Exception();
            };
        }

        public override INode VisitParenExp([NotNull] WDCLParser.ParenExpContext context)
        {
            return this.Visit(context.e);
        }

        public override INode VisitBinarExp([NotNull] WDCLParser.BinarExpContext context)
        {
            var left = (Expression)Visit(context.lE);
            var right = (Expression)Visit(context.rE);

            var op = context.op;
            switch (op.Type)
            {
                case WDCLParser.PLUS_OP:    return left + right;
                case WDCLParser.MINUS_OP: return left - right;
                case WDCLParser.MUL_OP: return left * right;
                case WDCLParser.DIV_OP: return left / right;
                case WDCLParser.MOD_OP: return left % right;
                case WDCLParser.POW_OP: return left ^ right;
                default: throw new ArgumentException("Unknown operator " + op.Text);
            }
        }

        public override INode VisitUnarExp([NotNull] WDCLParser.UnarExpContext context)
        {
            var e = (Expression)Visit(context.e);
            var op = context.op;
            switch (op.Type)
            {
                case WDCLParser.MINUS_OP: return -e;
                default: throw new ArgumentException("Invalid unary operator " + op.Text);
            }
        }

        public override INode VisitDriverExp([NotNull] WDCLParser.DriverExpContext context)
        {
            string drv = context.drv.Text;
            var ids = Identifiers.Instance;

            DataType t = ids.getTypeID(drv);

            if (t == DataType.Bool) throw new ArgumentException("Cannot use bool driver in a Expression");

            var driver = ids.getID(drv);

            return new Expression() { Type = t, Value = driver };
        }

        public override INode VisitSubcondition([NotNull] WDCLParser.SubconditionContext context)
        {
            string sub = context.sub.Text;
            var ids = Identifiers.Instance;

            DataType t = ids.getTypeID(sub);
            
            if (t != DataType.Bool) throw new ArgumentException("Cannot use driver in a Condition unless it is boolean");

            var subcondition = ids.getID(sub);

            return new Condition() { Value = (bool)subcondition };
        }
    }
}
