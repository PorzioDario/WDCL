using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WDCL.AST;

namespace WDCL
{
    public class Eval
    {
        public static bool EvalCondition(string condition)
        {
            AntlrInputStream inputStream = new AntlrInputStream(condition);
            WDCLLexer speakLexer = new WDCLLexer(inputStream);
            CommonTokenStream commonTokenStream = new CommonTokenStream(speakLexer);
            WDCLParser speakParser = new WDCLParser(commonTokenStream);

            WDCLParser.ParseContext parse = speakParser.parse();

            EvalVisitor visitor = new EvalVisitor();
            ConditionNodeEval result = (ConditionNodeEval)visitor.Visit(parse);

            return result.Value;
        }

        public static ExpressionNodeEval EvalExpression (string expression)
        {
            AntlrInputStream inputStream = new AntlrInputStream(expression);
            WDCLLexer speakLexer = new WDCLLexer(inputStream);
            CommonTokenStream commonTokenStream = new CommonTokenStream(speakLexer);
            WDCLParser speakParser = new WDCLParser(commonTokenStream);

            WDCLParser.ExpContext parse = speakParser.exp();

            EvalVisitor visitor = new EvalVisitor();
            ExpressionNodeEval result = (ExpressionNodeEval)visitor.VisitExp(parse);

            return result;
        }
    }
}
