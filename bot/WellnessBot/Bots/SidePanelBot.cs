using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AdaptiveCards;
using AdaptiveCards.Templating;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Teams;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using SidePanel.Controllers;

namespace Microsoft.BotBuilderSamples.Bots
{
    public class SidePanelBot : TeamsActivityHandler
    {
        private readonly IConfiguration _config;

        public SidePanelBot(IConfiguration configuration)
        {
            _config = configuration;
        }
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            if (turnContext.Activity.Value == null)
            {
                Attachment adaptiveCardAttachment = GetAdaptiveCardAttachment("AgendaCard.json", null);
                await turnContext.SendActivityAsync(MessageFactory.Attachment(adaptiveCardAttachment));
            }
            else
            {
                await HandleActions(turnContext);
            }
       }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var welcomeText = "Hello and welcome " + turnContext.Activity.From.Name + " to the Meeting Extensibility SidePanel app.";
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken);
                }
            }
        }

        protected override async Task OnConversationUpdateActivityAsync(ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            HomeController.serviceUrl = turnContext.Activity.ServiceUrl;
            HomeController.conversationId = turnContext.Activity.Conversation.Id;
            await base.OnConversationUpdateActivityAsync(turnContext, cancellationToken);
        }

        private Attachment GetAdaptiveCardAttachment(string fileName, object cardData)
        {
            var templateJson = File.ReadAllText("./Cards/" + fileName);
            AdaptiveCardTemplate template = new AdaptiveCardTemplate(templateJson);

            string cardJson = template.Expand(cardData);
            AdaptiveCardParseResult result = AdaptiveCard.FromJson(cardJson);

            // Get card from result
            AdaptiveCard card = result.Card;

            var adaptiveCardAttachment = new Attachment
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card,
            };
            return adaptiveCardAttachment;
        }

        private async Task HandleActions(ITurnContext<IMessageActivity> turnContext)
        {
            
        }
    }
}
