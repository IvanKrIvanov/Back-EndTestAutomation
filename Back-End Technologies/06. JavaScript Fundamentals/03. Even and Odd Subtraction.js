function solve(input) {
  let evenSum = 0;
  let oddSum = 0;

  for (let index = 0; index < input.length; index++) {
    let currentNumber = Number(input[index])
    if (currentNumber % 2 == 0) {
      evenSum += currentNumber;
    } else if (currentNumber % 2 != 0) {
      oddSum += currentNumber;
    }
  }
  console.log(evenSum - oddSum);
}
