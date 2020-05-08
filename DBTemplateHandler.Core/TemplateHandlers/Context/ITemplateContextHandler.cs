using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using System;

namespace DBTemplateHandler.Core.TemplateHandlers.Context
{
    public interface ITemplateContextHandlerIdentity
    {
        string StartContext { get; }

        string EndContext { get; }
        string Signature { get; }
        string ContextActionDescription { get; }
        bool isStartContextAndEndContextAnEntireWord { get; }
    }

    public interface ITemplateContextHandler : ITemplateContextHandlerIdentity
    {
        
        string TrimContextFromContextWrapper(string stringContext);

        string ProcessContext(string StringContext, IDatabaseContext databaseContext);

        string HandleTrimedContext(string StringTrimedContext, IDatabaseContext databaseContext);
    }
}