using Flerp.Properties;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading;

namespace Flerp.DomainModel
{
    public class FsFile : FsEntity
    {
        public FsFile(string path, DocumentBase parent = null)
        {
            FullPath = path;
            if (parent != null) ParentId = parent.Id;
        }


        protected override FileSystemInfo FsInfo { get { return new FileInfo(FullPath); } }

        public override string ComputeHash(CancellationTokenSource token)
        {
            byte[] hash;

            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(FullPath))
                {
                    if (token.IsCancellationRequested) return null;
                    hash = md5.ComputeHash(stream);
                }
            }
            return BitConverter.ToString(hash).Replace("-", string.Empty).ToLowerInvariant();
        }

        public override void Delete() { DeleteFile(FullPath); }

        private static void DeleteFile(string path)
        {
            if (!File.Exists(path)) return;

            var attr = File.GetAttributes(path);
            if ((attr & FileAttributes.ReadOnly) == FileAttributes.ReadOnly) File.SetAttributes(path, attr ^ FileAttributes.ReadOnly);
            File.Delete(path);
        }

        protected override void SetReadOnly()
        {
            var attr = File.GetAttributes(FullPath);

            attr = attr | FileAttributes.ReadOnly;
            File.SetAttributes(FullPath, attr);
        }

        protected override void SetReadWrite()
        {
            var attr = File.GetAttributes(FullPath);

            attr = attr & ~FileAttributes.ReadOnly;
            File.SetAttributes(FullPath, attr);
        }

        public FsDirectory ToDirectory()
        {
            var sourcePath = FullPath;
            string dirPath = null;
            string filePath = null;
            FsDirectory n = null;

            try
            {
                dirPath = sourcePath.Substring(0, sourcePath.Length - Path.GetExtension(sourcePath).Length);
                filePath = dirPath + Path.DirectorySeparatorChar + Path.GetFileName(sourcePath);

                Directory.CreateDirectory(dirPath);
                n = new FsDirectory(dirPath);

                var h = n.FsHash;                   // Force hash computation (to replace "dir_pad.flerp" if required).
                Console.WriteLine(h);

                File.Move(sourcePath, filePath);
                Parent.MasterExtension = string.Empty;

                return n;
            }
            finally
            {
                if (n == null)
                {
                    Parent.MasterExtension = Path.GetExtension(sourcePath);
                    if (filePath != null && File.Exists(filePath))
                    {
                        File.Move(filePath, sourcePath);
                        Directory.Delete(dirPath);
                    }
                }
                Parent.Save();
            }
        }

        public override FsEntity ToMaster(IPersistable entity, bool removeOriginal, CancellationTokenSource token)
        {
            var targetId = entity.Id;
            var targetPath = GetDocDir(targetId);
            var filePath = targetPath + GetFlerpFilename(targetId, FullPath);

            try
            {
                if (Extension == Resources.FileExtension_Zip)
                {
                    Directory.CreateDirectory(targetPath + GetFlerpFilename(targetId, FullPath, string.Empty));
                    filePath = targetPath + GetFlerpFilename(targetId, FullPath, string.Empty) + GetFlerpFilename(targetId, FullPath);
                }

                token.Token.ThrowIfCancellationRequested();

                if (removeOriginal) File.Move(FullPath, filePath);
                else File.Copy(FullPath, filePath, true);

                return Create(filePath);
            }
            catch
            {
                if (File.Exists(filePath)) Create(filePath).Delete();
                throw;
            }
        }
    }
}