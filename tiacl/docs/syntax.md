# Syntax

### Variables

Variables are immutable and can be declared by using ``decvar``. There are three valid types: ``int``, ``string``, and
``bool``.

Example:
```
decvar a_string = "This is a string";
decvar an_int = 21;
decvar a_bool = false;
```

Integers can be any 32 bit number and booleans can only be ``true`` ``false``.

### Functions

Functions are declared using ``decfun`` and can contain arguments.

Example:
```
decfun a_function(decvar passed_int :int) {
	// code
}
```

Arguments need to have a defined type.

#### Calling Functions

Calling functions can be done by using the function name followed by parentheses with any arguments in between them.
Any builtin function needs to start and end with an exclaimation mark (``!``).

Example:
```
a_function(0384750348); // Function in other example
!display("This is text")! // Print function
```

If a function is being called with a variable as a value, that variable name must be the same as the defined argument
name.

Example:
```
decfun a_function(decvar very_specific_name :string) {
	!display(very_specific_name)!
}

decfun main() {
	decvar very_specific_name = "some value";
	a_function(a_very_specific_name);
}
```

### If/else statements

Does not work.

### Math

Only two numbers can be used in a math opperation, ``+`` is used for adding, ``-`` is used for subtraction, ``*`` is
used for multiplication, ``/`` is used for subtraction.

Example:
```
1 + 3
```
