function solve(text, word) {
  let censored = text.replace(word, repeat(word));

  while (censored.includes(word)) {
    censored = censored.replace(word, repeat(word));
  }

  return censored;
  function repeat(word) {
    return "*".repeat(word.length);
  }
}
