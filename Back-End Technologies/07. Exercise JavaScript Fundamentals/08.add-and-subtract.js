function solve(firstNumber, secondNumber, thirdNumber) {

  const sum = (first, second) => first + second;
  const substract = (first, second) => first - second;

  const result = substract(sum(firstNumber, secondNumber), thirdNumber);

  console.log(result);
}
