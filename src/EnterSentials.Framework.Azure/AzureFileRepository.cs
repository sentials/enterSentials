using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace EnterSentials.Framework.Azure
{
    public class AzureFileRepository : ScopeLimitedObject, IFileRepository
    {
        private CloudBlobClient blobClient = null;
        private CloudBlobContainer blobContainer = null;


        private Guid GetFileIdFor(string fileName, params object[] keys)
        {
            Guard.AgainstNullOrEmpty(fileName, "File name cannot be empty.");
            //keys = keys ?? new object[1];
            
            //var nameBytes = Encoding.Default.GetBytes(fileName);
            //var keyBytes = Encoding.Default.GetBytes(keys.Aggregate("", (acc, key) => string.Format("{0}{1}", acc, key)));
            //var hashBytes = new byte[nameBytes.Length + keyBytes.Length];
            
            //Buffer.BlockCopy(nameBytes, 0, hashBytes, 0, nameBytes.Length);
            
            //if (keyBytes.Any())
            //    Buffer.BlockCopy(keyBytes, 0, hashBytes, nameBytes.Length, keyBytes.Length);

            //var hash = MD5.Create().ComputeHash(hashBytes);

            //return new Guid(hash);

            DateTime timeStamp = DateTime.UtcNow;
            
            var nameBytes = Encoding.Default.GetBytes(string.Format("{0}{1}", fileName, timeStamp.ToString()));
            var hash = MD5.Create().ComputeHash(nameBytes);

            return new Guid(hash);

        }


        public bool Exists(Guid fileId)
        {
            var doesOrNot = false;
            var blob = blobContainer.GetBlockBlobReference(fileId.ToString());
            if (blob != null)
            {
                try
                {
                    blob.FetchAttributes();
                    doesOrNot = true;
                }
                catch
                { }
            }

            return doesOrNot;
        }


        public bool Exists(out Guid fileId, string fileName, params object[] keys)
        {
            fileId = GetFileIdFor(fileName, keys);
            return Exists(fileId);
        }


        private void AddOrUpdateFile(Guid fileId, byte[] fileContent)
        {
           // blobContainer.GetBlockBlobReference(fileId.ToString()).UploadFromByteArray(fileContent, 0, fileContent.Length);
            var blobBlock = blobContainer.GetBlockBlobReference(fileId.ToString());
            blobBlock.UploadFromByteArray(fileContent, 0, fileContent.Length);
        }


        public void Update(Guid fileId, byte[] fileContent)
        { AddOrUpdateFile(fileId, fileContent); }

        public Guid AddOrUpdate(string fileName, byte[] fileContent, params object[] keys)
        {
            //var fileId = GetFileIdFor(fileName, keys);
            var fileId = Guid.NewGuid();
            AddOrUpdateFile(fileId, fileContent);
            return fileId;
        }


        public void Remove(Guid fileId)
        { blobContainer.GetBlockBlobReference(fileId.ToString()).DeleteIfExists(DeleteSnapshotsOption.IncludeSnapshots); }


        public byte[] Get(Guid fileId)
        {
            var content = (byte[]) null;

            using (var stream = new MemoryStream())
            {
                blobContainer.GetBlockBlobReference(fileId.ToString()).DownloadToStream(stream);
                //content = stream.GetBuffer();
                content = stream.ToArray();

            }

            return content;

        }


        protected override void OnDisposeExplicit()
        {
            base.OnDisposeExplicit();
            this.blobClient = null;
            this.blobContainer = null;
        }

        
        public AzureFileRepository(CloudStorageAccount storageAccount, string containerName)
	    {
            Guard.AgainstNull(storageAccount, "storageAccount");
            Guard.AgainstNullOrEmpty(containerName, "containerName");
            this.blobClient = storageAccount.CreateCloudBlobClient();
            this.blobContainer = blobClient.GetContainerReference(containerName);
	    }
    }
}
