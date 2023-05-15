module MyApp.Example

open System

let isLeapYear (y: int) : bool =
    (y % 4 = 0) && ((y % 100 <> 0) || (y % 400 = 0))

let testYears =
    [ (1988, true)
      (1992, true)
      (1996, true)
      (1600, true)
      (1700, false)
      (1800, false)
      (1900, false)
      (2000, true)
      (2100, false)
      (2200, false)
      (2300, false)
      (2400, true)
      (2500, false)
      (2600, false) ]

List.map (fun (y, b) -> isLeapYear y = b) testYears


let nextLeapYear (y: int) : int =
    let mutable y' = y + 1

    while (isLeapYear >> not) y' do
        y' <- y' + 1

    y'



nextLeapYear 1696

let years: int -> int seq = Seq.unfold (fun y -> Some(y, y + 1))

let nextLeapYear': int -> int = ((+) 1) >> years >> Seq.find isLeapYear

nextLeapYear' 1696

let leapYears = years >> Seq.filter isLeapYear

leapYears 1690 |> Seq.take 5
// [1692; 1696; 1704; 1708; 1712]

let rec quicksort =
    function
    | [] -> []
    | x :: xs ->
        xs
        |> List.partition (fun i -> i < x)
        ||> fun left right -> (quicksort left) @ [ x ] @ (quicksort right)

seq { for i in 1..100 -> i * i }

seq { 1..10 } |> Seq.map ((+) 1)


let rec allAdd1 xs =
    match xs with
    | [] -> []
    | x :: xs -> [ x + 1 ] @ allAdd1 xs

let rec allSub1 xs =
    match xs with
    | [] -> []
    | x :: xs -> [ x - 1 ] @ allSub1 xs

seq { 1..10 } |> List.ofSeq |> allAdd1


seq { 1..10 } |> List.ofSeq |> allSub1

let rec myMap f xs =
    match xs with
    | [] -> []
    | x :: xs -> [ f x ] @ myMap f xs

seq { 1..10 }
|> List.ofSeq
|> myMap (fun i -> i * i)
