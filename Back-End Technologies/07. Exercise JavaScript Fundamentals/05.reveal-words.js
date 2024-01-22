function solve(words, template) {
  const separatedWords = words.split(", ");

  for (const separatedWord of separatedWords) {
    template = template.replace(
      "*".repeat(separatedWord.length),
      separatedWord
    );
  }
  console.log(template);
}
