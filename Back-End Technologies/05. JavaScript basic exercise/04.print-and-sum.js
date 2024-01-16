function solve(startNumber, endNumber) {
    
    let message = "";
    
    let sum = 0;

    for(let i = startNumber; i <= endNumber; i += 1){
        sum += i;
        message += `${i} `
    }

    console.log(message.trimEnd);
    console.log(`Sum: ${sum}`);
}