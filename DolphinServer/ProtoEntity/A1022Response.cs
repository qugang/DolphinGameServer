// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: A1022Response.txt
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.ProtocolBuffers;
using pbc = global::Google.ProtocolBuffers.Collections;
using pbd = global::Google.ProtocolBuffers.Descriptors;
using scg = global::System.Collections.Generic;
namespace DolphinServer.ProtoEntity {

  namespace Proto {

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    public static partial class A1022Response {

      #region Extension registration
      public static void RegisterAllExtensions(pb::ExtensionRegistry registry) {
      }
      #endregion
      #region Static variables
      internal static pbd::MessageDescriptor internal__static_A1022Response__Descriptor;
      internal static pb::FieldAccess.FieldAccessorTable<global::DolphinServer.ProtoEntity.A1022Response, global::DolphinServer.ProtoEntity.A1022Response.Builder> internal__static_A1022Response__FieldAccessorTable;
      #endregion
      #region Descriptor
      public static pbd::FileDescriptor Descriptor {
        get { return descriptor; }
      }
      private static pbd::FileDescriptor descriptor;

      static A1022Response() {
        byte[] descriptorData = global::System.Convert.FromBase64String(
            string.Concat(
              "ChFBMTAyMlJlc3BvbnNlLnR4dCJ0Cg1BMTAyMlJlc3BvbnNlEhEKCUVycm9y", 
              "SW5mbxgBIAEoCRIRCglFcnJvckNvZGUYAiABKAUSCwoDVWlkGAMgASgJEg0K", 
              "BUJDYXJkGAQgASgFEgwKBENhcmQYBSABKAUSEwoLQnVaaGFuZ1R5cGUYBiAB", 
            "KAVCHKoCGURvbHBoaW5TZXJ2ZXIuUHJvdG9FbnRpdHk="));
        pbd::FileDescriptor.InternalDescriptorAssigner assigner = delegate(pbd::FileDescriptor root) {
          descriptor = root;
          internal__static_A1022Response__Descriptor = Descriptor.MessageTypes[0];
          internal__static_A1022Response__FieldAccessorTable = 
              new pb::FieldAccess.FieldAccessorTable<global::DolphinServer.ProtoEntity.A1022Response, global::DolphinServer.ProtoEntity.A1022Response.Builder>(internal__static_A1022Response__Descriptor,
                  new string[] { "ErrorInfo", "ErrorCode", "Uid", "BCard", "Card", "BuZhangType", });
          pb::ExtensionRegistry registry = pb::ExtensionRegistry.CreateInstance();
          RegisterAllExtensions(registry);
          return registry;
        };
        pbd::FileDescriptor.InternalBuildGeneratedFileFrom(descriptorData,
            new pbd::FileDescriptor[] {
            }, assigner);
      }
      #endregion

    }
  }
  #region Messages
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
  public sealed partial class A1022Response : pb::GeneratedMessage<A1022Response, A1022Response.Builder> {
    private A1022Response() { }
    private static readonly A1022Response defaultInstance = new A1022Response().MakeReadOnly();
    private static readonly string[] _a1022ResponseFieldNames = new string[] { "BCard", "BuZhangType", "Card", "ErrorCode", "ErrorInfo", "Uid" };
    private static readonly uint[] _a1022ResponseFieldTags = new uint[] { 32, 48, 40, 16, 10, 26 };
    public static A1022Response DefaultInstance {
      get { return defaultInstance; }
    }

    public override A1022Response DefaultInstanceForType {
      get { return DefaultInstance; }
    }

    protected override A1022Response ThisMessage {
      get { return this; }
    }

    public static pbd::MessageDescriptor Descriptor {
      get { return global::DolphinServer.ProtoEntity.Proto.A1022Response.internal__static_A1022Response__Descriptor; }
    }

    protected override pb::FieldAccess.FieldAccessorTable<A1022Response, A1022Response.Builder> InternalFieldAccessors {
      get { return global::DolphinServer.ProtoEntity.Proto.A1022Response.internal__static_A1022Response__FieldAccessorTable; }
    }

    public const int ErrorInfoFieldNumber = 1;
    private bool hasErrorInfo;
    private string errorInfo_ = "";
    public bool HasErrorInfo {
      get { return hasErrorInfo; }
    }
    public string ErrorInfo {
      get { return errorInfo_; }
    }

    public const int ErrorCodeFieldNumber = 2;
    private bool hasErrorCode;
    private int errorCode_;
    public bool HasErrorCode {
      get { return hasErrorCode; }
    }
    public int ErrorCode {
      get { return errorCode_; }
    }

