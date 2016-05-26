# EquationToCanonical

An application for transforming equation into canonical form. An equation can be of any order. It may contain any amount of variables and brackets.

The equation will be given in the following form:

  P1 + P2 + ... = ... + PN

where P1..PN - summands, which look like: 

  ax^k

where a - floating point value;
k - integer value;
x - variable (each summand can have many variables).
 
For example:
x^2 + 3.5xy + y = y^2 - xy + y

Should be transformed into:
x^2 - y^2 + 4.5xy = 0

The application is a C# console application with support of two modes of operation: “file” and “interactive”. In interactive mode application prompts user to enter equation and displays result on enter.  This is repeated until user presses Ctrl+C. In the file mode application processes a file specified as parameter and writes the output into separate file with “.out” extension. Input file contains lines with equations, each equation on separate line. 
