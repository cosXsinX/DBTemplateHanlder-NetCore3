using DBTemplateHandler.Core.Template;
using DBTemplateHandler.Core.TemplateHandlers.Context;
using DBTemplateHandler.Core.TemplateHandlers.Context.PreprocessorDeclarations;
using DBTemplateHandler.Service.Contracts.TypeMapping;
using System.Collections.Generic;
using System.Linq;

namespace DBTemplateHandler.Core.TemplateHandlers.Handlers
{
    public class TemplatePreprocessor
    {
        private readonly TemplateContextHandlerPackageProvider<AbstactPreprocessorContextHandler> preprocessorContextHandlerRegister;
        private readonly ContextVisitor<AbstactPreprocessorContextHandler> contextVisitor;
        
        public TemplatePreprocessor(TemplateHandlerNew templateHandlerNew, IList<ITypeMapping> typeMappings)
        {
            preprocessorContextHandlerRegister = new TemplateContextHandlerPackageProvider<AbstactPreprocessorContextHandler>(templateHandlerNew, typeMappings);
            contextVisitor = new ContextVisitor<AbstactPreprocessorContextHandler>(preprocessorContextHandlerRegister);
        }

        public void PreProcess(IEnumerable<ITemplateModel> templateModels)
        {
            if (templateModels == null) return;
            if (!templateModels.Any()) return;
            var handlers = preprocessorContextHandlerRegister.GetContextHandlerByStartContextSignature();
            var templateModelList = templateModels.ToList();
            templateModelList.ForEach(template => PreProcess(template,handlers));
        }

        private string ToContextValue(TemplateContextComposite contextComposite)
        {
            return $"{contextComposite.current.StartContextDelimiter}{contextComposite.current.InnerContent}{contextComposite.current.EndContextDelimiter}";
        }

        private void PreProcess(ITemplateModel templateModel,IDictionary<string,AbstactPreprocessorContextHandler> handlersByStartContext)
        {
            var contextes = contextVisitor.ExtractAllContextUntilDepth(templateModel.TemplatedFileContent, 1);
            var contextesAndProcessesResult = contextes.Select(context => 
                (contextAsString: ToContextValue(context),processedContextContent:PreProcess( ToContextValue(context), context, handlersByStartContext),context)).ToList();
            int lenghtSumDifference = 0;
            var result = templateModel.TemplatedFileContent;
            foreach(var current in contextesAndProcessesResult)
            {
                result =
                    result.Substring(0, current.context.current.StartIndex + lenghtSumDifference)+
                    current.processedContextContent +
                    result.Substring(current.context.current.StartIndex + current.contextAsString.Length + lenghtSumDifference, result.Length 
                        -(current.context.current.StartIndex + current.contextAsString.Length + lenghtSumDifference));
                lenghtSumDifference = lenghtSumDifference + (current.processedContextContent.Length - current.contextAsString.Length);
            }
            templateModel.TemplatedFileContent = result;
        }

        private string PreProcess(string templateFileContent,TemplateContextComposite contextComposite, 
            IDictionary<string, AbstactPreprocessorContextHandler> handlersByStartContext)
        {
            if (!handlersByStartContext.TryGetValue(contextComposite.current.StartContextDelimiter, out var handler)) return templateFileContent;
            return handler.processContext(templateFileContent);
        }
    }
}
