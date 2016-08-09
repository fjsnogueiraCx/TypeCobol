﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeCobol.Compiler.Scanner
{
    // WARNING : both enumerations (families / types) must stay in sync
    // WARNING : make sure to update the tables in TokenUtils if you add one more token family or one more token type

    public enum TokenFamily
    {
        //          0 : Error
        Invalid=0,
        //   1 ->   4 : Whitespace
        // p46: The separator comma and separator semicolon can
        // be used anywhere the separator space is used.
        Whitespace=1,
        //   5 ->   6 : Comments
        Comments=5,
        // 7 ->  11 : Separators - Syntax
        // -1 (EndOfFile)
        SyntaxSeparator=7,
        //  12 ->  16 : Special character word - Arithmetic operators
        ArithmeticOperator=12,
        //  17 ->  21 : Special character word - Relational operators
        RelationalOperator=17,
        //  22 ->  27 : Literals - Alphanumeric
        AlphanumericLiteral=22,
        //  28 ->  30 : Literals - Numeric 
        NumericLiteral=28,
        //  31 ->  33 : Literals - Syntax tokens
        SyntaxLiteral=31,
        //  34 ->  37 : Symbols
        Symbol=34,
        //  38 ->  56 : Keywords - Compiler directive starting tokens
        CompilerDirectiveStartingKeyword=38,
        //  57 ->  79 : Keywords - Code element starting tokens
        CodeElementStartingKeyword=57,
        //  80 ->  120: Keywords - Statement starting tokens
        StatementStartingKeyword=80,
        //  121 -> 141 : Keywords - Statement ending tokens
        StatementEndingKeyword=121,
        // 142 -> 172 : Keywords - Special registers
        SpecialRegisterKeyword=142,
        // 173 -> 186 : Keywords - Figurative constants
        FigurativeConstantKeyword=173,
        // 187 -> 188 : Keywords - Special object identifiers
        SpecialObjetIdentifierKeyword=187,
        // 189 -> 457 : Keywords - Syntax tokens
        SyntaxKeyword=189,
        // 459 -> 461 : Compiler directives
        CompilerDirective = 459,
        // 462 -> end : Internal token groups - used by the preprocessor only
        InternalTokenGroup = 462
    }

    public enum TokenType
    {
        // Error
        InvalidToken=0,
        // Separators - Whitespace
        SpaceSeparator=1,
        CommaSeparator=2,
        SemicolonSeparator=3,
        // Comments
        FloatingComment=5,
        CommentLine=6,
        // Separators - Syntax
        EndOfFile = -1,  // End of file => value -1 is expected by Antlr
        PeriodSeparator=7,
        ColonSeparator=8,
        LeftParenthesisSeparator=9,
        RightParenthesisSeparator=10,
        PseudoTextDelimiter = 11,
        // Special character word - Arithmetic operators
        PlusOperator=12,
        MinusOperator=13,
        DivideOperator=14,
        MultiplyOperator=15,
        PowerOperator=16,
        // Special character word - Relational operators
        LessThanOperator=17,
        GreaterThanOperator=18,
        LessThanOrEqualOperator=19,
        GreaterThanOrEqualOperator=20,
        EqualOperator=21,
        // Literals - Alphanumeric
        AlphanumericLiteral = 22,
        HexadecimalAlphanumericLiteral = 23,
        NullTerminatedAlphanumericLiteral = 24,
        NationalLiteral = 25,
        HexadecimalNationalLiteral = 26,
        DBCSLiteral = 27,
        // Literals - Numeric 
        IntegerLiteral = 28,
        DecimalLiteral = 29,
        FloatingPointLiteral = 30,
        // Literals - Syntax tokens
        PictureCharacterString = 31,
        CommentEntry = 32,
        ExecStatementText = 33,
        // Symbols
        FunctionName = 34,
        ExecTranslatorName = 35,
        PartialCobolWord = 36,
        UserDefinedWord = 37,
        // Keywords - Compiler directive starting tokens
        ASTERISK_CBL = 38,
        ASTERISK_CONTROL = 39,
        BASIS = 40,
        CBL = 41,
        COPY = 42,
        DELETE_CD = 43,
        EJECT = 44,
        ENTER = 45,
        EXEC_SQL_INCLUDE = 46,
        INSERT = 47,
        PROCESS = 48,
        READY = 49,
        RESET = 50,
        REPLACE = 51,
        SERVICE = 52,
        SKIP1 = 53,
        SKIP2 = 54,
        SKIP3 = 55,
        TITLE = 56,
        // Keywords - Code element starting tokens
        APPLY = 57,
        CONFIGURATION = 58,
        ELSE = 59,
        ENVIRONMENT = 60,
        FD = 61,
        FILE_CONTROL = 62,
        I_O_CONTROL = 63,
        ID = 64,
        IDENTIFICATION = 65,
        INPUT_OUTPUT = 66,
        LINKAGE = 67,
        LOCAL_STORAGE = 68,
        MULTIPLE = 69,
        OBJECT_COMPUTER = 70,
        REPOSITORY = 71,
        RERUN = 72,
        SAME = 73,
        SD = 74,
        SELECT = 75,
        SOURCE_COMPUTER = 76,
        SPECIAL_NAMES = 77,
        USE = 78,
        WORKING_STORAGE = 79,
        // Keywords - Statement starting tokens
        ACCEPT = 80,
        ADD = 81,
        ALTER = 82,
        CALL = 83,
        CANCEL = 84,
        CLOSE = 85,
        COMPUTE = 86,
        CONTINUE = 87,
        DELETE = 88,
        DISPLAY = 89,
        DIVIDE = 90,
        ENTRY = 91,
        EVALUATE = 92,
        EXEC = 93,
        EXECUTE = 94,
        EXIT = 95,
        GOBACK = 96,
        GO = 97,
        IF = 98,
        INITIALIZE = 99,
        INSPECT = 100,
        INVOKE = 101,
        MERGE = 102,
        MOVE = 103,
        MULTIPLY = 104,
        OPEN = 105,
        PERFORM = 106,
        READ = 107,
        RELEASE = 108,
        RETURN = 109,
        REWRITE = 110,
        SEARCH = 111,
        SET = 112,
        SORT = 113,
        START = 114,
        STOP = 115,
        STRING = 116,
        SUBTRACT = 117,
        UNSTRING = 118,
        WRITE = 119,
        XML = 120,
        // Keywords - Statement ending tokens
        END_ADD = 121,
        END_CALL = 122,
        END_COMPUTE = 123,
        END_DELETE = 124,
        END_DIVIDE = 125,
        END_EVALUATE = 126,
        END_EXEC = 127,
        END_IF = 128,
        END_INVOKE = 129,
        END_MULTIPLY = 130,
        END_PERFORM = 131,
        END_READ = 132,
        END_RETURN = 133,
        END_REWRITE = 134,
        END_SEARCH = 135,
        END_START = 136,
        END_STRING = 137,
        END_SUBTRACT = 138,
        END_UNSTRING = 139,
        END_WRITE = 140,
        END_XML = 141,
        // Keywords - Special registers
        ADDRESS = 142,
        DEBUG_CONTENTS = 143,
        DEBUG_ITEM = 144,
        DEBUG_LINE = 145,
        DEBUG_NAME = 146,
        DEBUG_SUB_1 = 147,
        DEBUG_SUB_2 = 148,
        DEBUG_SUB_3 = 149,
        JNIENVPTR = 150,
        LENGTH = 151,
        LINAGE_COUNTER = 152,
        RETURN_CODE = 153,
        SHIFT_IN = 154,
        SHIFT_OUT = 155,
        SORT_CONTROL = 156,
        SORT_CORE_SIZE = 157,
        SORT_FILE_SIZE = 158,
        SORT_MESSAGE = 159,
        SORT_MODE_SIZE = 160,
        SORT_RETURN = 161,
        TALLY = 162,
        WHEN_COMPILED = 163,
        XML_CODE = 164,
        XML_EVENT = 165,
        XML_INFORMATION = 166,
        XML_NAMESPACE = 167,
        XML_NAMESPACE_PREFIX = 168,
        XML_NNAMESPACE = 169,
        XML_NNAMESPACE_PREFIX = 170,
        XML_NTEXT = 171,
        XML_TEXT = 172,
        // Keywords - Figurative constants
        HIGH_VALUE = 173,
        HIGH_VALUES = 174,
        LOW_VALUE = 175,
        LOW_VALUES = 176,
        NULL = 177,
        NULLS = 178,
        QUOTE = 179,
        QUOTES = 180,
        SPACE = 181,
        SPACES = 182,
        ZERO = 183,
        ZEROES = 184,
        ZEROS = 185,
        SymbolicCharacter = 186,
        // Keywords - Special object identifiers
        SELF = 187,
        SUPER = 188,
        // Keywords - Syntax tokens
        ACCESS = 189,
        ADVANCING = 190,
        AFTER = 191,
        ALL = 192,
        ALPHABET = 193,
        ALPHABETIC = 194,
        ALPHABETIC_LOWER = 195,
        ALPHABETIC_UPPER = 196,
        ALPHANUMERIC = 197,
        ALPHANUMERIC_EDITED = 198,
        ALSO = 199,
        ALTERNATE = 200,
        AND = 201,
        ANY = 202,
        ARE = 203,
        AREA = 204,
        AREAS = 205,
        ASCENDING = 206,
        ASSIGN = 207,
        AT = 208,
        ATTRIBUTE = 209,
        ATTRIBUTES = 210,
        AUTHOR = 211,
        BEFORE = 212,
        BEGINNING = 213,
        BINARY = 214,
        BLANK = 215,
        BLOCK = 216,
        BOTTOM = 217,
        BY = 218,
        CHARACTER = 219,
        CHARACTERS = 220,
        CLASS = 221,
        CLASS_ID = 222,
        COBOL = 223,
        CODE = 224,
        CODE_SET = 225,
        COLLATING = 226,
        COM_REG = 227,
        COMMA = 228,
        COMMON = 229,
        COMP = 230,
        COMP_1 = 231,
        COMP_2 = 232,
        COMP_3 = 233,
        COMP_4 = 234,
        COMP_5 = 235,
        COMPUTATIONAL = 236,
        COMPUTATIONAL_1 = 237,
        COMPUTATIONAL_2 = 238,
        COMPUTATIONAL_3 = 239,
        COMPUTATIONAL_4 = 240,
        COMPUTATIONAL_5 = 241,
        CONTAINS = 242,
        CONTENT = 243,
        CONVERTING = 244,
        CORR = 245,
        CORRESPONDING = 246,
        COUNT = 247,
        CURRENCY = 248,
        DATA = 249,
        DATE = 250,
        DATE_COMPILED = 251,
        DATE_WRITTEN = 252,
        DAY = 253,
        DAY_OF_WEEK = 254,
        DBCS = 255,
        DEBUGGING = 256,
        DECIMAL_POINT = 257,
        DECLARATIVES = 258,
        DELIMITED = 259,
        DELIMITER = 260,
        DEPENDING = 261,
        DESCENDING = 262,
        DISPLAY_ARG = 263,
        DISPLAY_1 = 264,
        DIVISION = 265,
        DOWN = 266,
        DUPLICATES = 267,
        DYNAMIC = 268,
        EBCDIC = 269,
        EGCS = 270,
        ELEMENT = 271,
        ENCODING = 272,
        END = 273,
        END_OF_PAGE = 274,
        ENDING = 275,
        ENTRY_ARG = 276,
        EOP = 277,
        EQUAL = 278,
        ERROR = 279,
        EVERY = 280,
        EXCEPTION = 281,
        EXTEND = 282,
        EXTERNAL = 283,
        FACTORY = 284,
        FALSE = 285,
        FILE = 286,
        FILLER = 287,
        FIRST = 288,
        FOOTING = 289,
        FOR = 290,
        FROM = 291,
        FUNCTION = 292,
        FUNCTION_POINTER = 293,
        GENERATE = 294,
        GIVING = 295,
        GLOBAL = 296,
        GREATER = 297,
        GROUP_USAGE = 298,
        I_O = 299,
        IN = 300,
        INDEX = 301,
        INDEXED = 302,
        INHERITS = 303,
        INITIAL = 304,
        INPUT = 305,
        INSTALLATION = 306,
        INTO = 307,
        INVALID = 308,
        IS = 309,
        JUST = 310,
        JUSTIFIED = 311,
        KANJI = 312,
        KEY = 313,
        LABEL = 314,
        LEADING = 315,
        LEFT = 316,
        LESS = 317,
        LINAGE = 318,
        LINE = 319,
        LINES = 320,
        LOCK = 321,
        MEMORY = 322,
        METHOD = 323,
        METHOD_ID = 324,
        MODE = 325,
        MODULES = 326,
        MORE_LABELS = 327,
        NAME = 328,
        NAMESPACE = 329,
        NAMESPACE_PREFIX = 330,
        NATIONAL = 331,
        NATIONAL_EDITED = 332,
        NATIVE = 333,
        NEGATIVE = 334,
        NEW = 335,
        NEXT = 336,
        NO = 337,
        NONNUMERIC = 338,
        NOT = 339,
        NUMERIC = 340,
        NUMERIC_EDITED = 341,
        OBJECT = 342,
        OCCURS = 343,
        OF = 344,
        OFF = 345,
        OMITTED = 346,
        ON = 347,
        OPTIONAL = 348,
        OR = 349,
        ORDER = 350,
        ORGANIZATION = 351,
        OTHER = 352,
        OUTPUT = 353,
        OVERFLOW = 354,
        OVERRIDE = 355,
        PACKED_DECIMAL = 356,
        PADDING = 357,
        PAGE = 358,
        PARSE = 359,
        PASSWORD = 360,
        PIC = 361,
        PICTURE = 362,
        POINTER = 363,
        POSITION = 364,
        POSITIVE = 365,
        PROCEDURE = 366,
        PROCEDURE_POINTER = 367,
        PROCEDURES = 368,
        PROCEED = 369,
        PROCESSING = 370,
        PROGRAM = 371,
        PROGRAM_ID = 372,
        RANDOM = 373,
        RECORD = 374,
        RECORDING = 375,
        RECORDS = 376,
        RECURSIVE = 377,
        REDEFINES = 378,
        REEL = 379,
        REFERENCE = 380,
        REFERENCES = 381,
        RELATIVE = 382,
        RELOAD = 383,
        REMAINDER = 384,
        REMOVAL = 385,
        RENAMES = 386,
        REPLACING = 387,
        RESERVE = 388,
        RETURNING = 389,
        REVERSED = 390,
        REWIND = 391,
        RIGHT = 392,
        ROUNDED = 393,
        RUN = 394,
        SECTION = 395,
        SECURITY = 396,
        SEGMENT_LIMIT = 397,
        SENTENCE = 398,
        SEPARATE = 399,
        SEQUENCE = 400,
        SEQUENTIAL = 401,
        SIGN = 402,
        SIZE = 403,
        SORT_ARG = 404,
        SORT_MERGE = 405,
        SQL = 406,
        SQLIMS = 407,
        STANDARD = 408,
        STANDARD_1 = 409,
        STANDARD_2 = 410,
        STATUS = 411,
        SUPPRESS = 412,
        SYMBOL = 413,
        SYMBOLIC = 414,
        SYNC = 415,
        SYNCHRONIZED = 416,
        TALLYING = 417,
        TAPE = 418,
        TEST = 419,
        THAN = 420,
        THEN = 421,
        THROUGH = 422,
        THRU = 423,
        TIME = 424,
        TIMES = 425,
        TO = 426,
        TOP = 427,
        TRACE = 428,
        TRAILING = 429,
        TRUE = 430,
        TYPE = 431,
        UNBOUNDED = 432,
        UNIT = 433,
        UNTIL = 434,
        UP = 435,
        UPON = 436,
        USAGE = 437,
        USING = 438,
        VALIDATING = 439,
        VALUE = 440,
        VALUES = 441,
        VARYING = 442,
        WHEN = 443,
        WITH = 444,
        WORDS = 445,
        WRITE_ONLY = 446,
        XML_DECLARATION = 447,
        XML_SCHEMA = 448,
        YYYYDDD = 449,
        YYYYMMDD = 450,
// [Cobol 2002]
        TYPEDEF = 451,
        STRONG = 452,
// [/Cobol 2002]
// [TYPECOBOL]
		DECLARE = 453,
		END_DECLARE = 454,
		UNSAFE = 455,
		PUBLIC = 456,
		PRIVATE = 457,
		INOUT = 458,
// [/TYPECOBOL]
        // Group of tokens produced by the preprocessor
        // - compiler directives
        CompilerDirective = 459,
        CopyImportDirective = 460,
        ReplaceDirective = 461,
        // - internal token groups -> used by the preprocessor only
        ContinuationTokenGroup = 462
    }
}
