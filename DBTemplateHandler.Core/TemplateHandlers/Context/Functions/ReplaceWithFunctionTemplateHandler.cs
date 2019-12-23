using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using System;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Functions
{
    public class ReplaceWithFunctionTemplateHandler : AbstractFunctionTemplateContextHandler
    {

        public ReplaceWithFunctionTemplateHandler(TemplateHandlerNew templateHandlerNew) : base(templateHandlerNew) { }
        public override string StartContext { get => "{:TDB:FUNCTION:REPLACE("; }
        public string WithSeparator { get => "<-:WITH:["; }
        public override string EndContext { get => "])::}"; }
        public override bool isStartContextAndEndContextAnEntireWord => false;
        public override string ContextActionDescription => "Is replaced by the intern context with the first letter of intern context Uppercased";

        public override string processContext(string StringContext)
        {
            if (StringContext == null)
                throw new ArgumentNullException(nameof(StringContext));
            string TrimedStringContext =
                    TrimContextFromContextWrapper(StringContext);
            //Function performed operation
            if (TrimedStringContext.Equals("")) return TrimedStringContext;
            var splitted = TrimedStringContext.Split(new string[] { WithSeparator },StringSplitOptions.None) ;
            if (splitted.Length != 2) return TrimedStringContext;

            var handledResult = HandleTrimedContext(splitted[0]);
            var result = handledResult?.Replace(handledResult, splitted[1]);
            return result;
        }
    }
}
