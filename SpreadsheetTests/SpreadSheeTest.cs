namespace SpreadsheetTests;
using Spreadsheets;
using Formula;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
[TestClass]
public sealed class SpreadSheetTest
{
    
    // Primary Tests
    [TestMethod]
    public void GetNamesOfAllNonEmptyCells_TestNewSpreadSheet_ReturnsEmpty()
    {
        Spreadsheet spreadSheet = new Spreadsheet();
        spreadSheet.SetContentsOfCell("A1", "5.0");
        Assert.AreEqual(1,spreadSheet.GetNamesOfAllNonemptyCells().Count());
    }
    
    [TestMethod]
    public void GetCellContents_TestInvalidCellName_ThrownException()
    {
        Spreadsheet spreadSheet = new Spreadsheet();
        Assert.Throws<InvalidNameException>(() => spreadSheet.GetCellContents("25BB"));
    }
    
    [TestMethod]
    public void SetCellContents_TestInvalidCellName_ThrownException()
    {
        Spreadsheet spreadSheet = new Spreadsheet();
        Assert.Throws<InvalidNameException>(() => spreadSheet.SetContentsOfCell("2l68", "Bronson"));
    }
    
    // TESTS FOR  GetNamesOfAllNonemptyCells
    [TestMethod]
    public void GetNamesOfAllNonemptyCells_TestEmptySpreadSheet_ReturnsEmpty()
    {
        Spreadsheet spreadSheet = new Spreadsheet();
        Assert.AreEqual(0,spreadSheet.GetNamesOfAllNonemptyCells().Count());
    }

    [TestMethod]
    public void GetNamesOfAllNonemptyCells_TestEmptyString_ReturnsEmpty()
    {
        Spreadsheet spreadSheet = new Spreadsheet();
        spreadSheet.SetContentsOfCell("A1", "");
        Assert.AreEqual(0,spreadSheet.GetNamesOfAllNonemptyCells().Count());
    }

    [TestMethod]
    public void GetNamesOfAllNonemptyCells_TestFilledSpreadSheet_ReturnsExpectedValue()
    {
        Spreadsheet spreadSheet = new Spreadsheet();
        spreadSheet.SetContentsOfCell("A1", "9");
        spreadSheet.SetContentsOfCell("B1", "Bonjour");
        spreadSheet.SetContentsOfCell("C1", "=A1 +B2");
        spreadSheet.SetContentsOfCell("D1", "4");
        spreadSheet.SetContentsOfCell("E1", "");
        Assert.AreEqual(4,spreadSheet.GetNamesOfAllNonemptyCells().Count());
    }
    
    // Test for GetCellContents
    [TestMethod]
    public void GetCellContents_TestInvalidName_ThrownException()
    {
        Spreadsheet spreadSheet = new Spreadsheet();
        spreadSheet.SetContentsOfCell("A1", "9");
        spreadSheet.SetContentsOfCell("B1", "Bonjour");
        spreadSheet.SetContentsOfCell("C1", "=A1 +B2");
        spreadSheet.SetContentsOfCell("D1", "4");
        Assert.Throws<InvalidNameException>(() => spreadSheet.GetCellContents("A5E4"));
    }
    
    
    [TestMethod]
    public void GetCellContents_TestInvalidNameOnlyNumbers_ThrownException()
    {
        Spreadsheet spreadSheet = new Spreadsheet();
        spreadSheet.SetContentsOfCell("A1", "9");
        spreadSheet.SetContentsOfCell("B1", "Bonjour");
        spreadSheet.SetContentsOfCell("C1", "=A1 +B2");
        spreadSheet.SetContentsOfCell("D1", "4");
        Assert.Throws<InvalidNameException>(() => spreadSheet.GetCellContents("58"));
    }
    
    [TestMethod]
    public void GetCellContents_TestInvalidNameOnlyLetters_ThrownException()
    {
        Spreadsheet spreadSheet = new Spreadsheet();
        spreadSheet.SetContentsOfCell("A1", "9");
        spreadSheet.SetContentsOfCell("B1", "Bonjour");
        spreadSheet.SetContentsOfCell("C1", "=A1 +B2");
        spreadSheet.SetContentsOfCell("D1", "4");
        Assert.Throws<InvalidNameException>(() => spreadSheet.GetCellContents("AND"));
    }
    
    [TestMethod]
    public void GetCellContents_TestInvalidNameEmptyString_ThrownException()
    {
        Spreadsheet spreadSheet = new Spreadsheet();
        spreadSheet.SetContentsOfCell("A1", "9");
        spreadSheet.SetContentsOfCell("B1", "Bonjour");
        spreadSheet.SetContentsOfCell("C1", "=A1 +B2");
        spreadSheet.SetContentsOfCell("D1", "4");
        Assert.Throws<InvalidNameException>(() => spreadSheet.GetCellContents(""));
    }
    
    [TestMethod]
    public void GetCellContents_TestInvalidNameAllNumbersBeforeLetters_ThrownException()
    {
        Spreadsheet spreadSheet = new Spreadsheet();
        spreadSheet.SetContentsOfCell("A1", "9");
        spreadSheet.SetContentsOfCell("B1", "Bonjour");
        spreadSheet.SetContentsOfCell("C1", "=A1 +B2");
        spreadSheet.SetContentsOfCell("D1", "4");
        Assert.Throws<InvalidNameException>(() => spreadSheet.GetCellContents("125AA"));
    }
    
    [TestMethod]
    public void GetCellContents_TestCellContainedInTheSpreadsheet_ThrownException()
    {
        Spreadsheet spreadSheet = new Spreadsheet();
        spreadSheet.SetContentsOfCell("A1", "9.0");
        spreadSheet.SetContentsOfCell("B1", "Bonjour");
        spreadSheet.SetContentsOfCell("C1", "=A1 +B2");
        spreadSheet.SetContentsOfCell("D1", "4.0");
        Assert.AreEqual(9.0, spreadSheet.GetCellContents("A1"));
   
    }
    
