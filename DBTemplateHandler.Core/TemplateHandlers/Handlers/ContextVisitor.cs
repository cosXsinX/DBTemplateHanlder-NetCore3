using DBTemplateHandler.Core.TemplateHandlers.Context;
using DBTemplateHandler.Core.TemplateHandlers.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBTemplateHandler.Core.TemplateHandlers.Handlers
{
    public class Context
    {
        public string StartContextDelimiter { get; set; }
        public string EndContextDelimiter { get; set; }
        public string InnerContent { get; set; }
    }

    public class ContextComposite
    {
        public Context current { get; set; }
        public IList<ContextComposite> childs { get; set; }
    }


    public class ContextVisitor<U> where U : ITemplateContextHandler
    {
        private readonly ITemplateContextHandlerPackageProvider<U> templateContextHandlerPackageProvider;
        
        public ContextVisitor(ITemplateContextHandlerPackageProvider<U> templateContextHandlerPackageProvider)
        {
            if (templateContextHandlerPackageProvider == null) throw new ArgumentNullException(nameof(templateContextHandlerPackageProvider));
            this.templateContextHandlerPackageProvider = templateContextHandlerPackageProvider;
        }

        public IEnumerable<ContextComposite> ExtractAllContextUntilDepth(string templateContent,int depth)
        {
            var workingContent = templateContent;
            var earliestStartContext = templateContextHandlerPackageProvider.GetHandlerStartContextWordAtEarliestPosition(workingContent);
            if (earliestStartContext == null) yield break;
            var contextHandler = templateContextHandlerPackageProvider.GetStartContextCorrespondingContextHandler(earliestStartContext);
            var correspondingEndContext = contextHandler.EndContext;
            var workingContentWithoutStartContextAndLeftSide = StringUtilities.getRightPartOfSubmittedStringAfterFirstSearchedWordOccurence(workingContent, earliestStartContext);
            var earliestEndContext = templateContextHandlerPackageProvider.GetHandlerEndContextWordAtEarliestPosition(workingContentWithoutStartContextAndLeftSide);
            if (earliestEndContext == null) yield break;
            var contextContent = StringUtilities.getLeftPartOfSubmittedStringBeforeFirstSearchedWordOccurence(workingContentWithoutStartContextAndLeftSide, correspondingEndContext);
            if (contextContent == workingContentWithoutStartContextAndLeftSide) yield break;

            yield return new ContextComposite
            {
                current = new Context
                {
                    StartContextDelimiter = earliestStartContext,
                    EndContextDelimiter = correspondingEndContext,
                    InnerContent = contextContent,
                },
                childs = depth>0? ExtractAllContextUntilDepth(contextContent,depth-1).ToList() : new List<ContextComposite>(),
            };
            
            var unprocessedPart = StringUtilities.getRightPartOfSubmittedStringAfterFirstSearchedWordOccurence(workingContentWithoutStartContextAndLeftSide, correspondingEndContext);
            var followingContexts = ExtractAllContextUntilDepth(unprocessedPart,depth);
            foreach (var followingContext in followingContexts)
                yield return followingContext;
        }
    }
}
