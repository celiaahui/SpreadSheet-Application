// <copyright file="FormulaSyntaxTests.cs" company="UofU-CS3500">
//   Copyright 2024 UofU-CS3500. All rights reserved.
// </copyright>
// <authors> Celia AHUI </authors>
// <date> 09/04/2026 </date>

namespace FormulaTests;

using Formula;

/// <summary>
///   <para>
///     The following class shows the basics of how to use the MSTest framework,
///     including:
///   </para>
///   <list type="number">
///     <item> How to catch exceptions. </item>
///     <item> How a test of valid code should look. </item>
///   </list>
/// </summary>
[TestClass]
public class FormulaSyntaxTests
{
    // --- Tests for One Token Rule ---

    /// <summary>
    ///   <para>
    ///     This test makes sure the right kind of exception is thrown
    ///     when trying to create a formula with no tokens.
    ///   </para>
    ///   <remarks>
    ///     <list type="bullet">
    ///       <item>
    ///         We use the _ (discard) notation because the formula object
    ///         is not used after that point in the method.  Note: you can also
    ///         use _ when a method must match an interface but does not use
    ///         some of the required arguments to that method.
    ///       </item>
    ///       <item>
    ///         string.Empty is often considered best practice (rather than using "") because it
    ///         is explicit in intent (e.g., perhaps the coder forgot to but something in "").
    ///       </item>
    ///       <item>
    ///         The name of a test method should follow the MS standard:
    ///         https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices
    ///       </item>
    ///       <item>
    ///         All methods should be documented, but perhaps not to the same extent
    ///         as this one.  The remarks here are for your educational
    ///         purposes (i.e., a developer would assume another developer would know these
    ///         items) and would be superfluous in your code.
    ///       </item>
    ///       <item>
    ///         Notice the use of the attribute tag [ExpectedException] which tells the test
    ///         that the code should throw an exception, and if it doesn't an error has occurred;
    ///         i.e., the correct implementation of the constructor should result
    ///         in this exception being thrown based on the given poorly formed formula.
    ///       </item>
    ///     </list>
    ///   </remarks>
    ///   <example>
    ///     <code>
    ///        // here is how we call the formula constructor with a string representing the formula
    ///        _ = new Formula( "5+5" );
    ///     </code>
    ///   </example>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestNoTokens_Invalid()
    {
        Assert.Throws<FormulaFormatException>(() => _ = new Formula(""));
        // note: it is arguable that you should replace "" with string.Empty for readability and clarity of intent (e.g., not a cut-and-paste error or a "I forgot to put something there" error).
    }

    [TestMethod]
    public void FormulaConstructor_TestOneTokenRule_Valid()
    {
        _ = new Formula("a1");
    }

    // --- Tests for Valid Token Rule ---
    [TestMethod]
    public void FormulaConstructor_TestValidTokenRule_valid()
    {
        _ = new Formula("(25*7)");
    }

    [TestMethod]
    public void FormulaConstructor_TestTokenHashtag_Invalid()
    {
        Assert.Throws<FormulaFormatException>(() => _ = new Formula("#+#"));
    }

    [TestMethod]
    public void FormulaConstructor_TestTokenDollars_Invalid()
    {
        Assert.Throws<FormulaFormatException>(() => _ = new Formula("(5+6+$)"));
    }

    [TestMethod]
    public void FormulaConstructor_TestTokenAtSymbol_Invalid()
    {
        Assert.Throws<FormulaFormatException>(() => _ = new Formula("(a3 @ a2)"));
    }

    [TestMethod]
    public void FormulaConstructor_TestTokenAmpersand_Invalid()
    {
        Assert.Throws<FormulaFormatException>(() => _ = new Formula("(ba5 & c8)"));
    }

    [TestMethod]
    public void FormulaConstructor_TestTokePercentageSymbol_Invalid()
    {
        Assert.Throws<FormulaFormatException>(() => _ = new Formula("(e25%)"));
    }

    [TestMethod]
    public void FormulaConstructor_TestTokenInvalidEqualSignTest_Invalid()
    {
        Assert.Throws<FormulaFormatException>(() => _ = new Formula("(a2 = a2)"));
    }



