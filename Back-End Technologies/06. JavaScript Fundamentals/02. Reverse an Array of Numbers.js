function sum(n, input) {
    let reversArray= [];
    for (let index = 0; index < n; index++) {
        reversArray.unshift(input[index])
    }
    console.log(reversArray.join(" "));
}