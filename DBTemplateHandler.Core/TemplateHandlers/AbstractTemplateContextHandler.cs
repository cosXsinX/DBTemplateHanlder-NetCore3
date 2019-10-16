using System;
namespace DBTemplateHandler.Core.TemplateHandlers
{
    public abstract class AbstractTemplateContextHandler
    {
        public abstract string ContextStart { get; }
        public abstract string ContextEnd { get; }
        public abstract bool IsContextAnEntireWord { get; }
        public string Signature { get { return $"{ContextStart}{ContextEnd}"; } }
    }
}