    // --- Tests for Closing Parenthesis Rule

    [TestMethod]
    public void FormulaConstructor_TestValidClosingParenthesisRule_Valid()
    {
        _ = new Formula("(((1+2)+3)*4)");
    }

    [TestMethod]
    public void FormulaConstructor_TestMoreClosingParenthesis_Invalid()
    {
        Assert.Throws<FormulaFormatException>(() => _ = new Formula("((1*2)+2+4)*5)"));
    }
    // --- Tests for Balanced Parentheses Rule

    [TestMethod]
    public void FormulaConstructor_TestValidBalancedParenthesesRule_Valid()
    {
        _ = new Formula("(5+((3*7)-2))");
    }

    [TestMethod]
    public void FormulaConstructor_MoreOpeningParenthesis_Invalid()
    {
        Assert.Throws<FormulaFormatException>(() => _ = new Formula("((5+((3*7)-2))"));
    }

    [TestMethod]
    public void FormulaConstructor_lessOpeningParenthesis_Invalid()
    {
        Assert.Throws<FormulaFormatException>(() => _ = new Formula("((5+3*7)-2))"));
    }
    // --- Tests for First Token Rule

    /// <summary>
    ///   <para>
    ///     Make sure a simple well-formed formula is accepted by the constructor (the constructor
    ///     should not throw an exception).
    ///   </para>
    ///   <remarks>
    ///     This is an example of a test that is not expected to throw an exception, i.e., it succeeds.
    ///     In other words, the formula "1+1" is a valid formula which should not cause any errors.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestFirstTokenNumber_Valid()
    {
        _ = new Formula("1+1");
    }

    [TestMethod]
    public void FormulaConstructor_TestFirstTokenOpeningParenthesis_Valid()
    {
        _ = new Formula("(1+1)");
    }

    [TestMethod]
    public void FormulaConstructor_TestFirstTokenVariable_Valid()
    {
        _ = new Formula(" ab85 + 85");
    }

    [TestMethod]
    public void FormulaConstructor_TestFirstTokenClosingParenthesis_Invalid()
    {
        Assert.Throws<FormulaFormatException>(() => _ = new Formula(")2+5)"));
    }

    [TestMethod]
    public void FormulaConstructor_TestFirstTokenAdditionOperator_Invalid()
    {
        Assert.Throws<FormulaFormatException>(() => _ = new Formula("+2-5"));
    }


    // --- Tests for  Last Token Rule ---
    [TestMethod]
    public void FormulaConstructor_TestLastTokenNumber_Valid()
    {
        _ = new Formula("2-5");
    }

    [TestMethod]
    public void FormulaConstructor_TestLastTokenVariable_Valid()
    {
        _ = new Formula("hg2 + ab2");
    }

    [TestMethod]
    public void FormulaConstructor_TestLastTokenClosingParenthesis_Valid()
    {
        _ = new Formula("(1+2) + (5*78)");
    }


    [TestMethod]
    public void FormulaConstructor_TestLastTokenOperator_Invalid()
    {
        Assert.Throws<FormulaFormatException>(() => _ = new Formula("1+"));
    }

    [TestMethod]
    public void FormulaConstructor_TestLastTokenInvalidVariable_Invalid()
    {
        Assert.Throws<FormulaFormatException>(() => _ = new Formula("1+1b"));
    }

    [TestMethod]
    public void FormulaConstructor_TestLastTokenAtSymbol_Invalid()
    {
        Assert.Throws<FormulaFormatException>(() => _ = new Formula("1+2+@"));
    }
    // --- Tests for Parentheses/Operator Following Rule ---

    [TestMethod]
    public void FormulaConstructor_TestOpeningParenthesisFollowedByOpeningParenthesis_Valid()
    {
        _ = new Formula("((1+1)*2)");
    }

    [TestMethod]
    public void FormulaConstructor_TestOpeningParenthesisFollowedByVariable_Valid()
    {
        _ = new Formula("(after24 + 5)");
    }