    [TestMethod]
    public void GetCellContents_TestCellIsNotInTheSpreadsheet_ThrownException()
    {
        Spreadsheet spreadSheet = new Spreadsheet();
        spreadSheet.SetContentsOfCell("A1", "9.0");
        spreadSheet.SetContentsOfCell("B1", "Bonjour");
        spreadSheet.SetContentsOfCell("C1", "=A1 +B2");
        spreadSheet.SetContentsOfCell("D1", "4.0");
        Assert.AreEqual("", spreadSheet.GetCellContents("A41"));
   
    }
    
    
    [TestMethod]
    public void GetCellContents_TestLowerCaseCellName_ThrownException()
    {
        Spreadsheet spreadSheet = new Spreadsheet();
        spreadSheet.SetContentsOfCell("A1", "9.0");
        spreadSheet.SetContentsOfCell("B1", "Bonjour");
        spreadSheet.SetContentsOfCell("C1", "=A1 +B2");
        spreadSheet.SetContentsOfCell("D1", "4.0");
        Assert.AreEqual(4.0, spreadSheet.GetCellContents("d1"));
   
    }
    
    // Test for SetCellContents(string name, double number)
    [TestMethod]
    public void SetCellContents_TestInvalidName_ThrownException()
    {
        Spreadsheet spreadSheet = new Spreadsheet();
        Assert.Throws<InvalidNameException>(()=>  spreadSheet.SetContentsOfCell("555", "9.0"));
    }

    [TestMethod]
    public void SetCellContents_TestLowerCaseCellName_ReturnExpectedValue()
    {
        Spreadsheet spreadSheet = new Spreadsheet();
        spreadSheet.SetContentsOfCell("A1", "9.0");
        spreadSheet.SetContentsOfCell("B1", "=A1 +3");
       CollectionAssert.AreEqual(new List<string>{"A1", "B1"}, spreadSheet.SetContentsOfCell("a1", "8.0").ToList());
    }

    [TestMethod]
    public void SetCellContents_TestAddingANumberInAnEmptyCell_ReturnExpectedValue()
    {
        Spreadsheet spreadSheet = new Spreadsheet();
        IList<string> result = spreadSheet.SetContentsOfCell("A1", "9.0");
        Assert.AreEqual(9.0, spreadSheet.GetCellContents("A1"));
        CollectionAssert.AreEqual(new List<string> { "A1" }, result.ToList());
    }

    [TestMethod]
    public void SetCellContents_TestAddingANegativeNumber_ReturnExpectedValue()
    {
        Spreadsheet spreadSheet = new Spreadsheet();
        spreadSheet.SetContentsOfCell("A1", "9.0");
        spreadSheet.SetContentsOfCell("B1", "=A1 +B2");
        spreadSheet.SetContentsOfCell("C1", "=A1 * B1");
        IList<string> result = spreadSheet.SetContentsOfCell("A1", "-9.0");
        
        Assert.AreEqual(-9.0, spreadSheet.GetCellContents("A1"));
        CollectionAssert.AreEqual(new List<string> { "A1", "B1", "C1" }, result.ToList());
    }
    
    [TestMethod]
    public void SetCellContents_TestReplacingContentWithANumber_ReturnExpectedValue()
    {
        Spreadsheet spreadSheet = new Spreadsheet();
        spreadSheet.SetContentsOfCell("A1", "9.0");
        spreadSheet.SetContentsOfCell("B1", "=A1 + 7");
        IList<string> result = spreadSheet.SetContentsOfCell("A1", "-9.0");
        CollectionAssert.AreEqual(new List<string> { "A1", "B1" }, result.ToList());
        
        spreadSheet.SetContentsOfCell("B1", "0.0");
        IList<string> result2 = spreadSheet.SetContentsOfCell("A1", "-78.0");
        CollectionAssert.AreEqual(new List<string> { "A1" }, result2.ToList());
    }
    
    //Tests For  SetCellContents(string name, string text)

    [TestMethod]
    public void SetCellContents_TestInvalidName_ThrowException()
    {
        Spreadsheet spreadSheet = new Spreadsheet();
        Assert.Throws<InvalidNameException>(() => spreadSheet.SetContentsOfCell("wait", "a second"));
    }
    
    [TestMethod]
    public void SetCellContents_TestLowerCaseName_ContainsTheCell()
    {
        Spreadsheet spreadSheet = new Spreadsheet();
        spreadSheet.SetContentsOfCell("d4", "a second");
        ISet<string> result = spreadSheet.GetNamesOfAllNonemptyCells();
        Assert.Contains("D4", result.ToList());
    }
    
    [TestMethod]
    public void SetCellContents_TestEmptyStringText_DeletedTheCellInTheSpreadSheet()
    {
        Spreadsheet spreadSheet = new Spreadsheet();
        spreadSheet.SetContentsOfCell("d4", "a second");
        spreadSheet.SetContentsOfCell("D5", "hello");
        spreadSheet.SetContentsOfCell("D6", "world");
        
        spreadSheet.SetContentsOfCell("D6", "");
        Assert.DoesNotContain("D6", spreadSheet.GetNamesOfAllNonemptyCells().ToList());
    }
    

    [TestMethod]
    public void SetCellContents_TestText_ReturnsCorrectContents()
    {
        Spreadsheet spreadSheet = new Spreadsheet();
        spreadSheet.SetContentsOfCell("D4", "hello");

        Assert.AreEqual("hello", spreadSheet.GetCellContents("D4"));
    }

