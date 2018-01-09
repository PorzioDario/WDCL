grammar WDCL;

tokens { BOOLID, EXPRID }

@lexer::members {
	public static int COMMENTS = 1;
	public static int WHITE_SPACE = 2;
	public static int INVALID_CHAR = 3;

	public bool isACondition(string id) { return Identifiers.isCondition(id); }
}

/*
 * Parser Rules
 */

parse
	: C=condition EOF
	;

condition
	: TRUE											#true
    | FALSE											#false
    | '(' c=condition ')'							#parenCond
    | lE=exp op=EQ rE=exp							#comparisonCond
    | lE=exp op=NOTEQ rE=exp						#comparisonCond
    | lE=exp op=LT rE=exp							#comparisonCond
    | lE=exp op=GT rE=exp							#comparisonCond
    | lE=exp op=LTEQ rE=exp							#comparisonCond
    | lE=exp op=GTEQ rE=exp							#comparisonCond
    | sub=BOOLID									#subcondition
    | NOT '(' c=condition ')'						#notCond
    | lC=condition op=AND rC=condition				#boolCond
    | lC=condition op=OR rC=condition				#boolCond
    | lC=condition op=EQ rC=condition				#comparisonSubCond
    | lC=condition op=NOTEQ rC=condition			#comparisonSubCond

    //| ID EQ c=condition							#comparisonSubCond
    //| ID NOTEQ c=condition						#comparisonSubCond

	| drv=ID IN '(' exp (COMMA exp)* ')'			#setCond
	| drv=ID n=NOT IN '(' exp (COMMA exp)* ')'		#setCond
;

exp
	: atom=INT 										#atomExp
	| atom=FLOAT 									#atomExp
	| atom=STRING									#atomExp
	| atom=DATE										#atomExp
	| drv=EXPRID	 								#driverExp
	| '(' e=exp ')'									#parenExp
	| lE=exp op=POW_OP<assoc=right> rE=exp			#binarExp
	| lE=exp op=(MUL_OP|DIV_OP|MOD_OP) rE=exp		#binarExp
	| lE=exp op=(PLUS_OP|MINUS_OP) rE=exp			#binarExp
	| op=MINUS_OP e=exp								#unarExp
;


/*
 * Lexer Rules
 */

//OPERATORS:
//	BOOL
NOT	:	N O T;
AND	:	A N D;
OR	:	O R;
//	COMPARISON
EQ		:	'=';
NOTEQ	:	'<>';
LTEQ	:	'<=';
LT		:	'<';
GTEQ	:	'>=';
GT		:	'>';
//	SET
IN		:	I N;
COMMA	:	',';
//ARITHMETIC
PLUS_OP		:	'+';
MINUS_OP	:	'-';
MUL_OP		:	'*';
DIV_OP		:	'/';
MOD_OP		:	'%';
POW_OP		:	'^';

TRUE : T R U E ;
FALSE : F A L S E;

ID
	:	ID_LETTER (ID_LETTER | DIGIT)*
		{
			if(isACondition(Text)) { 
				Type = WDCLParser.BOOLID;
			}
			else {
				Type = WDCLParser.EXPRID;
			}
		}
	;

INT :	DIGIT+
    ;

FLOAT
    :   DIGIT+ '.' DIGIT+
	|	'.' DIGIT+
    ;

DATE
    : DIGIT DIGIT DIGIT DIGIT '/' DIGIT DIGIT '/' DIGIT DIGIT
    ;

/*	OLD VERSION */
STRING
    :  '"' ( ESC_SEQ | ~('\\'|'"') )* '"'
	//|  '\'' ( ESC_SEQ | ~('\\'|'\'') )* '\''
    ;

COMMENT
    :   '//' ~('\n'|'\r')* '\r'? '\n' -> channel(COMMENTS)
	;

/*
STRING : '"' ( ESC | . )*? '"';

LINE_COMMENT : '//' .*? '\r'? '\n' -> channel(HIDDEN)

*/

WS  :   ( ' ' | '\t' | '\r' | '\n' ) -> channel(WHITE_SPACE)
    ;

UNKNOWN_CHAR : . -> channel(INVALID_CHAR) ;

fragment
ESC_SEQ
    :   '\\' ('b'|'t'|'n'|'f'|'r'|'\"'|'\''|'\\')
    ;

fragment	DIGIT: [0-9];
fragment	ID_LETTER: 'a'..'z'|'A'..'Z'|'_';
fragment	A:('a'|'A');
fragment	D:('d'|'D');
fragment	E:('e'|'E');
fragment	F:('f'|'F');
fragment	I:('i'|'I');
fragment	L:('l'|'L');
fragment	N:('n'|'N');
fragment	O:('o'|'O');
fragment	R:('r'|'R');
fragment	S:('s'|'S');
fragment	T:('t'|'T');
fragment	U:('u'|'U');