function solve(wordToSearch, textForSearching) {
  const hasWord = textForSearching
    .toLowerCase()
    .split(" ")
    .includes(wordToSearch);

  if (hasWord) {
    console.log(wordToSearch);
  } else {
    console.log(`${wordToSearch} not found!`);
  }
}
