/*
 ============================================================================
 Name        : qrzForth.h
 Author      : chris
 Version     :
 Copyright   : GPL lizense 3
               ( chris (at) roboterclub-freiburg.de )
 Description :
 ============================================================================
 */

#ifdef __cplusplus
extern "C"
{
#endif

#ifndef __QRZFORTH__
  #define __QRZFORTH__

    #include <stdint.h>
    #include "t3.h"

    #define CODESTARTADDRESS        0x0040 // reserve 64 bytes for data stack and variables
    #define RAMHEAPSTART            0x1800 // location, where FORTH variables are stored, at this address the VM must provide RAM
    #define DICTIONARYSTARTADDRESS  0x2000

    #define REG0             0x7
    #define DATASTACKPOINTER 0x8
    #define DATASTACK        0x9

    #define RAMSIZE 0x200 // 1kb for Atmega328


    typedef uint16_t Index_t; // index into Forth Memory
    typedef Index_t Command_t;

    #define T3_1WORD 0x8000
    #define T3_2WORD 0x9000
    #define T3_ADDRESSMODE 0xA000

    #define T3_MASK 0x0FFF

    //#define DATASTACKSIZE 16
    //#define RETURNSTACKSIZE 32

 #endif

#ifdef __cplusplus
}
#endif
