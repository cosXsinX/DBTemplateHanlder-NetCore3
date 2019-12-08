using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Functions
{
    public class FirstLetterToUpperCaseFunctionTemplateHandler : AbstractFunctionTemplateContextHandler
    {

        public FirstLetterToUpperCaseFunctionTemplateHandler(TemplateHandlerNew templateHandlerNew) : base(templateHandlerNew) { }
        public override string StartContext { get => "{:TDB:FUNCTION:FIRST:CHARACTER:TO:UPPER:CASE("; }
        public override string EndContext { get => ")::}"; }
        public override bool isStartContextAndEndContextAnEntireWord => false;
        public override string ContextActionDescription => "Is replaced by the intern context with the first letter of intern context Uppercased";

        public override string processContext(string StringContext)
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


    }
}
