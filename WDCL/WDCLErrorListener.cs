using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;

namespace WDCL
{
    public class WDCLErrorListener : BaseErrorListener
    {
        public List<SyntaxError> ErrorList;

        public void ResetErrors()
        {
            ErrorList = new List<SyntaxError>();
        }

        public WDCLErrorListener()
        {
            ResetErrors();
        }

        public bool ParsedSuccess() { return ErrorList.Count == 0; }

        public override void SyntaxError(IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            IList<string> Stack = ((Parser)recognizer).GetRuleInvocationStack();
            Stack.Reverse();
            StringBuilder buf = new StringBuilder();
            buf.AppendLine("rule stack: " + Stack + " ");
            buf.AppendLine("line " + line + ":" + charPositionInLine + " at " + offendingSymbol + ": " + msg);
            
            ErrorList.Add(
                new SyntaxError() {
                    line = line,
                    charPositionInLine = charPositionInLine,
                    msg = buf.ToString(),
                    offendingSymbol = offendingSymbol,
                    exception = e
                });
        }
    }


}