    [TestMethod]
    public void FormulaConstructor_TestOpeningParenthesisFollowedByNumber_Valid()
    {
        _ = new Formula("(25 + 5)");
    }

    [TestMethod]
    public void FormulaConstructor_TestOpeningParenthesisFollowedByClosingParenthesis_Invalid()
    {
        Assert.Throws<FormulaFormatException>(() => _ = new Formula("(52 +25()* 37)"));
    }

    [TestMethod]
    public void FormulaConstructor_TestOpeningParenthesisFollowedByInvalidVariable_Invalid()
    {
        Assert.Throws<FormulaFormatException>(() => _ = new Formula("(25av +3 )"));
    }

    [TestMethod]
    public void FormulaConstructor_TestOpeningParenthesisFollowedByPercentageSymbol_Invalid()
    {
        Assert.Throws<FormulaFormatException>(() => _ = new Formula("(%25 + 3)"));
    }

    [TestMethod]
    public void FormulaConstructor_TestOperatorFollowedByNumber_Valid()
    {
        _ = new Formula("abc1 + 2");
    }

    [TestMethod]
    public void FormulaConstructor_TestOperatorFollowedByVariable_Valid()
    {
        _ = new Formula("ac1 - a2");
    }

    [TestMethod]
    public void FormulaConstructor_TestOperatorFollowedByOpeningParenthesis_Valid()
    {
        _ = new Formula("abc1 *(5 +2)");
    }

    [TestMethod]
    public void FormulaConstructor_TestOperatorFollowedByClosingParenthesis_Invalid()
    {
        Assert.Throws<FormulaFormatException>(() => _ = new Formula("(abe125 /)27)"));
    }

    [TestMethod]
    public void FormulaConstructor_TestOperatorFollowedByClosingInvalidVariable_Invalid()
    {
        Assert.Throws<FormulaFormatException>(() => _ = new Formula("(abe125 + 27a )"));
    }

    [TestMethod]
    public void FormulaConstructor_TestOperatorFollowedByOperator_Invalid()
    {
        Assert.Throws<FormulaFormatException>(() => _ = new Formula("(abe125 + + )"));
    }

    [TestMethod]
    public void FormulaConstructor_TestOperatorFollowedByDollarsSymbol_Invalid()
    {
        Assert.Throws<FormulaFormatException>(() => _ = new Formula("(abe125 + $ )"));
    }

    [TestMethod]
    public void FormulaConstructor_TestParenthesisRule_Invalid()
    {
        Assert.Throws<FormulaFormatException>(() => _ = new Formula("(%+2)"));
    }

    // --- Tests for Extra Following Rule ---
    [TestMethod]
    public void FormulaConstructor_TestNumberFollowedByNumber_Invalid()
    {
        Assert.Throws<FormulaFormatException>(() => _ = new Formula("(1 1)"));
    }

    [TestMethod]
    public void FormulaConstructor_TestNumberFollowedByVariable_Invalid()
    {
        Assert.Throws<FormulaFormatException>(() => _ = new Formula("(1 Then1)"));
    }

    [TestMethod]
    public void FormulaConstructor_TestVariableFollowedByNumber_Invalid()
    {
        Assert.Throws<FormulaFormatException>(() => _ = new Formula("(First1 2)"));
    }

    [TestMethod]
    public void FormulaConstructor_TestVariableFollowedByVariable_Invalid()
    {
        Assert.Throws<FormulaFormatException>(() => _ = new Formula("(First1 Second2)"));
    }

    [TestMethod]
    public void FormulaConstructor_TestNumberFollowedByParenthesis_Invalid()
    {
        Assert.Throws<FormulaFormatException>(() => _ = new Formula("5 (3+2)"));
    }

    [TestMethod]
    public void FormulaConstructor_TestVariableFollowedByParenthesis_Invalid()
    {
        Assert.Throws<FormulaFormatException>(() => _ = new Formula("x2 (3+2)"));
    }

    [TestMethod]
    public void FormulaConstructor_TestClosingParenthesisFollowedByNumber_Invalid()
    {
        Assert.Throws<FormulaFormatException>(() => _ = new Formula("(3+2) 5"));
    }

