﻿/*
 * Yet Another UserAgent Analyzer .NET Standard
 * Porting realized by Balzarotti Stefano, Copyright (C) OrbintSoft
 * 
 * Original Author and License:
 * 
 * Yet Another UserAgent Analyzer
 * Copyright (C) 2013-2018 Niels Basjes
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * All rights should be reserved to the original author Niels Basjes
 */

using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using OrbintSoft.Yauaa.Analyzer.Parse.UserAgentNS.Antlr4Source;
using System.Collections.Generic;

namespace OrbintSoft.Yauaa.Analyzer.Parse.UserAgentNS.Analyze
{
    public sealed class NumberRangeVisitor: UserAgentTreeWalkerBaseVisitor<NumberRangeList>
    {
        private static readonly int DEFAULT_MIN = 1;
        private static readonly int DEFAULT_MAX = 10;

        private static readonly Dictionary<string, int> MAX_RANGE = new Dictionary<string, int>();

        static NumberRangeVisitor() {
            // Hardcoded maximum values because of the parsing rules
            MAX_RANGE["agent"] = 1;
            MAX_RANGE["name"] = 1;
            MAX_RANGE["key"] = 1;

            // Did statistics on over 200K real useragents from 2015.
            // These are the maximum values from that test set (+ a little margin)
            MAX_RANGE["value"] = 2; // Max was 2
            MAX_RANGE["version"] = 5; // Max was 4
            MAX_RANGE["comments"] = 2; // Max was 2
            MAX_RANGE["entry"] = 20; // Max was much higher
            MAX_RANGE["product"] = 10; // Max was much higher

            MAX_RANGE["email"] = 2;
            MAX_RANGE["keyvalue"] = 3;
            MAX_RANGE["text"] = 8;
            MAX_RANGE["url"] = 3;
            MAX_RANGE["uuid"] = 4;
        }

        private NumberRangeVisitor()
        {
        }

        private static int GetMaxRange(UserAgentTreeWalkerParser.NumberRangeContext ctx)
        {
            RuleContext parent = ctx.Parent;
            if (!(parent is UserAgentTreeWalkerParser.StepDownContext)) {
                return DEFAULT_MAX;
            }
            string name = ((UserAgentTreeWalkerParser.StepDownContext)parent).name.Text;
            if (name == null)
            {
                return DEFAULT_MAX;
            }
            int? maxRange = MAX_RANGE[name];
            if (maxRange == null)
            {
                return DEFAULT_MAX;
            }
            return maxRange ?? 0;
        }

        internal static readonly NumberRangeVisitor NUMBER_RANGE_VISITOR = new NumberRangeVisitor();

        public static NumberRangeList GetList(UserAgentTreeWalkerParser.NumberRangeContext ctx)
        {
            return NUMBER_RANGE_VISITOR.Visit(ctx);
        }

        public override NumberRangeList VisitNumberRangeStartToEnd([NotNull] UserAgentTreeWalkerParser.NumberRangeStartToEndContext context)
        {
            return new NumberRangeList(
                int.Parse(context.rangeStart.Text),
                int.Parse(context.rangeEnd.Text));
        }

        public override NumberRangeList VisitNumberRangeSingleValue([NotNull] UserAgentTreeWalkerParser.NumberRangeSingleValueContext context)
        {
            int value = int.Parse(context.count.Text);
            return new NumberRangeList(value, value);
        }

        public override NumberRangeList VisitNumberRangeAll([NotNull] UserAgentTreeWalkerParser.NumberRangeAllContext context)
        {
            return new NumberRangeList(DEFAULT_MIN, GetMaxRange(context));
        }

        public override NumberRangeList VisitNumberRangeEmpty([NotNull] UserAgentTreeWalkerParser.NumberRangeEmptyContext context)
        {
            return new NumberRangeList(DEFAULT_MIN, GetMaxRange(context));
        }
    }
}
