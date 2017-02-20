000100 IDENTIFICATION DIVISION.                                         SQ2164.2
000200 PROGRAM-ID.                                                      SQ2164.2
000300     SQ216A.                                                      SQ2164.2
000400****************************************************************  SQ2164.2
000500*                                                              *  SQ2164.2
000600*    VALIDATION FOR:-                                          *  SQ2164.2
000700*    "ON-SITE VALIDATION, NATIONAL INSTITUTE OF STD & TECH.     ".SQ2164.2
000800*                                                              *  SQ2164.2
000900*    CREATION DATE     /     VALIDATION DATE                   *  SQ2164.2
001000*    "COBOL 85 VERSION 4.2, Apr  1993 SSVG                      ".SQ2164.2
001100*                                                              *  SQ2164.2
001200*        THIS ROUTINE TESTS THE CLAUSE:                           SQ2164.2
001300*             PADDING CHARACTER IS   "9" (LITERAL).               SQ2164.2
001400*                                                                 SQ2164.2
001500*        THE ROUTINE SQ216A CREATES A TAPE FILE WHICH HAS 750 FIXESQ2164.2
001600*    LENGTH RECORDS.  THE FILE IS THEN CLOSED AND OPENED AS AN    SQ2164.2
001700*    INPUT FILE.  THE FILE IS READ AND FIELDS IN THE INPUT RECORDSSQ2164.2
001800*    ARE COMPARED TO THE VALUES WRITTEN TO ENSURE THAT THE RECORDSSQ2164.2
001900*    WERE PROCESSED CORRECTLY.  THE FILE IS CLOSED AND OPENED     SQ2164.2
002000*    AGAIN AS AN INPUT FILE.  FOUR READ FORMAT OPTIONS ARE USED   SQ2164.2
002100*    TO READ THE FILE AND FIELDS IN THE RECORDS ARE VERIFIED.     SQ2164.2
002200*    THE OPEN, CLOSE, READ, AND WRITE STATEMENTS ARE TESTED FOR   SQ2164.2
002300*    LEVEL TWO            PADDING CHARCTER IS "9".                SQ2164.2
002400*                                                                 SQ2164.2
002500*    THE LAST 9 RECORDS MUST BE FILLED WITH THE PADDING CHARACTER SQ2164.2
002600*     "9".                                                        SQ2164.2
002700*                                                                 SQ2164.2
002800 ENVIRONMENT DIVISION.                                            SQ2164.2
002900 CONFIGURATION SECTION.                                           SQ2164.2
003000 SOURCE-COMPUTER.                                                 SQ2164.2
003100     XXXXX082.                                                    SQ2164.2
003200 OBJECT-COMPUTER.                                                 SQ2164.2
003300     XXXXX083.                                                    SQ2164.2
003400 INPUT-OUTPUT SECTION.                                            SQ2164.2
003500 FILE-CONTROL.                                                    SQ2164.2
003600     SELECT RAW-DATA   ASSIGN TO                                  SQ2164.2
003700     XXXXX062                                                     SQ2164.2
003800            ORGANIZATION IS INDEXED                               SQ2164.2
003900            ACCESS MODE IS RANDOM                                 SQ2164.2
004000            RECORD KEY IS RAW-DATA-KEY.                           SQ2164.2
004100     SELECT PRINT-FILE ASSIGN TO                                  SQ2164.2
004200     XXXXX055.                                                    SQ2164.2
004300     SELECT SQ-FS1 ASSIGN TO                                      SQ2164.2
004400     XXXXX001                                                     SQ2164.2
004500     ORGANIZATION IS SEQUENTIAL                                   SQ2164.2
004600     PADDING CHARACTER IS "9"                                     SQ2164.2
004700     ACCESS MODE IS SEQUENTIAL.                                   SQ2164.2
004800 DATA DIVISION.                                                   SQ2164.2
004900 FILE SECTION.                                                    SQ2164.2
005000                                                                  SQ2164.2
005100 FD  RAW-DATA.                                                    SQ2164.2
005200                                                                  SQ2164.2
005300 01  RAW-DATA-SATZ.                                               SQ2164.2
005400     05  RAW-DATA-KEY        PIC X(6).                            SQ2164.2
005500     05  C-DATE              PIC 9(6).                            SQ2164.2
005600     05  C-TIME              PIC 9(8).                            SQ2164.2
005700     05  C-NO-OF-TESTS       PIC 99.                              SQ2164.2
005800     05  C-OK                PIC 999.                             SQ2164.2
005900     05  C-ALL               PIC 999.                             SQ2164.2
006000     05  C-FAIL              PIC 999.                             SQ2164.2
006100     05  C-DELETED           PIC 999.                             SQ2164.2
006200     05  C-INSPECT           PIC 999.                             SQ2164.2
006300     05  C-NOTE              PIC X(13).                           SQ2164.2
006400     05  C-INDENT            PIC X.                               SQ2164.2
006500     05  C-ABORT             PIC X(8).                            SQ2164.2
006600 FD  PRINT-FILE                                                   SQ2164.2
006700     LABEL RECORDS                                                SQ2164.2
006800     XXXXX084                                                     SQ2164.2
006900     DATA RECORD IS PRINT-REC DUMMY-RECORD                        SQ2164.2
007000               .                                                  SQ2164.2
007100 01  PRINT-REC PICTURE X(120).                                    SQ2164.2
007200 01  DUMMY-RECORD PICTURE X(120).                                 SQ2164.2
007300 FD  SQ-FS1                                                       SQ2164.2
007400     LABEL RECORD STANDARD                                        SQ2164.2
007500     RECORD CONTAINS 120 CHARACTERS                               SQ2164.2
007600     BLOCK CONTAINS 13 RECORDS.                                   SQ2164.2
007700 01  SQ-FS1R1-F-G-120.                                            SQ2164.2
007800     02  FILLER PIC X(120).                                       SQ2164.2
007900 WORKING-STORAGE SECTION.                                         SQ2164.2
008000 01  WRK-CS-09V00 PIC S9(9) USAGE COMP VALUE ZERO.                SQ2164.2
008100 01  RECORDS-IN-ERROR PIC S9(5) USAGE COMP VALUE ZERO.            SQ2164.2
008200 01  ERROR-FLAG PIC 9 VALUE ZERO.                                 SQ2164.2
008300 01  EOF-FLAG   PICTURE 9 VALUE ZERO.                             SQ2164.2
008400 01  FILE-RECORD-INFORMATION-REC.                                 SQ2164.2
008500     03 FILE-RECORD-INFO-SKELETON.                                SQ2164.2
008600        05 FILLER                 PICTURE X(48)       VALUE       SQ2164.2
008700             "FILE=      ,RECORD=      /0,RECNO=000000,UPDT=00".  SQ2164.2
008800        05 FILLER                 PICTURE X(46)       VALUE       SQ2164.2
008900             ",ODO=0000,PGM=     ,LRECL=000000,BLKSIZ  =0000".    SQ2164.2
009000        05 FILLER                 PICTURE X(26)       VALUE       SQ2164.2
009100             ",LFIL=000000,ORG=  ,LBLR= ".                        SQ2164.2
009200        05 FILLER                 PICTURE X(37)       VALUE       SQ2164.2
009300             ",RECKEY=                             ".             SQ2164.2
009400        05 FILLER                 PICTURE X(38)       VALUE       SQ2164.2
009500             ",ALTKEY1=                             ".            SQ2164.2
009600        05 FILLER                 PICTURE X(38)       VALUE       SQ2164.2
009700             ",ALTKEY2=                             ".            SQ2164.2
009800        05 FILLER                 PICTURE X(7)        VALUE SPACE.SQ2164.2
009900     03 FILE-RECORD-INFO          OCCURS  10  TIMES.              SQ2164.2
010000        05 FILE-RECORD-INFO-P1-120.                               SQ2164.2
010100           07 FILLER              PIC X(5).                       SQ2164.2
010200           07 XFILE-NAME           PIC X(6).                      SQ2164.2
010300           07 FILLER              PIC X(8).                       SQ2164.2
010400           07 XRECORD-NAME         PIC X(6).                      SQ2164.2
010500           07 FILLER              PIC X(1).                       SQ2164.2
010600           07 REELUNIT-NUMBER     PIC 9(1).                       SQ2164.2
010700           07 FILLER              PIC X(7).                       SQ2164.2
010800           07 XRECORD-NUMBER       PIC 9(6).                      SQ2164.2
010900           07 FILLER              PIC X(6).                       SQ2164.2
011000           07 UPDATE-NUMBER       PIC 9(2).                       SQ2164.2
011100           07 FILLER              PIC X(5).                       SQ2164.2
011200           07 ODO-NUMBER          PIC 9(4).                       SQ2164.2
011300           07 FILLER              PIC X(5).                       SQ2164.2
011400           07 XPROGRAM-NAME        PIC X(5).                      SQ2164.2
011500           07 FILLER              PIC X(7).                       SQ2164.2
011600           07 XRECORD-LENGTH       PIC 9(6).                      SQ2164.2
011700           07 FILLER              PIC X(7).                       SQ2164.2
011800           07 CHARS-OR-RECORDS    PIC X(2).                       SQ2164.2
011900           07 FILLER              PIC X(1).                       SQ2164.2
012000           07 XBLOCK-SIZE          PIC 9(4).                      SQ2164.2
012100           07 FILLER              PIC X(6).                       SQ2164.2
012200           07 RECORDS-IN-FILE     PIC 9(6).                       SQ2164.2
012300           07 FILLER              PIC X(5).                       SQ2164.2
012400           07 XFILE-ORGANIZATION   PIC X(2).                      SQ2164.2
012500           07 FILLER              PIC X(6).                       SQ2164.2
012600           07 XLABEL-TYPE          PIC X(1).                      SQ2164.2
012700        05 FILE-RECORD-INFO-P121-240.                             SQ2164.2
012800           07 FILLER              PIC X(8).                       SQ2164.2
012900           07 XRECORD-KEY          PIC X(29).                     SQ2164.2
013000           07 FILLER              PIC X(9).                       SQ2164.2
013100           07 ALTERNATE-KEY1      PIC X(29).                      SQ2164.2
013200           07 FILLER              PIC X(9).                       SQ2164.2
013300           07 ALTERNATE-KEY2      PIC X(29).                      SQ2164.2
013400           07 FILLER              PIC X(7).                       SQ2164.2
013500 01  TEST-RESULTS.                                                SQ2164.2
013600     02 FILLER                    PICTURE X VALUE SPACE.          SQ2164.2
013700     02 FEATURE                   PICTURE X(20) VALUE SPACE.      SQ2164.2
013800     02 FILLER                    PICTURE X VALUE SPACE.          SQ2164.2
013900     02 P-OR-F                    PICTURE X(5) VALUE SPACE.       SQ2164.2
014000     02 FILLER                    PICTURE X  VALUE SPACE.         SQ2164.2
014100     02  PAR-NAME.                                                SQ2164.2
014200       03 FILLER PICTURE X(12) VALUE SPACE.                       SQ2164.2
014300       03  PARDOT-X PICTURE X  VALUE SPACE.                       SQ2164.2
014400       03 DOTVALUE PICTURE 99  VALUE ZERO.                        SQ2164.2
014500       03 FILLER PIC X(5) VALUE SPACE.                            SQ2164.2
014600     02 FILLER PIC X(10) VALUE SPACE.                             SQ2164.2
014700     02 RE-MARK PIC X(61).                                        SQ2164.2
014800 01  TEST-COMPUTED.                                               SQ2164.2
014900     02 FILLER PIC X(30) VALUE SPACE.                             SQ2164.2
015000     02 FILLER PIC X(17) VALUE "       COMPUTED=".                SQ2164.2
015100     02 COMPUTED-X.                                               SQ2164.2
015200     03 COMPUTED-A                PICTURE X(20) VALUE SPACE.      SQ2164.2
015300     03 COMPUTED-N REDEFINES COMPUTED-A PICTURE -9(9).9(9).       SQ2164.2
015400     03 COMPUTED-0V18 REDEFINES COMPUTED-A  PICTURE -.9(18).      SQ2164.2
015500     03 COMPUTED-4V14 REDEFINES COMPUTED-A  PICTURE -9(4).9(14).  SQ2164.2
015600     03 COMPUTED-14V4 REDEFINES COMPUTED-A  PICTURE -9(14).9(4).  SQ2164.2
015700     03       CM-18V0 REDEFINES COMPUTED-A.                       SQ2164.2
015800         04 COMPUTED-18V0                   PICTURE -9(18).       SQ2164.2
015900         04 FILLER                          PICTURE X.            SQ2164.2
016000     03 FILLER PIC X(50) VALUE SPACE.                             SQ2164.2
016100 01  TEST-CORRECT.                                                SQ2164.2
016200     02 FILLER PIC X(30) VALUE SPACE.                             SQ2164.2
016300     02 FILLER PIC X(17) VALUE "       CORRECT =".                SQ2164.2
016400     02 CORRECT-X.                                                SQ2164.2
016500     03 CORRECT-A                 PICTURE X(20) VALUE SPACE.      SQ2164.2
016600     03 CORRECT-N REDEFINES CORRECT-A PICTURE -9(9).9(9).         SQ2164.2
016700     03 CORRECT-0V18 REDEFINES CORRECT-A    PICTURE -.9(18).      SQ2164.2
016800     03 CORRECT-4V14 REDEFINES CORRECT-A    PICTURE -9(4).9(14).  SQ2164.2
016900     03 CORRECT-14V4 REDEFINES CORRECT-A    PICTURE -9(14).9(4).  SQ2164.2
017000     03      CR-18V0 REDEFINES CORRECT-A.                         SQ2164.2
017100         04 CORRECT-18V0                    PICTURE -9(18).       SQ2164.2
017200         04 FILLER                          PICTURE X.            SQ2164.2
017300     03 FILLER PIC X(50) VALUE SPACE.                             SQ2164.2
017400 01  CCVS-C-1.                                                    SQ2164.2
017500     02 FILLER PICTURE IS X(99) VALUE IS " FEATURE              PASQ2164.2
017600-    "SS  PARAGRAPH-NAME                                          SQ2164.2
017700-    "        REMARKS".                                           SQ2164.2
017800     02 FILLER PICTURE IS X(20) VALUE IS SPACE.                   SQ2164.2
017900 01  CCVS-C-2.                                                    SQ2164.2
018000     02 FILLER PICTURE IS X VALUE IS SPACE.                       SQ2164.2
018100     02 FILLER PICTURE IS X(6) VALUE IS "TESTED".                 SQ2164.2
018200     02 FILLER PICTURE IS X(15) VALUE IS SPACE.                   SQ2164.2
018300     02 FILLER PICTURE IS X(4) VALUE IS "FAIL".                   SQ2164.2
018400     02 FILLER PICTURE IS X(94) VALUE IS SPACE.                   SQ2164.2
018500 01  REC-SKL-SUB PICTURE 9(2) VALUE ZERO.                         SQ2164.2
018600 01  REC-CT PICTURE 99 VALUE ZERO.                                SQ2164.2
018700 01  DELETE-CNT                   PICTURE 999  VALUE ZERO.        SQ2164.2
018800 01  ERROR-COUNTER PICTURE IS 999 VALUE IS ZERO.                  SQ2164.2
018900 01  INSPECT-COUNTER PIC 999 VALUE ZERO.                          SQ2164.2
019000 01  PASS-COUNTER PIC 999 VALUE ZERO.                             SQ2164.2
019100 01  TOTAL-ERROR PIC 999 VALUE ZERO.                              SQ2164.2
019200 01  ERROR-HOLD PIC 999 VALUE ZERO.                               SQ2164.2
019300 01  DUMMY-HOLD PIC X(120) VALUE SPACE.                           SQ2164.2
019400 01  RECORD-COUNT PIC 9(5) VALUE ZERO.                            SQ2164.2
019500 01  CCVS-H-1.                                                    SQ2164.2
019600     02  FILLER   PICTURE X(27)  VALUE SPACE.                     SQ2164.2
019700     02 FILLER PICTURE X(67) VALUE                                SQ2164.2
019800     " FEDERAL SOFTWARE TESTING CENTER COBOL COMPILER VALIDATION  SQ2164.2
019900-    " SYSTEM".                                                   SQ2164.2
020000     02  FILLER     PICTURE X(26)  VALUE SPACE.                   SQ2164.2
020100 01  CCVS-H-2.                                                    SQ2164.2
020200     02 FILLER PICTURE X(52) VALUE IS                             SQ2164.2
020300     "CCVS85 FSTC COPY, NOT FOR DISTRIBUTION.".                   SQ2164.2
020400     02 FILLER PICTURE IS X(19) VALUE IS "TEST RESULTS SET-  ".   SQ2164.2
020500     02 TEST-ID PICTURE IS X(9).                                  SQ2164.2
020600     02 FILLER PICTURE IS X(40) VALUE IS SPACE.                   SQ2164.2
020700 01  CCVS-H-3.                                                    SQ2164.2
020800     02  FILLER PICTURE X(34) VALUE                               SQ2164.2
020900     " FOR OFFICIAL USE ONLY    ".                                SQ2164.2
021000     02  FILLER PICTURE X(58) VALUE                               SQ2164.2
021100     "COBOL 85 VERSION 4.2, Apr  1993 SSVG                      ".SQ2164.2
021200     02  FILLER PICTURE X(28) VALUE                               SQ2164.2
021300     "  COPYRIGHT   1985 ".                                       SQ2164.2
021400 01  CCVS-E-1.                                                    SQ2164.2
021500     02 FILLER PICTURE IS X(52) VALUE IS SPACE.                   SQ2164.2
021600     02 FILLER PICTURE IS X(14) VALUE IS "END OF TEST-  ".        SQ2164.2
021700     02 ID-AGAIN PICTURE IS X(9).                                 SQ2164.2
021800     02 FILLER PICTURE X(45) VALUE IS                             SQ2164.2
021900     " NTIS DISTRIBUTION COBOL 85".                               SQ2164.2
022000 01  CCVS-E-2.                                                    SQ2164.2
022100     02  FILLER                   PICTURE X(31)  VALUE            SQ2164.2
022200     SPACE.                                                       SQ2164.2
022300     02  FILLER                   PICTURE X(21)  VALUE SPACE.     SQ2164.2
022400     02 CCVS-E-2-2.                                               SQ2164.2
022500         03 ERROR-TOTAL PICTURE IS XXX VALUE IS SPACE.            SQ2164.2
022600         03 FILLER PICTURE IS X VALUE IS SPACE.                   SQ2164.2
022700         03 ENDER-DESC PIC X(46) VALUE "ERRORS ENCOUNTERED".      SQ2164.2
022800 01  CCVS-E-3.                                                    SQ2164.2
022900     02  FILLER PICTURE X(22) VALUE                               SQ2164.2
023000     " FOR OFFICIAL USE ONLY".                                    SQ2164.2
023100     02  FILLER PICTURE X(12) VALUE SPACE.                        SQ2164.2
023200     02  FILLER PICTURE X(58) VALUE                               SQ2164.2
023300     "ON-SITE VALIDATION, NATIONAL INSTITUTE OF STD & TECH.     ".SQ2164.2
023400     02  FILLER PICTURE X(13) VALUE SPACE.                        SQ2164.2
023500     02 FILLER PIC X(15) VALUE " COPYRIGHT 1985".                 SQ2164.2
023600 01  CCVS-E-4.                                                    SQ2164.2
023700     02 CCVS-E-4-1 PIC XXX VALUE SPACE.                           SQ2164.2
023800     02 FILLER PIC XXXX VALUE " OF ".                             SQ2164.2
023900     02 CCVS-E-4-2 PIC XXX VALUE SPACE.                           SQ2164.2
024000     02 FILLER PIC X(40) VALUE                                    SQ2164.2
024100      "  TESTS WERE EXECUTED SUCCESSFULLY".                       SQ2164.2
024200 01  XXINFO.                                                      SQ2164.2
024300     02 FILLER PIC X(30) VALUE "        *** INFORMATION  ***".    SQ2164.2
024400     02 INFO-TEXT.                                                SQ2164.2
024500     04 FILLER PIC X(20) VALUE SPACE.                             SQ2164.2
024600     04 XXCOMPUTED PIC X(20).                                     SQ2164.2
024700     04 FILLER PIC X(5) VALUE SPACE.                              SQ2164.2
024800     04 XXCORRECT PIC X(20).                                      SQ2164.2
024900 01  HYPHEN-LINE.                                                 SQ2164.2
025000     02 FILLER PICTURE IS X VALUE IS SPACE.                       SQ2164.2
025100     02 FILLER PICTURE IS X(65) VALUE IS "************************SQ2164.2
025200-    "*****************************************".                 SQ2164.2
025300     02 FILLER PICTURE IS X(54) VALUE IS "************************SQ2164.2
025400-    "******************************".                            SQ2164.2
025500 01  CCVS-PGM-ID PIC X(6) VALUE                                   SQ2164.2
025600     "SQ216A".                                                    SQ2164.2
025700 PROCEDURE DIVISION.                                              SQ2164.2
025800 CCVS1 SECTION.                                                   SQ2164.2
025900 OPEN-FILES.                                                      SQ2164.2
026000     OPEN I-O RAW-DATA.                                           SQ2164.2
026100     MOVE CCVS-PGM-ID TO RAW-DATA-KEY.                            SQ2164.2
026200     READ RAW-DATA INVALID KEY GO TO END-E-1.                     SQ2164.2
026300     MOVE "ABORTED " TO C-ABORT.                                  SQ2164.2
026400     ADD 1 TO C-NO-OF-TESTS.                                      SQ2164.2
026500     ACCEPT C-DATE  FROM DATE.                                    SQ2164.2
026600     ACCEPT C-TIME  FROM TIME.                                    SQ2164.2
026700     REWRITE RAW-DATA-SATZ INVALID KEY GO TO END-E-1.             SQ2164.2
026800 END-E-1.                                                         SQ2164.2
026900     CLOSE RAW-DATA.                                              SQ2164.2
027000     OPEN     OUTPUT PRINT-FILE.                                  SQ2164.2
027100     MOVE CCVS-PGM-ID TO TEST-ID. MOVE CCVS-PGM-ID TO ID-AGAIN.   SQ2164.2
027200     MOVE    SPACE TO TEST-RESULTS.                               SQ2164.2
027300     PERFORM  HEAD-ROUTINE THRU COLUMN-NAMES-ROUTINE.             SQ2164.2
027400     MOVE ZERO TO REC-SKL-SUB.                                    SQ2164.2
027500     PERFORM CCVS-INIT-FILE 9 TIMES.                              SQ2164.2
027600 CCVS-INIT-FILE.                                                  SQ2164.2
027700     ADD 1 TO REC-SKL-SUB.                                        SQ2164.2
027800     MOVE FILE-RECORD-INFO-SKELETON TO                            SQ2164.2
027900                  FILE-RECORD-INFO (REC-SKL-SUB).                 SQ2164.2
028000 CCVS-INIT-EXIT.                                                  SQ2164.2
028100     GO TO CCVS1-EXIT.                                            SQ2164.2
028200 CLOSE-FILES.                                                     SQ2164.2
028300     PERFORM END-ROUTINE THRU END-ROUTINE-13. CLOSE PRINT-FILE.   SQ2164.2
028400     OPEN I-O RAW-DATA.                                           SQ2164.2
028500     MOVE CCVS-PGM-ID TO RAW-DATA-KEY.                            SQ2164.2
028600     READ RAW-DATA INVALID KEY GO TO END-E-2.                     SQ2164.2
028700     MOVE "OK.     " TO C-ABORT.                                  SQ2164.2
028800     MOVE PASS-COUNTER TO C-OK.                                   SQ2164.2
028900     MOVE ERROR-HOLD   TO C-ALL.                                  SQ2164.2
029000     MOVE ERROR-COUNTER TO C-FAIL.                                SQ2164.2
029100     MOVE DELETE-CNT TO C-DELETED.                                SQ2164.2
029200     MOVE INSPECT-COUNTER TO C-INSPECT.                           SQ2164.2
029300     REWRITE RAW-DATA-SATZ INVALID KEY GO TO END-E-2.             SQ2164.2
029400 END-E-2.                                                         SQ2164.2
029500     CLOSE RAW-DATA.                                              SQ2164.2
029600 TERMINATE-CCVS.                                                  SQ2164.2
029700     EXIT PROGRAM.                                                SQ2164.2
029800 TERMINATE-CALL.                                                  SQ2164.2
029900     STOP     RUN.                                                SQ2164.2
030000 INSPT. MOVE "INSPT" TO P-OR-F. ADD 1 TO INSPECT-COUNTER.         SQ2164.2
030100 PASS.  MOVE "PASS " TO P-OR-F.  ADD 1 TO PASS-COUNTER.           SQ2164.2
030200 FAIL.  MOVE "FAIL*" TO P-OR-F.  ADD 1 TO ERROR-COUNTER.          SQ2164.2
030300 DE-LETE.  MOVE "*****" TO P-OR-F.  ADD 1 TO DELETE-CNT.          SQ2164.2
030400     MOVE "****TEST DELETED****" TO RE-MARK.                      SQ2164.2
030500 PRINT-DETAIL.                                                    SQ2164.2
030600     IF REC-CT NOT EQUAL TO ZERO                                  SQ2164.2
030700             MOVE "." TO PARDOT-X                                 SQ2164.2
030800             MOVE REC-CT TO DOTVALUE.                             SQ2164.2
030900     MOVE     TEST-RESULTS TO PRINT-REC. PERFORM WRITE-LINE.      SQ2164.2
031000     IF P-OR-F EQUAL TO "FAIL*"  PERFORM WRITE-LINE               SQ2164.2
031100        PERFORM FAIL-ROUTINE THRU FAIL-ROUTINE-EX                 SQ2164.2
031200          ELSE PERFORM BAIL-OUT THRU BAIL-OUT-EX.                 SQ2164.2
031300     MOVE SPACE TO P-OR-F. MOVE SPACE TO COMPUTED-X.              SQ2164.2
031400     MOVE SPACE TO CORRECT-X.                                     SQ2164.2
031500     IF     REC-CT EQUAL TO ZERO  MOVE SPACE TO PAR-NAME.         SQ2164.2
031600     MOVE     SPACE TO RE-MARK.                                   SQ2164.2
031700 HEAD-ROUTINE.                                                    SQ2164.2
031800     MOVE CCVS-H-1 TO DUMMY-RECORD. PERFORM WRITE-LINE 2 TIMES.   SQ2164.2
031900     MOVE CCVS-H-2 TO DUMMY-RECORD. PERFORM WRITE-LINE 5 TIMES.   SQ2164.2
032000     MOVE CCVS-H-3 TO DUMMY-RECORD. PERFORM WRITE-LINE 3 TIMES.   SQ2164.2
032100 COLUMN-NAMES-ROUTINE.                                            SQ2164.2
032200     MOVE CCVS-C-1 TO DUMMY-RECORD. PERFORM WRITE-LINE.           SQ2164.2
032300     MOVE CCVS-C-2 TO DUMMY-RECORD. PERFORM WRITE-LINE 2 TIMES.   SQ2164.2
032400     MOVE HYPHEN-LINE TO DUMMY-RECORD. PERFORM WRITE-LINE.        SQ2164.2
032500 END-ROUTINE.                                                     SQ2164.2
032600     MOVE HYPHEN-LINE TO DUMMY-RECORD. PERFORM WRITE-LINE 5 TIMES.SQ2164.2
032700 END-RTN-EXIT.                                                    SQ2164.2
032800     MOVE CCVS-E-1 TO DUMMY-RECORD. PERFORM WRITE-LINE 2 TIMES.   SQ2164.2
032900 END-ROUTINE-1.                                                   SQ2164.2
033000      ADD ERROR-COUNTER TO ERROR-HOLD ADD INSPECT-COUNTER TO      SQ2164.2
033100      ERROR-HOLD. ADD DELETE-CNT TO ERROR-HOLD.                   SQ2164.2
033200      ADD PASS-COUNTER TO ERROR-HOLD.                             SQ2164.2
033300*     IF PASS-COUNTER EQUAL TO ERROR-HOLD GO TO END-ROUTINE-12.   SQ2164.2
033400      MOVE PASS-COUNTER TO CCVS-E-4-1.                            SQ2164.2
033500      MOVE ERROR-HOLD TO CCVS-E-4-2.                              SQ2164.2
033600      MOVE CCVS-E-4 TO CCVS-E-2-2.                                SQ2164.2
033700      MOVE CCVS-E-2 TO DUMMY-RECORD PERFORM WRITE-LINE.           SQ2164.2
033800  END-ROUTINE-12.                                                 SQ2164.2
033900      MOVE "TEST(S) FAILED" TO ENDER-DESC.                        SQ2164.2
034000     IF       ERROR-COUNTER IS EQUAL TO ZERO                      SQ2164.2
034100         MOVE "NO " TO ERROR-TOTAL                                SQ2164.2
034200         ELSE                                                     SQ2164.2
034300         MOVE ERROR-COUNTER TO ERROR-TOTAL.                       SQ2164.2
034400     MOVE     CCVS-E-2 TO DUMMY-RECORD.                           SQ2164.2
034500     PERFORM WRITE-LINE.                                          SQ2164.2
034600 END-ROUTINE-13.                                                  SQ2164.2
034700     IF DELETE-CNT IS EQUAL TO ZERO                               SQ2164.2
034800         MOVE "NO " TO ERROR-TOTAL  ELSE                          SQ2164.2
034900         MOVE DELETE-CNT TO ERROR-TOTAL.                          SQ2164.2
035000     MOVE "TEST(S) DELETED     " TO ENDER-DESC.                   SQ2164.2
035100     MOVE CCVS-E-2 TO DUMMY-RECORD. PERFORM WRITE-LINE.           SQ2164.2
035200      IF   INSPECT-COUNTER EQUAL TO ZERO                          SQ2164.2
035300          MOVE "NO " TO ERROR-TOTAL                               SQ2164.2
035400      ELSE MOVE INSPECT-COUNTER TO ERROR-TOTAL.                   SQ2164.2
035500      MOVE "TEST(S) REQUIRE INSPECTION" TO ENDER-DESC.            SQ2164.2
035600      MOVE CCVS-E-2 TO DUMMY-RECORD. PERFORM WRITE-LINE.          SQ2164.2
035700     MOVE CCVS-E-3 TO DUMMY-RECORD. PERFORM WRITE-LINE.           SQ2164.2
035800 WRITE-LINE.                                                      SQ2164.2
035900     ADD 1 TO RECORD-COUNT.                                       SQ2164.2
036000     IF RECORD-COUNT GREATER 50                                   SQ2164.2
036100         MOVE DUMMY-RECORD TO DUMMY-HOLD                          SQ2164.2
036200         MOVE SPACE TO DUMMY-RECORD                               SQ2164.2
036300         WRITE DUMMY-RECORD AFTER ADVANCING PAGE                  SQ2164.2
036400         MOVE CCVS-C-1 TO DUMMY-RECORD PERFORM WRT-LN             SQ2164.2
036500         MOVE CCVS-C-2 TO DUMMY-RECORD PERFORM WRT-LN 2 TIMES     SQ2164.2
036600         MOVE HYPHEN-LINE TO DUMMY-RECORD PERFORM WRT-LN          SQ2164.2
036700         MOVE DUMMY-HOLD TO DUMMY-RECORD                          SQ2164.2
036800         MOVE ZERO TO RECORD-COUNT.                               SQ2164.2
036900     PERFORM WRT-LN.                                              SQ2164.2
037000 WRT-LN.                                                          SQ2164.2
037100     WRITE    DUMMY-RECORD AFTER ADVANCING 1 LINES.               SQ2164.2
037200     MOVE SPACE TO DUMMY-RECORD.                                  SQ2164.2
037300 BLANK-LINE-PRINT.                                                SQ2164.2
037400     PERFORM WRT-LN.                                              SQ2164.2
037500 FAIL-ROUTINE.                                                    SQ2164.2
037600     IF COMPUTED-X NOT EQUAL TO SPACE GO TO FAIL-ROUTINE-WRITE.   SQ2164.2
037700     IF CORRECT-X NOT EQUAL TO SPACE GO TO FAIL-ROUTINE-WRITE.    SQ2164.2
037800     MOVE "NO FURTHER INFORMATION, SEE PROGRAM." TO INFO-TEXT.    SQ2164.2
037900     MOVE XXINFO TO DUMMY-RECORD. PERFORM WRITE-LINE 2 TIMES.     SQ2164.2
038000     GO TO FAIL-ROUTINE-EX.                                       SQ2164.2
038100 FAIL-ROUTINE-WRITE.                                              SQ2164.2
038200     MOVE TEST-COMPUTED TO PRINT-REC PERFORM WRITE-LINE           SQ2164.2
038300     MOVE TEST-CORRECT TO PRINT-REC PERFORM WRITE-LINE 2 TIMES.   SQ2164.2
038400 FAIL-ROUTINE-EX. EXIT.                                           SQ2164.2
038500 BAIL-OUT.                                                        SQ2164.2
038600     IF COMPUTED-A NOT EQUAL TO SPACE GO TO BAIL-OUT-WRITE.       SQ2164.2
038700     IF CORRECT-A EQUAL TO SPACE GO TO BAIL-OUT-EX.               SQ2164.2
038800 BAIL-OUT-WRITE.                                                  SQ2164.2
038900     MOVE CORRECT-A TO XXCORRECT. MOVE COMPUTED-A TO XXCOMPUTED.  SQ2164.2
039000     MOVE XXINFO TO DUMMY-RECORD. PERFORM WRITE-LINE 2 TIMES.     SQ2164.2
039100 BAIL-OUT-EX. EXIT.                                               SQ2164.2
039200 CCVS1-EXIT.                                                      SQ2164.2
039300     EXIT.                                                        SQ2164.2
039400 SECT-SQ216A-0001 SECTION.                                        SQ2164.2
039500 WRITE-INIT-GF-01.                                                SQ2164.2
039600     MOVE "SQ-FS1" TO XFILE-NAME (1).                             SQ2164.2
039700     MOVE "R1-F-G" TO XRECORD-NAME (1).                           SQ2164.2
039800     MOVE "SQ216"     TO XPROGRAM-NAME (1).                       SQ2164.2
039900     MOVE 000120 TO XRECORD-LENGTH (1).                           SQ2164.2
040000     MOVE "RC"   TO CHARS-OR-RECORDS (1).                         SQ2164.2
040100     MOVE 0001   TO XBLOCK-SIZE (1).                              SQ2164.2
040200     MOVE 000750 TO RECORDS-IN-FILE (1).                          SQ2164.2
040300     MOVE "SQ"   TO XFILE-ORGANIZATION (1).                       SQ2164.2
040400     MOVE "S" TO XLABEL-TYPE (1).                                 SQ2164.2
040500     MOVE 000001 TO XRECORD-NUMBER (1).                           SQ2164.2
040600     OPEN OUTPUT SQ-FS1.                                          SQ2164.2
040700 WRITE-TEST-GF-01.                                                SQ2164.2
040800     MOVE FILE-RECORD-INFO-P1-120 (1)  TO SQ-FS1R1-F-G-120.       SQ2164.2
040900     WRITE SQ-FS1R1-F-G-120.                                      SQ2164.2
041000     IF XRECORD-NUMBER (1) EQUAL TO 750                           SQ2164.2
041100         GO TO WRITE-WRITE-GF-01.                                 SQ2164.2
041200     ADD 1 TO XRECORD-NUMBER (1).                                 SQ2164.2
041300     GO TO WRITE-TEST-GF-01.                                      SQ2164.2
041400 WRITE-WRITE-GF-01.                                               SQ2164.2
041500     MOVE "CREATE FILE SQ-FS1" TO FEATURE.                        SQ2164.2
041600     MOVE "WRITE-TEST-GF-01" TO PAR-NAME.                         SQ2164.2
041700     MOVE "FILE CREATED, RECS =" TO COMPUTED-A.                   SQ2164.2
041800     MOVE  XRECORD-NUMBER (1) TO CORRECT-18V0.                    SQ2164.2
041900     PERFORM PASS.                                                SQ2164.2
042000     PERFORM PRINT-DETAIL.                                        SQ2164.2
042100     CLOSE SQ-FS1.                                                SQ2164.2
042200*        A SEQUENTIAL TAPE FILE WITH 120 CHARACTER RECORDS        SQ2164.2
042300*    HAS BEEN CREATED. THE FILE CONTAINS 750 RECORDS.             SQ2164.2
042400 READ-INIT-F1-01.                                                 SQ2164.2
042500     MOVE ZERO TO WRK-CS-09V00.                                   SQ2164.2
042600*        THIS TEST READS AND CHECKS THE FILE CREATED IN           SQ2164.2
042700*    WRITE-TEST-GF-01.                                            SQ2164.2
042800     OPEN INPUT SQ-FS1.                                           SQ2164.2
042900 READ-TEST-F1-01.                                                 SQ2164.2
043000     READ SQ-FS1                                                  SQ2164.2
043100          AT END GO TO READ-TEST-F1-01-1.                         SQ2164.2
043200     MOVE   SQ-FS1R1-F-G-120 TO FILE-RECORD-INFO-P1-120 (1).      SQ2164.2
043300     ADD 1 TO WRK-CS-09V00.                                       SQ2164.2
043400     IF WRK-CS-09V00 GREATER THAN 750                             SQ2164.2
043500        MOVE "MORE THAN 750 RECORDS" TO RE-MARK                   SQ2164.2
043600        GO TO READ-FAIL-F1-01.                                    SQ2164.2
043700     IF WRK-CS-09V00 NOT EQUAL TO XRECORD-NUMBER (1)              SQ2164.2
043800         ADD 1 TO RECORDS-IN-ERROR                                SQ2164.2
043900         GO TO READ-TEST-F1-01.                                   SQ2164.2
044000     IF XFILE-NAME (1) NOT EQUAL TO "SQ-FS1"                      SQ2164.2
044100        ADD 1 TO RECORDS-IN-ERROR                                 SQ2164.2
044200        GO TO READ-TEST-F1-01.                                    SQ2164.2
044300     IF XLABEL-TYPE (1) NOT EQUAL TO "S"                          SQ2164.2
044400        ADD 1 TO RECORDS-IN-ERROR.                                SQ2164.2
044500     GO TO READ-TEST-F1-01.                                       SQ2164.2
044600 READ-TEST-F1-01-1.                                               SQ2164.2
044700     IF RECORDS-IN-ERROR EQUAL TO ZERO                            SQ2164.2
044800         GO TO READ-PASS-F1-01.                                   SQ2164.2
044900     MOVE "ERRORS IN READING SQ-FS1" TO RE-MARK.                  SQ2164.2
045000 READ-FAIL-F1-01.                                                 SQ2164.2
045100     MOVE "VII-12; PADDING CHARACTER"  TO RE-MARK.                SQ2164.2
045200     MOVE "RECORDS IN ERROR =" TO COMPUTED-A.                     SQ2164.2
045300     MOVE RECORDS-IN-ERROR TO CORRECT-18V0.                       SQ2164.2
045400     PERFORM FAIL.                                                SQ2164.2
045500     GO TO READ-WRITE-F1-01.                                      SQ2164.2
045600 READ-PASS-F1-01.                                                 SQ2164.2
045700     PERFORM PASS.                                                SQ2164.2
045800     MOVE "FILE VERIFIED RECS =" TO COMPUTED-A.                   SQ2164.2
045900     MOVE WRK-CS-09V00 TO CORRECT-18V0.                           SQ2164.2
046000 READ-WRITE-F1-01.                                                SQ2164.2
046100     MOVE "READ-TEST-F1-01"  TO PAR-NAME.                         SQ2164.2
046200     MOVE "VERIFY FILE SQ-FS1" TO FEATURE.                        SQ2164.2
046300     PERFORM PRINT-DETAIL.                                        SQ2164.2
046400 READ-CLOSE-F1-01.                                                SQ2164.2
046500     CLOSE SQ-FS1.                                                SQ2164.2
046600 READ-INIT-F1-02.                                                 SQ2164.2
046700     MOVE ZERO TO WRK-CS-09V00.                                   SQ2164.2
046800     MOVE ZERO TO RECORDS-IN-ERROR.                               SQ2164.2
046900     OPEN INPUT   SQ-FS1.                                         SQ2164.2
047000*            FOUR OPTIONS FOR THE READ STATEMENT ARE CHECKED      SQ2164.2
047100*    IN THIS SERIES OF TESTS.                                     SQ2164.2
047200     MOVE "LEV 2 PADDING CHARS " TO FEATURE.                      SQ2164.2
047300     MOVE "READ...RECORD AT END ..." TO RE-MARK.                  SQ2164.2
047400     MOVE "READ-TEST-F1-02" TO PAR-NAME.                          SQ2164.2
047500     MOVE ZERO TO ERROR-FLAG.                                     SQ2164.2
047600 READ-TEST-F1-02.                                                 SQ2164.2
047700     READ SQ-FS1 RECORD AT END                                    SQ2164.2
047800              MOVE "UNEXPECTED EOF" TO COMPUTED-A                 SQ2164.2
047900              MOVE 1 TO EOF-FLAG                                  SQ2164.2
048000              GO TO READ-FAIL-F1-02.                              SQ2164.2
048100     PERFORM RECORD-CHECK.                                        SQ2164.2
048200     IF WRK-CS-09V00 EQUAL TO 200                                 SQ2164.2
048300              GO TO READ-TEST-F1-02-1.                            SQ2164.2
048400             GO TO READ-TEST-F1-02.                               SQ2164.2
048500 RECORD-CHECK.                                                    SQ2164.2
048600     MOVE SQ-FS1R1-F-G-120 TO FILE-RECORD-INFO-P1-120 (1).        SQ2164.2
048700     ADD 1 TO WRK-CS-09V00.                                       SQ2164.2
048800     IF WRK-CS-09V00 NOT EQUAL TO XRECORD-NUMBER (1)              SQ2164.2
048900         ADD 1 TO RECORDS-IN-ERROR                                SQ2164.2
049000         MOVE 1 TO ERROR-FLAG.                                    SQ2164.2
049100 READ-TEST-F1-02-1.                                               SQ2164.2
049200     IF ERROR-FLAG EQUAL TO ZERO                                  SQ2164.2
049300         GO TO READ-PASS-F1-02.                                   SQ2164.2
049400     MOVE "ERROR IN RECORD(S)" TO COMPUTED-A.                     SQ2164.2
049500 READ-FAIL-F1-02.                                                 SQ2164.2
049600     PERFORM FAIL.                                                SQ2164.2
049700     GO TO READ-WRITE-F1-02.                                      SQ2164.2
049800 READ-PASS-F1-02.                                                 SQ2164.2
049900     PERFORM PASS.                                                SQ2164.2
050000 READ-WRITE-F1-02.                                                SQ2164.2
050100     PERFORM PRINT-DETAIL.                                        SQ2164.2
050200 READ-INIT-F1-F1-03.                                              SQ2164.2
050300     IF EOF-FLAG EQUAL TO 1                                       SQ2164.2
050400        GO TO  READ-EOF-06.                                       SQ2164.2
050500     MOVE ZERO TO ERROR-FLAG.                                     SQ2164.2
050600     MOVE "READ...AT END..." TO RE-MARK.                          SQ2164.2
050700     MOVE "READ-TEST-F1-03" TO PAR-NAME.                          SQ2164.2
050800 READ-TEST-F1-03.                                                 SQ2164.2
050900     READ SQ-FS1 AT END                                           SQ2164.2
051000         MOVE "UNEXPECTED EOF" TO COMPUTED-A                      SQ2164.2
051100         MOVE 1 TO EOF-FLAG                                       SQ2164.2
051200         GO TO READ-FAIL-F1-03.                                   SQ2164.2
051300     PERFORM RECORD-CHECK.                                        SQ2164.2
051400     IF WRK-CS-09V00 EQUAL TO 400                                 SQ2164.2
051500         GO TO READ-TEST-F1-03-1.                                 SQ2164.2
051600     GO TO READ-TEST-F1-03.                                       SQ2164.2
051700 READ-TEST-F1-03-1.                                               SQ2164.2
051800     IF ERROR-FLAG EQUAL TO ZERO                                  SQ2164.2
051900         GO TO READ-PASS-F1-03.                                   SQ2164.2
052000     MOVE "ERROR IN RECORD(S)" TO COMPUTED-A.                     SQ2164.2
052100 READ-FAIL-F1-03.                                                 SQ2164.2
052200     MOVE "VII-12; PADDING CHARACTER"  TO RE-MARK.                SQ2164.2
052300     PERFORM FAIL.                                                SQ2164.2
052400     GO TO READ-WRITE-F1-03.                                      SQ2164.2
052500 READ-PASS-F1-03.                                                 SQ2164.2
052600     PERFORM PASS.                                                SQ2164.2
052700 READ-WRITE-F1-03.                                                SQ2164.2
052800     PERFORM PRINT-DETAIL.                                        SQ2164.2
052900 READ-INIT-F1-04.                                                 SQ2164.2
053000     IF EOF-FLAG EQUAL TO 1                                       SQ2164.2
053100        GO TO  READ-EOF-06.                                       SQ2164.2
053200     MOVE ZERO TO ERROR-FLAG.                                     SQ2164.2
053300     MOVE "READ...RECORD END..." TO RE-MARK.                      SQ2164.2
053400     MOVE "READ-TEST-F1-04" TO PAR-NAME.                          SQ2164.2
053500 READ-TEST-F1-04.                                                 SQ2164.2
053600     READ SQ-FS1 RECORD END                                       SQ2164.2
053700          MOVE "UNEXPECTED EOF" TO COMPUTED-A                     SQ2164.2
053800          MOVE 1 TO EOF-FLAG                                      SQ2164.2
053900          GO TO READ-FAIL-F1-04.                                  SQ2164.2
054000     PERFORM RECORD-CHECK.                                        SQ2164.2
054100     IF WRK-CS-09V00 EQUAL TO 600                                 SQ2164.2
054200         GO TO READ-TEST-F1-04-1.                                 SQ2164.2
054300     GO TO READ-TEST-F1-04.                                       SQ2164.2
054400 READ-TEST-F1-04-1.                                               SQ2164.2
054500     IF ERROR-FLAG EQUAL TO ZERO                                  SQ2164.2
054600         GO TO READ-PASS-F1-04.                                   SQ2164.2
054700     MOVE "ERROR IN RECORD(S)" TO COMPUTED-A.                     SQ2164.2
054800 READ-FAIL-F1-04.                                                 SQ2164.2
054900     MOVE "VII-12; PADDING CHARACTER"  TO RE-MARK.                SQ2164.2
055000     PERFORM FAIL.                                                SQ2164.2
055100     GO TO READ-WRITE-F1-04.                                      SQ2164.2
055200 READ-PASS-F1-04.                                                 SQ2164.2
055300     PERFORM PASS.                                                SQ2164.2
055400 READ-WRITE-F1-04.                                                SQ2164.2
055500     PERFORM PRINT-DETAIL.                                        SQ2164.2
055600 READ-INIT-F1-05.                                                 SQ2164.2
055700     IF EOF-FLAG EQUAL TO 1                                       SQ2164.2
055800         GO TO  READ-EOF-06.                                      SQ2164.2
055900     MOVE ZERO TO ERROR-FLAG.                                     SQ2164.2
056000     MOVE "READ...END..." TO RE-MARK.                             SQ2164.2
056100     MOVE  "READ-TEST-F1-05" TO PAR-NAME.                         SQ2164.2
056200 READ-TEST-F1-05.                                                 SQ2164.2
056300     READ SQ-FS1 END GO TO READ-TEST-F1-05-1.                     SQ2164.2
056400     PERFORM RECORD-CHECK.                                        SQ2164.2
056500     IF WRK-CS-09V00 GREATER THAN 750                             SQ2164.2
056600          GO TO READ-TEST-F1-05-1.                                SQ2164.2
056700     GO TO READ-TEST-F1-05.                                       SQ2164.2
056800 READ-TEST-F1-05-1.                                               SQ2164.2
056900     IF ERROR-FLAG EQUAL TO ZERO                                  SQ2164.2
057000          GO TO READ-PASS-F1-05.                                  SQ2164.2
057100 READ-FAIL-F1-05.                                                 SQ2164.2
057200     MOVE "VII-12; PADDING CHARACTER"  TO RE-MARK.                SQ2164.2
057300     MOVE "ERROR IN RECORD(S)" TO COMPUTED-A.                     SQ2164.2
057400     PERFORM FAIL.                                                SQ2164.2
057500     GO TO READ-WRITE-F1-05.                                      SQ2164.2
057600 READ-PASS-F1-05.                                                 SQ2164.2
057700     PERFORM PASS.                                                SQ2164.2
057800 READ-WRITE-F1-05.                                                SQ2164.2
057900     PERFORM PRINT-DETAIL.                                        SQ2164.2
058000 READ-TEST-06.                                                    SQ2164.2
058100     IF RECORDS-IN-ERROR NOT EQUAL TO ZERO                        SQ2164.2
058200          MOVE "RECORDS IN ERROR =" TO COMPUTED-A                 SQ2164.2
058300          MOVE "VII-12; PADDING CHARACTER"  TO RE-MARK            SQ2164.2
058400          MOVE RECORDS-IN-ERROR TO CORRECT-18V0                   SQ2164.2
058500          GO TO READ-FAIL-06.                                     SQ2164.2
058600     IF WRK-CS-09V00 GREATER THAN 750                             SQ2164.2
058700     MOVE "MORE THAN 750 RECORDS; VII-12 PADDING CHARS" TO RE-MARKSQ2164.2
058800          GO TO READ-FAIL-06.                                     SQ2164.2
058900 READ-PASS-06.                                                    SQ2164.2
059000     PERFORM PASS.                                                SQ2164.2
059100     GO TO READ-WRITE-06.                                         SQ2164.2
059200 READ-EOF-06.                                                     SQ2164.2
059300     MOVE "LESS THAN 750 RECORDS" TO RE-MARK.                     SQ2164.2
059400     MOVE "RECORDS READ =" TO COMPUTED-A.                         SQ2164.2
059500     MOVE WRK-CS-09V00 TO CORRECT-18V0.                           SQ2164.2
059600 READ-FAIL-06.                                                    SQ2164.2
059700     PERFORM FAIL.                                                SQ2164.2
059800 READ-WRITE-06.                                                   SQ2164.2
059900     MOVE "READ-TEST-06 "  TO PAR-NAME.                           SQ2164.2
060000     MOVE "READ FILE SQ-FS1" TO FEATURE.                          SQ2164.2
060100     PERFORM PRINT-DETAIL.                                        SQ2164.2
060200 READ-CLOSE-003.                                                  SQ2164.2
060300     CLOSE SQ-FS1.                                                SQ2164.2
060400 TERMINATE-ROUTINE.                                               SQ2164.2
060500     EXIT.                                                        SQ2164.2
060600 CCVS-EXIT SECTION.                                               SQ2164.2
060700 CCVS-999999.                                                     SQ2164.2
060800     GO TO CLOSE-FILES.                                           SQ2164.2