    public const int UidFieldNumber = 3;
    private bool hasUid;
    private string uid_ = "";
    public bool HasUid {
      get { return hasUid; }
    }
    public string Uid {
      get { return uid_; }
    }

    public const int BCardFieldNumber = 4;
    private bool hasBCard;
    private int bCard_;
    public bool HasBCard {
      get { return hasBCard; }
    }
    public int BCard {
      get { return bCard_; }
    }

    public const int CardFieldNumber = 5;
    private bool hasCard;
    private int card_;
    public bool HasCard {
      get { return hasCard; }
    }
    public int Card {
      get { return card_; }
    }

    public const int BuZhangTypeFieldNumber = 6;
    private bool hasBuZhangType;
    private int buZhangType_;
    public bool HasBuZhangType {
      get { return hasBuZhangType; }
    }
    public int BuZhangType {
      get { return buZhangType_; }
    }

    public override bool IsInitialized {
      get {
        return true;
      }
    }

    public override void WriteTo(pb::ICodedOutputStream output) {
      CalcSerializedSize();
      string[] field_names = _a1022ResponseFieldNames;
      if (hasErrorInfo) {
        output.WriteString(1, field_names[4], ErrorInfo);
      }
      if (hasErrorCode) {
        output.WriteInt32(2, field_names[3], ErrorCode);
      }
      if (hasUid) {
        output.WriteString(3, field_names[5], Uid);
      }
      if (hasBCard) {
        output.WriteInt32(4, field_names[0], BCard);
      }
      if (hasCard) {
        output.WriteInt32(5, field_names[2], Card);
      }
      if (hasBuZhangType) {
        output.WriteInt32(6, field_names[1], BuZhangType);
      }
      UnknownFields.WriteTo(output);
    }

    private int memoizedSerializedSize = -1;
    public override int SerializedSize {
      get {
        int size = memoizedSerializedSize;
        if (size != -1) return size;
        return CalcSerializedSize();
      }
    }

    private int CalcSerializedSize() {
      int size = memoizedSerializedSize;
      if (size != -1) return size;

      size = 0;
      if (hasErrorInfo) {
        size += pb::CodedOutputStream.ComputeStringSize(1, ErrorInfo);
      }
      if (hasErrorCode) {
        size += pb::CodedOutputStream.ComputeInt32Size(2, ErrorCode);
      }
      if (hasUid) {
        size += pb::CodedOutputStream.ComputeStringSize(3, Uid);
      }
      if (hasBCard) {
        size += pb::CodedOutputStream.ComputeInt32Size(4, BCard);
      }
      if (hasCard) {
        size += pb::CodedOutputStream.ComputeInt32Size(5, Card);
      }
      if (hasBuZhangType) {
        size += pb::CodedOutputStream.ComputeInt32Size(6, BuZhangType);
      }
      size += UnknownFields.SerializedSize;
      memoizedSerializedSize = size;
      return size;
    }
    public static A1022Response ParseFrom(pb::ByteString data) {
      return ((Builder) CreateBuilder().MergeFrom(data)).BuildParsed();
    }
    public static A1022Response ParseFrom(pb::ByteString data, pb::ExtensionRegistry extensionRegistry) {
      return ((Builder) CreateBuilder().MergeFrom(data, extensionRegistry)).BuildParsed();
    }
    public static A1022Response ParseFrom(byte[] data) {
      return ((Builder) CreateBuilder().MergeFrom(data)).BuildParsed();
    }
    public static A1022Response ParseFrom(byte[] data, pb::ExtensionRegistry extensionRegistry) {
      return ((Builder) CreateBuilder().MergeFrom(data, extensionRegistry)).BuildParsed();
    }
    public static A1022Response ParseFrom(global::System.IO.Stream input) {
      return ((Builder) CreateBuilder().MergeFrom(input)).BuildParsed();
    }
    public static A1022Response ParseFrom(global::System.IO.Stream input, pb::ExtensionRegistry extensionRegistry) {
      return ((Builder) CreateBuilder().MergeFrom(input, extensionRegistry)).BuildParsed();
    }
    public static A1022Response ParseDelimitedFrom(global::System.IO.Stream input) {
      return CreateBuilder().MergeDelimitedFrom(input).BuildParsed();
    }
    public static A1022Response ParseDelimitedFrom(global::System.IO.Stream input, pb::ExtensionRegistry extensionRegistry) {
      return CreateBuilder().MergeDelimitedFrom(input, extensionRegistry).BuildParsed();
    }
    public static A1022Response ParseFrom(pb::ICodedInputStream input) {
      return ((Builder) CreateBuilder().MergeFrom(input)).BuildParsed();
    }
    public static A1022Response ParseFrom(pb::ICodedInputStream input, pb::ExtensionRegistry extensionRegistry) {
      return ((Builder) CreateBuilder().MergeFrom(input, extensionRegistry)).BuildParsed();
    }
    private A1022Response MakeReadOnly() {
      return this;
    }

