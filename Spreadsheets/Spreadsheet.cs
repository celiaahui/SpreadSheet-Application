// <copyright file="Spreadsheet.cs" company="UofU-CS3500">
// Copyright (c) 2024 UofU-CS3500. All rights reserved.
// </copyright>

// Written by Joe Zachary for CS 3500, September 2013
// Update by Profs Kopta, de St. Germain, Martin, Fall 2021, Fall 2024, Fall 2025
//     - Updated return types
//     - Updated documentation

using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Globalization;
using System.Linq.Expressions;
using System.Xml;

namespace Spreadsheets;

using Formula;
using DependencyGraph;

/// <summary>
///   <para>
///     Thrown to indicate that a change to a cell will cause a circular dependency.
///   </para>
/// </summary>
public class CircularException : Exception
{
}

/// <summary>
///   <para>
///     Thrown to indicate that a name parameter was invalid.
///   </para>
/// </summary>
public class InvalidNameException : Exception
{
}

/// <summary>
/// <para>
///   Thrown to indicate that a read or write attempt has failed with
///   an expected error message informing the user of what went wrong.
/// </para>
/// </summary>
public class SpreadsheetReadWriteException : Exception
{
    /// <summary>
    ///   <para>
    ///     Creates the exception with a message defining what went wrong.
    ///   </para>
    /// </summary>
    /// <param name="msg"> An informative message to the user. </param>
    public SpreadsheetReadWriteException( string msg )
    : base( msg )
    {
    }
}

/// <summary>
///   <para>
///     A Spreadsheet object represents the state of a simple spreadsheet.  A
///     spreadsheet represents an infinite number of named cells.
///   </para>
/// <para>
///     Valid Cell Names: A string is a valid cell name if and only if it is one or
///     more letters followed by one or more numbers, e.g., A5, BC27.
/// </para>
/// <para>
///    Cell names are case-insensitive, so "x1" and "X1" are the same cell name.
///    Your code should normalize (uppercased) any stored name but accept either.
/// </para>
/// <para>
///     A spreadsheet represents a cell corresponding to every possible cell name.  (This
///     means that a spreadsheet contains an infinite number of cells.)  In addition to
///     a name, each cell has a contents and a value.  The distinction is important.
/// </para>
/// <para>
///     The <b>contents</b> of a cell can be (1) a string, (2) a double, or (3) a Formula.
///     If the contents of a cell is set to the empty string, the cell is considered empty.
/// </para>
/// <para>
///     By analogy, the contents of a cell in Excel is what is displayed on
///     the editing line when the cell is selected.
/// </para>
/// <para>
///     In a new spreadsheet, the contents of every cell is the empty string. Note:
///     this is by definition (it is IMPLIED, not stored).
/// </para>
/// <para>
///     The <b>value</b> of a cell can be (1) a string, (2) a double, or (3) a FormulaError.
///     (By analogy, the value of an Excel cell is what is displayed in that cell's position
///     in the grid.) We are not concerned with cell values yet, only with their contents,
///     but for context:
/// </para>
/// <list type="number">
///   <item>If a cell's contents is a string, its value is that string.</item>
///   <item>If a cell's contents is a double, its value is that double.</item>
///   <item>
///     <para>
///       If a cell's contents is a Formula, its value is either a double or a FormulaError,
///       as reported by the Evaluate method of the Formula class.  For this assignment,
///       you are not dealing with values yet.
///     </para>
///   </item>
/// </list>
/// <para>
///     Spreadsheets are never allowed to contain a combination of Formulas that establish
///     a circular dependency.  A circular dependency exists when a cell depends on itself,
///     either directly or indirectly.
///     For example, suppose that A1 contains B1*2, B1 contains C1*2, and C1 contains A1*2.
///     A1 depends on B1, which depends on C1, which depends on A1.  That's a circular
///     dependency.
/// </para>
/// </summary>
public class Spreadsheet
{
    /// <summary>
    /// This dictionary contains the cells of the SpreadSheet. The Key of the
    /// dictionary represents the name of the cell and the value represents the Cell Object
    /// </summary>
    private readonly Dictionary<string, Cell> _cells= new Dictionary<string, Cell>();
    
