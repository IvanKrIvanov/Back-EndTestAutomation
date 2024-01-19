function solve(input) {
    let uniqueName = {};

    input.forEach(element => {
        let keyValuePair = element.split(" ")
        let name = keyValuePair[0]
        let number = keyValuePair[1]
        uniqueName[name] = [number]
    });

    for (let key in uniqueName){
        console.log(`${key} -> ${uniqueName[key]}`);
    }
}