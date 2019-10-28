using System;

namespace DBTemplateHandler.Core.TemplateHandlers.Context
{
    public interface ITemplateContextHandler
    {
        string StartContext { get; }

        string EndContext { get; }

        bool isStartContextAndEndContextAnEntireWord();

        String TrimContextFromContextWrapper(String stringContext);

        abstract String processContext(String StringContext);

        abstract String HandleTrimedContext(String StringTrimedContext);
    }
}