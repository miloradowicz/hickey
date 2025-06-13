using System.Diagnostics.CodeAnalysis;
using Npgsql.Replication;

namespace app.Models;

internal class DialogState
{
  public DialogState() { }

  [SetsRequiredMembers]
  public DialogState(User user)
  {
    (this.User, this.State) = (user, string.Empty);
  }

  [SetsRequiredMembers]
  public DialogState(User user, string state)
  {
    (this.User, this.State) = (user, state);
  }

  private string state = string.Empty;

  public required User User { get; set; }

  public string State
  {
    get => this.state;
    set
    {
      this.state = value;
      this.LastAction = DateTime.Now;
    }
  }

  public DateTime? LastAction { get; private set; } = null;
}