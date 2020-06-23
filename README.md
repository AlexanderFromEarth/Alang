# My Lang

## Types

- number: 0, 0., .0, 1e-12
- string: '', ""
- boolean: true, false
- functions: () => {}
- collection: [1, 2]
- record: {a: 1, b: 2}
- tuple: (1, 2)
- type: integer, float

## Operators

- :=, =
- =>
- |>
- |
- ==, <>, <(=), >(=)
- +, -, \*, /, %
- ||, &&, !
- (), x()

## Example

1. x := 1 # variable declaration
2. x = 1 # constant (read-only) declaration
3. print(x) # call a function
4. func1 \* func2 # function composition
5. 1 |> func1 # pipeline
6. #this is commentary
7. x := 1 # variable assigment
8. 1 / 2 # retunrs value that represent 0.5f
9. x: # pattern matching

   | 1 -> print(2) # value check

   | int -> print(x) # type check

   | 1, int -> print(x + 1) # combine conditions with ','

10. x = {

    a: 0,

    b: 1,

    ['a']: 3,

    ...y

    }

11. x = (1, 2, 3) # like in python
12. x = [1, 2, 3] # like in python, js
13. x = [1..3] # like in F#
