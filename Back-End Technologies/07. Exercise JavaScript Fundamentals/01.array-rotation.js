function solve(inputArray, numberOfRotation) {

  for (let index = 0; index < numberOfRotation; index++) {
    const firstElement = inputArray.shift()
    inputArray.push(firstElement)

  }
  console.log(inputArray.join(' '));
}