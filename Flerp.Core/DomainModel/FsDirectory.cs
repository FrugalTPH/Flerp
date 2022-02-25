using Flerp.Properties;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace Flerp.DomainModel
{
    public class FsDirectory : FsEntity
    {
        public FsDirectory(string path, DocumentBase parent = null)
        {
            FullPath = path;
            if (parent != null) ParentId = parent.Id;
        }


        protected override FileSystemInfo FsInfo { get { return new DirectoryInfo(FullPath); } }

        public override string ComputeHash(CancellationTokenSource token)
        {
            byte[] hash;

            using (var md5 = MD5.Create())
            {
                var files = Directory.GetFiles(FullPath, Resources.Wildcard_AnyFilenameDotAnyExtension, SearchOption.AllDirectories);
                var dirs = Directory.GetDirectories(FullPath, Resources.Wildcard_AnyFilenameDotAnyExtension, SearchOption.AllDirectories);

                if (!files.Any())
                {
                    CreateHiddenFile(FullPath + Path.DirectorySeparatorChar + Resources.FileExtension_Flerp);
                    files = Directory.GetFiles(FullPath, Resources.Wildcard_AnyFilenameDotAnyExtension, SearchOption.AllDirectories);
                }

                var entries = files.Concat(dirs).OrderBy(x => x).ToList();

                byte[] bytes = null;
                for (var i = 0; i < entries.Count; i++)
                {
                    token.Token.ThrowIfCancellationRequested();

                    var entry = entries[i];

                    var relPath = entry.Substring(FullPath.Length + 1);
                    bytes = Encoding.UTF8.GetBytes(relPath.ToLowerInvariant());
                    md5.TransformBlock(bytes, 0, bytes.Length, bytes, 0);

                    if (dirs.Contains(entry)) continue;

                    bytes = File.ReadAllBytes(entry);
                    if (i != entries.Count - 1) md5.TransformBlock(bytes, 0, bytes.Length, bytes, 0);
                }
                if (bytes != null) md5.TransformFinalBlock(bytes, 0, bytes.Length);
                hash = md5.Hash;
            }
            return BitConverter.ToString(hash).Replace(Resources.Delim_Hyphen, string.Empty).ToLowerInvariant();
        }

        private static void CreateHiddenFile(string filePath)
        {
            File.Create(filePath).Close();
            File.SetAttributes(filePath, FileAttributes.Hidden | FileAttributes.ReadOnly);
        }

        public override void Delete() { DeleteDirectory(FullPath); }
        
        private static void DeleteDirectory(string path)
        {
            if (!Directory.Exists(path)) throw new DirectoryNotFoundException();

            var subDirs = Directory.GetDirectories(path);
            foreach (var dir in subDirs) DeleteDirectory(dir);

            var files = Directory.GetFiles(path);
            foreach (var file in files)
            {
                var attr = File.GetAttributes(file);
                if ((attr & FileAttributes.ReadOnly) == FileAttributes.ReadOnly) File.SetAttributes(file, attr ^ FileAttributes.ReadOnly);
                File.Delete(file);
            }
            Directory.Delete(path);
        }

        protected override void SetReadOnly()
        {
            var files = ((DirectoryInfo)FsInfo).GetFiles(Resources.Wildcard_Any, SearchOption.AllDirectories);

            foreach (var file in files.Select(x => x.FullName))
            {
                var attributes = File.GetAttributes(file);

                if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly) continue;
                File.SetAttributes(file, attributes | FileAttributes.ReadOnly);
            }            
        }

        protected override void SetReadWrite()
        {
            var files = ((DirectoryInfo)FsInfo).GetFiles(Resources.Wildcard_Any, SearchOption.AllDirectories);

            foreach (var file in files.Select(x => x.FullName))
            {
                var attributes = File.GetAttributes(file);

                if ((attributes & FileAttributes.ReadOnly) != FileAttributes.ReadOnly) continue;
                File.SetAttributes(file, attributes & ~FileAttributes.ReadOnly);
            }
        }

        public FsFile ToFile()
        {
            FsFile n = null;
            string sourceDir = FullPath, sourceFile = null, targetFile = null;

            try
            {
                sourceFile = Directory.GetFiles(
                    sourceDir,
                    Resources.Wildcard_Any,
                    SearchOption.TopDirectoryOnly)
                    .First(x => !x.EndsWith(Resources.FileExtension_Flerp));

                var extension = Path.GetExtension(sourceFile);
                if (extension != null) Parent.MasterExtension = extension.ToLowerInvariant();

                targetFile = sourceDir + Parent.MasterExtension;

                File.Move(sourceFile, targetFile);
                Delete();

                n = new FsFile(targetFile);
                return n;
            }
            finally
            {
                if (n == null)
                {
                    Parent.MasterExtension = "";
                    if (!Directory.Exists(sourceDir)) Directory.CreateDirectory(sourceDir);
                    if (targetFile != null && !File.Exists(sourceFile)) File.Move(targetFile, sourceFile);

                    var h = new FsDirectory(sourceDir).FsHash;                              // Force hash computation (to replace "dir_pad.flerp" if required).
                    Console.WriteLine(h);
                }
                Parent.Save();
            }
        }
 
        public override FsEntity ToMaster(IPersistable entity, bool removeOriginal, CancellationTokenSource token)
        {
            var targetPath = GetDocDir(entity.Id) + GetFlerpFilename(entity.Id, FullPath);
            Directory.CreateDirectory(targetPath);
            var fs = Create(targetPath);

            try
            {
                var dirs = Directory.GetDirectories(
                    FullPath, 
                    Resources.Wildcard_AnyFilenameDotAnyExtension, 
                    SearchOption.AllDirectories)
                    .Select(x => new DirectoryInfo(x));

                var directoryInfos = dirs as DirectoryInfo[] ?? dirs.ToArray();
                foreach (var dir in directoryInfos)
                {
                    token.Token.ThrowIfCancellationRequested();
                    Directory.CreateDirectory(targetPath + dir.FullName.Substring(FullPath.Length));
                }

                var files = Directory.GetFiles(
                    FullPath, 
                    Resources.Wildcard_AnyFilenameDotAnyExtension, 
                    SearchOption.AllDirectories)
                    .Select(x => new FileInfo(x));

                foreach (var file in files)
                {
                    token.Token.ThrowIfCancellationRequested();
                    file.CopyTo(targetPath + file.FullName.Substring(FullPath.Length), true);
                }

                foreach (var dir in directoryInfos)
                {
                    token.Token.ThrowIfCancellationRequested();
                    new DirectoryInfo(targetPath + dir.FullName.Substring(FullPath.Length)).LastWriteTime = dir.LastWriteTime;
                }

                token.Token.ThrowIfCancellationRequested();
                if (removeOriginal) Delete();

                return fs;
            }
            catch
            {
                if (fs != null) fs.Delete();
                throw;
            }           
        }
                
    }
}