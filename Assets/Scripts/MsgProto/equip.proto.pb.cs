//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: equip.proto
// Note: requires additional types generated from: prototype.proto
// Note: requires additional types generated from: equiptype.proto
namespace Cmd
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"SendEquipList")]
  public partial class SendEquipList : global::ProtoBuf.IExtensible
  {
    public SendEquipList() {}
    
    private Cmd.EMessageID _id = Cmd.EMessageID.EQUIP_LIST_S;
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(Cmd.EMessageID.EQUIP_LIST_S)]
    public Cmd.EMessageID id
    {
      get { return _id; }
      set { _id = value; }
    }
    private readonly global::System.Collections.Generic.List<Cmd.EquipInfo> _list = new global::System.Collections.Generic.List<Cmd.EquipInfo>();
    [global::ProtoBuf.ProtoMember(2, Name=@"list", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<Cmd.EquipInfo> list
    {
      get { return _list; }
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"EquipLevelUpgrade")]
  public partial class EquipLevelUpgrade : global::ProtoBuf.IExtensible
  {
    public EquipLevelUpgrade() {}
    
    private Cmd.EMessageID _id = Cmd.EMessageID.EQUIP_LEVEL_UPGRADE_CS;
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(Cmd.EMessageID.EQUIP_LEVEL_UPGRADE_CS)]
    public Cmd.EMessageID id
    {
      get { return _id; }
      set { _id = value; }
    }
    private uint _equip_type;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"equip_type", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint equip_type
    {
      get { return _equip_type; }
      set { _equip_type = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"EquipLevelUpgradeRet")]
  public partial class EquipLevelUpgradeRet : global::ProtoBuf.IExtensible
  {
    public EquipLevelUpgradeRet() {}
    
    private Cmd.EMessageID _id = Cmd.EMessageID.EQUIP_LEVEL_UPGRADE_SC;
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(Cmd.EMessageID.EQUIP_LEVEL_UPGRADE_SC)]
    public Cmd.EMessageID id
    {
      get { return _id; }
      set { _id = value; }
    }
    private Cmd.EquipInfo _equip;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"equip", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public Cmd.EquipInfo equip
    {
      get { return _equip; }
      set { _equip = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"EquipQualityUpgrade")]
  public partial class EquipQualityUpgrade : global::ProtoBuf.IExtensible
  {
    public EquipQualityUpgrade() {}
    
    private Cmd.EMessageID _id = Cmd.EMessageID.EQUIP_QUALITY_UPGRADE_CS;
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(Cmd.EMessageID.EQUIP_QUALITY_UPGRADE_CS)]
    public Cmd.EMessageID id
    {
      get { return _id; }
      set { _id = value; }
    }
    private uint _equip_type;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"equip_type", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint equip_type
    {
      get { return _equip_type; }
      set { _equip_type = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"EquipQualityUpgradeRet")]
  public partial class EquipQualityUpgradeRet : global::ProtoBuf.IExtensible
  {
    public EquipQualityUpgradeRet() {}
    
    private Cmd.EMessageID _id = Cmd.EMessageID.EQUIP_QUALITY_UPGRADE_SC;
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(Cmd.EMessageID.EQUIP_QUALITY_UPGRADE_SC)]
    public Cmd.EMessageID id
    {
      get { return _id; }
      set { _id = value; }
    }
    private Cmd.EquipInfo _equip;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"equip", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public Cmd.EquipInfo equip
    {
      get { return _equip; }
      set { _equip = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
}