    /// <summary>
    /// This dependency graph keeps track of the relationship between all the cells in the Spreadsheet
    /// </summary>
    private readonly DependencyGraph _dependencyGraph = new DependencyGraph();

    /// <summary>
    /// Default Constructor that creates a new spreadsheet with only empty cells 
    /// </summary>
    public Spreadsheet()
    {
        Changed = false;
    }

    
    /// <summary>
    /// Represents a cell in the spreadsheet, with its name, contents, and value.
    /// The contents can be a string, a double, or a Formula.
    /// The value is not used yet and is set to null by the two-parameter constructor.
    /// </summary>
    private class Cell
    {
        /// <summary>
        /// Gets or sets the name of the cell.
        /// </summary>
        public string Name{get;set;}
        
        /// <summary>
        /// Gets or sets the contents of the cell: a string, double, or Formula.
        /// </summary>
        public object Content { get; set; }
        
        /// <summary>
        /// Gets or sets the value of the cell, which is not used yet.
        /// </summary>
        public object? Value { get; set; }
        
        public string StringForm
        {
            get
            {
                if (Content is double number)
                {
                    return number.ToString(CultureInfo.InvariantCulture);
                }
                
                if (Content is Formula formula)
                {
                    return "=" + formula.ToString();
                }
                
                return (string)Content;

            }
        }

       
       
        
        /// <summary>
        /// Creates a cell with the given name and contents, and sets its value to null.
        /// </summary>
        /// <param name="name">The name of the cell</param>
        /// <param name="content">The contents of the cell</param>

        public Cell(string name, object content)
        {
            Name = name;
            Content = content; 
            Value = null;
        }
        
    }
    /// <summary>
    ///   Provides a copy of the normalized names of all the cells in the spreadsheet
    ///   that contain information (i.e., non-empty cells).
    /// </summary>
    /// <returns>
    ///   A set of the names of all the non-empty cells in the spreadsheet.
    /// </returns>
    public ISet<string> GetNamesOfAllNonemptyCells()
    {
        HashSet<string> result = new HashSet<string>();
        List<string> copy = _cells.Keys.ToList();

        foreach (string name in copy)
        {
            result.Add(name.ToUpper());
        }

        return result;
    }

    /// <summary>
    ///   Returns the contents (as opposed to the value) of the named cell.
    /// </summary>
    ///
    /// <exception cref="InvalidNameException">
    ///   Thrown if the name is invalid.
    /// </exception>
    ///
    /// <param name="name">The name of the spreadsheet cell to query. </param>
    /// <returns>
    ///   The contents as either a string, a double, or a Formula.
    ///   See the class header summary.
    /// </returns>
    public object GetCellContents(string name)
    {
        if (!IsValid(name))
        {
            throw new InvalidNameException();
        }

        String name2 = name.ToUpper();
        if (_cells.TryGetValue(name2, out var cell))
        {
            return cell.Content;

        }

        return "";
    }

    /// <summary>
    ///  Set the contents of the named cell to the given number.
    /// </summary>
    ///
    /// <exception cref="InvalidNameException">
    ///   If the name is invalid, throw an InvalidNameException.
    /// </exception>
    ///
    /// <param name="name"> The name of the cell. </param>
    /// <param name="number"> The new contents of the cell. </param>
    /// <returns>
    ///   <para>
    ///     This method returns an ordered list consisting of the passed in name
    ///     followed by the names of all other cells whose value depends, directly
    ///     or indirectly, on the named cell.
    ///   </para>
    ///   <para>
    ///     The order must correspond to a valid dependency ordering for recomputing
    ///     all the cells, i.e., if you re-evaluate each cell in the order of the list,
    ///     the overall spreadsheet will be correctly updated.
    ///   </para>
    ///   <para>
    ///     For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
    ///     list [A1, B1, C1] is returned, i.e., A1 was changed, so then A1 must be
    ///     evaluated, followed by B1, followed by C1.
    ///   </para>
    /// </returns>
    private IList<string> SetCellContents(string name, double number)
    {
        
        _cells[name] = new Cell(name, number);
        
        //all dependees are deleted since the cell is now a number which means it doesn't depend on other cells 
        _dependencyGraph.ReplaceDependees(name, new HashSet<string>());
        List<string> finalResult =  GetCellsToRecalculate(name).ToList();
        
        return  finalResult;
    }

