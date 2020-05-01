using DBTemplateHandler.Core.TemplateHandlers.Context;
using DBTemplateHandler.Core.TemplateHandlers.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBTemplateHandler.Core.TemplateHandlers.Handlers
{
    public class TemplateContext
    {
        public string StartContextDelimiter { get; set; }
        public string EndContextDelimiter { get; set; }
        public string InnerContent { get; set; }
        public int StartIndex { get; set; }
        public int ContextDepth { get; set; }
        public string Content { get => String.Join(String.Empty, new[] { StartContextDelimiter, InnerContent, EndContextDelimiter }); }
    }

    public class DatabaseTemplateContext : TemplateContext{}
    public class TableTemplateContext : TemplateContext { }
    public class ColumnTemplateContext : TemplateContext { }
    public class ConstraintTemplateContext : TemplateContext { }
    public class FunctionTemplateContext: TemplateContext { }

    public class TemplateContextComposite
    {
        public TemplateContext current { get; set; }
        public IList<TemplateContextComposite> childs { get; set; }
    }


    public class ContextVisitor<U> where U : ITemplateContextHandler
    {
        private readonly ITemplateContextHandlerPackageProvider<U> templateContextHandlerPackageProvider;
        
        public ContextVisitor(ITemplateContextHandlerPackageProvider<U> templateContextHandlerPackageProvider)
        {
            if (templateContextHandlerPackageProvider == null) throw new ArgumentNullException(nameof(templateContextHandlerPackageProvider));
            this.templateContextHandlerPackageProvider = templateContextHandlerPackageProvider;
        }

        public HashSet<string> ToDelimiterFilter(IEnumerable<string> contextDelimiters)
        {
            return new HashSet<string>(contextDelimiters.SelectMany(ToDelimiterFilter).Distinct());
        }

        public IEnumerable<string> ToDelimiterFilter(string contextDelimiter)
        {
            string current = String.Empty;
            foreach(char character in contextDelimiter)
            {
                current = String.Concat(current, character);
                yield return current;
            }
        }


        private class StartContextInfo
        {
            public string StartContextIdentifier { get; set; }
            public int StartContextIndex { get; set; }
            public string CorrespondingEndContextIdentifier { get; set; }
        }


        private TemplateContextComposite ToNewContextComposite(TemplateContext context)
        {
            return new TemplateContextComposite()
            {
                current = context,
                childs = new List<TemplateContextComposite>(),
            };
        }

        private TemplateContextComposite ToRootContextComposite(string templateContent)
        {
            return new TemplateContextComposite()
            {
                current = new TemplateContext()
                {
                    ContextDepth = -200,
                    StartContextDelimiter = String.Empty,
                    EndContextDelimiter = String.Empty,
                    InnerContent = templateContent,
                    StartIndex = 0,
                },
                childs = new List<TemplateContextComposite>(),
            };
        }

        public IEnumerable<TemplateContextComposite> ExtractAllContextUntilDepth(string templateContent, int depth)
        {
            var contextes = ExtractAllContextUntilDepthPrivList(templateContent, depth);
            var contextComposites = contextes.Select(ToNewContextComposite).Reverse().ToList();
            Stack<TemplateContextComposite> contextCompositeStack = new Stack<TemplateContextComposite>();

            IList<TemplateContextComposite> parentContextCompositesChilds = new List<TemplateContextComposite>();
            var rootComposite = ToRootContextComposite(templateContent);
            TemplateContextComposite parentContextComposite = rootComposite;
            TemplateContextComposite formerContext = rootComposite;
            foreach (var context in contextComposites)
            {
                if(formerContext.current.ContextDepth < context.current.ContextDepth )
                {
                    parentContextCompositesChilds = new List<TemplateContextComposite>();
                    parentContextComposite = formerContext;
                    parentContextComposite.childs.Add(context);
                    contextCompositeStack.Push(context);
                    formerContext = context;
                    continue;
                }

                if(formerContext.current.ContextDepth == context.current.ContextDepth)
                {
                    parentContextComposite.childs.Add(context);
                    formerContext = context;
                    continue;
                }

                if(formerContext.current.ContextDepth> context.current.ContextDepth)
                {
                    var ancestors = contextCompositeStack.Where(m => m.current.ContextDepth < context.current.ContextDepth);
                    var parent = ancestors.FirstOrDefault();
                    if (parent == null)
                    {
                        parentContextComposite = rootComposite;
                        contextCompositeStack.Clear();
                    }
                    else
                    {
                        contextCompositeStack = new Stack<TemplateContextComposite>(ancestors);
                        parentContextComposite = parent;
                    }

                    parentContextComposite.childs.Add(context);
                    contextCompositeStack.Push(context);
                }
                formerContext = context;
            }
            return rootComposite.childs.Reverse().ToList();
        }

        private IEnumerable<TemplateContext> ExtractAllContextUntilDepthPriv(string templateContent,int depth)
        {
            var handlers = templateContextHandlerPackageProvider.GetHandlers();
            var handlersByStartContextes = handlers.ToDictionary(m => m.StartContext);
            var startContextes = new HashSet<string>(handlersByStartContextes.Keys);
            var endContextes = new HashSet<string>(handlers.Select(m => m.EndContext).Distinct());
            var startContextDelimiterFilter = ToDelimiterFilter(startContextes);
            var endContextDelimiterFilter = ToDelimiterFilter(endContextes);
            var currentCharAggregation = String.Empty;

            Stack<StartContextInfo> startContextInfoStack = new Stack<StartContextInfo>();
            for(int currentCharIndex = 0; currentCharIndex<templateContent.Length;currentCharIndex++ )
            {
                var currentChar = templateContent[currentCharIndex];
                currentCharAggregation = string.Concat(currentCharAggregation, currentChar);
                if(!startContextDelimiterFilter.Contains(currentCharAggregation) && !endContextDelimiterFilter.Contains(currentCharAggregation))
                {
                    currentCharAggregation = String.Empty;
                    continue;
                }

                if ((startContextDelimiterFilter.Contains(currentCharAggregation) || endContextDelimiterFilter.Contains(currentCharAggregation)) 
                    && (!startContextes.Contains(currentCharAggregation) && !endContextes.Contains(currentCharAggregation))) continue;

                if(startContextDelimiterFilter.Contains(currentCharAggregation) && startContextes.Contains(currentCharAggregation))
                {
                    var startContextInfo = new StartContextInfo()
                    {
                        StartContextIdentifier = currentCharAggregation,
                        StartContextIndex = currentCharIndex - currentCharAggregation.Length + 1,
                        CorrespondingEndContextIdentifier = handlersByStartContextes[currentCharAggregation].EndContext,
                    };
                    startContextInfoStack.Push(startContextInfo);
                    currentCharAggregation = String.Empty;
                    continue;
                }
                

                if(endContextDelimiterFilter.Contains(currentCharAggregation) && endContextes.Contains(currentCharAggregation))
                {
                    var lastStartContextInfo = startContextInfoStack.FirstOrDefault();
                    if(lastStartContextInfo == null || lastStartContextInfo?.CorrespondingEndContextIdentifier != currentCharAggregation)
                    {
                        currentCharAggregation = String.Empty;
                        continue;
                    }

                    int innerStartContextIndex = lastStartContextInfo.StartContextIndex + lastStartContextInfo.StartContextIdentifier.Length;
                    int InnerContextLen = currentCharIndex + 1 - innerStartContextIndex - lastStartContextInfo.CorrespondingEndContextIdentifier.Length;
                    int contextDepth = startContextInfoStack.Count;
                    if (depth + 1 >= contextDepth)
                    {
                        yield return new TemplateContext()
                        {
                            StartContextDelimiter = lastStartContextInfo.StartContextIdentifier,
                            EndContextDelimiter = lastStartContextInfo.CorrespondingEndContextIdentifier,
                            InnerContent = templateContent.Substring(innerStartContextIndex, InnerContextLen),
                            ContextDepth = contextDepth,
                            StartIndex = lastStartContextInfo.StartContextIndex,
                        };
                    }
                    startContextInfoStack.Pop();
                    currentCharAggregation = String.Empty;
                    continue;
                }
            }
        }

        private IList<TemplateContext> ExtractAllContextUntilDepthPrivList(string templateContent, int depth)
        {
            return ExtractAllContextUntilDepthPriv(templateContent, depth).ToList();
        }
    }
}