    [TestMethod]
    public void SetCellContents_TestReplaceText_ReturnsNewContents()
    {
        Spreadsheet spreadSheet = new Spreadsheet();
        spreadSheet.SetContentsOfCell("D4", "hello");
        spreadSheet.SetContentsOfCell("D4", "world");

        Assert.AreEqual("world", spreadSheet.GetCellContents("D4"));
    }

    [TestMethod]
    public void SetCellContents_TestReplaceDouble_ReturnsText()
    {
        Spreadsheet spreadSheet = new Spreadsheet();
        spreadSheet.SetContentsOfCell("D4", "10.0");
        spreadSheet.SetContentsOfCell("D4", "hello");

        Assert.AreEqual("hello", spreadSheet.GetCellContents("D4"));
    }

    [TestMethod]
    public void SetCellContents_TestNumberAsText_BecomesADouble()
    {
        Spreadsheet spreadSheet = new Spreadsheet();
        spreadSheet.SetContentsOfCell("D4", "123");

        Assert.AreEqual(123.0, spreadSheet.GetCellContents("D4"));
    }

    [TestMethod]
    public void SetCellContents_TestEmptyText_ReturnsEmptyContents()
    {
        Spreadsheet spreadSheet = new Spreadsheet();
        spreadSheet.SetContentsOfCell("D4", "hello");
        spreadSheet.SetContentsOfCell("D4", "");

        Assert.AreEqual("", spreadSheet.GetCellContents("D4"));
    }

    [TestMethod]
    public void SetCellContents_TestText_ReturnsChangedCell()
    {
        Spreadsheet spreadSheet = new Spreadsheet();

        var result = spreadSheet.SetContentsOfCell("D4", "hello");

        CollectionAssert.AreEquivalent(new[] { "D4" }, result.ToList());
    }
    
    [TestMethod]
    public void SetCellContents_TestSpacesOnly_IsNotEmpty()
    {
        Spreadsheet spreadSheet = new Spreadsheet();
        spreadSheet.SetContentsOfCell("D4", "   ");

        Assert.AreEqual("   ", spreadSheet.GetCellContents("D4"));
        Assert.Contains("D4", spreadSheet.GetNamesOfAllNonemptyCells().ToList());
    }

    [TestMethod]
    public void SetCellContents_TestFormulaAsText_RemainsText()
    {
        Spreadsheet spreadSheet = new Spreadsheet();
        spreadSheet.SetContentsOfCell("D4", "A1+2");

        Assert.AreEqual("A1+2", spreadSheet.GetCellContents("D4"));

        // Text should not create a dependency on A1.
        var changed = spreadSheet.SetContentsOfCell("A1", "5.0");
        Assert.DoesNotContain("D4", changed.ToList());
    }

    [TestMethod]
    public void SetCellContents_TestReplaceFormula_RemovesOldDependency()
    {
        Spreadsheet spreadSheet = new Spreadsheet();
        spreadSheet.SetContentsOfCell("D4", "=A1+2");
        spreadSheet.SetContentsOfCell("D4", "hello");

        var changed = spreadSheet.SetContentsOfCell("A1", "5.0");

        Assert.DoesNotContain("D4", changed.ToList());
        Assert.AreEqual("hello", spreadSheet.GetCellContents("D4"));
    }

    [TestMethod]
    public void SetCellContents_TestEmptyTextOnMissingCell_KeepsSpreadsheetEmpty()
    {
        Spreadsheet spreadSheet = new Spreadsheet();
        spreadSheet.SetContentsOfCell("D4", "");

        Assert.AreEqual("", spreadSheet.GetCellContents("D4"));
        Assert.AreEqual(0, spreadSheet.GetNamesOfAllNonemptyCells().Count());
    }

    [TestMethod]
    public void SetCellContents_TestTextChange_ReturnsIndirectDependents()
    {
        Spreadsheet spreadSheet = new Spreadsheet();
        spreadSheet.SetContentsOfCell("B1", "=A1+2");
        spreadSheet.SetContentsOfCell("C1", "=B1*3");

        var changed = spreadSheet.SetContentsOfCell("A1", "hello");

        CollectionAssert.AreEquivalent(new[] { "A1", "B1", "C1" }, changed.ToList());
    }
    
    // Tests for SetCellContents(string name, Formula formula)

    [TestMethod]
    public void SetCellContents_TestFormulaInvalidName_ThrowsException()
    {
        Spreadsheet spreadSheet = new Spreadsheet();

        Assert.Throws<InvalidNameException>(() =>
            spreadSheet.SetContentsOfCell("wait", "=A1+2"));
    }
    
    [TestMethod]
    public void SetCellContents_TestFormulaLowerCaseName_NormalizesName()
    {
        Spreadsheet spreadSheet = new Spreadsheet();
        Formula formula = new Formula("A1+2");

        spreadSheet.SetContentsOfCell("d4", "=" + formula.ToString());

        Assert.Contains("D4", spreadSheet.GetNamesOfAllNonemptyCells().ToList());
        Assert.AreEqual(formula, spreadSheet.GetCellContents("D4"));
    }
    
    [TestMethod]
    public void SetCellContents_TestFormula_ReplacesOldContents()
    {
        Spreadsheet spreadSheet = new Spreadsheet();
        spreadSheet.SetContentsOfCell("D4", "hello");
        Formula formula = new Formula("A1+2");

        spreadSheet.SetContentsOfCell("D4", "=" + formula.ToString());

        Assert.AreEqual(formula, spreadSheet.GetCellContents("D4"));
    }
    
