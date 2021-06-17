# ReminiscentNumbers
Smart numbers for C# which can be modified in traditional ways (+, -, *, /), but where all operations can be tracked, added, removed, applied in different orders and reverted back to origin. Helpful for card games or RPGs or anything which needs stacked number modifications e.g.

Idea is, that a number is initialized with its base value, whereas every operation on this number is executed in a given order while providing an easy and readable coding.
For example: 
smartNumber = 10; //10 - initialize base value
smartNumber *= 2; //20 - add modification operation Multiply by 2
smartNumber += 2; //24 - add modification operation Add 2, but which is calculated before the multiply operation (Depends on the defined operations execution order)
smartNumber++; //26
smartNumber.RemoveAllModifications(); //10 - Return to base value 10

0.1
First simple test class for long data type
ModificationTypes for Add, Substract, Multiply, Divide
Modification Dictionary to keep track of operations
Execution order Array for operation execution order
Dictionary of modifications to keep track of all operations
basic operators + - * /
IEquatable