    public static Builder CreateBuilder() { return new Builder(); }
    public override Builder ToBuilder() { return CreateBuilder(this); }
    public override Builder CreateBuilderForType() { return new Builder(); }
    public static Builder CreateBuilder(A1022Response prototype) {
      return new Builder(prototype);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    public sealed partial class Builder : pb::GeneratedBuilder<A1022Response, Builder> {
      protected override Builder ThisBuilder {
        get { return this; }
      }
      public Builder() {
        result = DefaultInstance;
        resultIsReadOnly = true;
      }
      internal Builder(A1022Response cloneFrom) {
        result = cloneFrom;
        resultIsReadOnly = true;
      }

      private bool resultIsReadOnly;
      private A1022Response result;

      private A1022Response PrepareBuilder() {
        if (resultIsReadOnly) {
          A1022Response original = result;
          result = new A1022Response();
          resultIsReadOnly = false;
          MergeFrom(original);
        }
        return result;
      }

      public override bool IsInitialized {
        get { return result.IsInitialized; }
      }

      protected override A1022Response MessageBeingBuilt {
        get { return PrepareBuilder(); }
      }

      public override Builder Clear() {
        result = DefaultInstance;
        resultIsReadOnly = true;
        return this;
      }

      public override Builder Clone() {
        if (resultIsReadOnly) {
          return new Builder(result);
        } else {
          return new Builder().MergeFrom(result);
        }
      }

      public override pbd::MessageDescriptor DescriptorForType {
        get { return global::DolphinServer.ProtoEntity.A1022Response.Descriptor; }
      }

      public override A1022Response DefaultInstanceForType {
        get { return global::DolphinServer.ProtoEntity.A1022Response.DefaultInstance; }
      }

      public override A1022Response BuildPartial() {
        if (resultIsReadOnly) {
          return result;
        }
        resultIsReadOnly = true;
        return result.MakeReadOnly();
      }

      public override Builder MergeFrom(pb::IMessage other) {
        if (other is A1022Response) {
          return MergeFrom((A1022Response) other);
        } else {
          base.MergeFrom(other);
          return this;
        }
      }

      public override Builder MergeFrom(A1022Response other) {
        if (other == global::DolphinServer.ProtoEntity.A1022Response.DefaultInstance) return this;
        PrepareBuilder();
        if (other.HasErrorInfo) {
          ErrorInfo = other.ErrorInfo;
        }
        if (other.HasErrorCode) {
          ErrorCode = other.ErrorCode;
        }
        if (other.HasUid) {
          Uid = other.Uid;
        }
        if (other.HasBCard) {
          BCard = other.BCard;
        }
        if (other.HasCard) {
          Card = other.Card;
        }
        if (other.HasBuZhangType) {
          BuZhangType = other.BuZhangType;
        }
        this.MergeUnknownFields(other.UnknownFields);
        return this;
      }

      public override Builder MergeFrom(pb::ICodedInputStream input) {
        return MergeFrom(input, pb::ExtensionRegistry.Empty);
      }

      public override Builder MergeFrom(pb::ICodedInputStream input, pb::ExtensionRegistry extensionRegistry) {
        PrepareBuilder();
        pb::UnknownFieldSet.Builder unknownFields = null;
        uint tag;
        string field_name;
        while (input.ReadTag(out tag, out field_name)) {
          if(tag == 0 && field_name != null) {
            int field_ordinal = global::System.Array.BinarySearch(_a1022ResponseFieldNames, field_name, global::System.StringComparer.Ordinal);
            if(field_ordinal >= 0)
              tag = _a1022ResponseFieldTags[field_ordinal];
            else {
              if (unknownFields == null) {
                unknownFields = pb::UnknownFieldSet.CreateBuilder(this.UnknownFields);
              }
              ParseUnknownField(input, unknownFields, extensionRegistry, tag, field_name);
              continue;
            }
          }
          switch (tag) {
            case 0: {
              throw pb::InvalidProtocolBufferException.InvalidTag();
            }
            default: {
              if (pb::WireFormat.IsEndGroupTag(tag)) {
                if (unknownFields != null) {
                  this.UnknownFields = unknownFields.Build();
                }
                return this;
              }
              if (unknownFields == null) {
                unknownFields = pb::UnknownFieldSet.CreateBuilder(this.UnknownFields);
              }
              ParseUnknownField(input, unknownFields, extensionRegistry, tag, field_name);
              break;
            }
            case 10: {
              result.hasErrorInfo = input.ReadString(ref result.errorInfo_);
              break;
            }
            case 16: {
              result.hasErrorCode = input.ReadInt32(ref result.errorCode_);
              break;
            }
            case 26: {
              result.hasUid = input.ReadString(ref result.uid_);
              break;
            }
            case 32: {
              result.hasBCard = input.ReadInt32(ref result.bCard_);
              break;
            }
            case 40: {
              result.hasCard = input.ReadInt32(ref result.card_);
              break;
            }
            case 48: {
              result.hasBuZhangType = input.ReadInt32(ref result.buZhangType_);
              break;
            }
          }
        }

        if (unknownFields != null) {
          this.UnknownFields = unknownFields.Build();
        }
        return this;
      }


      public bool HasErrorInfo {
        get { return result.hasErrorInfo; }
      }
      public string ErrorInfo {
        get { return result.ErrorInfo; }
        set { SetErrorInfo(value); }
      }
      public Builder SetErrorInfo(string value) {
        pb::ThrowHelper.ThrowIfNull(value, "value");
        PrepareBuilder();
        result.hasErrorInfo = true;
        result.errorInfo_ = value;
        return this;
      }
      public Builder ClearErrorInfo() {
        PrepareBuilder();
        result.hasErrorInfo = false;
        result.errorInfo_ = "";
        return this;
      }

      public bool HasErrorCode {
        get { return result.hasErrorCode; }
      }
      public int ErrorCode {
        get { return result.ErrorCode; }
        set { SetErrorCode(value); }
      }
      public Builder SetErrorCode(int value) {
        PrepareBuilder();
        result.hasErrorCode = true;
        result.errorCode_ = value;
        return this;
      }
      public Builder ClearErrorCode() {
        PrepareBuilder();
        result.hasErrorCode = false;
        result.errorCode_ = 0;
        return this;
      }

      public bool HasUid {
        get { return result.hasUid; }
      }
      public string Uid {
        get { return result.Uid; }
        set { SetUid(value); }
      }
      public Builder SetUid(string value) {
        pb::ThrowHelper.ThrowIfNull(value, "value");
        PrepareBuilder();
        result.hasUid = true;
        result.uid_ = value;
        return this;
      }
      public Builder ClearUid() {
        PrepareBuilder();
        result.hasUid = false;
        result.uid_ = "";
        return this;
      }

      public bool HasBCard {
        get { return result.hasBCard; }
      }
      public int BCard {
        get { return result.BCard; }
        set { SetBCard(value); }
      }
      public Builder SetBCard(int value) {
        PrepareBuilder();
        result.hasBCard = true;
        result.bCard_ = value;
        return this;
      }
      public Builder ClearBCard() {
        PrepareBuilder();
        result.hasBCard = false;
        result.bCard_ = 0;
        return this;
      }

      public bool HasCard {
        get { return result.hasCard; }
      }
      public int Card {
        get { return result.Card; }
        set { SetCard(value); }
      }
      public Builder SetCard(int value) {
        PrepareBuilder();
        result.hasCard = true;
        result.card_ = value;
        return this;
      }
      public Builder ClearCard() {
        PrepareBuilder();
        result.hasCard = false;
        result.card_ = 0;
        return this;
      }

      public bool HasBuZhangType {
        get { return result.hasBuZhangType; }
      }
      public int BuZhangType {
        get { return result.BuZhangType; }
        set { SetBuZhangType(value); }
      }
      public Builder SetBuZhangType(int value) {
        PrepareBuilder();
        result.hasBuZhangType = true;
        result.buZhangType_ = value;
        return this;
      }
      public Builder ClearBuZhangType() {
        PrepareBuilder();
        result.hasBuZhangType = false;
        result.buZhangType_ = 0;
        return this;
      }
    }
    static A1022Response() {
      object.ReferenceEquals(global::DolphinServer.ProtoEntity.Proto.A1022Response.Descriptor, null);
    }
  }

  #endregion

}

#endregion Designer generated code
