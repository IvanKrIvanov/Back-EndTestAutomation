import { createCalculator } from "./04. Add-Subtract.js";
import { expect } from "chai";

describe("createCalculator", () => {
  it("should return zero if no operation are executed on the calculator", () => {
    // Arrange
    const calculator = createCalculator();
    // Act
    const result = calculator.get();
    // Assert
    expect(result).to.equals(0);
  });
  it("should return a negative number if only substract operations are executed with positive numbers on the calculator", () => {
    // Arrange
    const calculator = createCalculator();
    // Act
    calculator.subtract(3);
    calculator.subtract(3);
    calculator.subtract(10);
    const result = calculator.get();
    // Assert
    expect(result).to.equals(-16);
  });
  it("should return a positive number if only add operations are executed with positive numbers on the calculator", () => {
    // Arrange
    const calculator = createCalculator();
    // Act
    calculator.add(2);
    calculator.add(2);
    calculator.add(100);
    const result = calculator.get();
    // Assert
    expect(result).to.equals(104);
  });
  it("should handle number as strings", () => {
    // Arrange
    const calculator = createCalculator();
    // Act
    calculator.add("2");
    calculator.add("2");
    calculator.add("100");
    calculator.subtract("104");
    const result = calculator.get();
    // Assert
    expect(result).to.equals(0);
  });
  it("should handle a mix or operations", () => {
    // Arrange
    const calculator = createCalculator();
    // Act
    calculator.add(2);
    calculator.add(2);
    calculator.add(100);
    calculator.add(-4);
    calculator.subtract(-100);
    calculator.subtract(200);
    const result = calculator.get();
    // Assert
    expect(result).to.equals(0);
  });
});
