// <authors> Celia AHUI </authors>
// <date> 09/18/2026 </date>
namespace DependencyGraphTests;

using DependencyGraph;

/// <summary>
///   This is a test class for DependencyGraphTest and is intended
///   to contain all DependencyGraphTest Unit Tests
/// </summary>
[TestClass]
public class DependencyGraphTests
{
    /// <summary>
    /// </summary>
    [TestMethod]
    [Timeout( 2000, CooperativeCancellation = true)]  // 2 second run time limit
    public void StressTest()
    {
        DependencyGraph dg = new();

        // A bunch of strings to use
        const int size = 200;
        string[] letters = new string[size];
        for ( int i = 0; i < size; i++ )
        {
            letters[i] = string.Empty + ( (char) ( 'a' + i ) );
        }

        // The correct answers
        HashSet<string>[] dependents = new HashSet<string>[size];
        HashSet<string>[] dependees = new HashSet<string>[size];
        for ( int i = 0; i < size; i++ )
        {
            dependents[i] = [];
            dependees[i] = [];
        }

        // Add a bunch of dependencies
        for ( int i = 0; i < size; i++ )
        {
            for ( int j = i + 1; j < size; j++ )
            {
                dg.AddDependency( letters[i], letters[j] );
                dependents[i].Add( letters[j] );
                dependees[j].Add( letters[i] );
            }
        }

        // Remove a bunch of dependencies
        for ( int i = 0; i < size; i++ )
        {
            for ( int j = i + 4; j < size; j += 4 )
            {
                dg.RemoveDependency( letters[i], letters[j] );
                dependents[i].Remove( letters[j] );
                dependees[j].Remove( letters[i] );
            }
        }

        // Add some back
        for ( int i = 0; i < size; i++ )
        {
            for ( int j = i + 1; j < size; j += 2 )
            {
                dg.AddDependency( letters[i], letters[j] );
                dependents[i].Add( letters[j] );
                dependees[j].Add( letters[i] );
            }
        }

        // Remove some more
        for ( int i = 0; i < size; i += 2 )
        {
            for ( int j = i + 3; j < size; j += 3 )
            {
                dg.RemoveDependency( letters[i], letters[j] );
                dependents[i].Remove( letters[j] );
                dependees[j].Remove( letters[i] );
            }
        }

        // Make sure everything is right
        for ( int i = 0; i < size; i++ )
        {
            Assert.IsTrue( dependents[i].SetEquals( new HashSet<string>( dg.GetDependents( letters[i] ) ) ) );
            Assert.IsTrue( dependees[i].SetEquals( new HashSet<string>( dg.GetDependees( letters[i] ) ) ) );
        }
        
    }
    
    [TestMethod]
    public void DependencyGraphConstructor_TestEmptyGraph_Valid()
    {
        DependencyGraph graph = new DependencyGraph();
        Assert.AreEqual(0, graph.Size);
        Assert.IsFalse(graph.HasDependees("a"));
        Assert.IsFalse(graph.HasDependents("a"));
        Assert.AreEqual(0,graph.GetDependees("a").Count());
        Assert.AreEqual(0,graph.GetDependents("a").Count());
    }

    
    //  Tests for Method Size
    [TestMethod]
    public void DependencyGraphSize_TestNonEmptyGraph_Valid()
    {
        DependencyGraph graph = new DependencyGraph();
        graph.AddDependency("5+3","4*2");
        graph.AddDependency("5+3","1+1");
        graph.AddDependency("87*9","74-6");
        Assert.AreEqual(3, graph.Size);
        
    }

    [TestMethod]
    public void DependencyGraphSize_TestDuplicateDependency_Valid()
    {
        DependencyGraph graph = new DependencyGraph();
        
        graph.AddDependency("5+3","4*2");
        graph.AddDependency("5+3","4*2");
        
        Assert.AreEqual(1, graph.Size);
    }
    
