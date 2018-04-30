﻿using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeCobol.Test.Compiler.Parser;
using TypeCobol.Test.Utils;

namespace TypeCobol.Test.Parser.Performance
{
    [TestClass]
    public class Performance
    {
        static readonly string AntlrFolder = PlatformUtils.GetPathForProjectFile("Parser") + Path.DirectorySeparatorChar + "Performance";

        [TestMethod]
        [TestCategory("Parsing")]
        [TestProperty("Time", "fast")]
        [Ignore]//Recovery Error To Test For Cup
        public void AntlrPerformanceProfiler()
        {
            Paths paths = new Paths(AntlrFolder, AntlrFolder, AntlrFolder + Path.DirectorySeparatorChar + "AntlrTest.pgm", new AntlrName());
            TestUnit unit = new TestUnit(new Multipass(paths));
            unit.Init(new[] { ".pgm", ".cpy" }, false, true);
            unit.Parse();

            unit.Compare(unit.Compiler.CompilationResultsForProgram.AntlrResult);
        }
    }
}