    [TestMethod]
    public void SetCellContents_TestFormula_ReturnsDependentsInOrder()
    {
        Spreadsheet spreadSheet = new Spreadsheet();
        spreadSheet.SetContentsOfCell("B1", "=A1+2");
        spreadSheet.SetContentsOfCell("C1", "=B1*3");

        var changed = spreadSheet.SetContentsOfCell("A1", "=D1+1");

        CollectionAssert.AreEqual(new[] { "A1", "B1", "C1" }, changed.ToList());
    }
    
    [TestMethod]
    public void SetCellContents_TestFormula_CreatesDependency()
    {
        Spreadsheet spreadSheet = new Spreadsheet();
        spreadSheet.SetContentsOfCell("B1", "=A1+2");

        var changed = spreadSheet.SetContentsOfCell("A1", "5.0");

        CollectionAssert.AreEqual(new[] { "A1", "B1" }, changed.ToList());
    }
    
    [TestMethod]
    public void SetCellContents_TestReplaceFormula_UpdatesDependencies()
    {
        Spreadsheet spreadSheet = new Spreadsheet();
        spreadSheet.SetContentsOfCell("B1", "=A1+2");

        spreadSheet.SetContentsOfCell("B1", "=C1+3");

        // B1 should no longer depend on A1.
        var changedA1 = spreadSheet.SetContentsOfCell("A1", "5.0");
        Assert.DoesNotContain("B1", changedA1.ToList());

        // B1 should now depend on C1.
        var changedC1 = spreadSheet.SetContentsOfCell("C1", "7.0");
        Assert.Contains("B1", changedC1.ToList());
    }

    [TestMethod]
    public void SetCellContents_TestSelfDependency_ThrowsException()
    {
        Spreadsheet spreadSheet = new Spreadsheet();

        Assert.Throws<CircularException>(() =>
            spreadSheet.SetContentsOfCell("A1", "=A1+2"));

        Assert.AreEqual("", spreadSheet.GetCellContents("A1"));
        Assert.AreEqual(0, spreadSheet.GetNamesOfAllNonemptyCells().Count());
    }

    [TestMethod]
    public void SetCellContents_TestLowerCaseSelfDependency_ThrowsException()
    {
        Spreadsheet spreadSheet = new Spreadsheet();

        Assert.Throws<CircularException>(() => spreadSheet.SetContentsOfCell("a1", "=A1+2"));
    }
    
    [TestMethod]
    public void SetCellContents_TestIndirectCircularDependency_ThrowsException()
    {
        Spreadsheet spreadSheet = new Spreadsheet();
        spreadSheet.SetContentsOfCell("B1", "=A1+1");
        spreadSheet.SetContentsOfCell("C1", "=B1+1");

        Assert.Throws<CircularException>(() => spreadSheet.SetContentsOfCell("A1", "=C1+1"));
    }

    [TestMethod]
    public void SetCellContents_TestDeleteCell_ReturnsDependents()
    {
        Spreadsheet spreadSheet = new Spreadsheet();
        spreadSheet.SetContentsOfCell("A1", "5.0");
        spreadSheet.SetContentsOfCell("B1", "=A1+2");

        var changed = spreadSheet.SetContentsOfCell("A1", "");

        CollectionAssert.AreEqual(new[] { "A1", "B1" }, changed.ToList());
        Assert.AreEqual("", spreadSheet.GetCellContents("A1"));
        Assert.Contains("B1", spreadSheet.GetNamesOfAllNonemptyCells().ToList());
    }
    
    [TestMethod]
    public void SetCellContents_TestCircularChange_KeepsOldContentsAndDependencies()
    {
        Spreadsheet spreadSheet = new Spreadsheet();
        Formula oldFormula = new Formula("D1+2");

        spreadSheet.SetContentsOfCell("A1", "=" + oldFormula.ToString());
        spreadSheet.SetContentsOfCell("B1", "=A1+1");

        Assert.Throws<CircularException>(() => spreadSheet.SetContentsOfCell("A1", "=B1+C1"));

        Assert.AreEqual(oldFormula, spreadSheet.GetCellContents("A1"));

        var changedD1 = spreadSheet.SetContentsOfCell("D1", "5.0");
        CollectionAssert.AreEqual(new[] { "D1", "A1", "B1" }, changedD1.ToList());

        var changedC1 = spreadSheet.SetContentsOfCell("C1", "3.0");
        CollectionAssert.AreEqual(new[] { "C1" }, changedC1.ToList());
    }
    
    //Additional Tests
    [TestMethod]
    public void CellMethods_TestNullName_ThrowsInvalidNameException()
    {
        Spreadsheet spreadSheet = new Spreadsheet();

        Assert.Throws<InvalidNameException>(() => spreadSheet.GetCellContents(null!));

        Assert.Throws<InvalidNameException>(() => spreadSheet.SetContentsOfCell(null!, "5.0"));

        Assert.Throws<InvalidNameException>(() => spreadSheet.SetContentsOfCell(null!, "hello"));

        Assert.Throws<InvalidNameException>(() => spreadSheet.SetContentsOfCell(null!, "=5"));
    }
    
    [TestMethod]
    public void GetNamesOfAllNonemptyCells_TestModifyReturnedSet_DoesNotChangeSpreadsheet()
    {
        Spreadsheet spreadSheet = new Spreadsheet();
        spreadSheet.SetContentsOfCell("A1", "5.0");

        ISet<string> names = spreadSheet.GetNamesOfAllNonemptyCells();
        names.Clear();
        names.Add("B1");

        CollectionAssert.AreEquivalent(
            new[] { "A1" }, spreadSheet.GetNamesOfAllNonemptyCells().ToList());

        Assert.AreEqual(5.0, spreadSheet.GetCellContents("A1"));
        Assert.AreEqual("", spreadSheet.GetCellContents("B1"));
    }
    
