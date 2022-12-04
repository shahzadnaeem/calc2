# Expression Calcuator with Sprache (Template Project)

## References

This is from [Sprache/samples](https://github.com/sprache/Sprache/blob/develop/samples/LinqyCalculator/ExpressionParser.cs)

## .NET Setup

### global.json

  [MS Learn:global-json](https://learn.microsoft.com/en-us/dotnet/core/tools/global-json)

  ```sh
  # Don't need a global.json as .NET 7.0 works with VScode/Omnisharp just fine
  # Had a few errors due to forgetting to add all projects to the solution file!
  # Command to create one, should it be needed is:
  $ dotnet new globaljson --sdk-version 6.0.403

  # NOTE: Make sure that you have the CORRECT <TargetFramework> in ALL your .csproj files
  #       Only an issue if you have been messing about with earlier versions as above!
  ```

### Create a solution

  ```sh
  $ cd calc2
  $ dotnet new sln
  $ cp .../templates/cs/gitignore .gitingore
  $ git init
  # Initial setup
  # Add README.md and LICENSE
  $ ga .
  $ gc -m "Intial files"
  # First commit
  ```

## Create Projects

  ```sh
  $ dotnet new classlib -o src/CalcLib
  $ dotnet new console -o src/Calc
  $ dotnet new xunit -o tests/Calc.Test

  # Now add references

  $ cd src/Calc
  $ dotnet add reference ../CalcLib/CalcLib.csproj

  $ cd ../../tests/Calc.Test
  $ dotnet add reference ../../src/CalcLib/CalcLib.csproj

  ```

## Update Solution

  ```sh
  $ dotnet sln add src/CalcLib/CalcLib.csproj 
  $ dotnet sln add src/Calc/Calc.csproj 
  $ dotnet sln add tests/Calc.Test
  # Add all projects - ensures many things, including VSCode/Omnisharp working

  ```

## Running and Testing

A Makefile has been added with the following targets:

- build
- clean
- watch
- run
- test
- Test Coverage (text and html generated, following select output shown)
  - [MS Learn:Code Coverage](https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-code-coverage?tabs=linux)
  - coverage-report
  - coverage-report-html
