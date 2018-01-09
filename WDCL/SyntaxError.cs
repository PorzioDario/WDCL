using Antlr4.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WDCL
{
    public class SyntaxError
    {
        public int line;
        public int charPositionInLine;
        public string msg;
        public IToken offendingSymbol;
        public RecognitionException exception;
    }

    public class SyntaxErrorOrder : IComparer<SyntaxError>
    {
        public int Compare(SyntaxError x, SyntaxError y)
        {
            if (x.line < y.line) return -1;
            else if (x.line > y.line) return 1;

            if (x.charPositionInLine < y.charPositionInLine) return -1;
            else if (x.charPositionInLine > y.charPositionInLine) return 1;

            return 0;
        }
    }
}