        // Tests for GetCellValue and the indexer
    [TestMethod]
    public void GetCellValue_EmptyCell_ReturnsEmptyString()
    {
        Spreadsheet spreadSheet = new Spreadsheet();
        Assert.AreEqual("", spreadSheet.GetCellValue("A1"));
        Assert.AreEqual("", spreadSheet["a1"]);
    }

    [TestMethod]
    public void GetCellValue_NumberAndText_ReturnTheirValues()
    {
        Spreadsheet spreadSheet = new Spreadsheet();
        spreadSheet.SetContentsOfCell("A1", "12.5");
        spreadSheet.SetContentsOfCell("B1", "hello");
        Assert.AreEqual(12.5, spreadSheet.GetCellValue("a1"));
        Assert.AreEqual("hello", spreadSheet.GetCellValue("b1"));
        Assert.AreEqual(12.5, spreadSheet["A1"]);
        Assert.AreEqual("hello", spreadSheet["B1"]);
    }

    [TestMethod]
    public void GetCellValue_Formula_ReturnsResultAndPreservesContents()
    {
        Spreadsheet spreadSheet = new Spreadsheet();
        spreadSheet.SetContentsOfCell("A1", "5");
        spreadSheet.SetContentsOfCell("B1", "=A1*2+3");
        Assert.AreEqual(13.0, spreadSheet.GetCellValue("B1"));
        Assert.AreEqual(13.0, spreadSheet["b1"]);
        Assert.AreEqual(new Formula("A1*2+3"), spreadSheet.GetCellContents("B1"));
    }

    [TestMethod]
    public void GetCellValue_TestConstantFormula_ReturnsResult()
    {
        Spreadsheet spreadSheet = new Spreadsheet();
        spreadSheet.SetContentsOfCell("A1", "=2+3*4");
        Assert.AreEqual(14.0, spreadSheet.GetCellValue("A1"));
    }

    [TestMethod]
    public void GetCellValueAndIndexer_TestInvalidName_ThrowInvalidNameException()
    {
        string[] cases = { "", "25BB", "A", " A1", "A1\n", null! };

        foreach (string name in cases)
        {
            Spreadsheet spreadSheet = new Spreadsheet();
            Assert.Throws<InvalidNameException>(() => spreadSheet.GetCellValue(name));
            Assert.Throws<InvalidNameException>(() => { _ = spreadSheet[name]; });
        }
    }

    [TestMethod]
    public void GetCellValue_TestUndefinedReference_ReturnsFormulaError()
    {
        Spreadsheet spreadSheet = new Spreadsheet();
        spreadSheet.SetContentsOfCell("B1", "=A1+2");
        Assert.IsTrue(spreadSheet.GetCellValue("B1") is FormulaError);
        Assert.AreEqual("", spreadSheet.GetCellValue("A1"));
    }

    [TestMethod]
    public void GetCellValue_TestTextReference_ReturnsFormulaError()
    {
        Spreadsheet spreadSheet = new Spreadsheet();
        spreadSheet.SetContentsOfCell("A1", "hello");
        spreadSheet.SetContentsOfCell("B1", "=A1+2");
        Assert.IsTrue(spreadSheet.GetCellValue("B1") is FormulaError);
    }

    [TestMethod]
    public void GetCellValue_TestDivisionByZero_PropagatesFormulaError()
    {
        Spreadsheet spreadSheet = new Spreadsheet();
        spreadSheet.SetContentsOfCell("A1", "0");
        spreadSheet.SetContentsOfCell("B1", "=5/A1");
        spreadSheet.SetContentsOfCell("C1", "=B1+2");
        Assert.IsTrue(spreadSheet.GetCellValue("B1") is FormulaError);
        Assert.IsTrue(spreadSheet.GetCellValue("C1") is FormulaError);
        spreadSheet.SetContentsOfCell("A1", "5");
        Assert.AreEqual(1.0, spreadSheet.GetCellValue("B1"));
        Assert.AreEqual(3.0, spreadSheet.GetCellValue("C1"));
    }

    // Tests for SetContentsOfCell and recalculation
    [TestMethod]
    public void SetContentsOfCell_TestZeroReference_IsNumeric()
    {
        Spreadsheet spreadSheet = new Spreadsheet();
        spreadSheet.SetContentsOfCell("A1", "0");
        spreadSheet.SetContentsOfCell("B1", "=A1+2");
        Assert.AreEqual(2.0, spreadSheet.GetCellValue("B1"));
    }

    [TestMethod]
    public void SetContentsOfCell_TestChain_UpdatesAllValuesInOrder()
    {
        Spreadsheet spreadSheet = new Spreadsheet();
        spreadSheet.SetContentsOfCell("A1", "5");
        spreadSheet.SetContentsOfCell("B1", "=A1*2");
        spreadSheet.SetContentsOfCell("C1", "=B1+A1");
        IList<string> changed = spreadSheet.SetContentsOfCell("a1", "7");
        CollectionAssert.AreEqual(new[] { "A1", "B1", "C1" }, changed.ToList());
        Assert.AreEqual(7.0, spreadSheet.GetCellValue("A1"));
        Assert.AreEqual(14.0, spreadSheet.GetCellValue("B1"));
        Assert.AreEqual(21.0, spreadSheet.GetCellValue("C1"));
    }

   

    [TestMethod]
    public void SetContentsOfCell_TestDeleteReference_UpdatesDependentErrors()
    {
        Spreadsheet spreadSheet = new Spreadsheet();
        spreadSheet.SetContentsOfCell("A1", "5");
        spreadSheet.SetContentsOfCell("B1", "=A1+2");
        spreadSheet.SetContentsOfCell("C1", "=B1*3");
        spreadSheet.SetContentsOfCell("A1", "");
        Assert.AreEqual("", spreadSheet.GetCellValue("A1"));
        Assert.IsTrue(spreadSheet.GetCellValue("B1") is FormulaError);
        Assert.IsTrue(spreadSheet.GetCellValue("C1") is FormulaError);
        spreadSheet.SetContentsOfCell("A1", "4");
        Assert.AreEqual(18.0, spreadSheet.GetCellValue("C1"));
    }

