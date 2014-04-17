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
          LDA 10       \ data stack entries ->A
          STA +ABS 8   \ A->data stack pointer
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
					 
 : pusha 
					STA +ABS 7 \ A->reg0
					
					\ increment data stack pointer
          LDA +ABS 8
          SAF
          PLUS 0 
          STA +ABS 8
          
          LDA +ABS 7 \ reg0->A
				 	STA +IND 8 \ A->(dsp)
          ;         
          
: 1+ 		
					LDA 0
					SAF
					PLUS +IND 8
					STA +IND 8
					;

: + 
					popa
					CAF
					PLUS +IND 8
					STA +IND 8
					;

: dup 
					LDA +IND 8 \ (dsp)->A
					pusha
					;
					
h# \ switch to hex input mode

: 0=		
					LDA +ABS 8 	\ dsp->A
					DEC 0 			\ A--
					STA +ABS 7 	\ A->reg0
					LDA +IND 7 	\ (reg0)->A
					
					CMP +IND 8 \ compare A,(dsp)
					
					JMPEQ +REL 4
					LDA 0
					JMP +REL 2
					LDA 1FF
					
					STA +IND 8 \ A->(dsp)
					;
					
: 0<> 0= 0= ;
						
					

