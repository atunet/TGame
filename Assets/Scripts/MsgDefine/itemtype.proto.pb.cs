//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: itemtype.proto
namespace Cmd
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"ItemInfo")]
  public partial class ItemInfo : global::ProtoBuf.IExtensible
  {
    public ItemInfo() {}
    
    private ulong _thisid;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"thisid", DataFormat = global::ProtoBuf.DataFormat.FixedSize)]
    public ulong thisid
    {
      get { return _thisid; }
      set { _thisid = value; }
    }
    private uint _itemid;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"itemid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint itemid
    {
      get { return _itemid; }
      set { _itemid = value; }
    }
    private ulong _uid;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"uid", DataFormat = global::ProtoBuf.DataFormat.FixedSize)]
    public ulong uid
    {
      get { return _uid; }
      set { _uid = value; }
    }
    private uint _num;
    [global::ProtoBuf.ProtoMember(4, IsRequired = true, Name=@"num", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint num
    {
      get { return _num; }
      set { _num = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"ItemNotiType")]
  public partial class ItemNotiType : global::ProtoBuf.IExtensible
  {
    public ItemNotiType() {}
    
    private uint _type;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"type", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint type
    {
      get { return _type; }
      set { _type = value; }
    }
    private uint _itemid;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"itemid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint itemid
    {
      get { return _itemid; }
      set { _itemid = value; }
    }
    private uint _count;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"count", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint count
    {
      get { return _count; }
      set { _count = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"ItemCountType")]
  public partial class ItemCountType : global::ProtoBuf.IExtensible
  {
    public ItemCountType() {}
    
    private uint _itemid;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"itemid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint itemid
    {
      get { return _itemid; }
      set { _itemid = value; }
    }
    private uint _count;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"count", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint count
    {
      get { return _count; }
      set { _count = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
}