    [TestMethod]
    public void SetContentsOfCell_TestReplaceReferenceWithText_UpdatesDependentErrors()
    {
        Spreadsheet spreadSheet = new Spreadsheet();
        spreadSheet.SetContentsOfCell("A1", "5");
        spreadSheet.SetContentsOfCell("B1", "=A1+2");
        spreadSheet.SetContentsOfCell("A1", "hello");
        Assert.AreEqual("hello", spreadSheet.GetCellValue("A1"));
        Assert.IsTrue(spreadSheet.GetCellValue("B1") is FormulaError);
    }

    [TestMethod]
    public void SetContentsOfCell_TestReplaceFormulaWithNumber_UpdatesValueAndDependencies()
    {
        Spreadsheet spreadSheet = new Spreadsheet();
        spreadSheet.SetContentsOfCell("A1", "5");
        spreadSheet.SetContentsOfCell("B1", "=A1+2");
        spreadSheet.SetContentsOfCell("C1", "=B1*3");
        spreadSheet.SetContentsOfCell("B1", "4");
        Assert.AreEqual(4.0, spreadSheet.GetCellValue("B1"));
        Assert.AreEqual(12.0, spreadSheet.GetCellValue("C1"));
        CollectionAssert.AreEqual(new[] { "A1" }, spreadSheet.SetContentsOfCell("A1", "9").ToList());
        Assert.AreEqual(12.0, spreadSheet.GetCellValue("C1"));
    }

    [TestMethod]
    public void SetContentsOfCell_TestInvalidFormula_PreservesContentsValuesAndDependencies()
    {
        string[] cases = { "=", "=1+", "=(1+2" };

        foreach (string content in cases)
        {
            Spreadsheet spreadSheet = new Spreadsheet();
            spreadSheet.SetContentsOfCell("A1", "5");
            spreadSheet.SetContentsOfCell("B1", "=A1+2");
            Assert.Throws<FormulaFormatException>(() => spreadSheet.SetContentsOfCell("B1", content));
            Assert.AreEqual(new Formula("A1+2"), spreadSheet.GetCellContents("B1"));
            Assert.AreEqual(7.0, spreadSheet.GetCellValue("B1"));
            spreadSheet.SetContentsOfCell("A1", "8");
            Assert.AreEqual(10.0, spreadSheet.GetCellValue("B1"));
        }
    }

    [TestMethod]
    public void SetContentsOfCell_TestCircularChange_PreservesStoredValues()
    {
        Spreadsheet spreadSheet = new Spreadsheet();
        spreadSheet.SetContentsOfCell("A1", "5");
        spreadSheet.SetContentsOfCell("B1", "=A1+2");
        Assert.Throws<CircularException>(() => spreadSheet.SetContentsOfCell("A1", "=B1+1"));
        Assert.AreEqual(5.0, spreadSheet.GetCellValue("A1"));
        Assert.AreEqual(7.0, spreadSheet.GetCellValue("B1"));
        spreadSheet.SetContentsOfCell("A1", "8");
        Assert.AreEqual(10.0, spreadSheet.GetCellValue("B1"));
    }

    [TestMethod]
    public void SetContentsOfCell_TestInvalidNameBeforeInvalidFormula_ThrowsInvalidNameException()
    {
        Spreadsheet spreadSheet = new Spreadsheet();
        Assert.Throws<InvalidNameException>(() => spreadSheet.SetContentsOfCell("invalid", "="));
        Assert.IsFalse(spreadSheet.Changed);
        Assert.AreEqual(0, spreadSheet.GetNamesOfAllNonemptyCells().Count);
    }

   

    // Tests for Changed
    [TestMethod]
    public void Changed_TestNewSpreadsheetAndReads_RemainsFalse()
    {
        Spreadsheet spreadSheet = new Spreadsheet();
        Assert.IsFalse(spreadSheet.Changed);
        spreadSheet.GetCellContents("A1");
        spreadSheet.GetCellValue("A1");
        spreadSheet.GetNamesOfAllNonemptyCells();
        _ = spreadSheet["A1"];
        Assert.IsFalse(spreadSheet.Changed);
    }

    [TestMethod]
    public void Changed_TestSuccessfulSet_BecomesTrue()
    {
        string[] cases = { "5", "hello", "=2+3", "" };

        foreach (string content in cases)
        {
            Spreadsheet spreadSheet = new Spreadsheet();
            spreadSheet.SetContentsOfCell("A1", content);
            Assert.IsTrue(spreadSheet.Changed);
        }
    }

    [TestMethod]
    public void Changed_TestFailedFormulaOnNewSpreadsheet_RemainsFalse()
    {
        Spreadsheet spreadSheet = new Spreadsheet();
        Assert.Throws<FormulaFormatException>(() => spreadSheet.SetContentsOfCell("A1", "="));
        Assert.IsFalse(spreadSheet.Changed);
        Assert.Throws<CircularException>(() => spreadSheet.SetContentsOfCell("A1", "=A1+1"));
        Assert.IsFalse(spreadSheet.Changed);
    }

