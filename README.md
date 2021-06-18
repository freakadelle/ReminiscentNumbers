# ReminiscentNumbers
Smart numbers for C# which can be modified in traditional ways (+, -, *, /), but where all operations apply on the base value and can be tracked, added, removed, applied in different orders and reverted back to origin. Helpful for card games or RPGs or anything which needs stacked number modifications e.g.
<br>
<br>
Idea is, that a number is initialized with its base value, whereas every operation on this number is executed on its base value and in a given order while providing an easy and readable coding. Useful when multiple instances have an impact on an attribute like health of a character. Increasing a health of 100 by 10% 3 times in a row would results in 133.1 because it stacks up. What I want, is to increase 3 times 10% of the base value of 100, which results in 130.
For example: 

```csharp
smartNumber = 100; //100 - initialize base value
smartNumber *= 1.1f; //110 - add modification operation increase 10%
smartNumber *= 1.1f; //120 - add another 10% depending on the base value
smartNumber += 1.1f; //130 - ....
smartNumber += 70; //200 Add 70
smartNumber *= 1.1f; //210 - another 10% is another 10 value
smartNumber.SetBaseValue(10); //91.3 Reset the base value from 100 to 10 but keep track of all other operations
smartNumber.RemoveAllModifications(); //100 - Return to base value 100
```
<br>

0.1 
* First simple test class for long data type
* ModificationTypes for Add, Substract, Multiply, Divide
* Modification Dictionary to keep track of operations
* Execution order Array for operation execution order
* Dictionary of modifications to keep track of all operations
* basic operators + - * /
* IEquatable

