function solve(number) {
    const numberAsString = number.toString();
    let totalsum = 0;

    for (const char of numberAsString) {
        const charAsNumber = parseInt(char, 10);
        totalsum += charAsNumber;
    }
    console.log(totalsum);
}