    /// <summary>
    ///   The contents of the named cell becomes the given text.
    /// </summary>
    ///
    /// <exception cref="InvalidNameException">
    ///   If the name is invalid, throw an InvalidNameException.
    /// </exception>
    /// <param name="name"> The name of the cell. </param>
    /// <param name="text"> The new contents of the cell. </param>
    /// <returns>
    ///   The same list as defined in <see cref="SetCellContents(string, double)"/>.
    /// </returns>
    private IList<string> SetCellContents(string name, string text)
    {
        
        if (text == "")
        {
            _cells.Remove(name);
        }
        else
        {
            _cells[name] = new Cell(name, text);
        }
        
        _dependencyGraph.ReplaceDependees(name, new HashSet<string>());
        
        return   GetCellsToRecalculate(name).ToList();
    }

    /// <summary>
    ///   Set the contents of the named cell to the given formula.
    /// </summary>
    /// <exception cref="InvalidNameException">
    ///   If the name is invalid, throw an InvalidNameException.
    /// </exception>
    /// <exception cref="CircularException">
    ///   <para>
    ///     If changing the contents of the named cell to be the formula would
    ///     cause a circular dependency, throw a CircularException, and no
    ///     change is made to the spreadsheet.
    ///   </para>
    /// </exception>
    /// <param name="name"> The name of the cell. </param>
    /// <param name="formula"> The new contents of the cell. </param>
    /// <returns>
    ///   The same list as defined in <see cref="SetCellContents(string, double)"/>.
    /// </returns>
    private IList<string> SetCellContents(string name, Formula formula)
    {
        
        List<string> oldDependees = _dependencyGraph.GetDependees(name).ToList();
        
        _dependencyGraph.ReplaceDependees(name, formula.GetVariables());
        List<string> result;
        
        try
        {
           
            result = GetCellsToRecalculate(name).ToList();
        }
        catch (CircularException)
        {
           
            _dependencyGraph.ReplaceDependees(name, oldDependees);
            throw;
        }
        
        _cells[name] = new Cell(name, formula);

        return result;
    }

    /// <summary>
    ///   Returns an enumeration, without duplicates, of the names of all cells whose
    ///   values depend directly on the value of the named cell.
    /// </summary>
    /// <param name="name"> This <b>MUST</b> be a valid name.  </param>
    /// <returns>
    ///   <para>
    ///     Returns an enumeration, without duplicates, of the names of all cells
    ///     that contain formulas containing name.
    ///   </para>
    ///   <para>For example, suppose that: </para>
    ///   <list type="bullet">
    ///      <item>A1 contains 3</item>
    ///      <item>B1 contains the formula A1 * A1</item>
    ///      <item>C1 contains the formula B1 + A1</item>
    ///      <item>D1 contains the formula B1 - C1</item>
    ///   </list>
    ///   <para> The direct dependents of A1 are B1 and C1. </para>
    /// </returns>
    private IEnumerable<string> GetDirectDependents(string name)
    {
        if (!IsValid(name))
        {
            throw new InvalidNameException();
        }
        
        return _dependencyGraph.GetDependents(name.ToUpper());
    }