    [TestMethod]
    public void FormulaConstructor_TestClosingParenthesisFollowedByVariable_Invalid()
    {
        Assert.Throws<FormulaFormatException>(() => _ = new Formula("(3+2) x2"));
    }

    [TestMethod]
    public void FormulaConstructor_TestClosingParenthesisFollowedByOpeningParenthesis_Invalid()
    {
        Assert.Throws<FormulaFormatException>(() => _ = new Formula("(3+2) (8*7)"));
    }
    // WRITE 60 TESTS 

    //Test for upper case letters 
    [TestMethod]
    public void FormulaConstructor_TestValidUpperCaseLetters_Valid()
    {
        _ = new Formula("AA1 + BB2");
    }

    //Test for lower case letters tokens
    [TestMethod]
    public void FormulaConstructor_TestValidLowerCaseToken_Valid()
    {
        _ = new Formula("after4");
    }

    //Test for negative number
    [TestMethod]
    public void FormulaConstructor_TestNegativeNumber_Invalid()
    {
        Assert.Throws<FormulaFormatException>(() => _ = new Formula("-1+55"));
    }

    //Tests for variable tokens
    [TestMethod]
    public void FormulaConstructor_TestValidVariableToken_Valid()
    {
        _ = new Formula("(Weave123 - abc58)");
    }

    [TestMethod]
    public void FormulaConstructor_TestInvalidVariableToken_Invalid()
    {
        Assert.Throws<FormulaFormatException>(() => _ = new Formula("(2158a+5)"));
    }

    //Test for Brackets and Square Brackets
    [TestMethod]
    public void FormulaConstructor_TestBracketInsteadOfParenthesis_Invalid()
    {
        Assert.Throws<FormulaFormatException>(() => _ = new Formula("{3*7}"));
    }

    [TestMethod]
    public void FormulaConstructor_TestSquareBracketsInsteadOfParenthesis_Invalid()
    {
        Assert.Throws<FormulaFormatException>(() => _ = new Formula("[3*7]"));
    }

    //Test for expressions containing space
    [TestMethod]
    public void FormulaConstructor_TestExpressionWithSpace_Valid()
    {
        _ = new Formula("(3 * 7)");
    }

    //Other Tests
    [TestMethod]
    public void FormulaConstructor_TestDeepNestedExpression_Valid()
    {
        _ = new Formula("(((a1+25)*(b2-7))/(c3+4))");
    }

    [TestMethod]
    public void FormulaConstructor_TestMultipleNestedVariables_Valid()
    {
        _ = new Formula("((abc12+def34)*(ghi56/(jkl78-9)))");
    }

    [TestMethod]
    public void FormulaConstructor_TestDeepExtraClosingParenthesis_Invalid()
    {
        Assert.Throws<FormulaFormatException>(() => _ = new Formula("((a1+5)*(b2-(7+3))))"));
    }

    [TestMethod]
    public void FormulaConstructor_TestDeepMissingClosingParenthesis_Invalid()
    {
        Assert.Throws<FormulaFormatException>(() => _ = new Formula("(((a1+5)*(b2-7))/(c3+4)"));
    }

    [TestMethod]
    public void FormulaConstructor_TestOperatorSequenceInsideNestedFormula_Invalid()
    {
        Assert.Throws<FormulaFormatException>(() => _ = new Formula("((a1+5)*(b2/+7))"));
    }

    [TestMethod]
    public void FormulaConstructor_TestMissingOperatorBetweenNumberAndVariable_Invalid()
    {
        Assert.Throws<FormulaFormatException>(() => _ = new Formula("((a1+5)*(27 b2))"));
    }

    [TestMethod]
    public void FormulaConstructor_TestAdjacentNestedParentheses_Invalid()
    {
        Assert.Throws<FormulaFormatException>(() => _ = new Formula("((a1+5)(b2-7))"));
    }

    [TestMethod]
    public void FormulaConstructor_TestVariableImmediatelyBeforeParent_Invalid()
    {
        Assert.Throws<FormulaFormatException>(() => _ = new Formula("(abc12*(def34(5+7)))"));
    }


