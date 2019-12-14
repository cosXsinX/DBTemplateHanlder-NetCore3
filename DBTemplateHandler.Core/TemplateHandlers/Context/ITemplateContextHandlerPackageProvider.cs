using System.Collections.Generic;

namespace DBTemplateHandler.Core.TemplateHandlers.Context
{
    public interface ITemplateContextHandlerPackageProvider<U> where U : ITemplateContextHandler
    {
        bool ContainsAHandlerEndContextOfType(string submittedString);
        bool ContainsAHandlerStartContextOfType(string submittedString);
        int CountEndContextWordIn(string submittedString);
        int CountStartContextWordIn(string submittedString);
        IDictionary<string, U> GetContextHandlerByEndContextSignature();
        IDictionary<string, U> GetContextHandlerByStartContextSignature();
        string GetHandlerEndContextWordAtEarliestPosition(string submittedString);
        string GetHandlerEndContextWordAtLatestPosition(string submittedString);
        IList<U> GetHandlers();
        string GetHandlerStartContextWordAtEarliestPosition(string SubmittedString);
        string GetHandlerStartContextWordAtLattestPosition(string SubmittedString);
        U GetStartContextCorrespondingContextHandler(string StartContextWrapper);
        string GetStartContextCorrespondingEndContext(string StartContextWrapper);
    }
}