    /// <summary>
    ///   <para>
    ///     This method is implemented for you, but makes use of your GetDirectDependents.
    ///   </para>
    ///   <para>
    ///     Returns an enumeration of the names of all cells whose values must
    ///     be recalculated, assuming that the contents of the cell referred
    ///     to by name has changed.  The cell names are enumerated in an order
    ///     in which the calculations should be done.
    ///   </para>
    ///   <exception cref="CircularException">
    ///     If the cell referred to by name is involved in a circular dependency,
    ///     throws a CircularException.
    ///   </exception>
    ///   <para>
    ///     For example, suppose that:
    ///   </para>
    ///   <list type="number">
    ///     <item>
    ///       A1 contains 5
    ///     </item>
    ///     <item>
    ///       B1 contains the formula A1 + 2.
    ///     </item>
    ///     <item>
    ///       C1 contains the formula A1 + B1.
    ///     </item>
    ///     <item>
    ///       D1 contains the formula A1 * 7.
    ///     </item>
    ///     <item>
    ///       E1 contains 15
    ///     </item>
    ///   </list>
    ///   <para>
    ///     If A1 has changed, then A1, B1, C1, and D1 must be recalculated,
    ///     and they must be recalculated in an order which has A1 first, and B1 before C1
    ///     (there are multiple such valid orders).
    ///     The method will produce one of those enumerations.
    ///   </para>
    ///   <para>
    ///      PLEASE NOTE THAT THIS METHOD DEPENDS ON THE METHOD GetDirectDependents.
    ///      IT WON'T WORK UNTIL GetDirectDependents IS IMPLEMENTED CORRECTLY.
    ///   </para>
    /// </summary>
    /// <param name="name"> The name of the cell.  Requires that name be a valid cell name.</param>
    /// <returns>
    ///    Returns an enumeration of the names of all cells whose values must
    ///    be recalculated.
    /// </returns>
    private IEnumerable<string> GetCellsToRecalculate(string name)
    {
        LinkedList<string> changed = new();
        HashSet<string> visited = [];
        Visit(name, name, visited, changed);
        return changed;
    }

    /// <summary>
    /// Finds all the cells that depend directly or indirectly on the given cell
    /// and puts them, along with that cell, in the order they need to be recalculated.
    /// Throws a CircularException if a dependency leads back to the starting cell.
    /// </summary>
    private void Visit(string start, string name, ISet<string> visited, LinkedList<string> changed)
    {
        //Mark this cell as visited
        visited.Add(name);
     
        // Go through the cells that depend directly on this cell.
        foreach (string n in GetDirectDependents(name))
        {
            // If we get back to the starting cell, there is a circular dependency.
            if (n.Equals(start))
            {
                throw new CircularException();
            }
            else if (!visited.Contains(n))
            {
                // If we have not visited this cell yet, check its dependents too.
                Visit(start, n, visited, changed);
            }
        }

        // Add this cell before its dependents so it gets recalculated first.
        changed.AddFirst(name);
    }

    /// <summary>
    /// Helper method to verify if a cell name is valid
    /// </summary>
    /// <param name="name"></param>
    /// <returns>
    /// returns true is the name is valid(one or more letters with one or more numbers) and false if the name is invalid 
    /// </returns>
    private bool IsValid(string? name)
    {
        return name != null &&  Regex.IsMatch(name, @"\A[a-zA-Z]+[0-9]+\z");
    
    }
    

    /// <summary>
    ///   <para>
    ///     Return the value of the named cell, as defined by
    ///     <see cref="GetCellValue(string)"/>.
    ///   </para>
    /// </summary>
    /// <param name="name"> The cell in question. </param>
    /// <returns>
    ///   <see cref="GetCellValue(string)"/>
    /// </returns>
    /// <exception cref="InvalidNameException">
    ///   If the provided name is invalid, throws an InvalidNameException.
    /// </exception>
    public object this[string name]
    {
      get {
          if (!IsValid(name))
          {
              throw new InvalidNameException();
          }

          return GetCellValue(name);
      }
    }


	  /// <summary>
    /// True if this spreadsheet has been changed since it was
    /// created or saved (whichever happened most recently),
    /// False otherwise.
    /// </summary>
    public bool Changed { get; private set; }


