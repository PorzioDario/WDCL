﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;
using WDCL.AST;
using Antlr4.Runtime;

namespace WDCL
{
    internal class EvalVisitor : WDCLBaseVisitor<INodeEval>
    {
        public override INodeEval VisitParse([NotNull] WDCLParser.ParseContext context)
        {
            return this.Visit(context.C);
        }

        public override INodeEval VisitTrue([NotNull] WDCLParser.TrueContext context)
        {
            return new ConditionNodeEval() { Value = true };
        }

        public override INodeEval VisitFalse([NotNull] WDCLParser.FalseContext context)
        {
            return new ConditionNodeEval() { Value = false };
        }

        public override INodeEval VisitParenCond([NotNull] WDCLParser.ParenCondContext context)
        {
            return this.Visit(context.c);
        }

        public override INodeEval VisitNotCond([NotNull] WDCLParser.NotCondContext context)
        {
            var innerCond = (ConditionNodeEval)this.Visit(context.c);

            innerCond.Value = !innerCond.Value;

            return innerCond;
        }

        public override INodeEval VisitBoolCond([NotNull] WDCLParser.BoolCondContext context)
        {
            var left = (ConditionNodeEval)Visit(context.lC);
            var right = (ConditionNodeEval)Visit(context.rC);
            var op = context.op;

            bool result;
            switch (op.Type)
            {
                case WDCLParser.AND: result = left.Value && right.Value; break;
                case WDCLParser.OR:  result = left.Value || right.Value; break;
                default:    throw new ArgumentException("Unknown operator " + op.Text);
            }

            return new ConditionNodeEval() { Value = result };
        }

        public override INodeEval VisitComparisonCond([NotNull] WDCLParser.ComparisonCondContext context)
        {
            var left = (ExpressionNodeEval)Visit(context.lE);
            var right = (ExpressionNodeEval)Visit(context.rE);
            
            var op = context.op;
            switch (op.Type)
            {
                case WDCLParser.EQ:     return new ConditionNodeEval() { Value = left == right };
                case WDCLParser.NOTEQ:  return new ConditionNodeEval() { Value = left != right };
                case WDCLParser.LTEQ:   return new ConditionNodeEval() { Value = left <= right };
                case WDCLParser.LT:     return new ConditionNodeEval() { Value = left < right };
                case WDCLParser.GTEQ:   return new ConditionNodeEval() { Value = left >= right };
                case WDCLParser.GT:     return new ConditionNodeEval() { Value = left > right };
                default:    throw new ArgumentException("Unknown operator " + op.Text);
            }
        }

        public override INodeEval VisitAtomExp([NotNull] WDCLParser.AtomExpContext context)
        {
            var type = context.atom.Type;
            
            switch (type)
            {
                case WDCLParser.INT: return new ExpressionNodeEval() { Type = DataType.Int, Value = int.Parse(context.GetText()) };
                case WDCLParser.FLOAT: return new ExpressionNodeEval() { Type = DataType.Double, Value = double.Parse(context.GetText()) };
                case WDCLParser.STRING: return new ExpressionNodeEval() { Type = DataType.String, Value = context.GetText().Replace("\"","") };
                case WDCLParser.DATE:
                    var year = int.Parse(context.atom.Text.Substring(0, 4));
                    var month = int.Parse(context.atom.Text.Substring(5,2));
                    var day = int.Parse(context.atom.Text.Substring(8,2));
                    return new ExpressionNodeEval() {
                    Type = DataType.Date,
                    Value = new DateTime(year,month,day)
                    };
                default: throw new Exception();
            };
        }

        public override INodeEval VisitParenExp([NotNull] WDCLParser.ParenExpContext context)
        {
            return this.Visit(context.e);
        }

        public override INodeEval VisitBinarExp([NotNull] WDCLParser.BinarExpContext context)
        {
            var left = (ExpressionNodeEval)Visit(context.lE);
            var right = (ExpressionNodeEval)Visit(context.rE);

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

        public override INodeEval VisitUnarExp([NotNull] WDCLParser.UnarExpContext context)
        {
            var e = (ExpressionNodeEval)Visit(context.e);
            var op = context.op;
            switch (op.Type)
            {
                case WDCLParser.MINUS_OP: return -e;
                default: throw new ArgumentException("Invalid unary operator " + op.Text);
            }
        }

        public override INodeEval VisitDriverExp([NotNull] WDCLParser.DriverExpContext context)
        {
            string drv = context.drv.Text;
            var ids = Identifiers.Instance;

            DataType t = ids.getTypeID(drv);

            if (t == DataType.Bool || t == DataType.Cond) throw new ArgumentException("Cannot use bool driver or subcondition in a Expression");

            //resolve complex driver
            if (t == DataType.Expr)
            {
                ids.resolveID(drv);
            }

            var driver = ids.getID(drv);

            return new ExpressionNodeEval() { Type = ids.getTypeID(drv), Value = driver };
        }

        public override INodeEval VisitSubcondition([NotNull] WDCLParser.SubconditionContext context)
        {
            string sub = context.sub.Text;
            var ids = Identifiers.Instance;

            DataType t = ids.getTypeID(sub);
            
            if (t == DataType.Cond)
            {
                ids.resolveID(sub);
                
                var value = (bool)ids.getID(sub);

                return new ConditionNodeEval() { Value = value };
            }
            
            if (t != DataType.Bool) throw new ArgumentException("Cannot use driver in a Condition unless it is boolean");

            var subcondition = ids.getID(sub);

            return new ConditionNodeEval() { Value = (bool)subcondition };
        }

        public override INodeEval VisitComparisonSubCond([NotNull] WDCLParser.ComparisonSubCondContext context)
        {
            var left = (ConditionNodeEval)Visit(context.lC);
            var right = (ConditionNodeEval)Visit(context.rC);
            var op = context.op;

            bool result;
            switch (op.Type)
            {
                case WDCLParser.EQ: result = left.Value == right.Value; break;
                case WDCLParser.NOTEQ: result = left.Value != right.Value; break;
                default: throw new ArgumentException("Unknown operator " + op.Text);
            }

            return new ConditionNodeEval() { Value = result };
        }

        public override INodeEval VisitSetCond([NotNull] WDCLParser.SetCondContext context)
        {
            var drv = context.drv.Text;
            var ids = Identifiers.Instance;

            DataType t = ids.getTypeID(drv);

            if (t == DataType.Bool || t == DataType.Cond) throw new ArgumentException("Cannot use bool driver or subcondition in a Set Condition");

            //resolve complex driver
            if (t == DataType.Expr)
            {
                ids.resolveID(drv);
            }
            
            var driver = new ExpressionNodeEval() { Type = ids.getTypeID(drv), Value = ids.getID(drv) };

            EvalVisitor visitor = new EvalVisitor();
            var expList = context.exp();
            List<ExpressionNodeEval> expressions = new List<ExpressionNodeEval>();

            foreach (WDCLParser.ExpContext e in expList)
            {
                ExpressionNodeEval result;
                result = (ExpressionNodeEval)visitor.Visit(e);
                if (result.Type != t)
                {
                    throw new ArgumentException("Each element of the list must be of the same type of the driver");
                }

                expressions.Add(result);
            }
            
            bool value = expressions.Contains(driver, new exprComparer());

            if (context.n != null)
                value = !value;

            return new ConditionNodeEval() { Value = value };
        }
    }
}
