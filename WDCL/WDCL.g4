grammar WDCL;

/*
 * Parser Rules
 */

parse
	: C=condition EOF
	;

condition
	: 'true'									#true
    | 'false'									#false
    | sub=ID									#subcondition
    | '(' c=condition ')'						#parenCond
    | lE=exp op=EQ rE=exp						#comparisonCond
    | lE=exp op=NOTEQ rE=exp					#comparisonCond
    | lE=exp op=LT rE=exp						#comparisonCond
    | lE=exp op=GT rE=exp						#comparisonCond
    | lE=exp op=LTEQ rE=exp						#comparisonCond
    | lE=exp op=GTEQ rE=exp						#comparisonCond
    | NOT '(' c=condition ')'					#notCond
    | lC=condition op=AND rC=condition			#boolCond
    | lC=condition op=OR rC=condition			#boolCond
    //| lC=condition EQ rC=condition			#comparisonSubCond
    //| lC=condition NOTEQ rC=condition			#comparisonSubCond

    //| ID EQ c=condition						#comparisonSubCond
    //| ID NOTEQ c=condition					#comparisonSubCond

	//| ID IN '(' exp (COMMA exp)* ')'			#setCond
	//| ID NOT IN '(' exp (COMMA exp)* ')'		#setCond
;

exp
	: atom=INT 									#atomExp
	| atom=FLOAT 								#atomExp
	| atom=STRING								#atomExp
	| atom=DATE									#atomExp
	| drv=ID									#driverExp
	| '(' e=exp ')'								#parenExp
	| lE=exp op=PLUS_OP rE=exp					#binarExp
	| lE=exp op=MINUS_OP rE=exp 				#binarExp
	| lE=exp op=MUL_OP rE=exp					#binarExp
	| lE=exp op=DIV_OP rE=exp 					#binarExp
	| lE=exp op=MOD_OP rE=exp 					#binarExp
	| lE=exp op=POW_OP rE=exp					#binarExp
	| op=MINUS_OP e=exp							#unarExp
;




/*
 * Lexer Rules
 */

//OPERATORS:
//	BOOL
NOT	:	'NOT';
AND	:	'AND';
OR	:	'OR';
//	COMPARISON
EQ		:	'=';
NOTEQ	:	'<>';
LTEQ	:	'<=';
LT		:	'<';
GTEQ	:	'>=';
GT		:	'>';
//	SET
IN		:	'IN';
COMMA	:	',';
//ARITHMETIC
PLUS_OP		:	'+';
MINUS_OP	:	'-';
MUL_OP		:	'*';
DIV_OP		:	'/';
MOD_OP		:	'%';
POW_OP		:	'^';

ID
	:	([a-zA-Z]|'_') ([a-zA-Z0-9]|'_')*
	;

//ID  :	([a-z]|[A-Z]|'_') ([a-z]|[A-Z]||[0-9]|'_')*
//    ;

INT :	[0-9]+
    ;

FLOAT
    :   [0-9]+ '.' [0-9]+
    ;

DATE
    : DIGIT DIGIT DIGIT DIGIT '/' DIGIT DIGIT '/' DIGIT DIGIT
    ;

fragment     DIGIT: [0-9];


COMMENT
    :   '//' ~('\n'|'\r')* '\r'? '\n' -> channel(HIDDEN)
	;


WS  :   ( ' ' | '\t' | '\r' | '\n' ) -> skip
    ;

STRING
    :  '"' ( ESC_SEQ | ~('\\'|'"') )* '"'
    ;

fragment
ESC_SEQ
    :   '\\' ('b'|'t'|'n'|'f'|'r'|'\"'|'\''|'\\')
    ;

