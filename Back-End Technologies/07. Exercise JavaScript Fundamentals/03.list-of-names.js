function solve(listOfNames) {
  listOfNames.sort((a, b) => a.localeCompare(b))

  for (let index = 1; index <= listOfNames.length; index++) {
    console.log(`${index}.${listOfNames[index - 1]}`);
  }
}
