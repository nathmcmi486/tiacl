# Tiacl Command

Running the ``tiacl`` command with the ``--help`` or ``-h`` argument will display a list of argument options. This is
it's output:

```command line
TIACL command options:
--help or -h:
        Displays this message
-i <file_name>.tic
        Start interpreting program
-c <file_name>.tic
        Start compiling program
```

The ``-c`` argument does not do anything. Tiacl files (``.tic``) can only be interpreted, example:
```command line
./tiacl -i some_file.tic
```