    [TestMethod]
    public void Changed_TestSaveThenFailedChanges_RemainsFalse()
    {
        WithTemporaryDirectory(directory =>
        {
            Spreadsheet spreadSheet = new Spreadsheet();
            spreadSheet.SetContentsOfCell("A1", "5");
            spreadSheet.SetContentsOfCell("B1", "=A1+2");
            spreadSheet.Save(Path.Combine(directory, "spreadSheet.json"));
            Assert.IsFalse(spreadSheet.Changed);
            Assert.Throws<InvalidNameException>(() => spreadSheet.SetContentsOfCell("invalid", "5"));
            Assert.IsFalse(spreadSheet.Changed);
            Assert.Throws<FormulaFormatException>(() => spreadSheet.SetContentsOfCell("A1", "="));
            Assert.IsFalse(spreadSheet.Changed);
            Assert.Throws<CircularException>(() => spreadSheet.SetContentsOfCell("A1", "=B1+1"));
            Assert.IsFalse(spreadSheet.Changed);
            spreadSheet.SetContentsOfCell("A1", "6");
            Assert.IsTrue(spreadSheet.Changed);
        });
    }

    // Tests for Save and the file constructor
    [TestMethod]
    public void Save_TestAllContentTypes_WritesRequiredJsonStructure()
    {
        WithTemporaryDirectory(directory =>
        {
            Spreadsheet spreadSheet = new Spreadsheet();
            spreadSheet.SetContentsOfCell("a1", "12.5");
            spreadSheet.SetContentsOfCell("B1", "hello");
            spreadSheet.SetContentsOfCell("C1", "=A1+2");
            spreadSheet.SetContentsOfCell("D1", "temporary");
            spreadSheet.SetContentsOfCell("D1", "");
            string filename = Path.Combine(directory, "spreadSheet.json");
            spreadSheet.Save(filename);
            using JsonDocument json = JsonDocument.Parse(File.ReadAllText(filename));
            JsonElement cells = json.RootElement.GetProperty("Cells");
            Assert.AreEqual(1, json.RootElement.EnumerateObject().Count());
            Assert.AreEqual(3, cells.EnumerateObject().Count());
            Assert.AreEqual("12.5", cells.GetProperty("A1").GetProperty("StringForm").GetString());
            Assert.AreEqual("hello", cells.GetProperty("B1").GetProperty("StringForm").GetString());
            Assert.AreEqual("=" + new Formula("A1+2").ToString(), cells.GetProperty("C1").GetProperty("StringForm").GetString());
            foreach (JsonProperty cell in cells.EnumerateObject())
            {
                Assert.AreEqual(1, cell.Value.EnumerateObject().Count());
            }
            Assert.IsFalse(spreadSheet.Changed);
            Assert.AreEqual(14.5, spreadSheet.GetCellValue("C1"));
        });
    }

    [TestMethod]
    public void SaveAndLoad_TestEmptySpreadsheet_RemainsEmptyAndUnchanged()
    {
        WithTemporaryDirectory(directory =>
        {
            string filename = Path.Combine(directory, "empty.json");
            new Spreadsheet().Save(filename);
            using JsonDocument json = JsonDocument.Parse(File.ReadAllText(filename));
            Assert.AreEqual(0, json.RootElement.GetProperty("Cells").EnumerateObject().Count());
            Spreadsheet loaded = new Spreadsheet(filename);
            Assert.AreEqual(0, loaded.GetNamesOfAllNonemptyCells().Count);
            Assert.AreEqual("", loaded.GetCellValue("A1"));
            Assert.IsFalse(loaded.Changed);
        });
    }

    [TestMethod]
    public void SaveAndLoad_TestAllTypes_PreservesContentsValuesAndDependencies()
    {
        WithTemporaryDirectory(directory =>
        {
            Spreadsheet spreadSheet = new Spreadsheet();
            spreadSheet.SetContentsOfCell("A1", "12.5");
            spreadSheet.SetContentsOfCell("B1", "=A1*2");
            spreadSheet.SetContentsOfCell("C1", "=B1+A1");
            spreadSheet.SetContentsOfCell("D1", "quotes: \"hello\"\nnext line\\end");
            string filename = Path.Combine(directory, "spreadSheet.json");
            spreadSheet.Save(filename);
            Spreadsheet loaded = new Spreadsheet(filename);
            CollectionAssert.AreEquivalent(new[] { "A1", "B1", "C1", "D1" }, loaded.GetNamesOfAllNonemptyCells().ToList());
            Assert.AreEqual(12.5, loaded.GetCellContents("A1"));
            Assert.AreEqual(new Formula("A1*2"), loaded.GetCellContents("B1"));
            Assert.AreEqual(37.5, loaded.GetCellValue("C1"));
            Assert.AreEqual("quotes: \"hello\"\nnext line\\end", loaded.GetCellValue("D1"));
            Assert.IsFalse(loaded.Changed);
            loaded.SetContentsOfCell("A1", "4");
            Assert.AreEqual(12.0, loaded.GetCellValue("C1"));
            Assert.IsTrue(loaded.Changed);
        });
    }

    [TestMethod]
    public void Save_TestExistingFile_OverwritesPreviousContents()
    {
        WithTemporaryDirectory(directory =>
        {
            string filename = Path.Combine(directory, "spreadSheet.json");
            Spreadsheet spreadSheet = new Spreadsheet();
            spreadSheet.SetContentsOfCell("A1", "hello");
            spreadSheet.Save(filename);
            spreadSheet.SetContentsOfCell("A1", "");
            spreadSheet.SetContentsOfCell("B1", "7");
            spreadSheet.Save(filename);
            Spreadsheet loaded = new Spreadsheet(filename);
            Assert.AreEqual("", loaded.GetCellContents("A1"));
            Assert.AreEqual(7.0, loaded.GetCellValue("B1"));
            Assert.AreEqual(1, loaded.GetNamesOfAllNonemptyCells().Count);
        });
    }