    [TestMethod]
    public void FormulaConstructor_TestEmptyParenthesesDeepInside_Invalid()
    {
        Assert.Throws<FormulaFormatException>(() => _ = new Formula("((a1+5)*(b2+()))"));
    }
    
    [TestMethod]
    public void FormulaConstructor_TestNullFormula_Invalid()
    {
        Assert.Throws<FormulaFormatException>(() => new Formula(null));
    }
    
    //Test for ToString()
    [TestMethod]
    public void ToString_TestVariableNormalization_Valid()
    {
        Formula formula = new Formula("x1 + Y1");

        string result = formula.ToString();

        Assert.AreEqual("X1+Y1", result);
    }
    
    [TestMethod]
    public void ToString_TestNumberNormalization_Valid()
    {
        Formula formula = new Formula("x1 + 5.0000");

        string result = formula.ToString();

        Assert.AreEqual("X1+5", result);
    }
    
    [TestMethod]
    public void ToString_TestDecimalNormalization_Valid()
    {
        Formula formula = new Formula("x1 * 2.500");

        string result = formula.ToString();

        Assert.AreEqual("X1*2.5", result);
    }
    
    [TestMethod]
    public void ToString_TestParenthesesPreserved_Valid()
    {
        Formula formula = new Formula("(x1 + 5) * y2");

        string result = formula.ToString();

        Assert.AreEqual("(X1+5)*Y2", result);
    }
    
    [TestMethod]
    public void ToString_TestNestedParenthesesPreserved_Valid()
    {
        Formula formula = new Formula("((x1 + 5) * (y2 - 3.00))");

        string result = formula.ToString();

        Assert.AreEqual("((X1+5)*(Y2-3))", result);
    }
    
    [TestMethod]
    public void ToString_TestAlreadyNormalizedFormula_Valid()
    {
        Formula formula = new Formula("X1+5*Y2");

        string result = formula.ToString();

        Assert.AreEqual("X1+5*Y2", result);
    }
    
    [TestMethod]
    public void ToString_TestSingleVariableNormalization_Valid()
    {
        Formula formula = new Formula("abc123");

        string result = formula.ToString();

        Assert.AreEqual("ABC123", result);
    }
    
    [TestMethod]
    public void ToString_TestSingleNumberNormalization_Valid()
    {
        Formula formula = new Formula("007.5000");

        string result = formula.ToString();

        Assert.AreEqual("7.5", result);
    }
    
    [TestMethod]
    public void ToString_TestScientificNotation_Valid()
    {
        Formula formula = new Formula("x1 + 2e5");

        string result = formula.ToString();

        Assert.AreEqual("X1+200000", result);
    }
    
    // Tests for GetVariable
    
    [TestMethod]
    public void GetVariables_TestSingleVariable_Valid()
    {
        Formula formula = new Formula("x1");

        ISet<string> variables = formula.GetVariables();

        Assert.IsTrue(variables.SetEquals(new HashSet<string> { "X1" }));
    }
    
    [TestMethod]
    public void GetVariables_TestMultipleVariables_Valid()
    {
        Formula formula = new Formula("x1+y2*z3");

        ISet<string> variables = formula.GetVariables();

        Assert.IsTrue(variables.SetEquals(new HashSet<string> { "X1", "Y2", "Z3" }));
    }
    
    [TestMethod]
    public void GetVariables_TestDuplicateVariable_Valid()
    {
        Formula formula = new Formula("x1+x1");

        ISet<string> variables = formula.GetVariables();

        Assert.IsTrue(variables.SetEquals(new HashSet<string> { "X1" }));
    }

    
    [TestMethod]
    public void GetVariables_TestDuplicateVariableDifferentCase_Valid()
    {
        Formula formula = new Formula("x1+X1");

        ISet<string> variables = formula.GetVariables();

        Assert.IsTrue(variables.SetEquals(new HashSet<string> { "X1" }));
    }
 
    
    [TestMethod]
    public void GetVariables_TestNoVariables_Valid()
    {
        Formula formula = new Formula("5+3*(2+1)");

        ISet<string> variables = formula.GetVariables();

        Assert.IsEmpty(variables);
    }