    //Tests for method HasDependents
    [TestMethod]
    public void DependencyGraphHasDependents_TestNodeHasDependents_Valid()
    {
        DependencyGraph graph = new DependencyGraph();
        
        graph.AddDependency("28*8","4-2");
        Assert.IsTrue(graph.HasDependents("28*8"));
    }
    
    [TestMethod]
    public void DependencyGraphHasDependents_TestNodeHasNoDependents_Invalid()
    {
        DependencyGraph graph = new DependencyGraph();
        
        graph.AddDependency("28*8","4-2");
        Assert.IsFalse(graph.HasDependents("4-2"));
    }
    
    [TestMethod]
    public void DependencyGraphHasDependents_TestMultipleNodes_Valid()
    {
        DependencyGraph graph = new DependencyGraph();

        graph.AddDependency("A", "B");
        graph.AddDependency("A", "C");
        graph.AddDependency("B", "D");
        graph.AddDependency("C", "D");
        graph.AddDependency("E", "F");

        Assert.IsTrue(graph.HasDependents("A"));
        Assert.IsTrue(graph.HasDependents("B"));
        Assert.IsTrue(graph.HasDependents("C"));
        Assert.IsTrue(graph.HasDependents("E"));

        Assert.IsFalse(graph.HasDependents("D"));
        Assert.IsFalse(graph.HasDependents("F"));
        Assert.IsFalse(graph.HasDependents("X"));
    }
    
    [TestMethod]
    public void DependencyGraphHasDependents_TestAfterRemovingDependencies_Invalid()
    {
        DependencyGraph graph = new DependencyGraph();

        graph.AddDependency("A", "B");
        graph.AddDependency("A", "C");

        Assert.IsTrue(graph.HasDependents("A"));

        graph.RemoveDependency("A", "B");
        Assert.IsTrue(graph.HasDependents("A"));

        graph.RemoveDependency("A", "C");
        Assert.IsFalse(graph.HasDependents("A"));
    }

    //Tests for HasDependees Method
    [TestMethod]
    public void DependencyGraphHasDependees_TestNodeHasDependees_Valid()
    {
      DependencyGraph graph = new DependencyGraph();
      graph.AddDependency("5-8", "7+8");
      graph.AddDependency("5-8", "58+7");
      graph.AddDependency("85*7", "58+7");
      graph.AddDependency("85*2", "85*7");
      
      Assert.IsTrue(graph.HasDependees("58+7"));
      Assert.IsTrue(graph.HasDependees("85*7"));
      Assert.IsTrue(graph.HasDependees("7+8"));
    }

    [TestMethod]
    public void DependencyGraphHasDependees_TestNodeHasNoDependees_Invalid()
    {
        DependencyGraph graph = new DependencyGraph();
        graph.AddDependency("5-8", "7+8");
        graph.AddDependency("5-8", "58+7");
        graph.AddDependency("85*7", "58+7");
        graph.AddDependency("85*2", "85*7");

        Assert.IsFalse(graph.HasDependees("5-8"));
    }
    
    [TestMethod]
    public void DependencyGraphHasDependees_TestAfterRemovingDependencies_False()
    {
        DependencyGraph graph = new DependencyGraph();

        graph.AddDependency("5+3", "4*2");
        graph.AddDependency("1+1", "4*2");

        Assert.IsTrue(graph.HasDependees("4*2"));

        graph.RemoveDependency("5+3", "4*2");
        Assert.IsTrue(graph.HasDependees("4*2"));

        graph.RemoveDependency("1+1", "4*2");
        Assert.IsFalse(graph.HasDependees("4*2"));
    }

