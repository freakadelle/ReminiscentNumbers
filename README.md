# ReminiscentNumbers
Smart numbers for C# which can be modified in traditional ways (+, -, *, /), but where operations can be applied on the base value and can be tracked, added, removed, applied in different orders and reverted back to origin/base value. Helpful for card games or RPGs or anything which needs stacked number modifications with memory.
<br>

## The Idea
Idea is, that a number is initialized with its base value, whereas every operation on this number can be executed on its base value and in a given order (prioritizing operators/calculation methods) while providing an easy and readable coding style. Useful when multiple instances modify an attribute like health of a character. Increasing this attribute (initial value = 100) by 10% 3 times in a row would result in 133.1f (It should be 130 because its 3 times 100*0.1). This happens, because the health-attribute-number don't know where it originates from (It has no memory) and the modifying instances should'nt need to know what the base value of this attribute is...This is not their job. You could track the base value of eacht attribute separately in each class and recalculate them in a static way or providing each class with a "IncreaseHealth"-Method-Wrapper, but this is not the intended way IMHO. Dealing with 1 attribute instead of 2 seems better + you gain better control over the whole calculation-system and the modification-history.
For example: 

```csharp
Member health = 100;                           //Initialize Member with basevalue 100

health.Modify(ModType.MULTIPLY, 1.1);   //Multiply by 10% from base
Debug.Log(health);                      //Base: 100  current: 110

health.Modify(ModType.MULTIPLY, 1.1);   //Multiply by 10% from base
Debug.Log(health);                      //Base: 100  current: 120

health.Modify(ModType.MULTIPLY, 1.1);   //Multiply by 10% from base
Debug.Log(health);                      //Base: 100  current: 130

health += 100;                          //Increase base Value by another 100
Debug.Log(health);                      //Base: 200  current: 260

health.Modify(ModType.ADD, 40);         //Add 40
Debug.Log(health);                      //Base: 200  current: 300

health /= 10;                           //Divide Base Value by 10
Debug.Log(health);                      //Base: 20  current: 66
```

## Design Goals
* Should be accessible and easy to use like other primitive number types
* No need to initialize or call constructor

## Problems
* classes needs initialization/constructor call. Try to replace constructor call by implicit operator for more intuitive usage
* struct would be great as it is always initialized with default values. But comes with some greater handicaps like large memory allocation for callByVal passes.
* struct also needs initialization for nullable types
* when RemNumbers are definded in classes, they have to be initialized. They can be initialized via implicit inline field-initialization (which is okay but not perfect). But if RemNumbers are defined in structs, they will be null as they cannot be field-initialized... what is bad
* There is no generic type limitation for number types. So for now it is defined as long. Hope to make it generic some time
* combine custom calculation order (single calculations gets prios) and operator-bound calculation order (+-*/ gets prios), when modifying RemNumbers.
* when allowing remNumbers to be initialized via implicit operator, it could be critical because all previous operations/modifications gets lost.
* Fields are not updated in Unity Inspector when changed. Need somehow to update them maunally?!

## Changelogs

### 0.2
#### New Features
* ToString now delivers base and current value
* Implicit / Explicit Type Conversion to float, int, byte. So you can now throw RemNumbers in common number methods-parameters
* Field initialization possible
* Base and current value now visible in unity Inspector. current value non-editable
### changes
* operators now modify the base value of RemNumbers
* RemNumber now called Member
#### Bugs fixed
* modifications List does not throw Nullref anymore when struct is not initialized

### 0.1 
* First simple test class for long data type
* ModificationTypes for Add, Substract, Multiply, Divide
* Modification Dictionary to keep track of operations
* Execution order Array for operation execution order
* Dictionary of modifications to keep track of all operations
* basic operators + - * /
* IEquatable Interface implemented



