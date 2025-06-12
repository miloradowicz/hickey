using System.Diagnostics.CodeAnalysis;

namespace app.Models;

internal class Device : DeviceBase
{
  [SetsRequiredMembers]
  public Device(string Name, string Address, ushort Port, Credentials credentials) : base(Name, Address, Port)
  {
    this.Credentials = new Credentials(credentials);
  }

  [SetsRequiredMembers]
  public Device(DeviceBase device, Credentials credentials) : base(device.Name, device.Address, device.Port)
  {
    this.Credentials = new Credentials(credentials);
  }

  public uint Id { get; set; }
  public required Credentials Credentials { get; set; }

  public void Deconstruct(out string Name, out string Address, out ushort Port, out Credentials Credentials)
  {
    (Name, Address, Port, Credentials) = (this.Name, this.Address, this.Port, this.Credentials);
  }
}
