using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using System;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Functions
{
    public class FirstLetterToUpperCaseFunctionTemplateHandler : AbstractFunctionTemplateContextHandler
    {

        public FirstLetterToUpperCaseFunctionTemplateHandler(ITemplateHandler templateHandlerNew) : base(templateHandlerNew) { }
        public override string StartContext { get => "{:TDB:FUNCTION:FIRST:CHARACTER:TO:UPPER:CASE("; }
        public override string EndContext { get => ")::}"; }
        public override bool isStartContextAndEndContextAnEntireWord => false;
        public override string ContextActionDescription => "Is replaced by the intern context with the first letter of intern context Uppercased";

        public override string ProcessContext(string StringContext, IDatabaseContext databaseContext)
        {
            ControlContext(StringContext, databaseContext);
            string TrimedStringContext =
                    TrimContextFromContextWrapper(StringContext);
            TrimedStringContext = HandleTrimedContext(TrimedStringContext,databaseContext);
            //Function performed operation
            if (TrimedStringContext.Equals("")) return TrimedStringContext;
            string s1 = TrimedStringContext.Substring(0, 1).ToUpper();
            return s1 + TrimedStringContext.Substring(1);
        }

       
    }
}
