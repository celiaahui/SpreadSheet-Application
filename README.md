
# Spreadsheet Application

A spreadsheet application built with **C# and .NET**, focusing on formula evaluation, cell dependency management, and object-oriented software design.

This project implements the core logic of a spreadsheet engine, with a graphical user interface currently under development.

## Technologies Used

- **Programming Language:** C#
- **Framework:** .NET
- **Testing Framework:** MSTest
- **IDE:** JetBrains Rider
- **Version Control:** Git & GitHub

## Features

### 1. Formula Evaluation
- Parses and evaluates mathematical expressions.
- Supports arithmetic operations and operator precedence.
- Handles cell references within formulas.
- Validates formulas and handles invalid expressions.

### 2. Dependency Graph
- Tracks dependencies between spreadsheet cells.
- Manages relationships between dependent cells.
- Supports recalculation when referenced cells change.

### 3. Spreadsheet Engine
- Manages cell contents and calculated values.
- Supports numbers, text, and formulas.
- Tracks non-empty cells.
- Handles invalid cell names and circular dependencies.

### 4. Unit Testing
- Uses MSTest for automated testing.
- Tests formula evaluation and dependency management.
- Validates spreadsheet operations and exception handling.

## Project Structure

```text
Spreadsheet-Application/
├── DependencyGraph/
├── DependencyGraphTests/
├── Formula/
├── FormulaTests/
├── Spreadsheets/
├── SpreadsheetTests/
├── Spreadsheet.sln
├── .gitignore
└── README.md
```

## Development Status

**Status: In Progress**

The core spreadsheet engine has been implemented.

### Next Steps
- [ ] Develop a graphical user interface (GUI).
- [ ] Create an interactive spreadsheet grid.
- [ ] Allow users to edit cells through the interface.
- [ ] Display calculated values in real time.
- [ ] Integrate the GUI with the spreadsheet engine.
- [ ] Add screenshots of the completed application.

## What I Learned

Through this project, I gained hands-on experience with:

- Object-oriented programming in C#.
- Designing and implementing data structures.
- Managing dependencies using graph-based structures.
- Parsing and evaluating mathematical expressions.
- Writing automated unit tests.
- Building modular and maintainable software.

## Author

**Celia Ahui**

Computer Science Student

[GitHub Profile](https://github.com/celiaahui)
