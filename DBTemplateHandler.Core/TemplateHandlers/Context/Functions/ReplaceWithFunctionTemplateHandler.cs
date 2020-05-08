using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using System;
using System.Linq;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Functions
{
    public class ReplaceWithFunctionTemplateHandler : AbstractFunctionTemplateContextHandler
    {

        public ReplaceWithFunctionTemplateHandler(ITemplateHandler templateHandlerNew) : base(templateHandlerNew) { }
        public override string StartContext { get => "{:TDB:FUNCTION:REPLACE("; }
        public string WithSeparator { get => "<-:WITH:["; }
        public string BySeparator { get => "]:BY:["; }
        public override string EndContext { get => "])::}"; }
        public override bool isStartContextAndEndContextAnEntireWord => false;
        public override string ContextActionDescription => "Is replaced by the intern context with the first letter of intern context Uppercased";


        public override string ProcessContext(string StringContext, IDatabaseContext databaseContext)
        {
            ControlContext(StringContext, databaseContext);
            string TrimedStringContext =
                    TrimContextFromContextWrapper(StringContext);
            //Function performed operation
            if (TrimedStringContext.Equals(String.Empty)) return TrimedStringContext;
            var splitted = TrimedStringContext.Split(new string[] { WithSeparator }, StringSplitOptions.None);
            if (splitted.Length < 2) return TrimedStringContext;

            string ReplacedByValue = splitted.Last();
            var ReplaceByValueSplitted = ReplacedByValue.Split(BySeparator);
            if (ReplaceByValueSplitted.Length != 2) return TrimedStringContext;

            var replaced = ReplaceByValueSplitted[0];
            var replacement = ReplaceByValueSplitted[1];

            var handledResult =
                HandleTrimedContext(
                    string.Join(WithSeparator,
                        splitted.Take(splitted.Length - 1)),databaseContext);
            var result = handledResult?.Replace(replaced, replacement);
            return result;
        }
    }
}
