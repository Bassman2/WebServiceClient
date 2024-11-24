namespace WebServiceClient.XSerializer;

// default XML Serializer is not trimmable and AOT compatible

public interface IXSerializable
{
    void ReadX(XElement elm);
    //void WriteX(XElement elm);
}

