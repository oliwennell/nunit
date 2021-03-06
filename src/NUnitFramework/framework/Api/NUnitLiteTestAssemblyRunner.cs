﻿// ***********************************************************************
// Copyright (c) 2014 Charlie Poole
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// ***********************************************************************

#if NUNITLITE
using System;
using System.IO;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Execution;

namespace NUnit.Framework.Api
{
    /// <summary>
    /// Default NUnitLite implementation of ITestAssemblyRunner
    /// </summary>
    public class NUnitLiteTestAssemblyRunner : AbstractTestAssemblyRunner
    {
#if !SILVERLIGHT && !NETCF
        TextWriter _savedOut;
        TextWriter _savedErr;
#endif

        #region Constructor

        /// <summary>
        /// Construct an NUnitLiteTestAssemblyRunner
        /// </summary>
        public NUnitLiteTestAssemblyRunner(ITestAssemblyBuilder builder) : base(builder) { }

        #endregion

        #region AbstractTestAssemblyRunner Overrides

        /// <summary>
        /// Run selected tests asynchronously, notifying the listener interface as it progresses.
        /// </summary>
        /// <param name="listener">Interface to receive EventListener notifications.</param>
        /// <returns></returns>
        public override void StartRun(ITestListener listener)
        {
#if !SILVERLIGHT && !NETCF
            _savedOut = Console.Out;
            _savedErr = Console.Error;

            Console.SetOut(new TextCapture(_savedOut));
            Console.SetError(new TextCapture(_savedErr));
#endif

            Context.Dispatcher = new SimpleWorkItemDispatcher();
            Context.Dispatcher.Dispatch(TopLevelWorkItem);
        }

#if !SILVERLIGHT && !NETCF
        /// <summary>
        /// Handle the the Completed event for the top level work item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnRunCompleted(object sender, EventArgs e)
        {
            Console.SetOut(_savedOut);
            Console.SetError(_savedErr);

            base.OnRunCompleted(sender, e);
        }
#endif

        #endregion
    }
}
#endif