    [TestMethod]
    public void GetVariables_TestVariablesInsideParentheses_Valid()
    {
        Formula formula = new Formula("(x1+y2)*(z3+5)");

        ISet<string> variables = formula.GetVariables();

        Assert.IsTrue(variables.SetEquals(new HashSet<string> { "X1", "Y2", "Z3" }));
    }

    [TestMethod]
    public void GetVariables_TestLongVariableNames_Valid()
    {
        Formula formula = new Formula("abc123+AAab1");

        ISet<string> variables = formula.GetVariables();

        Assert.IsTrue(variables.SetEquals(new HashSet<string> { "ABC123", "AAAB1" }));
    }

    [TestMethod]
    public void GetVariables_TestRepeatedVariablesManyTimes_Valid()
    {
        Formula formula = new Formula("x1+x1*x1-X1");

        ISet<string> variables = formula.GetVariables();

        Assert.IsTrue(variables.SetEquals(new HashSet<string> { "X1" }));
    }

    [TestMethod]
    public void GetVariables_TestMixedCaseVariables_Valid()
    {
        Formula formula = new Formula("a1+Bb2+cCC3");

        ISet<string> variables = formula.GetVariables();

        Assert.IsTrue(variables.SetEquals(new HashSet<string> { "A1", "BB2", "CCC3" }));
    }

    
    [TestMethod]
    public void GetVariables_TestReturnedSetDoesNotModifyFormula_Valid()
    {
        Formula formula = new Formula("x1+y2");

        ISet<string> variables = formula.GetVariables();

        variables.Clear();

        ISet<string> variablesAgain = formula.GetVariables();

        Assert.IsTrue(variablesAgain.SetEquals(new HashSet<string> { "X1", "Y2" }));
    }
    
    //Test for the overloaded == methods
    [TestMethod]
    public void OperatorEquals_TestTwoIdenticalFormulas_Valid()
    {
        Formula f1 = new Formula("A5+7");
        Formula f2 = new Formula("A5+7");
        
        Assert.IsTrue(f1 == f2);
    }
    
    [TestMethod]
    public void OperatorEquals_TestSameReference_Valid()
    {
        Formula f1 = new Formula("A5+7");
        Formula f2 = f1;
        
        Assert.IsTrue(f1 == f2);
    }
    
    [TestMethod]
    public void OperatorEquals_TestOneNullFormula_Invalid()
    {
        Formula f1 = new Formula("A5+7");
        Formula? f2 = null;
        
        Assert.IsFalse(f1 == f2!);
    }
    
    [TestMethod]
    public void OperatorEquals_TestDifferentFormulas_Invalid()
    {
        Formula f1 = new Formula("A5+7");
        Formula f2 = new Formula("A5*7");
        
        Assert.IsFalse(f1 == f2);
    }
    
    [TestMethod]
    public void OperatorEquals_TestBothNull_Valid()
    {
        Formula? f1 = null;
        Formula? f2 = null;
        
        Assert.IsTrue(f1! == f2!);
    }
    
    [TestMethod]
    public void OperatorEquals_TestEquivalentNormalizedFormulas_Valid()
    {
        Formula f1 = new Formula("A5+7.000");
        Formula f2 =new Formula("A5+7");
        
        Assert.IsTrue(f1 == f2);
    }
    
    // Test for operators !=
    [TestMethod]
    public void OperatorNotEquals_TestEquivalentNormalizedFormulas_Valid()
    {
        Formula f1 = new Formula("A5+7.000");
        Formula f2 =new Formula("A5+7");
        
        Assert.IsFalse(f1 != f2);
    }
    
    [TestMethod]
    public void OperatorNotEquals_TestBothNull_Valid()
    {
        Formula? f1 = null;
        Formula? f2 = null;
        
        Assert.IsFalse(f1! != f2!);
    }
    
    [TestMethod]
    public void OperatorNotEquals_TestTwoIdenticalFormulas_Valid()
    {
        Formula f1 = new Formula("A5+7");
        Formula f2 = new Formula("A5+7");
        
        Assert.IsFalse(f1 != f2);
    }
    
