﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Dynamo.Models;
using NUnit.Framework;

namespace Dynamo.Tests
{
    public class SearchSideEffects : DynamoViewModelUnitTest
    {
        [Test]
        [Category("UnitTests")]
        public void WhenStartingDynamoInputAndOutputNodesAreMissingFromSearch()
        {
            Assert.IsAssignableFrom( typeof(HomeWorkspaceModel), ViewModel.Model.CurrentWorkspace );

            // search and results are correct
            ViewModel.SearchViewModel.SearchAndUpdateResults("Input");
            Assert.AreEqual(0, ViewModel.SearchViewModel.SearchResults.Count(x => x.Name == "Input"));

            ViewModel.SearchViewModel.SearchAndUpdateResults("Output");
            Assert.AreEqual(0, ViewModel.SearchViewModel.SearchResults.Count(x => x.Name == "Output"));
        }

        [Test]
        public void WhenHomeWorkspaceIsFocusedInputAndOutputNodesAreMissingFromSearch()
        {
            // goto custom node workspace
            var model = ViewModel.Model;
            var examplePath = Path.Combine(GetTestDirectory(), @"core\combine", "Sequence2.dyf");
            ViewModel.OpenCommand.Execute(examplePath);

            Assert.AreEqual(model.CurrentWorkspace.Name, "Sequence2");

            // go to homeworkspace
            ViewModel.Model.CurrentWorkspace =
                ViewModel.Model.Workspaces.OfType<HomeWorkspaceModel>().First();

            Assert.AreEqual(model.CurrentWorkspace.Name, "Home");

            // search and results are correct
            ViewModel.SearchViewModel.SearchAndUpdateResults("Input");
            Assert.AreEqual(0, ViewModel.SearchViewModel.SearchResults.Count(x => x.Name == "Input"));

            ViewModel.SearchViewModel.SearchAndUpdateResults("Output");
            Assert.AreEqual(0, ViewModel.SearchViewModel.SearchResults.Count(x => x.Name == "Output"));
        }

        [Test]
        [Category("Failure")]
        public void WhenCustomNodeWorkspaceIsFocusedInputAndOutputNodesArePresentInSearch()
        {
            // goto custom node workspace
            var model = ViewModel.Model;
            var examplePath = Path.Combine(GetTestDirectory(), @"core\combine", "Sequence2.dyf");
            ViewModel.OpenCommand.Execute(examplePath);

            Assert.AreEqual(model.CurrentWorkspace.Name, "Sequence2");

            // search and results are correct
            ViewModel.SearchViewModel.SearchAndUpdateResults("Input");
            Assert.AreEqual(1, ViewModel.SearchViewModel.SearchResults.Count(x => x.Name == "Input"));
            Assert.AreEqual("Input", ViewModel.SearchViewModel.SearchResults[0].Name);

            ViewModel.SearchViewModel.SearchAndUpdateResults("Output");
            Assert.AreEqual(1, ViewModel.SearchViewModel.SearchResults.Count(x => x.Name == "Output"));
            Assert.AreEqual("Output", ViewModel.SearchViewModel.SearchResults[0].Name);

        }
    }
}
