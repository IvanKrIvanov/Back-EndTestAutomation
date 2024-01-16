function solve(typeOfFruit, weight, pricePerKg) {
    const pricePerGr = pricePerKg / 1000
    const totalPrice = weight * pricePerGr
    const weightInKg = weight / 1000

    console.log(`I need $${totalPrice.toFixed(2)} to buy ${weightInKg.toFixed(2)} kilograms ${typeOfFruit}.`);
}