{

	T3 Forth
	
	.... work in progress ....
	
}

\ T3 Forth Memory Map
\ ===================
\ 5: regn2 ( helper register below next of stack )
\ 6: regn ( helper register to next of stack )
\ 7: reg0 ( helper register )
\ 8: data stack pointer
\ 9: data stack area

: init 
          \ initialize data stack pointer
          LDA 9       \ data stack entries ->A
          STA +ABS 8   \ A->data stack pointer
          ; 
          
 \ PUSHA is mandatory for the forth system
 \ the compiler will place a call to this word
 \ when a constant shall be pushed on stack
 : PUSHA 
					STA +ABS 7 \ A->reg0
					
					\ increment data stack pointer
          LDA +ABS 8
          SAF
          PLUS 0 
          STA +ABS 8
          
          LDA +ABS 7 \ reg0->A
				 	STA +IND 8 \ A->(dsp)
          ;        
: dsp1+ 
					\ increment data stack pointer
          LDA +ABS 8
          SAF
          PLUS 0 
          STA +ABS 8
          ;
: drop
					\ decrement data stack pointer
          LDA +ABS 8
          DEC 0 
          STA +ABS 8
          ;
          
: popreg0
					LDA +IND 8 \ (dsp)->A
					STA +ABS 7 \ A->reg0
					drop
					;
: popa
					LDA +IND 8 \ (dsp)->A
					STA +ABS 7 \ A->reg0
					drop			 \ decrement data stack pointer
					LDA +ABS 7 \ reg0->A
					;
					 
  
          
: 1+ ( n -- n+1 )
					LDA 0
					SAF
					PLUS +IND 8
					STA +IND 8
					;
: 2+ ( n -- n+2 )
					popa
					CAF
					PLUS 2
					PUSHA
					;
					
: + ( n1 n2 -- sum )
					popa
					CAF
					PLUS +IND 8
					STA +IND 8
					;


					
h# \ switch to hex input mode

: = ( n1 n2 -- f )
					popa
					
					CMP +IND 8 			\ compare A,(dsp)
					
					JMPEQ +REL 4		\ if equal jump to A=1FF
					LDA 0						\ if not equal A=0
					JMP +REL 2			\ jump to store
					LDA 1FF					
					
					STA +IND 8
					;

: 0= ( n -- f ) 
					0 = 
					;
						
: 1- ( n -- n-1 )
          LDA +IND 8
          DEC 0 
          STA +IND 8
          ;
          
: 2- ( n -- n-2 )
          LDA +IND 8
          DEC 0
          DEC 0 
          STA +IND 8
					;
					
: invert ( n -- ~n )
					INVM +IND 8
					STA +IND 8
					;
													
\ shift left one bit
: _shl ( n -- n<<1 )
          LDA +IND 8
          SFL +VAL 1
          STA +IND 8
					;		
					
\ shift right one bit
: _asr ( n -- n<<1 )
          LDA +IND 8
          SFR +VAL 1
          STA +IND 8
					;		

: and ( n1 n2 -- n1&n2 )
					popa
					AND +IND 8
					STA +IND 8
					;
					
: or ( n1 n2 -- n1|n2 )
					popa
					OR +IND 8
					STA +IND 8
					;

: xor ( n1 n2 -- n1|n2 )
					popa
					EXOR +IND 8
					STA +IND 8
					;

: <> ( n1 n2 -- f )
					= invert
					;
			
: > ( n1 n2 -- f )
					popa
					
					CMP +IND 8 			\ compare A,(dsp)
					
					JMPGT +REL 4		\ if equal jump to A=1FF
					LDA 0						\ if not equal A=0
					JMP +REL 2			\ jump to store
					LDA 1FF					
					
					STA +IND 8
					;					

: >r ( n -- )
					LDA +IND 8 \ (dsp)->A
					STA +ABS 7 \ A->reg0
					drop			 \ decrement data stack pointer
					LDA +ABS 7 \ reg0->A
					PHA				 \ A->(RSP++)
					;

: r> ( -- n )
					\ increment data stack pointer
          LDA +ABS 8
          SAF
          PLUS 0 
          STA +ABS 8

					\ get A from return stack and store into data stack
				 	PLA \ (--RSP)->A
				 	STA +IND 8 \ A->(dsp)	
;

: dup ( n - n n )
					LDA +IND 8 \ (dsp)->A
					PUSHA
					;

: swap ( n1 n2 -- n2 n1 )
					\ create pointer to next of stack	
					LDA +ABS 8 \ dsp->A
					DEC 0			 
					STA +ABS 6 \ A-1->regn
					
					\ save top of stack in reg0
					LDA +IND 8 \ (dsp)->A
					STA +ABS 7 \ A->reg0
					
					\ copy next to top of stack
					LDA +IND 6 \ (regn)->A
					STA +IND 8 \ A->(dsp)
					
					\ store reg0 in next
					LDA +ABS 7 \ reg0->A
					STA +IND 6 \ A->(regn)
;					

: over ( n1 n2 -- n1 n2 n1 )
					\ create pointer to next of stack	
					LDA +ABS 8 \ dsp->A
					DEC 0			 
					STA +ABS 6 \ A-1->regn
					
					\ load next of stack
					LDA +IND 6 \ (regn)->A
					
					PUSHA
;

: rot ( a b c -- b c a ) 
					\ create pointer to next of stack	
					LDA +ABS 8 \ dsp->A
					DEC 0			 
					STA +ABS 6 \ A-1->regn
					
					\ create pointer below next of stack	
					DEC 0			 
					STA +ABS 5 \ A-1->regn2
					
					\ save top of stack in reg0
					LDA +IND 8 \ (dsp)->A
					STA +ABS 7 \ A->reg0
					
					\ store (regn2) in top of stack
					LDA +IND 5
					STA +IND 8
					
					\ store next below next
					LDA +IND 6
					STA +IND 5
					
					\ store saved top of stack in next
					LDA +ABS 7
					STA +IND 6
					
					;