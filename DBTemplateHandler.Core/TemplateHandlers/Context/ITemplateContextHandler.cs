using System;

namespace DBTemplateHandler.Core.TemplateHandlers.Context
{
    public interface ITemplateContextHandler
    {
        string StartContext { get; }

        string EndContext { get; }
        string Signature { get; }
        string ContextActionDescription { get; }
        bool isStartContextAndEndContextAnEntireWord { get; }
        string TrimContextFromContextWrapper(string stringContext);

        string processContext(string StringContext);

        string HandleTrimedContext(string StringTrimedContext);
    }
}