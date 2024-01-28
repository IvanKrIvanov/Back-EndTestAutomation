import { mathEnforcer } from "./03. Math Enforcer.js";
import { expect } from "chai";

describe("mathEnforcer", () => {
  describe("addFive", () => {
    it("should return undefined when pass string as input", () => {
      // Arange
      const stringInput = "someString";
      // Act
      const undefinedResult = mathEnforcer.addFive(stringInput);
      // Assert
      expect(undefinedResult).to.be.undefined;
    });
    it("should return undefined when pass undefined as input", () => {
      // Arange
      const undefinedInput = "5";
      // Act
      const undefinedResult = mathEnforcer.addFive(undefinedInput);
      // Assert
      expect(undefinedResult).to.be.undefined;
    });
    it("should return undefined when pass number as string as input", () => {
      // Arange
      const numberAsStringInput = undefined;
      // Act
      const undefinedResult = mathEnforcer.addFive(numberAsStringInput);
      // Assert
      expect(undefinedResult).to.be.undefined;
    });
    it("should return correct result when pass floating number as inputand assert with close.to", () => {
      // Arange
      const floatingInput = 1.01;
      // Act
      const correctResult = mathEnforcer.addFive(floatingInput);
      // Assert
      expect(correctResult).to.be.closeTo(6.01, 0.01);
    });
    it("should return correct result when pass floating number as input and assert with equal", () => {
      // Arange
      const floatingInput = 1.01;
      // Act
      const correctResult = mathEnforcer.addFive(floatingInput);
      // Assert
      expect(correctResult).to.be.equal(6.01);
    });
    it("should return correct result when pass floating number with a lot of digits as input and assert with close.to", () => {
      // Arange
      const floatingInput = 1.00000001;
      // Act
      const correctResult = mathEnforcer.addFive(floatingInput);
      // Assert
      expect(correctResult).to.be.closeTo(6.01, 0.01);
    });
    it("should return correct result when pass number as input", () => {
      // Arange
      const numberInput = 5;
      // Act
      const correctResult = mathEnforcer.addFive(numberInput);
      // Assert
      expect(correctResult).to.be.equal(10);
    });
    it("should return correct result when pass negative number as input", () => {
      // Arange
      const negativeInput = -15;
      // Act
      const correctResult = mathEnforcer.addFive(negativeInput);
      // Assert
      expect(correctResult).to.be.equal(-10);
    });
    it("should return correct result when expect 0 as result", () => {
      // Arange
      const negativeInput = -5;
      // Act
      const correctResult = mathEnforcer.addFive(negativeInput);
      // Assert
      expect(correctResult).to.be.equal(0);
    });
  });
  describe("substractTen", () => {
    it("should return undefined when pass string as input", () => {
      // Arange
      const stringInput = "someString";
      // Act
      const undefinedResult = mathEnforcer.subtractTen(stringInput);
      // Assert
      expect(undefinedResult).to.be.undefined;
    });
    it("should return undefined when pass undefined as input", () => {
      // Arange
      const undefinedInput = undefined;
      // Act
      const undefinedResult = mathEnforcer.subtractTen(undefinedInput);
      // Assert
      expect(undefinedResult).to.be.undefined;
    });
    it("should return undefined when pass number as string as input", () => {
      // Arange
      const numberAsStringInput = "5";
      // Act
      const undefinedResult = mathEnforcer.subtractTen(numberAsStringInput);
      // Assert
      expect(undefinedResult).to.be.undefined;
    });
    it("should return correct result when pass floating number as inputand assert with close.to", () => {
      // Arange
      const floatingInput = 1.01;
      // Act
      const correctResult = mathEnforcer.subtractTen(floatingInput);
      // Assert
      expect(correctResult).to.be.closeTo(-8.99, 0.01);
    });
    it("should return correct result when pass floating number as input and assert with equal", () => {
      // Arange
      const floatingInput = 1.01;
      // Act
      const correctResult = mathEnforcer.subtractTen(floatingInput);
      // Assert
      expect(correctResult).to.be.equal(-8.99);
    });
    it("should return correct result when pass floating number with a lot of digits as input and assert with close.to", () => {
      // Arange
      const floatingInput = 1.00000001;
      // Act
      const correctResult = mathEnforcer.subtractTen(floatingInput);
      // Assert
      expect(correctResult).to.be.closeTo(-8.99, 0.01);
    });
    it("should return correct result when pass number as input", () => {
      // Arange
      const numberInput = 5;
      // Act
      const correctResult = mathEnforcer.subtractTen(numberInput);
      // Assert
      expect(correctResult).to.be.equal(-5);
    });
    it("should return correct result when pass negative number as input", () => {
      // Arange
      const negativeInput = -15;
      // Act
      const correctResult = mathEnforcer.subtractTen(negativeInput);
      // Assert
      expect(correctResult).to.be.equal(-25);
    });
    it("should return correct result when expect 0 as result", () => {
      // Arange
      const negativeInput = -10;
      // Act
      const correctResult = mathEnforcer.subtractTen(negativeInput);
      // Assert
      expect(correctResult).to.be.equal(0);
    });
  });
  describe("sum", () => {
    it("should return undefined when Param1: incorrect and Param2: correct", () => {
      // Arange
      const incorrectFirstParam = "someString";
      const correctSecondParam = 5;
      // Act
      const undefinedResult = mathEnforcer.sum(
        incorrectFirstParam,
        correctSecondParam
      );
      // Assert
      expect(undefinedResult).to.be.undefined;
    });
    it("should return undefined when Param1: correct and Param2: incorrect", () => {
      // Arange
      const correctFirstParam = 5;
      const incorrectSecondParam = "someString";
      // Act
      const undefinedResult = mathEnforcer.sum(
        correctFirstParam,
        incorrectSecondParam
      );
      // Assert
      expect(undefinedResult).to.be.undefined;
    });
    it("should return undefined when Param1: number as string and Param2: correct", () => {
      // Arange
      const numberAsStringFirstParam = "5";
      const correctSecondParam = 5;
      // Act
      const undefinedResult = mathEnforcer.sum(
        numberAsStringFirstParam,
        correctSecondParam
      );
      // Assert
      expect(undefinedResult).to.be.undefined;
    });
    it("should return undefined when Param1: correct and Param2: correct", () => {
      // Arange
      const correctFirstParam = 5;
      const correctSecondParam = 5;
      // Act
      const undefinedResult = mathEnforcer.sum(
        correctFirstParam,
        correctSecondParam
      );
      // Assert
      expect(undefinedResult).to.be.equal(10);
    });
    it("should return undefined when Param1: negative and Param2: negative", () => {
      // Arange
      const correctFirstParam = -5;
      const correctSecondParam = -5;
      // Act
      const undefinedResult = mathEnforcer.sum(
        correctFirstParam,
        correctSecondParam
      );
      // Assert
      expect(undefinedResult).to.be.equal(-10);
    });
    it("should return undefined when Param1: floating number and Param2: correct with close.to", () => {
      // Arange
      const floatingNumberFirstParam = 5.01;
      const correctSecondParam = 5;
      // Act
      const undefinedResult = mathEnforcer.sum(
        floatingNumberFirstParam,
        correctSecondParam
      );
      // Assert
      expect(undefinedResult).to.be.equal(10.1);
      expect(undefinedResult).to.be.closeTo(10.1, 0.01);
    });
  });
});