	  /// <summary>
    /// Constructs a spreadsheet using the saved data in the file referred to by
    /// the given filename.
    /// <see cref="Save(string)"/>
    /// </summary>
    /// <exception cref="SpreadsheetReadWriteException">
    ///   Thrown if the file can not be loaded into a spreadsheet for any reason
    /// </exception>
    /// <param name="filename">The path to the file containing the spreadsheet to load</param>
    public Spreadsheet(string filename)
    {
        try
        {
            string json = File.ReadAllText(filename);
            Dictionary<string, Dictionary<string, Dictionary<string, string>>>? result = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, Dictionary<string, string>>>>(json);
            
            //Verify that the result isn't null
            if (result == null)
            {
                throw new SpreadsheetReadWriteException("Invalid spreadsheet data."); 
            }
            //Get the real contents of the spreadsheet which is here the value of "Cells"
            Dictionary<string, Dictionary<string, string>> cellsContent = result["Cells"];

            if (cellsContent == null)
            {
                throw new SpreadsheetReadWriteException("Invalid spreadsheet data.");
            }
            //Iterates over each cell name in the dictionary and sets their contents;
            foreach (string cellName in cellsContent.Keys)
            {
                
                Dictionary<string, string> stringContent = cellsContent[cellName];

                //Verify if the value of the cell is null 
                if (stringContent == null)
                {
                    throw new SpreadsheetReadWriteException("Invalid spreadsheet data.");
                }
                string finalContent = stringContent["StringForm"];
                
                //Verify if the StringForm is null 
                if (finalContent == null)
                {
                    throw new SpreadsheetReadWriteException("Invalid spreadsheet data.");
                }
                SetContentsOfCell(cellName,finalContent);
            }
        }
        catch
        {
            throw new SpreadsheetReadWriteException("Unable to load the Spreadsheet");
        }

        Changed = false;
    }


    /// <summary>
    /// Saves this spreadsheet to a file
    /// </summary>
    /// <param name="filename"> The name (with path) of the file to save to.</param>
    /// <exception cref="SpreadsheetReadWriteException">
    ///   If there are any problems opening, writing, or closing the file,
    ///   the method should throw a SpreadsheetReadWriteException with an
    ///   explanatory message.
    /// </exception>
    public void Save( string filename )
	  {

          try
          {
           
             
              Dictionary<string, Dictionary<string, string>> innerDictionary = new Dictionary<string, Dictionary<string, string>>();
              foreach (string cellName in _cells.Keys)
              {
                  Cell oldValue =  _cells[cellName];
                  if (oldValue == null)
                  {
                      throw new SpreadsheetReadWriteException("Unable to save the spreadsheet");
                  }

                  string text =  oldValue.StringForm;
                  Dictionary<string, string> newValue = new Dictionary<string, string> { { "StringForm", text } };
                  innerDictionary[cellName] = newValue;
              }

              Dictionary<string, Dictionary<string, Dictionary<string, string>>> result =
                  new Dictionary<string, Dictionary<string, Dictionary<string, string>>>
                  {
                      { "Cells" , innerDictionary }
                  };
              string json = JsonSerializer.Serialize(result);
              File.WriteAllText(filename, json);
          }
          catch
          {
              throw new SpreadsheetReadWriteException("Unable to save the spreadsheet");
          }
          Changed = false;
      }

    /// <summary>
    ///   <para>
    ///     Return the value of the named cell.
    ///   </para>
    /// </summary>
    /// <param name="name"> The cell in question. </param>
    /// <returns>
    ///   Returns the value (as opposed to the contents) of the named cell.  The return
    ///   value should be either a string, a double, or a CS3500.Formula.FormulaError.
    /// </returns>
    /// <exception cref="InvalidNameException">
    ///   If the provided name is invalid, throws an InvalidNameException.
    /// </exception>
    public object GetCellValue( string name )
	  {
        
