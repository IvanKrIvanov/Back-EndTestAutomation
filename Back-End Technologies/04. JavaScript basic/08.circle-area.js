function solve(input) {
    `use stricts`;

    const typeOfParam = typeof input;

    if (typeOfParam === 'number') {
        const area = input ** 2 * Math.PI;
        console.log(area.toFixed(2));
    }else{
        console.log(`We can not calculate the circle area, because we receive a ${typeOfParam}.`);
    }
}
solve (3)