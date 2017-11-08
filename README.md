# WDCL - Condition Parser

**WDCL** is a simple language which allow to express conditions in a syntax similar to SQL.

This project is a CSharp DLL that parse WDCL using ANTLR4 and let you *evaluate* your condition or *translate* it in CSharp code.

This DLL include also a singleton class called **Identifiers** which let you define and populate a Dictionary of IDs of the following types that can be used in your condition.
- Int
- Double
- String
- Date (not fully supported yet)
- Bool

Further implementation will include, as types:
- *Complex Driver* which are IDs binded to expressions
- *Subcondition* which are IDs binded to conditions.
