using System;

namespace DBTemplateHandler.Core.TemplateHandlers.Context
{
    public interface ITemplateContextHandler
    {
        String getStartContextStringWrapper();

        String getEndContextStringWrapper();

        bool isStartContextAndEndContextAnEntireWord();

        String TrimContextFromContextWrapper(String stringContext);

        abstract String processContext(String StringContext);

        abstract String HandleTrimedContext(String StringTrimedContext);
    }
}