    [TestMethod]
    public void Save_TestInvalidPaths_ThrowsAndPreservesChangedState()
    {
        WithTemporaryDirectory(directory =>
        {
            Spreadsheet spreadSheet = new Spreadsheet();
            spreadSheet.SetContentsOfCell("A1", "5");
            Assert.Throws<SpreadsheetReadWriteException>(() => spreadSheet.Save(directory));
            Assert.IsTrue(spreadSheet.Changed);
            Assert.Throws<SpreadsheetReadWriteException>(() => spreadSheet.Save(Path.Combine(directory, "missing", "spreadSheet.json")));
            Assert.IsTrue(spreadSheet.Changed);
            Assert.Throws<SpreadsheetReadWriteException>(() => spreadSheet.Save(""));
            Assert.IsTrue(spreadSheet.Changed);
            Assert.Throws<SpreadsheetReadWriteException>(() => spreadSheet.Save(null!));
            Assert.IsTrue(spreadSheet.Changed);
            Assert.AreEqual(5.0, spreadSheet.GetCellValue("A1"));
            Spreadsheet empty = new Spreadsheet();
            Assert.Throws<SpreadsheetReadWriteException>(() => empty.Save(directory));
            Assert.IsFalse(empty.Changed);
        });
    }

    [TestMethod]
    public void Constructor_TestFileUnavailable_ThrowsSpreadsheetReadWriteException()
    {
        WithTemporaryDirectory(directory =>
        {
            Assert.Throws<SpreadsheetReadWriteException>(() => new Spreadsheet(Path.Combine(directory, "missing.json")));
            Assert.Throws<SpreadsheetReadWriteException>(() => new Spreadsheet(directory));
            Assert.Throws<SpreadsheetReadWriteException>(() => new Spreadsheet(""));
            Assert.Throws<SpreadsheetReadWriteException>(() => new Spreadsheet(null!));
        });
    }

    [TestMethod]
    public void Constructor_TestInvalidFileData_ThrowsSpreadsheetReadWriteException()
    {
        string[] cases = { "", "not json", "null", "{}", "{\"Cells\":null}", "{\"Cells\":{\"A1\":null}}", "{\"Cells\":{\"A1\":{}}}", "{\"Cells\":{\"A1\":{\"StringForm\":null}}}", "{\"Cells\":{\"A1\":{\"StringForm\":5}}}", "{\"Cells\":{\"invalid\":{\"StringForm\":\"5\"}}}", "{\"Cells\":{\"A1\":{\"StringForm\":\"=\"}}}", "{\"Cells\":{\"A1\":{\"StringForm\":\"=A1+1\"}}}", "{\"Cells\":{\"A1\":{\"StringForm\":\"=B1+1\"},\"B1\":{\"StringForm\":\"=A1+1\"}}}" };

        foreach (string content in cases)
        {
            WithTemporaryDirectory(directory =>
            {
                string filename = Path.Combine(directory, "invalid.json");
                File.WriteAllText(filename, content);
                Assert.Throws<SpreadsheetReadWriteException>(() => new Spreadsheet(filename));
            });
        }
    }

    [TestMethod]
    public void Constructor_TestFormulasBeforeReferences_RecalculatesAfterLoading()
    {
        WithTemporaryDirectory(directory =>
        {
            string filename = Path.Combine(directory, "spreadSheet.json");
            File.WriteAllText(filename,
                "{\"Cells\":{\"c1\":{\"StringForm\":\"=B1+A1\"},\"b1\":{\"StringForm\":\"=A1*2\"},\"a1\":{\"StringForm\":\"5\"}}}");
            Spreadsheet spreadSheet = new Spreadsheet(filename);
            Assert.AreEqual(15.0, spreadSheet.GetCellValue("C1"));
            Assert.AreEqual(10.0, spreadSheet.GetCellValue("B1"));
            CollectionAssert.AreEquivalent(new[] { "A1", "B1", "C1" }, spreadSheet.GetNamesOfAllNonemptyCells().ToList());
            Assert.IsFalse(spreadSheet.Changed);
        });
    }

    [TestMethod]
    public void Constructor_TestFormulaWithMissingReference_LoadsFormulaError()
    {
        WithTemporaryDirectory(directory =>
        {
            string filename = Path.Combine(directory, "spreadSheet.json");
            File.WriteAllText(filename, "{\"Cells\":{\"B1\":{\"StringForm\":\"=A1+2\"}}}");
            Spreadsheet spreadSheet = new Spreadsheet(filename);
            Assert.IsTrue(spreadSheet.GetCellValue("B1") is FormulaError);
            Assert.IsFalse(spreadSheet.Changed);
            spreadSheet.SetContentsOfCell("A1", "5");
            Assert.AreEqual(7.0, spreadSheet.GetCellValue("B1"));
        });
    }

    
    [TestMethod]
    public void SetContentsOfCell_TestSharedDependencyChain_ProducesCorrectValues()
    {
        Spreadsheet spreadSheet = new Spreadsheet();
        spreadSheet.SetContentsOfCell("A1", "1");
        for (int i = 2; i <= 30; i++)
        {
            spreadSheet.SetContentsOfCell("A" + i, "=A" + (i - 1) + "+A" + (i - 1));
        }
        Assert.AreEqual(536870912.0, spreadSheet.GetCellValue("A30"));
        spreadSheet.SetContentsOfCell("A1", "2");
        Assert.AreEqual(1073741824.0, spreadSheet.GetCellValue("A30"));
    }

    /// <summary>
    /// Creates a temporary directory, runs the test, and deletes the directory afterward.
    /// </summary>
    /// <param name="test">
    /// The test to run, which receives the path to the temporary directory.
    /// </param>
    private static void WithTemporaryDirectory(Action<string> test)
    {
        string directory = Path.Combine(Path.GetTempPath(), "SpreadsheetTests_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(directory);
        try
        {
            test(directory);
        }
        finally
        {
            Directory.Delete(directory, true);
        }
    }
}