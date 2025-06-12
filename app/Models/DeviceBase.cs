
using System.Diagnostics.CodeAnalysis;

namespace app.Models;

[SetsRequiredMembers]
internal class DeviceBase(string name, string address, ushort port)
{
  public required string Name { get; set; } = name;
  public required string Address { get; set; } = address;
  public required ushort Port { get; set; } = port;

  public void Deconstruct(out string Name, out string Address, out ushort Port)
  {
    (Name, Address, Port) = (this.Name, this.Address, this.Port);
  }
}