    [TestMethod]
    public void DependencyGraphGetDependents_TestAbsentNode_Invalid()
    {
        DependencyGraph graph = new DependencyGraph();
        graph.AddDependency("5-8", "7+8");
        graph.AddDependency("5-8", "58+7");
        graph.AddDependency("3+3", "11+7");
        graph.AddDependency("5/8", "11+7");
        
        Assert.AreEqual(0, graph.GetDependents("5/25").Count());
        Assert.AreEqual(0, graph.GetDependents("5*25").Count());
        Assert.AreEqual(0, graph.GetDependents("11+7").Count());
        Assert.AreEqual(0, graph.GetDependents("58+7").Count());
        Assert.IsFalse(graph.GetDependents("5+3").Contains("85-8"));
    }
    
    [TestMethod]
    public void DependencyGraphGetDependents_TestOneDependent_Valid()
    {
        DependencyGraph graph = new DependencyGraph();

        graph.AddDependency("5+3", "4*2");
        
        Assert.AreEqual(1, graph.GetDependents("5+3").Count());
        Assert.IsTrue(graph.GetDependents("5+3").Contains("4*2"));
    }
    
    [TestMethod]
    public void DependencyGraphGetDependents_TestMultipleDependents_Valid()
    {
        DependencyGraph graph = new DependencyGraph();

        graph.AddDependency("5+3", "4*2");
        graph.AddDependency("5+3", "1+1");
        graph.AddDependency("5+3", "7-2");

        Assert.AreEqual(3, graph.GetDependents("5+3").Count());
        Assert.IsTrue(graph.GetDependents("5+3").Contains("4*2"));
        Assert.IsTrue(graph.GetDependents("5+3").Contains("1+1"));
        Assert.IsTrue(graph.GetDependents("5+3").Contains("7-2"));
    }
    
    //Tests For GetDependees Method
    [TestMethod]
    public void DependencyGraphGetDependees_TestOneDependee_Valid()
    {
        DependencyGraph graph = new DependencyGraph();

        graph.AddDependency("5+3", "4*2");

        Assert.AreEqual(1, graph.GetDependees("4*2").Count());
        Assert.IsTrue(graph.GetDependees("4*2").Contains("5+3"));
    }
    
    [TestMethod]
    public void DependencyGraphGetDependees_TestMultipleDependees_Valid()
    {
        DependencyGraph graph = new DependencyGraph();

        graph.AddDependency("5+3", "4*2");
        graph.AddDependency("1+1", "4*2");
        graph.AddDependency("7-2", "4*2");

        Assert.AreEqual(3, graph.GetDependees("4*2").Count());
        Assert.IsTrue(graph.GetDependees("4*2").Contains("5+3"));
        Assert.IsTrue(graph.GetDependees("4*2").Contains("1+1"));
        Assert.IsTrue(graph.GetDependees("4*2").Contains("7-2"));
    }
    
    [TestMethod]
    public void DependencyGraphGetDependees_TestNoDependees_Valid()
    {
        DependencyGraph graph = new DependencyGraph();
    
        graph.AddDependency("5+3", "4*2");
    
        Assert.AreEqual(0, graph.GetDependees("5+3").Count());
    }
    
    //Tests for AddDependency method
    [TestMethod]
    public void DependencyGraphAddDependency_TestOneDependency_Valid()
    {
        DependencyGraph graph = new DependencyGraph();

        graph.AddDependency("5+3", "4*2");

        Assert.AreEqual(1, graph.Size);
        Assert.IsTrue(graph.GetDependents("5+3").Contains("4*2"));
        Assert.IsTrue(graph.GetDependees("4*2").Contains("5+3"));
    }
    
    [TestMethod]
    public void DependencyGraphAddDependency_TestExistingDependee_Valid()
    {
        DependencyGraph graph = new DependencyGraph();

        graph.AddDependency("5+3", "4*2");
        graph.AddDependency("5+3", "1+1");

        Assert.AreEqual(2, graph.Size);
        Assert.AreEqual(2, graph.GetDependents("5+3").Count());
        Assert.IsTrue(graph.GetDependents("5+3").Contains("4*2"));
        Assert.IsTrue(graph.GetDependents("5+3").Contains("1+1"));
    }
    