    [TestMethod]
    public void OperatorNotEquals_TestSameReference_Valid()
    {
        Formula f1 = new Formula("A5+7");
        Formula f2 = f1;
        
        Assert.IsFalse(f1 != f2);
    }
    
    [TestMethod]
    public void OperatorNotEquals_TestOneNullFormula_Invalid()
    {
        Formula f1 = new Formula("A5+7");
        Formula? f2 = null;
        
        Assert.IsFalse(f1 == f2!);
    }
    
    [TestMethod]
    public void OperatorNotEquals_TestDifferentFormulas_Invalid()
    {
        Formula f1 = new Formula("A5+7");
        Formula f2 = new Formula("A5*7");
        
        Assert.IsTrue(f1!= f2);
    }
    
    //Tests for Equals
    [TestMethod]
    public void Equals_TestSameFormula_Valid()
    {
        Formula f1 = new Formula("X1+5");
        Formula f2 = new Formula("X1+5");
    
        Assert.IsTrue(f1.Equals(f2));
    }
    
    [TestMethod]
    public void Equals_TestSameCanonicalFormula_Valid()
    {
        Formula f1 = new Formula("x1 + 5.000");
        Formula f2 = new Formula("X1+5");

        Assert.IsTrue(f1.Equals(f2));
    }
    
    [TestMethod]
    public void Equals_TestDifferentFormula_Invalid()
    {
        Formula f1 = new Formula("X1+5");
        Formula f2 = new Formula("X1+6");
    
        Assert.IsFalse(f1.Equals(f2));
    }
    
    [TestMethod]
    public void Equals_TestNull_Invalid()
    {
        Formula f1 = new Formula("X1+5");

        Assert.IsFalse(f1.Equals(null));
    }
    
    [TestMethod]
    public void Equals_TestNotFormula_Invalid()
    {
        Formula f1 = new Formula("X1+5");
        object other = "X1+5";

        Assert.IsFalse(f1.Equals(other));
    }
    
    [TestMethod]
    public void Equals_TestSameObject_valid()
    {
        Formula f1 = new Formula("X1+5");

        Object other = f1;
        Assert.IsTrue(f1.Equals(other));
    }
    
    //Tests Evaluate
    [TestMethod]
    public void Evaluate_TestAddition_ReturnsCorrectValue()
    {
        Formula f = new Formula("2+4");
        object result = f.Evaluate(v => 0);
        Assert.AreEqual(6.0, result);
    }
    
    [TestMethod]
    public void Evaluate_TestSubtraction_ReturnsCorrectValue()
    {
         Formula f = new Formula("4-1"); 
         object result = f.Evaluate(v => 0);
         Assert.AreEqual(3.0, result);
    }

    [TestMethod]
    public void Evaluate_TestMultiplication_ReturnsCorrectValue()
    {
        Formula f = new Formula("4*10"); 
        object result = f.Evaluate(v => 0);
        Assert.AreEqual(40.0, result); 
    }

    [TestMethod]
    public void Evaluate_TestDivisionPerZero_ReturnFormulaError()
    {
        Formula f = new Formula("5/0");
        object result = f.Evaluate(v => 0);
        Assert.IsInstanceOfType(result, typeof(FormulaError));
    }
    
    [TestMethod]
    public void Evaluate_TestOperatorPrecedence_ReturnsCorrectValue()
    {
        Formula f = new Formula("2+3*4");
    
        object result = f.Evaluate(v => 0);
    
        Assert.AreEqual(14.0, result);
    }

    [TestMethod]
    public void Evaluate_TestParenthesis_ReturnsCorrectValue()
    {
        Formula f = new Formula("20 - (4*3)");
        object result = f.Evaluate(v => 0);
        
        Assert.AreEqual(8.0, result);
    }

    [TestMethod]
    public void Evaluate_TestComplexExpression_ReturnsCorrectValue()
    {
        Formula f = new Formula("(2+3)/2+2");
        object result = f.Evaluate(v => 0);
        Assert.AreEqual(4.5, result);
    }

