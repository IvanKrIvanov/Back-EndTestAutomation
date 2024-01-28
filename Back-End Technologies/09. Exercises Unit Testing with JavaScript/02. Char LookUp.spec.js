import { lookupChar } from "./02. Char Lookup.js";
import { expect } from "chai";

describe("lookUpChar", () => {
  it("should return undefined when first parameter is from incorrect and second parameter is correct type", () => {
    // Arrange
    const incorrectFirstParam = 123;
    const correctSecondParam = 1;
    // Act
    const undefinedResult = lookupChar(incorrectFirstParam, correctSecondParam);
    // Assert
    expect(undefinedResult).to.be.undefined;
  });
  it("should return undefined when first parameter is from correct and second parameter is incorrect type", () => {
    // Arrange
    const correctFirstParam = "string";
    const incorrectSecondParam = "10";
    // Act
    const undefinedResult = lookupChar(correctFirstParam, incorrectSecondParam);
    // Assert
    expect(undefinedResult).to.be.undefined;
  });
  it("should return undefined when first parameter is from correct and second parameter is incorrect float type", () => {
    // Arrange
    const correctFirstParam = "string";
    const incorrectFloatSecondParam = 10.10;
    // Act
    const undefinedResult = lookupChar(correctFirstParam, incorrectFloatSecondParam);
    // Assert
    expect(undefinedResult).to.be.undefined;
  });
  it("should return incorrect index when first parameter is from correct and second parameter is bigget than the string length", () => {
    // Arrange
    const correctFirstParam = "string";
    const biggerLengthSecondParam = 15;
    // Act
    const incorrectResult = lookupChar(correctFirstParam, biggerLengthSecondParam);
    // Assert
    expect(incorrectResult).to.be.equal('Incorrect index');
  });
  it("should return incorrect index when first parameter is from correct and second parameter is lower than the string length", () => {
    // Arrange
    const correctFirstParam = "string";
    const lowerLengthSecondParam = -15;
    // Act
    const incorrectResult = lookupChar(correctFirstParam, lowerLengthSecondParam);
    // Assert
    expect(incorrectResult).to.be.equal('Incorrect index');
  });
  it("should return correct when all the parameters are correct", () => {
    // Arrange
    const correctFirstParam = "string";
    const correctSecondParam = 1;
    // Act
    const correctResult = lookupChar(correctFirstParam, correctSecondParam);
    // Assert
    expect(correctResult).to.be.equal('t');
  });
});
