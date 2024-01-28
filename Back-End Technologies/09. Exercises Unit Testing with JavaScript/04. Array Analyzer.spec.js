import { analyzeArray } from "./04. Array Analyzer.js";
import { expect } from "chai";

describe("analyzeArray", () => {
  it("should return undefined when pass non-array as input", () => {
    // Arange
    const nonArrayInput = "someString";
    // Act
    const undefinedResult = analyzeArray(nonArrayInput);
    // Assert
    expect(undefinedResult).to.be.undefined;
  });
  it("should return undefined when pass empty array as input", () => {
    // Arange
    const emptyArrayInput = [];
    // Act
    const undefinedResult = analyzeArray(emptyArrayInput);
    // Assert
    expect(undefinedResult).to.be.undefined;
  });
  it("should return correct value when pass array with different numbers as input", () => {
    // Arange
    const arrayInput = [3, 5, -2, 4, 1];
    // Act
    const correctResult = analyzeArray(arrayInput);
    // Assert
    expect(correctResult).to.deep.equal({min:-2, max: 5, length: 5});
  });
  it("should return correct value when pass array with single numbers as input", () => {
    // Arange
    const arrayInput = [3];
    // Act
    const correctResult = analyzeArray(arrayInput);
    // Assert
    expect(correctResult).to.deep.equal({min:3, max: 3, length: 1});
  });
  it("should return correct value when pass array with same numbers as input", () => {
    // Arange
    const arrayInput = [7,7,7,7,7];
    // Act
    const correctResult = analyzeArray(arrayInput);
    // Assert
    expect(correctResult).to.deep.equal({min:7, max: 7, length: 5});
  });
});