    [TestMethod]
    public void Evaluate_TestWithVariable_ReturnsCorrectValue()
    {
        Formula f = new Formula("(2+A6)*5+2");

        object result = f.Evaluate(v =>
        {
            if (v == "A6")
            {
                return 7;
            }

            throw new ArgumentException();
        });
        Assert.AreEqual(47.0,result);
    }
    
    [TestMethod]
    public void Evaluate_TestUndefinedVariable_ReturnsFormulaError()
    {
        Formula f = new Formula("A1+2");
    
        object result = f.Evaluate(v => throw new ArgumentException());
    
        Assert.IsInstanceOfType(result, typeof(FormulaError));
    }

    [TestMethod]
    public void Evaluate_TestDivisionByParenthesis_ReturnsCorrectValue()
    {
        Formula f = new Formula("20/(2+3)");

        object result = f.Evaluate(v => 0);

        Assert.AreEqual(4.0, result);
    }
    
    [TestMethod]
    public void Evaluate_TestAdditionBeforeParentheses_ReturnsCorrectValue()
    {
        Formula f = new Formula("1+(2+3)");

        object result = f.Evaluate(v => 0);

        Assert.AreEqual(6.0, result);
    }
    
    [TestMethod]
    public void Evaluate_TestSingleNumber_ReturnsCorrectValue()
    {
        Formula f = new Formula("5");
        object result = f.Evaluate(v => 0);

        Assert.AreEqual(5.0, result);
    }
    
    [TestMethod]
    public void Evaluate_TestLeftToRightSubtraction_ReturnsCorrectValue()
    {
        Formula f = new Formula("10-3-2");
        object result = f.Evaluate(v => 0);

        Assert.AreEqual(5.0, result);
    }
    
    [TestMethod]
    public void Evaluate_TestParenthesisWithoutInnerOperator_ReturnsCorrectValue()
    {
        Formula f = new Formula("5*(2)");
        object result = f.Evaluate(v => 0);

        Assert.AreEqual(10.0, result);
    }

    [TestMethod]
    public void Evaluate_TestNestedParentheses_ReturnsCorrectValue()
    {
        Formula f = new Formula("2*(3+(4*2))");
        object result = f.Evaluate(v => 0);

        Assert.AreEqual(22.0, result);
    }
    
    //Tests for GetHashCode
    [TestMethod]
    public void GetHashCode_TestEqualFormulas_ReturnsCorrectResult()
    {
        Formula f1 = new Formula("a5+7.0");
        Formula f2 = new Formula("A5+7");

        Assert.AreEqual(f1.GetHashCode(), f2.GetHashCode());
    }
    
    [TestMethod]
    public void GetHashCode_TestSameFormulaDifferentSpacing_ReturnsSameHashCode()
    {
        Formula f1 = new Formula("A1 + 2");
        Formula f2 = new Formula("A1+2");

        Assert.AreEqual(f1.GetHashCode(), f2.GetHashCode());
    }
    
    [TestMethod]
    public void GetHashCode_TestSameCanonicalFormula_ReturnsSameHashCode()
    {
        Formula f1 = new Formula("x1+5.000");
        Formula f2 = new Formula("X1+5");

        Assert.AreEqual(f1.GetHashCode(), f2.GetHashCode());
    }
    
    [TestMethod]
    public void GetHashCode_TestSameObject_ReturnsSameHashCode()
    {
        Formula f = new Formula("A5+7");

        int firstHash = f.GetHashCode();
        int secondHash = f.GetHashCode();

        Assert.AreEqual(firstHash, secondHash);
    }
    
    [TestMethod]
    public void GetHashCode_TestComplexEqualFormulas_ReturnSameHashCode()
    {
        Formula f1 = new Formula("(x1+5.0)*Y2");
        Formula f2 = new Formula("(X1+5)*y2");
    
        Assert.AreEqual(f1.GetHashCode(), f2.GetHashCode());
    }
    
    
 
    
    
    
    
    
    
    
    
    
    
    
    
}
    
    

    
