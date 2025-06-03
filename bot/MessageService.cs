using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using bot.Endpoints;
using bot.Models;

namespace bot.Services;

internal class MessageService(BotContext context, IHttpClientFactory clientFactory)
{
  private readonly BotContext context = context;
  private readonly IHttpClientFactory clientFactory = clientFactory;

  private void Transition(DialogState state, string input)
  {
    switch (state.State)
    {
      case "":
        {

          break;
        }

      case
    }
  }

  private async Task<string> HandleS(string input, string state)
  {
    uint id;
    if (input.ToLower() == "/cancel")
    {
      return string.Empty;
    }
    else if (uint.TryParse(input, out id))
    {
      using var client = this.clientFactory.CreateClient("api-client");

      try
      {

        if (id == 0)
        {
          var response = await client.GetAsync(ApiEndpoints.Status);
        }
        else
        {
          var response = await client.GetAsync($"{ApiEndpoints.Status}/{id}");
        }
      }
      catch
      {

      }

      return string.Empty;
    }
    else
    {
      return state;
    }
  }

  private async Task<string> HandleR(string input, string state)
  {

  }

  private void HandleC(string input)
  {

  }

  public void Update(string input)
  {

  }
}