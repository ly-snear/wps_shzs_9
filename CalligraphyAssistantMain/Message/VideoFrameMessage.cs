// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: VideoFrameMessage.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Calligraphyassistant.Message {

  /// <summary>Holder for reflection information generated from VideoFrameMessage.proto</summary>
  public static partial class VideoFrameMessageReflection {

    #region Descriptor
    /// <summary>File descriptor for VideoFrameMessage.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static VideoFrameMessageReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChdWaWRlb0ZyYW1lTWVzc2FnZS5wcm90bxIcQ2FsbGlncmFwaHlhc3Npc3Rh",
            "bnQuTWVzc2FnZSJRChFWaWRlb0ZyYW1lTWVzc2FnZRINCgVGcmFtZRgBIAEo",
            "BRINCgVXaWR0aBgCIAEoBRIOCgZIZWlnaHQYAyABKAUSDgoGUGFja2V0GAQg",
            "ASgMYgZwcm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Calligraphyassistant.Message.VideoFrameMessage), global::Calligraphyassistant.Message.VideoFrameMessage.Parser, new[]{ "Frame", "Width", "Height", "Packet" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class VideoFrameMessage : pb::IMessage<VideoFrameMessage>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<VideoFrameMessage> _parser = new pb::MessageParser<VideoFrameMessage>(() => new VideoFrameMessage());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<VideoFrameMessage> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Calligraphyassistant.Message.VideoFrameMessageReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public VideoFrameMessage() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public VideoFrameMessage(VideoFrameMessage other) : this() {
      frame_ = other.frame_;
      width_ = other.width_;
      height_ = other.height_;
      packet_ = other.packet_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public VideoFrameMessage Clone() {
      return new VideoFrameMessage(this);
    }

    /// <summary>Field number for the "Frame" field.</summary>
    public const int FrameFieldNumber = 1;
    private int frame_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int Frame {
      get { return frame_; }
      set {
        frame_ = value;
      }
    }

    /// <summary>Field number for the "Width" field.</summary>
    public const int WidthFieldNumber = 2;
    private int width_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int Width {
      get { return width_; }
      set {
        width_ = value;
      }
    }

    /// <summary>Field number for the "Height" field.</summary>
    public const int HeightFieldNumber = 3;
    private int height_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int Height {
      get { return height_; }
      set {
        height_ = value;
      }
    }

    /// <summary>Field number for the "Packet" field.</summary>
    public const int PacketFieldNumber = 4;
    private pb::ByteString packet_ = pb::ByteString.Empty;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public pb::ByteString Packet {
      get { return packet_; }
      set {
        packet_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as VideoFrameMessage);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(VideoFrameMessage other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Frame != other.Frame) return false;
      if (Width != other.Width) return false;
      if (Height != other.Height) return false;
      if (Packet != other.Packet) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (Frame != 0) hash ^= Frame.GetHashCode();
      if (Width != 0) hash ^= Width.GetHashCode();
      if (Height != 0) hash ^= Height.GetHashCode();
      if (Packet.Length != 0) hash ^= Packet.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (Frame != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(Frame);
      }
      if (Width != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(Width);
      }
      if (Height != 0) {
        output.WriteRawTag(24);
        output.WriteInt32(Height);
      }
      if (Packet.Length != 0) {
        output.WriteRawTag(34);
        output.WriteBytes(Packet);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (Frame != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(Frame);
      }
      if (Width != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(Width);
      }
      if (Height != 0) {
        output.WriteRawTag(24);
        output.WriteInt32(Height);
      }
      if (Packet.Length != 0) {
        output.WriteRawTag(34);
        output.WriteBytes(Packet);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int CalculateSize() {
      int size = 0;
      if (Frame != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Frame);
      }
      if (Width != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Width);
      }
      if (Height != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Height);
      }
      if (Packet.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeBytesSize(Packet);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(VideoFrameMessage other) {
      if (other == null) {
        return;
      }
      if (other.Frame != 0) {
        Frame = other.Frame;
      }
      if (other.Width != 0) {
        Width = other.Width;
      }
      if (other.Height != 0) {
        Height = other.Height;
      }
      if (other.Packet.Length != 0) {
        Packet = other.Packet;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            Frame = input.ReadInt32();
            break;
          }
          case 16: {
            Width = input.ReadInt32();
            break;
          }
          case 24: {
            Height = input.ReadInt32();
            break;
          }
          case 34: {
            Packet = input.ReadBytes();
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 8: {
            Frame = input.ReadInt32();
            break;
          }
          case 16: {
            Width = input.ReadInt32();
            break;
          }
          case 24: {
            Height = input.ReadInt32();
            break;
          }
          case 34: {
            Packet = input.ReadBytes();
            break;
          }
        }
      }
    }
    #endif

  }

  #endregion

}

#endregion Designer generated code
