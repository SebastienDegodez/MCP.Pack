---
mode: agent
---

# MCP .NET Test & Coverage Prompt

This prompt describes how to run all tests and generate a code coverage report for the MCP solution using .NET CLI and ReportGenerator.

## Prerequisites
- .NET 7+ SDK installed
- [ReportGenerator](https://github.com/danielpalme/ReportGenerator) installed globally:
  ```sh
  dotnet tool install -g dotnet-reportgenerator-globaltool
  ```

## Steps

1. **Run all tests and collect code coverage:**
   ```sh
   dotnet test --collect:"XPlat Code Coverage"
   ```
   This will generate a coverage file (Cobertura format) in a `TestResults` subfolder.

2. **Find the coverage file and generate an HTML report:**
   ```sh
   reportgenerator -reports:"**/coverage.cobertura.xml" -targetdir:"coveragereport" -reporttypes:Html
   ```
   The HTML report will be available at `coveragereport/index.html`.

3. **Open the report:**
   Open report in browser to view the code coverage results.
<!--Open `coveragereport/index.html` in your browser to view the results.
-->

---

**Tip:**
- Run these commands from the root of your solution.
- If you want to clean previous results, delete the `TestResults` and `coveragereport` folders before running.
