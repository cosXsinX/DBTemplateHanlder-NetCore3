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

        public override string processContext(string StringContext)
        {
            return ProcessContext(StringContext, new ProcessorDatabaseContext() { });
        }

        public override string ProcessContext(string StringContext, IDatabaseContext databaseContext)
        {
            if (databaseContext == null) throw new ArgumentNullException(nameof(databaseContext)); 
            if (StringContext == null)
                throw new Exception($"The provided {nameof(StringContext)} is null");
            string TrimedStringContext =
                    TrimContextFromContextWrapper(StringContext);
            TrimedStringContext = HandleTrimedContext(TrimedStringContext);
            //Function performed operation
            if (TrimedStringContext.Equals("")) return TrimedStringContext;
            string s1 = TrimedStringContext.Substring(0, 1).ToUpper();
            return s1 + TrimedStringContext.Substring(1);
        }
    }
}
