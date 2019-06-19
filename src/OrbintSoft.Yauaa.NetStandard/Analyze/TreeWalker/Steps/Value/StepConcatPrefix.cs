﻿//-----------------------------------------------------------------------
// <copyright file="StepConcatPrefix.cs" company="OrbintSoft">
//    Yet Another User Agent Analyzer for .NET Standard
//    porting realized by Stefano Balzarotti, Copyright 2018 (C) OrbintSoft
//
//    Original Author and License:
//
//    Yet Another UserAgent Analyzer
//    Copyright(C) 2013-2018 Niels Basjes
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//    https://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//
// </copyright>
// <author>Stefano Balzarotti, Niels Basjes</author>
// <date>2018, 11, 24, 12:48</date>
// <summary></summary>
//-----------------------------------------------------------------------

namespace OrbintSoft.Yauaa.Analyze.TreeWalker.Steps.Value
{
    using System;
    using System.Runtime.Serialization;
    using Antlr4.Runtime.Tree;

    /// <summary>
    /// Defines the <see cref="StepConcatPrefix" />.
    /// </summary>
    [Serializable]
    public class StepConcatPrefix : Step
    {
        /// <summary>
        /// Defines the prefix.
        /// </summary>
        private readonly string prefix;

        /// <summary>
        /// Initializes a new instance of the <see cref="StepConcatPrefix"/> class.
        /// </summary>
        /// <param name="info">The info<see cref="SerializationInfo"/>.</param>
        /// <param name="context">The context<see cref="StreamingContext"/>.</param>
        public StepConcatPrefix(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.prefix = (string)info.GetValue("prefix", typeof(string));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StepConcatPrefix"/> class.
        /// </summary>
        /// <param name="prefix">The prefix<see cref="string"/>.</param>
        public StepConcatPrefix(string prefix)
        {
            this.prefix = prefix;
        }

        /// <summary>
        /// The CanFail.
        /// </summary>
        /// <returns>The <see cref="bool"/>.</returns>
        public override bool CanFail()
        {
            return false;
        }

        /// <summary>
        /// The GetObjectData.
        /// </summary>
        /// <param name="info">The info<see cref="SerializationInfo"/>.</param>
        /// <param name="context">The context<see cref="StreamingContext"/>.</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("prefix", this.prefix, typeof(string));
        }

        /// <summary>
        /// The ToString.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public override string ToString()
        {
            return "ConcatPrefix(" + this.prefix + ")";
        }

        /// <summary>
        /// The Walk.
        /// </summary>
        /// <param name="tree">The tree<see cref="IParseTree"/>.</param>
        /// <param name="value">The value<see cref="string"/>.</param>
        /// <returns>The <see cref="WalkList.WalkResult"/>.</returns>
        public override WalkList.WalkResult Walk(IParseTree tree, string value)
        {
            var actualValue = this.GetActualValue(tree, value);
            var filteredValue = this.prefix + actualValue;
            return this.WalkNextStep(tree, filteredValue);
        }
    }
}
