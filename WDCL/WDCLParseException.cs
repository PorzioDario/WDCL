using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WDCL
{
    public class WDCLParseException :Exception
    {
        public List<SyntaxError> Errors;
        public string ComplexIDParsed;
        public string ErrorMessage;

        public WDCLParseException(List<SyntaxError> errors, string ID = "")
        {
            Errors = errors;
            ComplexIDParsed = ID;
            ErrorMessage = "Errore durante il parsing di un ID complesso";
        }
        public WDCLParseException(string errorMessage)
        {
            ErrorMessage = errorMessage;
            ComplexIDParsed = "";
        }
    }
}
