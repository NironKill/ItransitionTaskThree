Non-transitive dice game.

If the arguments are incorrect, you must display a neat error message,not a stacktrace—what exactly is wrong and an example of how to do it right (e.g., user specified only two dice or no dice at all, used non-integers, etc.).
All messages should be in English.

Important: dice configuration is passed as command line arguments; you don't "parse" it from the input stream.

The victory is defined as follows—computer and user select different dice, perform their "throws," and whoever rolls higher wins. 

The first step of the game is to determine who makes the first move. 
You have to prove to the user that choice is fair (it's not enough to generate a random bit 0 or 1; the user needs a proof of the fair play). 

When the users make the throw, they select dice using CLI "menu" and "generate" a random value with the help of the computer. 
The options consist of all the available dice, the exit (cancel) option, and the help option.

When the computer makes the throw, it selects dice and "generates" a random value. 
 
