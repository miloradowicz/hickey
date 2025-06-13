
using System.Diagnostics.CodeAnalysis;

namespace app.Models;

internal class DeviceBase
{
  [SetsRequiredMembers]
  public DeviceBase(string name, string address, ushort port)
  {
    (this.Name, this.Address, this.Port) = (name, address, port);
  }

  public required string Name { get; set; }
  public required string Address { get; set; }
  public required ushort Port { get; set; }

  public void Deconstruct(out string Name, out string Address, out ushort Port)
  {
    (Name, Address, Port) = (this.Name, this.Address, this.Port);
  }
}
