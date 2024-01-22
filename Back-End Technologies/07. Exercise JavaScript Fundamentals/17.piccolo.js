function manageParkingLot(input) {
    const parkingLot = new Set();
  
    for (const entry of input) {
      const [direction, carNumber] = entry.split(', ');
      if (direction === 'IN') {
        parkingLot.add(carNumber);
      } else if (direction === 'OUT') {
        parkingLot.delete(carNumber);
      }
    }
  
    const sortedCarNumbers = Array.from(parkingLot).sort();
  
    if (sortedCarNumbers.length > 0) {
      console.log(sortedCarNumbers.join('\n'));
    } else {
      console.log('Parking Lot is Empty');
    }
  }
  

  
  