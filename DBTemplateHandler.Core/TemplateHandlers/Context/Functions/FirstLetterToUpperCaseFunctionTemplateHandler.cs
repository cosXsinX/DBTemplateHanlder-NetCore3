using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Functions
{
    public class FirstLetterToUpperCaseFunctionTemplateHandler : AbstractFunctionTemplateContextHandler
    {
        public const String START_CONTEXT_WORD = "{:TDB:FUNCTION:FIRST:CHARACTER:TO:UPPER:CASE(";
        public const String END_CONTEXT_WORD = ")::}";
        public override string StartContext { get => START_CONTEXT_WORD; }
        public override string EndContext { get => END_CONTEXT_WORD; }

        public override String processContext(String StringContext)
        {
            if (StringContext == null)
                throw new Exception($"The provided {nameof(StringContext)} is null");
            String TrimedStringContext =
                    TrimContextFromContextWrapper(StringContext);
            TrimedStringContext = HandleTrimedContext(TrimedStringContext);
            //Function performed operation
            if (TrimedStringContext.Equals("")) return TrimedStringContext;
            String s1 = TrimedStringContext.Substring(0, 1).ToUpper();
            return s1 + TrimedStringContext.Substring(1);
        }


        public override bool isStartContextAndEndContextAnEntireWord => false;
    }
}