    [TestMethod]
    public void DependencyGraphAddDependency_TestExistingDependent_Valid()
    {
        DependencyGraph graph = new DependencyGraph();

        graph.AddDependency("5+3", "4*2");
        graph.AddDependency("1+1", "4*2");

        Assert.AreEqual(2, graph.Size);
        Assert.AreEqual(2, graph.GetDependees("4*2").Count());
        Assert.IsTrue(graph.GetDependees("4*2").Contains("5+3"));
        Assert.IsTrue(graph.GetDependees("4*2").Contains("1+1"));
    }
    
    [TestMethod]
    public void DependencyGraphAddDependency_TestDuplicateDependency_Valid()
    {
        DependencyGraph graph = new DependencyGraph();

        graph.AddDependency("5+3", "4*2");
        graph.AddDependency("5+3", "4*2");

        Assert.AreEqual(1, graph.Size);
        Assert.AreEqual(1, graph.GetDependents("5+3").Count());
        Assert.AreEqual(1, graph.GetDependees("4*2").Count());
    }
    
    //Tests for RemoveDependency method
    [TestMethod]
    public void DependencyGraphRemoveDependency_TestExistingDependency_Valid()
    {
        DependencyGraph graph = new DependencyGraph();

        graph.AddDependency("5+3", "4*2");
        graph.RemoveDependency("5+3", "4*2");

        Assert.AreEqual(0, graph.Size);
        Assert.IsFalse(graph.GetDependents("5+3").Contains("4*2"));
        Assert.IsFalse(graph.GetDependees("4*2").Contains("5+3"));
    }
    
    [TestMethod]
    public void DependencyGraphRemoveDependency_TestNonExistingDependency_Valid()
    {
        DependencyGraph graph = new DependencyGraph();

        graph.AddDependency("5+3", "4*2");

        graph.RemoveDependency("1+1", "7-2");

        Assert.AreEqual(1, graph.Size);
        Assert.IsTrue(graph.GetDependents("5+3").Contains("4*2"));
        Assert.IsTrue(graph.GetDependees("4*2").Contains("5+3"));
    }
    
    [TestMethod]
    public void DependencyGraphRemoveDependency_TestOneOfMultipleDependencies_Valid()
    {
        DependencyGraph graph = new DependencyGraph();

        graph.AddDependency("5+3", "4*2");
        graph.AddDependency("5+3", "1+1");
        graph.AddDependency("7-2", "4*2");

        graph.RemoveDependency("5+3", "4*2");

        Assert.AreEqual(2, graph.Size);

        Assert.IsFalse(graph.GetDependents("5+3").Contains("4*2"));
        Assert.IsTrue(graph.GetDependents("5+3").Contains("1+1"));

        Assert.IsFalse(graph.GetDependees("4*2").Contains("5+3"));
        Assert.IsTrue(graph.GetDependees("4*2").Contains("7-2"));
    }
    
    //Tests for ReplaceDependents method
    [TestMethod]
    public void DependencyGraphReplaceDependents_TestReplaceExistingDependents_Valid()
    {
        DependencyGraph graph = new DependencyGraph();

        graph.AddDependency("5+3", "4*2");
        graph.AddDependency("5+3", "1+1");

        graph.ReplaceDependents("5+3",  [ "7-2", "9*3" ]);

        Assert.AreEqual(2, graph.Size);

        Assert.IsFalse(graph.GetDependents("5+3").Contains("4*2"));
        Assert.IsFalse(graph.GetDependents("5+3").Contains("1+1"));

        Assert.IsTrue(graph.GetDependents("5+3").Contains("7-2"));
        Assert.IsTrue(graph.GetDependents("5+3").Contains("9*3"));
    }
    
    [TestMethod]
    public void DependencyGraphReplaceDependents_TestNonExistingNode_Valid()
    {
        DependencyGraph graph = new DependencyGraph();

        graph.ReplaceDependents("5+3", ["4*2", "1+1"]);

        Assert.AreEqual(2, graph.Size);
        Assert.IsTrue(graph.GetDependents("5+3").Contains("4*2"));
        Assert.IsTrue(graph.GetDependents("5+3").Contains("1+1"));
    }
    
