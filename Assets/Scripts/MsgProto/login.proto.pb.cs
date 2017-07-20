//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: login.proto
// Note: requires additional types generated from: prototype.proto
// Note: requires additional types generated from: errorcode.proto
namespace Cmd
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"VerifyVersion")]
  public partial class VerifyVersion : global::ProtoBuf.IExtensible
  {
    public VerifyVersion() {}
    
    private Cmd.EMessageID _id = Cmd.EMessageID.VERIFY_VERSION_CS;
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(Cmd.EMessageID.VERIFY_VERSION_CS)]
    public Cmd.EMessageID id
    {
      get { return _id; }
      set { _id = value; }
    }
    private uint _clientversion;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"clientversion", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint clientversion
    {
      get { return _clientversion; }
      set { _clientversion = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"LoginReq")]
  public partial class LoginReq : global::ProtoBuf.IExtensible
  {
    public LoginReq() {}
    
    private Cmd.EMessageID _id = Cmd.EMessageID.LOGIN_LOGIN_CS;
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(Cmd.EMessageID.LOGIN_LOGIN_CS)]
    public Cmd.EMessageID id
    {
      get { return _id; }
      set { _id = value; }
    }
    private byte[] _account;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"account", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public byte[] account
    {
      get { return _account; }
      set { _account = value; }
    }
    private uint _zoneid;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"zoneid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint zoneid
    {
      get { return _zoneid; }
      set { _zoneid = value; }
    }
    private string _verifier = "";
    [global::ProtoBuf.ProtoMember(4, IsRequired = false, Name=@"verifier", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string verifier
    {
      get { return _verifier; }
      set { _verifier = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"LoginRet")]
  public partial class LoginRet : global::ProtoBuf.IExtensible
  {
    public LoginRet() {}
    
    private Cmd.EMessageID _id = Cmd.EMessageID.LOGIN_LOGIN_SC;
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(Cmd.EMessageID.LOGIN_LOGIN_SC)]
    public Cmd.EMessageID id
    {
      get { return _id; }
      set { _id = value; }
    }
    private byte[] _account = null;
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"account", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public byte[] account
    {
      get { return _account; }
      set { _account = value; }
    }
    private uint _tempid = default(uint);
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"tempid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint tempid
    {
      get { return _tempid; }
      set { _tempid = value; }
    }
    private string _gatewayip = "";
    [global::ProtoBuf.ProtoMember(4, IsRequired = false, Name=@"gatewayip", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string gatewayip
    {
      get { return _gatewayip; }
      set { _gatewayip = value; }
    }
    private uint _gatewayport = default(uint);
    [global::ProtoBuf.ProtoMember(5, IsRequired = false, Name=@"gatewayport", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint gatewayport
    {
      get { return _gatewayport; }
      set { _gatewayport = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"LoginGatewayReq")]
  public partial class LoginGatewayReq : global::ProtoBuf.IExtensible
  {
    public LoginGatewayReq() {}
    
    private Cmd.EMessageID _id = Cmd.EMessageID.LOGIN_GATEW_CS;
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(Cmd.EMessageID.LOGIN_GATEW_CS)]
    public Cmd.EMessageID id
    {
      get { return _id; }
      set { _id = value; }
    }
    private byte[] _account;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"account", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public byte[] account
    {
      get { return _account; }
      set { _account = value; }
    }
    private uint _tempid;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"tempid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint tempid
    {
      get { return _tempid; }
      set { _tempid = value; }
    }
    private string _appVersion = "";
    [global::ProtoBuf.ProtoMember(4, IsRequired = false, Name=@"appVersion", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string appVersion
    {
      get { return _appVersion; }
      set { _appVersion = value; }
    }
    private uint _deviceId = default(uint);
    [global::ProtoBuf.ProtoMember(5, IsRequired = false, Name=@"deviceId", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint deviceId
    {
      get { return _deviceId; }
      set { _deviceId = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"LoginGatewayRet")]
  public partial class LoginGatewayRet : global::ProtoBuf.IExtensible
  {
    public LoginGatewayRet() {}
    
    private Cmd.EMessageID _id = Cmd.EMessageID.LOGIN_GATEW_SC;
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(Cmd.EMessageID.LOGIN_GATEW_SC)]
    public Cmd.EMessageID id
    {
      get { return _id; }
      set { _id = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"LoginCrossReq")]
  public partial class LoginCrossReq : global::ProtoBuf.IExtensible
  {
    public LoginCrossReq() {}
    
    private Cmd.EMessageID _id = Cmd.EMessageID.LOGIN_CROSS_CS;
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(Cmd.EMessageID.LOGIN_CROSS_CS)]
    public Cmd.EMessageID id
    {
      get { return _id; }
      set { _id = value; }
    }
    private ulong _userid;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"userid", DataFormat = global::ProtoBuf.DataFormat.FixedSize)]
    public ulong userid
    {
      get { return _userid; }
      set { _userid = value; }
    }
    private uint _tempid;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"tempid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint tempid
    {
      get { return _tempid; }
      set { _tempid = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"LoginCrossRet")]
  public partial class LoginCrossRet : global::ProtoBuf.IExtensible
  {
    public LoginCrossRet() {}
    
    private Cmd.EMessageID _id = Cmd.EMessageID.LOGIN_CROSS_SC;
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(Cmd.EMessageID.LOGIN_CROSS_SC)]
    public Cmd.EMessageID id
    {
      get { return _id; }
      set { _id = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
}