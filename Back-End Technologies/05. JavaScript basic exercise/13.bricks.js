function solve(numberOfBricks, numberOfWokers, cartCapacity) {
    
    const totalBricksPerTrip = numberOfWokers * cartCapacity

    const numberOfTrips = Math.ceil(numberOfBricks / totalBricksPerTrip)

    console.log(numberOfTrips);

}
