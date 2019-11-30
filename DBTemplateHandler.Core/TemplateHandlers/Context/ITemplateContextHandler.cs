using System;

namespace DBTemplateHandler.Core.TemplateHandlers.Context
{
    public interface ITemplateContextHandler
    {
        string StartContext { get; }

        string EndContext { get; }
        string Signature();
        bool isStartContextAndEndContextAnEntireWord { get; }

        String TrimContextFromContextWrapper(String stringContext);

        abstract String processContext(String StringContext);

        abstract String HandleTrimedContext(String StringTrimedContext);
    }
}