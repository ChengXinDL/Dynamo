﻿using System;
using System.IO;
using System.Reflection;
using System.Threading;
using Dynamo;
using Dynamo.Controls;
using Dynamo.Models;
using Dynamo.Utilities;
using Dynamo.ViewModels;

using DynamoUtilities;

using NUnit.Framework;

namespace DynamoCoreUITests
{
    public class DynamoTestUIBase
    {
        protected DynamoViewModel ViewModel { get; set; }
        protected DynamoView View { get; set; }
        protected DynamoModel Model { get; set; }

        protected string ExecutingDirectory
        {
            get { return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); }
        }

        protected string TempFolder { get; private set; }

        [SetUp]
        public virtual void Start()
        {
            DynamoPathManager.Instance.InitializeCore(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));

            DynamoPathManager.PreloadAsmLibraries(DynamoPathManager.Instance);

            AppDomain.CurrentDomain.AssemblyResolve += AssemblyHelper.ResolveAssembly;
            CreateTemporaryFolder();

            // Setup Temp PreferenceSetting Location for testing
            PreferenceSettings.DYNAMO_TEST_PATH = Path.Combine(TempFolder, "UserPreferenceTest.xml");

            Model = DynamoModel.Start(
                new DynamoModel.StartConfiguration()
                {
                    StartInTestMode = true
                });

            ViewModel = DynamoViewModel.Start(
                new DynamoViewModel.StartConfiguration()
                {
                    DynamoModel = Model
                });

            //create the view
            View = new DynamoView(ViewModel);
            View.Show();

            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
        }

        [TearDown]
        public void Exit()
        {
            //Ensure that we leave the workspace marked as
            //not having changes.
            Model.HomeSpace.HasUnsavedChanges = false;

            if (View.IsLoaded)
                View.Close();

            if (ViewModel != null)
            {
                var shutdownParams = new DynamoViewModel.ShutdownParams(
                    shutdownHost: false, allowCancellation: false);

                ViewModel.PerformShutdownSequence(shutdownParams);
                ViewModel = null;
            }

            View = null;
            Model = null;

            GC.Collect();

            try
            {
                var directory = new DirectoryInfo(TempFolder);
                directory.Delete(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        [TestFixtureTearDown]
        public void FinalTearDown()
        {
            // Fix for COM exception on close
            // See: http://stackoverflow.com/questions/6232867/com-exceptions-on-exit-with-wpf 
            //Dispatcher.CurrentDispatcher.InvokeShutdown();
        }

        #region Utility functions

        public static string GetTestDirectory(string executingDirectory)
        {
            var directory = new DirectoryInfo(executingDirectory);
            return Path.Combine(directory.Parent.Parent.Parent.FullName, "test");
        }

        protected void CreateTemporaryFolder()
        {
            string tempPath = Path.GetTempPath();
            TempFolder = Path.Combine(tempPath, "dynamoTmp\\" + Guid.NewGuid().ToString("N"));

            if (!Directory.Exists(TempFolder))
                Directory.CreateDirectory(TempFolder);
        }

        #endregion
    }
}