    //Tests for ReplaceDependees method
    [TestMethod]
    public void DependencyGraphReplaceDependees_TestReplaceExistingDependees_Valid()
    {
        DependencyGraph graph = new DependencyGraph();

        graph.AddDependency("5+3", "4*2");
        graph.AddDependency("1+1", "4*2");

        graph.ReplaceDependees("4*2", ["7-2", "9*3"]);

        Assert.AreEqual(2, graph.Size);

        Assert.IsFalse(graph.GetDependees("4*2").Contains("5+3"));
        Assert.IsFalse(graph.GetDependees("4*2").Contains("1+1"));

        Assert.IsTrue(graph.GetDependees("4*2").Contains("7-2"));
        Assert.IsTrue(graph.GetDependees("4*2").Contains("9*3"));
    }
    
    [TestMethod]
    public void DependencyGraphReplaceDependees_TestNonExistingNode_Valid()
    {
        DependencyGraph graph = new DependencyGraph();

        graph.ReplaceDependees("4*2", ["5+3", "1+1"]);

        Assert.AreEqual(2, graph.Size);
        Assert.IsTrue(graph.GetDependees("4*2").Contains("5+3"));
        Assert.IsTrue(graph.GetDependees("4*2").Contains("1+1"));
    }
    
    [TestMethod]
    public void DependencyGraphReplaceDependees_TestReplaceWithEmpty_Valid()
    {
        DependencyGraph graph = new DependencyGraph();

        graph.AddDependency("5+3", "4*2");
        graph.AddDependency("1+1", "4*2");

        graph.ReplaceDependees("4*2", []);

        Assert.AreEqual(0, graph.Size);
        Assert.AreEqual(0, graph.GetDependees("4*2").Count());
        Assert.IsFalse(graph.HasDependees("4*2"));
    }
    
    //Tests for Edge Cases 
    [TestMethod]
    public void DependencyGraphRemoveDependency_TestExistingDependeeWrongDependent_Valid()
    {
        DependencyGraph graph = new DependencyGraph();

        graph.AddDependency("5+3", "4*2");
        graph.AddDependency("5+3", "1+1");

        graph.RemoveDependency("5+3", "7-2");

        Assert.AreEqual(2, graph.Size);
        Assert.IsTrue(graph.GetDependents("5+3").Contains("4*2"));
        Assert.IsTrue(graph.GetDependents("5+3").Contains("1+1"));
    }
    
    [TestMethod]
    public void DependencyGraphRemoveDependency_TestExistingDependentWrongDependee_Valid()
    {
        DependencyGraph graph = new DependencyGraph();

        graph.AddDependency("5+3", "4*2");
        graph.AddDependency("1+1", "4*2");

        graph.RemoveDependency("7-2", "4*2");

        Assert.AreEqual(2, graph.Size);
        Assert.IsTrue(graph.GetDependees("4*2").Contains("5+3"));
        Assert.IsTrue(graph.GetDependees("4*2").Contains("1+1"));
    }
    
    [TestMethod]
    public void DependencyGraphReplaceDependents_TestUpdatesDependees_Valid()
    {
        DependencyGraph graph = new DependencyGraph();

        graph.AddDependency("5+3", "4*2");
        graph.AddDependency("5+3", "1+1");

        graph.ReplaceDependents("5+3", ["7-2", "9*3"]);

        Assert.IsFalse(graph.GetDependees("4*2").Contains("5+3"));
        Assert.IsFalse(graph.GetDependees("1+1").Contains("5+3"));

        Assert.IsTrue(graph.GetDependees("7-2").Contains("5+3"));
        Assert.IsTrue(graph.GetDependees("9*3").Contains("5+3"));
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
}
