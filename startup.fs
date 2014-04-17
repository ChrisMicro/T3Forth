{

	T3 Forth
	
	.... work in progress ....
	
}

\ T3 Forth Memory Map
\ ===================
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
					 
  
          
: 1+ 		
					LDA 0
					SAF
					PLUS +IND 8
					STA +IND 8
					;
: 2+
					popa
					CAF
					PLUS 2
					PUSHA
					;
: + 
					popa
					CAF
					PLUS +IND 8
					STA +IND 8
					;

: dup 
					LDA +IND 8 \ (dsp)->A
					PUSHA
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

: 0= ( n -- f ) \ not working yet
					0 = 
					;
						
					

