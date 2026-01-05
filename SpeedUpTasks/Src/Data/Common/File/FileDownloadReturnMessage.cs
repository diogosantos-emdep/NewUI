using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace Emdep.Geos.Data.Common.File
{

    [MessageContract(IsWrapped = true)]
    public class FileDownloadReturnMessage : IDisposable
    {
        public FileDownloadReturnMessage(FileMetaData metaData, Stream stream)
        {
            this.DownloadedFileMetadata = metaData;
            this.FileByteStream = stream;
        }
        public FileDownloadReturnMessage()
        {

        }
        [MessageHeader(MustUnderstand = true)]
        public FileMetaData DownloadedFileMetadata;
        [MessageBodyMember(Order = 1)]
        public Stream FileByteStream;

        public void Dispose()
        {
            if (FileByteStream != null)
            {
                FileByteStream.Close();
                FileByteStream.Dispose();
                FileByteStream = null;
            }  
        }
    }
}