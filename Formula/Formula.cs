// <summary>
//   <para>
//     This code is provided to start your assignment.  It was written
//     by Profs Joe, Danny, Jim, and Travis.  You should keep this attribution
//     at the top of your code where you have your header comment, along
//     with any other required information.
//   </para>
//   <para>
//     You should remove/add/adjust comments in your file as appropriate
//     to represent your work and any changes you make.
//   </para>
// </summary>
// <authors> Celia AHUI </authors>
// <date> 09/11/2026 </date>



namespace Formula;

using System.Text.RegularExpressions;
using System.Globalization;

/// <summary>
///   <para>
///     This class represents formulas written in standard infix notation using standard precedence
///     rules.  The allowed symbols are non-negative numbers written using double-precision
///     floating-point syntax; variables that consist of one or more letters followed by
///     one or more numbers; parentheses; and the four operator symbols +, -, *, and /.
///   </para>
///   <para>
///     Spaces are significant only insofar that they delimit tokens.  For example, "xy" is
///     a single variable, "x y" consists of two variables "x" and y; "x23" is a single variable;
///     and "x 23" consists of a variable "x" and a number "23".  Otherwise, spaces are to be removed.
///   </para>
///   <para>
///     For Assignment Two, you are to implement the following functionality:
///   </para>
///   <list type="bullet">
///     <item>
///        Formula Constructor which checks the syntax of a formula.
///     </item>
///     <item>
///        Get Variables
///     </item>
///     <item>
///        ToString
///     </item>
///   </list>
/// </summary>
public class Formula 
{
    /// <summary>
    ///   All variables are letters followed by numbers.  This pattern
    ///   represents valid variable name strings.
    /// </summary>
    private const string VariableRegExPattern = @"[a-zA-Z]+\d+";
    //Keep Track of the variables that we have already encountered
    private  readonly HashSet<string> _variablesEncountered;
    private readonly string  _normalizedFormula;
    /// <summary>
    ///   Initializes a new instance of the <see cref="Formula"/> class.
    ///   <para>
    ///     Creates a Formula from a string that consists of an infix expression written as
    ///     described in the class comment.  If the expression is syntactically incorrect,
    ///     throws a FormulaFormatException with an explanatory Message.  See the assignment
    ///     specifications for the syntax rules you are to implement.
    ///   </para>
    ///   <para>
    ///     Non-Exhaustive Example Errors:
    ///   </para>
    ///   <list type="bullet">
    ///     <item>
    ///        Invalid variable name, e.g., x, x1x  (Note: x1 is valid, but would be normalized to X1)
    ///     </item>
    ///     <item>
    ///        Empty formula, e.g., string.Empty
    ///     </item>
    ///     <item>
    ///        Mismatched Parentheses, e.g., "(("
    ///     </item>
    ///     <item>
    ///        Invalid Following Rule, e.g., "2x+5"
    ///     </item>
    ///   </list>
    /// </summary>
    /// <param name="formula"> The string representation of the formula to be created.</param>
    public Formula( string formula )
    {
        if (formula is null)
        {
            throw new FormulaFormatException("The formula cannot be null.");
        }
        
        //Transform the given string into a list of token
        List<string> tokens = GetTokens(formula);
        //List of valid operators
        string[] operators = [ "+", "-", "*", "/" ];
        
        //One Token Rule 
        if (tokens.Count == 0)
        {
            throw new FormulaFormatException("Error!, The String list is empty");
        }

        int openingParenthesis = 0;
        int closingParenthesis = 0;
        string previous;
        _variablesEncountered = new HashSet<string>();
        string firstToken = tokens[0];
        
        
        //First Token Rule
        // I added additional info for the Try Parse because my computer has a French configuration
        if (double.TryParse(firstToken, NumberStyles.Float, CultureInfo.InvariantCulture, out _))
        {
            previous = firstToken;
        }
        else if (firstToken == "(")
        {
            previous = firstToken;
            openingParenthesis++;
            
        }
        
        else if (IsVar(firstToken))
        {
            previous = firstToken;
            _variablesEncountered.Add(firstToken.ToUpper());
            
        }
        else
        {
            throw new FormulaFormatException("The first Token is invalid");
        }

        //Verify the other tokens 
        for (int i = 1; i < tokens.Count - 1; i++)
        {
            String current = tokens[i];

            if (previous == "(" || operators.Contains(previous))
            {
                if (IsVar(current))
                {
                    _variablesEncountered.Add(current.ToUpper());
                    previous = current;
                }
                else if (double.TryParse(current, NumberStyles.Float, CultureInfo.InvariantCulture, out _))
                {
                    previous = current;
                }
                else if (current == "(")
                {
                    previous = current;
                    openingParenthesis++;
                }
                else
                {
                    throw new FormulaFormatException("An opening parenthesis or an operator is followed by an invalid token");
                }
            }
            else if (double.TryParse(previous, NumberStyles.Float, CultureInfo.InvariantCulture, out _)|| previous == ")" || IsVar(previous))
            {
                if (current == ")")
                {
                    closingParenthesis++;
                    //Closing parentheses Rule
                    if (openingParenthesis < closingParenthesis)
                    {
                        throw new FormulaFormatException(
                            "The formula has one or more closing parentheses without a matching opening parenthesis");
                    }
                    previous = current;
                }
                
                else if (operators.Contains(current))
                {
                    previous = current;
                }
                    
                else
                {
                    throw new FormulaFormatException("A number, a variable or a closing parenthesis is followed by an invalid token");
                }
            }
        }
        //Last token
        string lastToken = tokens[^1];
        if (double.TryParse(lastToken, NumberStyles.Float, CultureInfo.InvariantCulture, out _) )
        {
            if (tokens.Count > 1)
             //extra following rule    
            {
                if (double.TryParse(previous, NumberStyles.Float, CultureInfo.InvariantCulture, out _) || IsVar(previous) || previous == ")")
                {
                    throw new FormulaFormatException("A number cannot follow a number, variable, or closing parenthesis");
                }
            }

            
        }
        
        else if (IsVar(lastToken))
        {
            if (tokens.Count > 1)
                
             //extra following rule    
            {
                if (double.TryParse(previous, NumberStyles.Float, CultureInfo.InvariantCulture, out _)|| IsVar(previous) || previous == ")")
                {
                    throw new FormulaFormatException("A variable cannot follow a number, variable, or closing parenthesis");
                }
            }
            
            _variablesEncountered.Add(lastToken.ToUpper());
        }
        
        else if (lastToken == ")")
        {
            closingParenthesis++;
            if (openingParenthesis < closingParenthesis)
            {
                throw new FormulaFormatException("There are more closing parentheses than opening parentheses");
            }
            
            if (operators.Contains(previous) || previous == "(")
            {
                throw new FormulaFormatException("the Parenthesis or Operator Following Rule has been broken");
            }
           
            
       
            
        }
        else
        {
            throw new FormulaFormatException("The last token is invalid");
        }

        if (openingParenthesis != closingParenthesis)
        {
            throw new FormulaFormatException("The formula has unbalanced parentheses");
        }
        
        string result = "";

        foreach (string token in tokens)
        {
            if (IsVar(token))
            {
                result += token.ToUpper();
            }
            else if (double.TryParse(token, NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
            {
                result += value.ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                result += token;
            }
        }

        _normalizedFormula = result;
        
        


       
    }

    /// <summary>
    ///   <para>
    ///     Returns a set of all the variables in the formula.
    ///   </para>
    ///   <remarks>
    ///     Important: no variable may appear more than once in the returned set, even
    ///     if it is used more than once in the Formula.
	///     Variables should be returned in canonical form, having all letters converted
	///     to uppercase.
    ///   </remarks>
    ///   <list type="bullet">
    ///     <item>new("x1+y1*z1").GetVariables() should return a set containing "X1", "Y1", and "Z1".</item>
    ///     <item>new("x1+X1"   ).GetVariables() should return a set containing "X1".</item>
    ///   </list>
    /// </summary>
    /// <returns> the set of variables (string names) representing the variables referenced by the formula. </returns>
    public ISet<string> GetVariables( )
    {
        HashSet<string> variables = new HashSet<string>(_variablesEncountered);
        return variables;
    }

    /// <summary>
    ///   <para>
    ///     Returns a string representation of a canonical form of the formula.
    ///   </para>
    ///   <para>
    ///     The string will contain no spaces.
    ///   </para>
    ///   <para>
    ///     If the string is passed to the Formula constructor, the new Formula f
    ///     will be such that this.ToString() == f.ToString().
    ///   </para>
    ///   <para>
    ///     All the variable and number tokens in the string will be normalized.
    ///     For numbers, this means that the original string token is converted to
    ///     a number using double.Parse or double.TryParse, then converted back to a
    ///     string using double.ToString.
    ///     For variables, this means all letters are uppercased.
    ///   </para>
    ///   <para>
    ///       For example:
    ///   </para>
    ///   <code>
    ///       new("x1 + Y1").ToString() should return "X1+Y1"
    ///       new("x1 + 5.0000").ToString() should return "X1+5".
    ///   </code>
    ///   <para>
    ///     This method should execute in O(1) time.
    ///   </para>
    /// </summary>
    /// <returns>
    ///   A canonical version (string) of the formula. All "equal" formulas
    ///   should have the same value here.
    /// </returns>
    public override string ToString( )
    {
       
        return _normalizedFormula;
    }
    

    /// <summary>
    ///   Reports whether "token" is a variable.  It must be one or more letters
    ///   followed by one or more numbers.
    /// </summary>
    /// <param name="token"> A token that may be a variable. </param>
    /// <returns> true if the string matches the requirements, e.g., A1 or a1. </returns>
    private static bool IsVar( string token )
    {
        // notice the use of ^ and $ to denote that the entire string being matched is just the variable
        string standaloneVarPattern = $"^{VariableRegExPattern}$";
        return Regex.IsMatch( token, standaloneVarPattern );
    }

    /// <summary>
    ///   <para>
    ///     Given an expression, enumerates the tokens that compose it.
    ///   </para>
    ///   <para>
    ///     Tokens returned are:
    ///   </para>
    ///   <list type="bullet">
    ///     <item>left paren</item>
    ///     <item>right paren</item>
    ///     <item>one of the four operator symbols</item>
    ///     <item>a string consisting of one or more letters followed by one or more numbers</item>
    ///     <item>a double literal</item>
    ///     <item>and anything that doesn't match one of the above patterns</item>
    ///   </list>
    ///   <para>
    ///     There are no empty tokens; white space is ignored (except to separate other tokens).
    ///   </para>
    /// </summary>
    /// <param name="formula"> A string representing an infix formula such as 1*B1/3.0. </param>
    /// <returns> The ordered list of tokens in the formula. </returns>
    private static List<string> GetTokens( string formula )
    {
        List<string> results = [];

        string lpPattern = @"\(";
        string rpPattern = @"\)";
        string opPattern = @"[\+\-*/]";
        string doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
        string spacePattern = @"\s+";

        // Overall pattern
        string pattern = string.Format(
                                        "({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                        lpPattern,
                                        rpPattern,
                                        opPattern,
                                        VariableRegExPattern,
                                        doublePattern,
                                        spacePattern);

        // Enumerate matching tokens that don't consist solely of white space.
        foreach ( string s in Regex.Split( formula, pattern, RegexOptions.IgnorePatternWhitespace ) )
        {
            if ( !Regex.IsMatch( s, @"^\s*$", RegexOptions.Singleline ) )
            {
                results.Add(s);
            }
        }

        return results;
    }
    /// <summary>
    ///   <para>
    ///     Reports whether f1 == f2, using the notion of equality from the <see cref="Equals"/> method.
    ///   </para>
    /// </summary>
    /// <param name="f1"> The first of two formula objects. </param>
    /// <param name="f2"> The second of two formula objects. </param>
    /// <returns> true if the two formulas are the same.</returns>
    public static bool operator ==( Formula f1, Formula f2 )
    {
        if (ReferenceEquals(f1, f2))
        {
            return true;
        }

        if (ReferenceEquals(f1, null) || ReferenceEquals(f2, null))
        {
            return false;
        }

        return f1.Equals(f2);

    }
    
    /// <summary>
    ///   <para>
    ///     Reports whether f1 != f2, using the notion of equality from the <see cref="Equals"/> method.
    ///   </para>
    /// </summary>
    /// <param name="f1"> The first of two formula objects. </param>
    /// <param name="f2"> The second of two formula objects. </param>
    /// <returns> true if the two formulas are not equal to each other.</returns>
    public static bool operator !=( Formula f1, Formula f2 )
    {
        return !(f1 == f2);
    }
    
    /// <summary>
    ///   <para>
    ///     Determines if two formula objects represent the same formula.
    ///   </para>
    ///   <para>
    ///     By definition, if the parameter is null or does not reference
    ///     a Formula Object then return false.
    ///   </para>
    ///   <para>
    ///     Two Formulas are considered equal if their canonical string representations
    ///     (as defined by ToString) are equal.
    ///   </para>
    /// </summary>
    /// <param name="obj"> The other object.</param>
    /// <returns>
    ///   True if the two objects represent the same formula.
    /// </returns>
    public override bool Equals( object? obj )
    {
        if (obj is Formula other)
        {
            string firstTokens = this.ToString();
            string secondTokens = other.ToString();
            
            return firstTokens.Equals( secondTokens );
            
            
        }

        return false;
    }
    
    
    /// <summary>
    ///   <para>
    ///     Evaluates this Formula, using the lookup delegate to determine the values of
    ///     variables.
    ///   </para>
    ///   <remarks>
    ///     When the lookup method is called, it will always be passed a normalized (capitalized)
    ///     variable name.  The lookup method will throw an ArgumentException if there is
    ///     not a definition for that variable token.
    ///   </remarks>
    ///   <para>
    ///     If no undefined variables or divisions by zero are encountered when evaluating
    ///     this Formula, the numeric value of the formula is returned.  Otherwise, a
    ///     FormulaError is returned (with a meaningful explanation as the Reason property).
    ///   </para>
    ///   <para>
    ///     This method should never throw an exception.
    ///   </para>
    /// </summary>
    /// <param name="lookup">
    ///   <para>
    ///     Given a variable symbol as its parameter, lookup returns the variable's value
    ///     (if it has one) or throws an ArgumentException (otherwise).  This method will expect
    ///     variable names to be normalized.
    ///   </para>
    /// </param>
    /// <returns> Either a double or a FormulaError, based on evaluating the formula.</returns>
    public object Evaluate( Lookup lookup )
    {
       Stack<double> values = new Stack<double>();
       Stack<string> operators = new Stack<string>();

       List<string> tokens = GetTokens(_normalizedFormula);

       
       foreach (string token in tokens)
       {
           
           double result;
           if (double.TryParse(token, NumberStyles.Float, CultureInfo.InvariantCulture, out double number))
           {
               if (operators.Count > 0 && operators.Peek() == "*")
               {
                   result = number * values.Pop();
                   values.Push(result);
                   operators.Pop();
               }
               
               else if (operators.Count > 0 && operators.Peek() == "/")
               {
                   if (number == 0)
                   {
                       return new FormulaError("A number cannot be divided by 0");
                   }
                   else
                   {
                       result = values.Pop() / number;
                       values.Push(result);
                       operators.Pop();
                   }
               }

               else
               {
                   values.Push(number);
               }
           }
           
           // Token is a variable
           else if (IsVar(token))
           {
               double value;
               try
               {
                   value = lookup(token);
               }
               catch (ArgumentException)
               {
                   return new FormulaError("undefined variable ");
               }
               if (operators.Count > 0 && operators.Peek() == "*")
               {
                   result = value * values.Pop();
                   values.Push(result);
                   operators.Pop();
               }
               
               else if (operators.Count > 0 && operators.Peek() == "/")
               {
                   if (value == 0)
                   {
                       return new FormulaError("A number cannot be divided by 0");
                   }
                   else
                   {
                       result = values.Pop() / value;
                       values.Push(result);
                       operators.Pop();
                   }
               }

               else
               {
                   values.Push(value);
               }
               
           }
           
           // token is either + or -
           else if (token == "+" || token == "-")
           {
               if (operators.Count > 0 && operators.Peek() == "+")
               {
                   double right = values.Pop();
                   double left = values.Pop();
                   result = right + left;
                   values.Push(result);
                   operators.Pop();
               }
               else if (operators.Count > 0 && operators.Peek() == "-")
               {
                   double right = values.Pop();
                   double left = values.Pop();
                   result = left - right;
                   values.Push(result);
                   operators.Pop();
               }
               
               operators.Push(token);
               
           }
           
           // Token is either * or / or (
           else if (token == "*" || token == "/"|| token == "(")
           {
               operators.Push(token);
           }
           
           
           //Token  is )
           else if (token == ")")
           {
               if (operators.Count > 0 && operators.Peek() == "+")
               {
                   double right = values.Pop();
                   double left = values.Pop();
                   result = right + left;
                   values.Push(result);
                   operators.Pop(); // we delete the +
                  
               }
               
               else if (operators.Count > 0 && operators.Peek() == "-")
               {
                   double right = values.Pop();
                   double left = values.Pop();
                   result = left - right;
                   values.Push(result);
                   operators.Pop(); // we delete the -
                   
               }

               operators.Pop();

               if (operators.Count > 0 && operators.Peek() == "*")
               {
                   double right = values.Pop();
                   double left = values.Pop();

                   result = right * left;
                   values.Push(result);
                   operators.Pop();
               }
               else if (operators.Count > 0 && operators.Peek() == "/")
               {
                   double right = values.Pop();
                   double left = values.Pop();
                   if (right == 0)
                   {
                       return new FormulaError("A number cannot be divided by 0");
                   }

                   result = left/right;
                   values.Push(result);
                   operators.Pop();
               }
               
               
           }
           
          
       }

       if (operators.Count == 0)
       {
           return values.Pop();
       }
       else
       {
           double right = values.Pop();
           double left = values.Pop();
           string op = operators.Pop();

           if (op == "+")
           {
               return left + right;
           }
           else if (op == "-")
           {
               return left - right;
           }

           return new FormulaError("Unexpected operator");
       }
    }
    
    /// <summary>
    ///   <para>
    ///     Returns a hash code for this Formula.  If f1.Equals(f2), then it must be the
    ///     case that f1.GetHashCode() == f2.GetHashCode().  Ideally, the probability that two
    ///     randomly-generated unequal Formulas have the same hash code should be miniscule.
    ///   </para>
    /// </summary>
    /// <returns> The hashcode for the object. </returns>
    public override int GetHashCode( )
    {
        return ToString().GetHashCode();
    }
}


/// <summary>
///   Used to report syntax errors in the argument to the Formula constructor.
/// </summary>
public class FormulaFormatException : Exception
{
    /// <summary>
    ///   Initializes a new instance of the <see cref="FormulaFormatException"/> class.
    ///   <para>
    ///      Constructs a FormulaFormatException containing the explanatory message.
    ///   </para>
    /// </summary>
    /// <param name="message"> A developer defined message describing why the exception occured.</param>
    public FormulaFormatException( string message )
        : base( message )
    {
        // All this does is call the base constructor. No extra code needed.
    }
    
 
}
/// <summary>
/// Used as a possible return value of the Formula.Evaluate method.
/// </summary>
public class FormulaError
{
    /// <summary>
    ///   Initializes a new instance of the <see cref="FormulaError"/> class.
    ///   <para>
    ///     Constructs a FormulaError containing the explanatory reason.
    ///   </para>
    /// </summary>
    /// <param name="message"> Contains a message for why the error occurred.</param>
    public FormulaError( string message )
    {
        Reason = message;
    }

    /// <summary>
    ///  Gets the reason why this FormulaError was created.
    /// </summary>
    public string Reason { get; private set; }
}

/// <summary>
///   Any method meeting this type signature can be used for
///   looking up the value of a variable.
/// </summary>
/// <exception cref="ArgumentException">
///   If a variable name is provided that is not recognized by the implementing method,
///   then the method should throw an ArgumentException.
/// </exception>
/// <param name="variableName">
///   The name of the variable (e.g., "A1") to lookup.
/// </param>
/// <returns> The value of the given variable (if one exists). </returns>
public delegate double Lookup( string variableName );
