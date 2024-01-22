function solve(arrayOfEmployes) {
  // const employeeData = [];

  // for (const employeeRaw of arrayOfEmployes) {
  //   employeeData.push({
  //     name: employeeRaw,
  //     personalNumber: employeeRaw.length,
  //   });
  // }
  const employeeData = arrayOfEmployes.map((employeeRaw) => ({
    name: employeeRaw,
    personalNumber: employeeRaw.length,
  }));
  employeeData.forEach((employee) =>
    console.log(
      `Name: ${employee.name} -- Personal Number: ${employee.personalNumber}`
    )
  );
}
