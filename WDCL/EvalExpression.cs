using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WDCL.AST;

namespace WDCL
{
    public class EvalExpression
    {
        private string Expression;
        private WDCLErrorListener errorListener;
        private WDCLParser.ExpContext AST;

        public EvalExpression(string expr)
        {
            Expression = expr;
            errorListener = new WDCLErrorListener();
        }

        public bool parse()
        {
            AntlrInputStream inputStream = new AntlrInputStream(Expression);
            WDCLLexer Lex = new WDCLLexer(inputStream);
            CommonTokenStream TokenStream = new CommonTokenStream(Lex);
            WDCLParser Par = new WDCLParser(TokenStream);

            Par.RemoveErrorListeners();
            Par.AddErrorListener(errorListener);

            AST = Par.exp();

            //per ogni unknown char creo un SyntaxError
            TokenStream.GetTokens().Where(t => t.Channel == WDCLLexer.INVALID_CHAR).ToList().ForEach(c =>
            {
                errorListener.ErrorList.Add(new SyntaxError() {
                    line = c.Line,
                    charPositionInLine = c.StartIndex,
                    msg = "Invalid Character: " + c.Text,
                    offendingSymbol = c
                });
            });

            return errorListener.ParsedSuccess();
        }

        public List<SyntaxError> getSyntaxErrors()
        {
            errorListener.ErrorList.Sort(new SyntaxErrorOrder());

            return errorListener.ErrorList;
        }

        public ExpressionNodeEval getResult()
        {
            if (AST == null) throw new WDCLParseException("The condition may not have been parsed or the parse failed. Check the Syntax Errors!");
            
            EvalVisitor visitor = new EvalVisitor();
            ExpressionNodeEval result = (ExpressionNodeEval)visitor.Visit(AST);

            return result;
        }

    }
}
