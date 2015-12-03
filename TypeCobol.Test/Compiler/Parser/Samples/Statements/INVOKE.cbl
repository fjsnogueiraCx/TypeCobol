﻿INVOKE identifier x.
INVOKE SELF x.
INVOKE SUPER 'value'.
INVOKE classname NEW.
* USING
INVOKE x y USING BY VALUE a.
INVOKE x y USING VALUE 1 a 'str' VALUE LENGTH OF b c.
* RETURNING
INVOKE x y RETURNING a.
* both USING and RETURNING
INVOKE x y USING BY VALUE a RETURNING b.
* END-INVOKE
INVOKE SELF x END-INVOKE.
INVOKE x y USING BY VALUE a END-INVOKE.
INVOKE x y RETURNING a END-INVOKE.
INVOKE x y USING BY VALUE a RETURNING b END-INVOKE.
*   ////////////
*  // ERRORS
* only alphanumeric or national literal
INVOKE x 42
* only one USING
INVOKE x y USING VALUE a USING VALUE b
* only one RETURNING
INVOKE x y RETURNING a RETURNING b
* no reference modification in RETURNING
INVOKE x y RETURNING a(1:2)
* only one identifier in RETURNING
INVOKE x y RETURNING a(1) b(2)
* USING and RETURNING inverted
INVOKE x y RETURNING a USING BY VALUE b