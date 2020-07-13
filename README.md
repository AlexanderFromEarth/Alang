# My Lang

## Types

- [ ] number: 0, 0., .0, 1e-12 (part impl)
- [ ] string: '', "" (not impl)
- [x] boolean: true, false (impl)
- [x] functions: () => ... (impl)
- [ ] collection: [1, 2] (not impl)
- [ ] record: {a: 1, b: 2} (not impl)
- [ ] tuple: (1, 2) (not impl)
- [ ] type: integer, float

## Operators

- [x] = (impl)
- [x] => (impl)
- [x] |> (impl)
- [ ] | (not impl)
- [x] ==, !=, <(=), >(=) (impl)
- [ ] +, -, \*, /, % (part impl)
- [x] ||, &&, ! (impl)
- [x] (), x() (impl)

## Example

1. x = 1 # constant (read-only) declaration
2. print(x) # call a function
3. func1 \* func2 # function composition
4. 1 |> func1 # pipeline
5. #this is commentary
6. x := 1 # variable assigment
7. 1 / 2 # retunrs value that represent 0.5f
8. x: # pattern matching

   | 1 -> print(2) # value check

   | int -> print(x) # type check

   | 1, int -> print(x + 1) # combine conditions with ','

9. x = {

   a: 0,

   b: 1,

   ['a']: 3,

   ...y

   }

10. x = (1, 2, 3) # like in python
11. x = [1, 2, 3] # like in python, js
12. x = [1..3] # like in F#