          if (!IsValid(name))
          {
              throw new InvalidNameException();
          }

          name = name.ToUpper();

          if (_cells.TryGetValue(name, out Cell? cell))
          {
              return cell.Value!;
          }

          return "";

      }

    /// <summary>
    /// Returns the numeric value of the named cell for formula evaluation.
    /// </summary>
    /// <param name="name">The name of the cell.</param>
    /// <returns>The numeric value of the cell.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown if the cell does not contain a numeric value.
    /// </exception>
    /// <exception cref="InvalidNameException">
    /// Thrown if the cell name is invalid.
    /// </exception>
    private double LookUpCell(string cellName)
    {
        Object value = GetCellValue(cellName);
        if (value is double )
        {
            return (double)value;
        }

        throw new ArgumentException();


    }

    /// <summary>
    ///   <para>
    ///     Set the contents of the named cell to be the provided string
    ///     which will either represent (1) a string, (2) a number, or
    ///     (3) a formula (based on the prepended '=' character).
    ///   </para>
    ///   <para>
    ///     Rules of parsing the input string:
    ///   </para>
    ///   <list type="bullet">
    ///     <item>
    ///       <para>
    ///         If 'content' parses as a double, the contents of the named
    ///         cell becomes that double.
    ///       </para>
    ///     </item>
    ///     <item>
    ///         If the string does not begin with an '=', the contents of the
    ///         named cell becomes 'content'.
    ///     </item>
    ///     <item>
    ///       <para>
    ///         If 'content' begins with the character '=', an attempt is made
    ///         to parse the remainder of content into a Formula f using the Formula
    ///         constructor.  There are then three possibilities:
    ///       </para>
    ///       <list type="number">
    ///         <item>
    ///           If the remainder of content cannot be parsed into a Formula, a
    ///           CS3500.Formula.FormulaFormatException is thrown.
    ///         </item>
    ///         <item>
    ///           Otherwise, if changing the contents of the named cell to be f
    ///           would cause a circular dependency, a CircularException is thrown,
    ///           and no change is made to the spreadsheet.
    ///         </item>
    ///         <item>
    ///           Otherwise, the contents of the named cell becomes f.
    ///         </item>
    ///       </list>
    ///     </item>
    ///   </list>
    /// </summary>
    /// <returns>
    ///   <para>
    ///     The method returns a list consisting of the name plus the names
    ///     of all other cells whose value depends, directly or indirectly,
    ///     on the named cell. The order of the list should be any order
    ///     such that if cells are re-evaluated in that order, their dependencies
    ///     are satisfied by the time they are evaluated.
    ///   </para>
    ///   <example>
    ///     For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
    ///     list {A1, B1, C1} is returned.
    ///   </example>
    /// </returns>
    /// <exception cref="InvalidNameException">
    ///     If name is invalid, throws an InvalidNameException.
    /// </exception>
    /// <exception cref="CircularException">
    ///     If a formula would result in a circular dependency, throws CircularException.
    /// </exception>
    public IList<string> SetContentsOfCell( string name, string content )
	  {

          if (!IsValid(name))
          {
              throw new InvalidNameException();
          }

          name = name.ToUpper();
          IList<string> result;

          if (double.TryParse(content, NumberStyles.Float,
                  CultureInfo.InvariantCulture, out double number))
          {
              result = SetCellContents(name, number);
          }
          else if (content.StartsWith("="))
          {
              Formula formula = new Formula(content.Substring(1));
              result = SetCellContents(name, formula);
          }
          else
          {
              result = SetCellContents(name, content);
          }

          foreach (string cellName in result)
          {
              if (!_cells.TryGetValue(cellName, out Cell? cell))
              {
                  continue;
              }

              if (cell.Content is Formula formula)
              {
                  
                  cell.Value = formula.Evaluate(LookUpCell);
              }
              else
              {
                  cell.Value = cell.Content;
              }
          }

          Changed = true;
          return result;

      }

}